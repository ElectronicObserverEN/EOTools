using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOTools.Models.EquipmentUpgrade;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;
using EOTools.DataBase;

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

    private DbContext DbContext { get; }

    public ObservableCollection<EquipmentUpgradeImprovmentCostEquipmentRequirementViewModel> EquipmentsRequired { get; set; } = new();
    public ObservableCollection<EquipmentUpgradeImprovmentCostUseItemRequirementViewModel> UseItemsRequired { get; set; } = new();

    public EquipmentUpgradeImprovmentCostDetail Model { get; set; }

    public EquipmentUpgradeImprovmentCostDetailViewModel(EquipmentUpgradeImprovmentCostDetail model, DbContext db)
    {
        DbContext = db;
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
        UseItemsRequired = new(Model.ConsumableDetail.Select(eq => new EquipmentUpgradeImprovmentCostUseItemRequirementViewModel(eq)).ToList());
    }

    public void SaveChanges()
    {
        Model.DevmatCost = DevmatCost;
        Model.SliderDevmatCost = SliderDevmatCost;
        Model.ImproveMatCost = ImproveMatCost;
        Model.SliderImproveMatCost = SliderImproveMatCost;
        
        foreach (EquipmentUpgradeImprovmentCostEquipmentRequirementViewModel vm in EquipmentsRequired)
        {
            vm.SaveChanges();

            if (!Model.EquipmentDetail.Contains(vm.Model))
            {
                DbContext.Add(vm.Model);
                Model.EquipmentDetail.Add(vm.Model);
            }
            else
            {
            }
        }
        
        foreach (EquipmentUpgradeImprovmentCostUseItemRequirementViewModel vm in UseItemsRequired)
        {
            vm.SaveChanges();

            if (!Model.ConsumableDetail.Contains(vm.Model))
            {
                DbContext.Add(vm.Model);
                Model.ConsumableDetail.Add(vm.Model);
            }
            else
            {
            }
        }
    }
    
    [RelayCommand]
    public void AddEquipmentRequirement()
    {
        EquipmentUpgradeImprovmentCostEquipmentRequirementViewModel vm = new(new());

        vm.OpenEquipmentPicker();

        if (vm.Id > 0)
        {
            DbContext.Add(vm.Model);
            EquipmentsRequired.Add(vm);
        }
    }

    [RelayCommand]
    public void RemoveEquipmentRequirement(EquipmentUpgradeImprovmentCostEquipmentRequirementViewModel vm)
    {
        DbContext.Remove(vm.Model);
        EquipmentsRequired.Remove(vm);
    }

    [RelayCommand]
    public void AddUseItemRequirement()
    {
        EquipmentUpgradeImprovmentCostUseItemRequirementViewModel vm = new(new());

        vm.OpenEquipmentPicker();

        if (vm.Id > 0)
        {
            DbContext.Add(vm.Model);
            UseItemsRequired.Add(vm);
        }
    }

    [RelayCommand]
    public void RemoveUseItemRequirement(EquipmentUpgradeImprovmentCostUseItemRequirementViewModel vm)
    {
        DbContext.Remove(vm.Model);
        UseItemsRequired.Remove(vm);
    }
}
