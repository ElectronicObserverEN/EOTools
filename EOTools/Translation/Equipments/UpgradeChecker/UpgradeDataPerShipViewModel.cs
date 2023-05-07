using EOTools.Models.Ships;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EOTools.Translation.Equipments.UpgradeChecker;

public class UpgradeDataPerShipViewModel
{
    public ShipModel ShipModel { get; set; }

    public List<UpgradeDataPerDayViewModel> Days { get; set; } = new();

    public UpgradeDataPerShipViewModel(ShipModel model)
    {
        ShipModel = model;

        Days = Enum.GetValues<DayOfWeek>().Select(day => new UpgradeDataPerDayViewModel(day, ShipModel)).ToList();
    }
}
