using EOTools.Models.EquipmentUpgrade;
using EOTools.Models.Ships;
using EOTools.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EOTools.Translation.Equipments.UpgradeChecker;

public class UpgradeDataPerDayViewModel
{
    public DayOfWeek Day { get; set; }

    public List<EquipmentUpgradeDataModel> Improvments { get; set; } = new();

    public UpgradeDataPerDayViewModel(DayOfWeek day, ShipModel ship) : this(day)
    {
        Improvments = EquipmentUpgradesService.Instance.AllUpgradeModel
            .Where(upg => upg.Improvement
                .Any(imp => imp.Helpers
                    .Any(helperGroup => helperGroup.CanHelpOnDays.Any(days => days.Day == day) && helperGroup.ShipIds.Any(ships => ships.ShipId == ship.ApiId)))).ToList();
    }

    public UpgradeDataPerDayViewModel(DayOfWeek day)
    {
        Day = day;
    }
}
