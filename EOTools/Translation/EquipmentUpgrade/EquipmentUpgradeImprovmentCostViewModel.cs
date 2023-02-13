using CommunityToolkit.Mvvm.ComponentModel;
using EOTools.Models.EquipmentUpgrade;

namespace EOTools.Translation.EquipmentUpgrade;

public partial class EquipmentUpgradeImprovmentCostViewModel : ObservableObject
{
    [ObservableProperty]
    private int fuel;

    [ObservableProperty]
    private int ammo;

    [ObservableProperty]
    private int steel;

    [ObservableProperty]
    private int bauxite;

    public EquipmentUpgradeImprovmentCostDetailViewModel Cost0To5ViewModel { get; set; } = new(new());
    public EquipmentUpgradeImprovmentCostDetailViewModel Cost6To9ViewModel { get; set; } = new(new());
    public EquipmentUpgradeImprovmentCostDetailViewModel CostMaxViewModel { get; set; } = new(new());

    public EquipmentUpgradeImprovmentCost Model { get; set; }

    public EquipmentUpgradeImprovmentCostViewModel(EquipmentUpgradeImprovmentCost model)
    {
        Model = model;

        LoadFromModel();
    }

    public void LoadFromModel()
    {
        Fuel = Model.Fuel;
        Ammo = Model.Ammo;
        Steel = Model.Steel;
        Bauxite = Model.Bauxite;

        Cost0To5ViewModel = new(Model.Cost0To5);
        Cost6To9ViewModel = new(Model.Cost6To9);

        CostMaxViewModel = Model.CostMax is null ? new(new()) : new(Model.CostMax);
    }

    public void SaveChanges()
    {
        Model.Fuel = Fuel;
        Model.Ammo = Ammo;
        Model.Steel = Steel;
        Model.Bauxite = Bauxite;

        Cost0To5ViewModel.SaveChanges();
        Model.Cost0To5 = Cost0To5ViewModel.Model;

        Cost6To9ViewModel.SaveChanges();
        Model.Cost6To9 = Cost6To9ViewModel.Model;

        CostMaxViewModel.SaveChanges();

        if (CostMaxViewModel.DevmatCost == 0 && CostMaxViewModel.SliderDevmatCost == 0 && CostMaxViewModel.ImproveMatCost == 0 && CostMaxViewModel.SliderImproveMatCost == 0)
        {
            Model.CostMax = null;
        }
        else
        {
            Model.CostMax = CostMaxViewModel.Model;
        }
    }
}
