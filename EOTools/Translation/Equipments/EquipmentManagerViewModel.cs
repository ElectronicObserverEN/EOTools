using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOTools.DataBase;
using EOTools.Models;
using EOTools.Models.EquipmentUpgrade;
using EOTools.Tools;
using ModernWpf.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EOTools.Translation.Equipments;

public partial class EquipmentManagerViewModel : ObservableObject
{
    public ObservableCollection<EquipmentViewModel> EquipmentList { get; set; } = new();

    public EquipmentManagerViewModel()
    {
        ReloadEquipmentList();
    }

    private void ReloadEquipmentList()
    {
        using EOToolsDbContext db = new();
        EquipmentList = new(db.Equipments.Select(model => new EquipmentViewModel(model)).ToList());
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
                eq.UpgradeData = model.ToString()
                    .Replace("\r\n", "")
                    .Replace(" ", "");
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
        JObject translationsJson = JsonHelper.ReadJsonObject(AppSettings.EquipmentTLFilePath);
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

    [RelayCommand]
    public void UpdateUpgrades()
    {
        UpdateEquipmentDataService service = new();
        service.UpdateEquipmentUpgrades();
    }
    
    #endregion
}
