using CommunityToolkit.Mvvm.ComponentModel;
using EOTools.DataBase;
using EOTools.Models.EquipmentUpgrade;
using Microsoft.EntityFrameworkCore;

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

    private DbContext DbContext { get; }

    public EquipmentUpgradeImprovmentCostDetailViewModel Cost0To5ViewModel { get; set; } 
    public EquipmentUpgradeImprovmentCostDetailViewModel Cost6To9ViewModel { get; set; } 
    public EquipmentUpgradeImprovmentCostDetailViewModel CostMaxViewModel { get; set; }

    public EquipmentUpgradeImprovmentCost Model { get; set; }

    public EquipmentUpgradeImprovmentCostViewModel(EquipmentUpgradeImprovmentCost model, DbContext db)
    {
        Model = model;
        DbContext = db;

        LoadFromModel();
    }

    public void LoadFromModel()
    {
        Fuel = Model.Fuel;
        Ammo = Model.Ammo;
        Steel = Model.Steel;
        Bauxite = Model.Bauxite;

        Cost0To5ViewModel = new(Model.Cost0To5, DbContext);
        Cost6To9ViewModel = new(Model.Cost6To9, DbContext);

        CostMaxViewModel = Model.CostMax is null ? new(new(), DbContext) : new(Model.CostMax, DbContext);
    }

    public void SaveChanges()
    {
        Model.Fuel = Fuel;
        Model.Ammo = Ammo;
        Model.Steel = Steel;
        Model.Bauxite = Bauxite;

        Cost0To5ViewModel.SaveChanges();

        Cost6To9ViewModel.SaveChanges();

        CostMaxViewModel.SaveChanges();

        if (CostMaxViewModel.DevmatCost == 0 && CostMaxViewModel.SliderDevmatCost == 0 && CostMaxViewModel.ImproveMatCost == 0 && CostMaxViewModel.SliderImproveMatCost == 0)
        {
            Model.CostMax = null;
        }
        else if (Model.CostMax is null)
        {
            Model.CostMax = CostMaxViewModel.Model;
            DbContext.Add(CostMaxViewModel.Model);
        }
    }
}
