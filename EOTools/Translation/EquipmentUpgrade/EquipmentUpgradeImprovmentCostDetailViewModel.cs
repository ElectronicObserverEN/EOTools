using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOTools.Models.EquipmentUpgrade;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EOTools.Translation.EquipmentUpgrade;

public partial class EquipmentUpgradeImprovmentCostDetailViewModel : ObservableObject
{
    [ObservableProperty]
    private int devmatCost;
    [ObservableProperty]
    private int sliderDevmatCost;
    [ObservableProperty]
    private int improveMatCost;
    [ObservableProperty]
    private int sliderImproveMatCost;

    public ObservableCollection<EquipmentUpgradeImprovmentCostEquipmentRequirementViewModel> EquipmentsRequired { get; set; } = new();

    public EquipmentUpgradeImprovmentCostDetail Model { get; set; }

    public EquipmentUpgradeImprovmentCostDetailViewModel(EquipmentUpgradeImprovmentCostDetail model)
    {
        Model = model;

        LoadFromModel();
    }

    public void LoadFromModel()
    {
        DevmatCost = Model.DevmatCost;
        SliderDevmatCost = Model.SliderDevmatCost;
        ImproveMatCost = Model.ImproveMatCost;
        SliderImproveMatCost = Model.SliderImproveMatCost;

        EquipmentsRequired = new(Model.EquipmentDetail.Select(eq => new EquipmentUpgradeImprovmentCostEquipmentRequirementViewModel(eq)).ToList());
    }

    public void SaveChanges()
    {
        Model.DevmatCost = DevmatCost;
        Model.SliderDevmatCost = SliderDevmatCost;
        Model.ImproveMatCost = ImproveMatCost;
        Model.SliderImproveMatCost = SliderImproveMatCost;

        Model.EquipmentDetail = new();

        foreach (EquipmentUpgradeImprovmentCostEquipmentRequirementViewModel vm in EquipmentsRequired)
        {
            vm.SaveChanges();
            Model.EquipmentDetail.Add(vm.Model);
        }
    }

    [RelayCommand]
    public void AddEquipmentRequirement()
    {
        EquipmentUpgradeImprovmentCostEquipmentRequirementViewModel vm = new(new());

        vm.OpenEquipmentPicker();

        if (vm.Id > 0)
        {
            EquipmentsRequired.Add(vm);
        }
    }

    [RelayCommand]
    public void RemoveEquipmentRequirement(EquipmentUpgradeImprovmentCostEquipmentRequirementViewModel vm)
    {
        EquipmentsRequired.Remove(vm);
    }
}
