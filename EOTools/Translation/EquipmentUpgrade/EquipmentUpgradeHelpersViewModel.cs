using CommunityToolkit.Mvvm.Input;
using EOTools.DataBase;
using EOTools.Models.EquipmentUpgrade;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EOTools.Translation.EquipmentUpgrade;

public partial class EquipmentUpgradeHelpersViewModel
{
    public int ShipId { get; set; }

    public DayOfWeek Day { get; set; }

    public ObservableCollection<EquipmentUpgradeHelpersShipModel> Ships { get; set; } = new();
    public ObservableCollection<EquipmentUpgradeHelpersDayModel> CanHelpOnDays { get; set; } = new();

    public EquipmentUpgradeHelpersModel Model { get; set; }

    public List<DayOfWeek> Days => Enum.GetValues<DayOfWeek>().ToList();

    private EOToolsDbContext DbContext { get; } = new();

    public EquipmentUpgradeHelpersViewModel(EquipmentUpgradeHelpersModel model, EOToolsDbContext db)
    {
        DbContext = db;
        Model = model;

        LoadFromModel();
    }

    public void LoadFromModel()
    {
        Ships = new(Model.ShipIds);
        CanHelpOnDays = new(Model.CanHelpOnDays);
    }

    public void SaveChanges()
    {
        Model.CanHelpOnDays = CanHelpOnDays.ToList();
        Model.ShipIds = Ships.ToList();
    }


    [RelayCommand]
    public void AddShipId()
    {
        EquipmentUpgradeHelpersShipModel model = new()
        {
            ShipId = ShipId
        };
        Ships.Add(model);
        DbContext.Entry(Model).State = EntityState.Modified;
        DbContext.Add(model);
    }

    [RelayCommand]
    public void AddDay()
    {
        EquipmentUpgradeHelpersDayModel model = new()
        {
            Day = Day
        };
        CanHelpOnDays.Add(model);
        DbContext.Entry(Model).State = EntityState.Modified;
        DbContext.Add(model);
    }

    [RelayCommand]
    public void RemoveShipId(EquipmentUpgradeHelpersShipModel id)
    {
        Ships.Remove(id);
        DbContext.Remove(id);
    }

    [RelayCommand]
    public void RemoveDay(EquipmentUpgradeHelpersDayModel day)
    {
        CanHelpOnDays.Remove(day);
        DbContext.Remove(day);
    }
}
