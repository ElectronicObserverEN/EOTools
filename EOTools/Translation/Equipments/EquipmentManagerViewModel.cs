using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOTools.DataBase;
using EOTools.Models;
using EOTools.Models.EquipmentUpgrade;
using EOTools.Tools;
using EOTools.Translation.Equipments.UpgradeChecker;
using EOTools.Translation.FitBonus.FitBonusChecker;
using ModernWpf.Controls;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.DependencyInjection;
using EOTools.Translation.FitBonus;

namespace EOTools.Translation.Equipments;

public partial class EquipmentManagerViewModel : ObservableObject
{
    public ObservableCollection<EquipmentViewModel> EquipmentList { get; set; } = new();

    private FitBonusManager FitBonusManager { get; }

    [ObservableProperty]
    private string filter = "";

    public EquipmentManagerViewModel()
    {
        FitBonusManager = Ioc.Default.GetRequiredService<FitBonusManager>();

        ReloadEquipmentList();

        PropertyChanged += EquipmentManagerViewModel_PropertyChanged;
    }

    private void EquipmentManagerViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName is not nameof(Filter)) return;

        ReloadEquipmentList();
    }

    private void ReloadEquipmentList()
    {
        using EOToolsDbContext db = new();

        IEnumerable<EquipmentViewModel> allEquips = db.Equipments.Select(model => new EquipmentViewModel(model));
        string upperCaseFilter = Filter.ToUpperInvariant();
        EquipmentList = new(allEquips.Where(eq => string.IsNullOrEmpty(upperCaseFilter) || eq.Model.NameEN.ToUpperInvariant().Contains(upperCaseFilter) || eq.Model.NameJP.ToUpperInvariant().Contains(upperCaseFilter)));
        OnPropertyChanged(nameof(EquipmentList));
    }

    private void AddNewEquipment(EquipmentModel model)
    {
        EquipmentViewModel vm = new(model);

        using EOToolsDbContext db = new();
        db.Equipments.Add(model);
        db.SaveChanges();

        EquipmentList.Add(vm);
    }

    private async Task ShowEditDialog(EquipmentViewModel vm, bool newEntity)
    {
        EquipmentViewModel vmEdit = new(vm.Model);

        EquipmentEditView view = new(vmEdit);

        if (view.ShowDialog() == true)
        {
            vm.ApiId = vmEdit.ApiId;
            vm.NameEN = vmEdit.NameEN;
            vm.NameJP = vmEdit.NameJP;
            vm.CanBeCrafted = vmEdit.CanBeCrafted;
            vm.Upgrades = vmEdit.Upgrades;

            try
            {
                vm.SaveChanges();

                if (newEntity)
                {
                    AddNewEquipment(vm.Model);
                }
                else
                {
                    using EOToolsDbContext db = new();
                    db.Update(vm.Model);
                    db.SaveChanges();

                    ReloadEquipmentList();
                }
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }

                ContentDialog errorDialog = new ContentDialog();
                errorDialog.Content = $"{ex.Message}\n\n\n\n{ex.StackTrace}";
                errorDialog.CloseButtonText = "Close";

                await errorDialog.ShowAsync();

                await ShowEditDialog(vm, newEntity);
            }
        }
    }

    [RelayCommand]
    public async Task ShowAddEquipmentDialog()
    {
        EquipmentViewModel vm = new(new());
        await ShowEditDialog(vm, true);
    }

    [RelayCommand]
    public async Task EditEquipment(EquipmentViewModel vm)
    {
        await ShowEditDialog(vm, false);
    }

    [RelayCommand]
    public void RemoveEquipment(EquipmentViewModel vm)
    {
        using EOToolsDbContext db = new();
        db.Remove(vm.Model);
        db.SaveChanges();

        EquipmentList.Remove(vm);

        ReloadEquipmentList();
    }

    #region Data import and export stuff
    public string EquipmentUpgradeFilePath => Path.Combine(AppSettings.ElectronicObserverDataFolderPath, "Data", "EquipmentUpgrades.json");

    [RelayCommand]
    public void ImportUpgradeData()
    {
        JArray upgrades = JsonHelper.ReadJsonArray(EquipmentUpgradeFilePath);

        using EOToolsDbContext db = new();

        foreach (JToken model in upgrades)
        {
            EquipmentModel? eq = db.Equipments.Where(eq => eq.ApiId == model.Value<int>("eq_id")).FirstOrDefault();

            if (eq != null)
            {
                /*eq.UpgradeData = model.ToString()
                    .Replace("\r\n", "")
                    .Replace(" ", "");*/
            }
        }

        db.SaveChanges();
    }

    [RelayCommand]
    public void ImportFromTranslations()
    {
        // CSV file exported from EO converted to json
        JArray equipments = JsonHelper.ReadJsonArray("EquipmentData.json");

        // Translations to get JP name : 
        List<EquipData> translations = LoadEquipmentTranslations();

        EquipmentList.Clear();
        using EOToolsDbContext db = new();
        db.Equipments.RemoveRange(db.Equipments);
        db.SaveChanges();

        foreach (JObject equipmentJson in equipments)
        {
            EquipmentModel model = new()
            {
                ApiId = equipmentJson.Value<int>("装備ID"),
                NameEN = equipmentJson.Value<string>("装備名"),
            };

            model.NameJP = translations.FirstOrDefault(eq => eq.NameEN == model.NameEN)?.NameJP ?? model.NameEN;

            AddNewEquipment(model);
        }
    }

    private List<EquipData> LoadEquipmentTranslations()
    {
        JObject translationsJson = JsonHelper.ReadJsonObject(UpdateEquipmentDataService.EquipmentTranslationsFilePath);
        JObject jsonEquipList = translationsJson["equipment"].Value<JObject>();
        List<EquipData> results = new();

        // --- Equips
        foreach (JProperty equip in jsonEquipList.Properties())
        {
            string _nameEN = equip.Name;
            string _nameJP = jsonEquipList[_nameEN].ToString();

            results.Add(new(_nameEN, _nameJP));
        }

        return results;
    }


    [RelayCommand]
    public void UpdateTranslations()
    {
        UpdateEquipmentDataService service = new();
        service.UpdateEquipmentTranslations();
    }

    private void ConvertJsonToDb()
    {
        using EOToolsDbContext db = new();

        List<EquipmentUpgradeImprovmentCostDetail> costs = new();

        costs.AddRange(db.EquipmentUpgrades
            .SelectMany(up => up.Improvement)
            .Select(imp => imp.Costs)
            .Select(cost => cost.Cost0To5));

        costs.AddRange(db.EquipmentUpgrades
            .SelectMany(up => up.Improvement)
            .Select(imp => imp.Costs)
            .Select(cost => cost.Cost6To9));

        costs.AddRange(db.EquipmentUpgrades
            .SelectMany(up => up.Improvement)
            .Select(imp => imp.Costs)
            .Select(cost => cost.CostMax)
            .Where(costDetail => costDetail != null)
            .Cast<EquipmentUpgradeImprovmentCostDetail>());

        db.RemoveRange(costs.SelectMany(cost => cost.ConsumableDetail));
        db.SaveChanges();

        db.RemoveRange(costs.SelectMany(cost => cost.EquipmentDetail));
        db.SaveChanges();

        db.RemoveRange(db.EquipmentUpgrades.SelectMany(up => up.Improvement).SelectMany(imp => imp.Helpers));
        db.SaveChanges();

        db.RemoveRange(db.EquipmentUpgrades.SelectMany(up => up.Improvement).Select(imp => imp.ConversionData).Where(conv => conv != null).Cast<EquipmentUpgradeConversionModel>());
        db.SaveChanges();

        db.RemoveRange(db.EquipmentUpgrades.SelectMany(up => up.Improvement));
        db.SaveChanges();

        db.RemoveRange(db.EquipmentUpgrades
            .SelectMany(up => up.Improvement)
            .Select(imp => imp.Costs));
        db.SaveChanges();
        costs = new();

        costs.AddRange(db.EquipmentUpgrades
            .SelectMany(up => up.Improvement)
            .Select(imp => imp.Costs)
            .Select(cost => cost.Cost0To5));

        costs.AddRange(db.EquipmentUpgrades
            .SelectMany(up => up.Improvement)
            .Select(imp => imp.Costs)
            .Select(cost => cost.Cost6To9));

        costs.AddRange(db.EquipmentUpgrades
            .SelectMany(up => up.Improvement)
            .Select(imp => imp.Costs)
            .Select(cost => cost.CostMax)
            .Where(costDetail => costDetail != null)
            .Cast<EquipmentUpgradeImprovmentCostDetail>());

        db.RemoveRange(costs);
        db.SaveChanges();

        db.EquipmentUpgrades.RemoveRange(db.EquipmentUpgrades);
        db.SaveChanges();

        foreach (EquipmentModel equipment in db.Equipments)
        {
            /*if (equipment.UpgradeData is null)
            {
                db.EquipmentUpgrades.Add(new()
                {
                    EquipmentId = equipment.ApiId,
                });
            }
            else
            {
                EquipmentUpgradeDataModel model = JsonHelper.ReadJsonFromString<EquipmentUpgradeDataModel>(equipment.UpgradeData);

                List<EquipmentUpgradeHelpersModel> helpers = model.Improvement.SelectMany(m => m.Helpers).ToList();

                foreach (EquipmentUpgradeHelpersModel helper in helpers)
                {
                    helper.CanHelpOnDays = helper.CanHelpOnDaysList.Select(m => new EquipmentUpgradeHelpersDayModel() { Day = m }).ToList();
                    helper.ShipIds = helper.ShipIdsList.Select(m => new EquipmentUpgradeHelpersShipModel() { ShipId = m }).ToList();
                }

                db.EquipmentUpgrades.Add(model);
            }*/
        }

        db.SaveChanges();
    }

    [RelayCommand]
    public void UpdateUpgrades()
    {
        UpdateEquipmentDataService service = new();
        service.UpdateEquipmentUpgrades();
    }

    [RelayCommand]
    public void OpenEquipmentUpgradeChecker()
    {
        UpgradeCheckerView view = new UpgradeCheckerView();
        view.Show();
    }

    [RelayCommand]
    private async Task UpdateFitBonus()
    {
        await FitBonusManager.UpdateThenSaveFileThenPush();
    }

    [RelayCommand]
    private void OpenFitBonusChecker()
    {
        new FitBonusCheckerView().Show();
    }
    #endregion
}
