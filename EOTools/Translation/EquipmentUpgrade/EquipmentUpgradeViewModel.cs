﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOTools.DataBase;
using EOTools.Models.EquipmentUpgrade;
using EOTools.Tools.EquipmentPicker;
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

    public EquipmentUpgradeImprovmentViewModel(EquipmentUpgradeImprovmentModel model)
    {
        Model = model;
        LoadFromModel();
    }

    public void LoadFromModel()
    {
        ConversionViewModel = Model.ConversionData is null ? null : new(Model.ConversionData);
        CostViewModel = new(Model.Costs);
        Helpers = new(Model.Helpers.Select(model => new EquipmentUpgradeHelpersViewModel(model)));
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
        Helpers.Add(new(new()));
    }
}
