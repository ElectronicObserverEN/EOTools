using EOTools.Extensions;
using EOTools.Models.EquipmentUpgrade;
using EOTools.Models.Ships;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EOTools.Translation.Equipments.UpgradeChecker;

public class UpgradeDataPerDayAndShipViewModel
{
    public DayOfWeek Day { get; set; }
    public string DayString => Enum.GetName(Day) ?? "";

    public List<EquipmentUpgradeDataModel> Improvments { get; set; } = new();

    public string ImprovmentsString => string.Join(", ", Improvments.Select(upg => upg.GetEquipmentString()).ToArray());

    public ShipModel ShipModel { get; set; }

    public UpgradeDataPerDayAndShipViewModel(DayOfWeek day, ShipModel ship, List<EquipmentUpgradeDataModel> improvments)
    {
        ShipModel = ship;
        Improvments = improvments;
        Day = day;
    }
}
