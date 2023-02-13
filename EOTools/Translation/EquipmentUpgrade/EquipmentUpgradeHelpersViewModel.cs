using CommunityToolkit.Mvvm.Input;
using EOTools.Models.EquipmentUpgrade;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EOTools.Translation.EquipmentUpgrade;

public partial class EquipmentUpgradeHelpersViewModel
{
    public int ShipId { get; set; }

    public DayOfWeek Day { get; set; }

    public ObservableCollection<int> Ships { get; set; } = new();
    public ObservableCollection<DayOfWeek> CanHelpOnDays { get; set; } = new();

    public EquipmentUpgradeHelpersModel Model { get; set; }

    public List<DayOfWeek> Days => Enum.GetValues<DayOfWeek>().ToList();

    public EquipmentUpgradeHelpersViewModel(EquipmentUpgradeHelpersModel model)
    {
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
        Ships.Add(ShipId);
    }

    [RelayCommand]
    public void AddDay()
    {
        CanHelpOnDays.Add(Day);
    }

    [RelayCommand]
    public void RemoveShipId(int id)
    {
        Ships.Remove(id);
    }

    [RelayCommand]
    public void RemoveDay(DayOfWeek day)
    {
        CanHelpOnDays.Remove(day);
    }
}
