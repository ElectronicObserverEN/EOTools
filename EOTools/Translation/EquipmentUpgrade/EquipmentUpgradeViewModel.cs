using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOTools.DataBase;
using EOTools.Models.EquipmentUpgrade;
using EOTools.Tools.EquipmentPicker;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;

namespace EOTools.Translation.EquipmentUpgrade;

public partial class EquipmentUpgradeImprovmentViewModel : ObservableObject
{
    public EquipmentUpgradeImprovmentModel Model { get; set; }

    [ObservableProperty]
    private EquipmentUpgradeConversionViewModel? conversionViewModel;

    public EquipmentUpgradeImprovmentCostViewModel CostViewModel { get; set; } = new(new());

    public ObservableCollection<EquipmentUpgradeHelpersViewModel> Helpers { get; set; } = new();

    public string AfterConversionEquipmentName => ConversionViewModel?.Equipment?.NameEN ?? "Select an equipment";

    private EOToolsDbContext DbContext { get; } = new();

    public EquipmentUpgradeImprovmentViewModel(EquipmentUpgradeImprovmentModel model, EOToolsDbContext db)
    {
        DbContext = db;
        Model = model;
        LoadFromModel();
    }

    public void LoadFromModel()
    {
        ConversionViewModel = Model.ConversionData is null ? null : new(Model.ConversionData);
        CostViewModel = new(Model.Costs);
        Helpers = new(Model.Helpers.Select(model => new EquipmentUpgradeHelpersViewModel(model, DbContext)));
    }

    public void SaveChanges()
    {
        ConversionViewModel?.SaveChanges();
        CostViewModel.SaveChanges();

        foreach (EquipmentUpgradeHelpersViewModel helpers in Helpers)
        {
            helpers.SaveChanges();
        }

        Model.ConversionData = ConversionViewModel?.Model;
        Model.Costs = CostViewModel.Model;
        Model.Helpers = Helpers.Select(vm => vm.Model).ToList();

        foreach (var helper in Model.Helpers)
        {
            helper.Improvment = Model;
        }
    }

    [RelayCommand]
    private void OpenEquipmentPicker()
    {
        using EOToolsDbContext db = new();

        EquipmentPickerViewModel vm = new(db.Equipments.ToList());

        EquipmentDataPickerView picker = new(vm);

        if (picker.ShowDialog() == true && vm.SelectedEquipment != null)
        {
            ConversionViewModel = new(new()
            {
                EquipmentLevelAfter = 0,
                IdEquipmentAfter = vm.SelectedEquipment.ApiId
            });

            OnPropertyChanged(nameof(AfterConversionEquipmentName));
        }
    }


    [RelayCommand]
    private void ClearEquipment()
    {
        ConversionViewModel = null;
        OnPropertyChanged(nameof(AfterConversionEquipmentName));
    }

    [RelayCommand]
    private void AddHelpers()
    {
        EquipmentUpgradeHelpersModel model = new();
        Helpers.Add(new(model, DbContext));
        DbContext.Add(model);
    }

    [RelayCommand]
    private void RemoveHelpers(EquipmentUpgradeHelpersViewModel vm)
    {
        Helpers.Remove(vm);
        DbContext.Remove(vm.Model);
    }
}
