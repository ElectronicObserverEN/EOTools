using EOTools.DataBase;
using EOTools.Models.Ships;
using System;
using System.Linq;

namespace EOTools.Translation.Equipments.UpgradeChecker;

public class EquipmentCantBeUpgradedViewModel : UpgradeIssueViewModel
{
    public int EquipmentId { get; set; }
    public DayOfWeek Day { get; set; }

    public override string Message => $"This equipment can't be upgraded on this day : {GetEquipmentString()} ({Enum.GetName(Day)})";

    public EquipmentCantBeUpgradedViewModel(ShipModel ship) : base(ship)
    {

    }

    public string GetEquipmentString()
    {
        using EOToolsDbContext db = new();
        return db.Equipments.FirstOrDefault(eq => eq.ApiId == EquipmentId)?.NameEN ?? "";
    }

    // override object.Equals
    public override bool Equals(object? obj)
    {
        //       
        // See the full list of guidelines at
        //   http://go.microsoft.com/fwlink/?LinkID=85237  
        // and also the guidance for operator== at
        //   http://go.microsoft.com/fwlink/?LinkId=85238
        //

        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        EquipmentCantBeUpgradedViewModel issue = (obj as EquipmentCantBeUpgradedViewModel)!;

        if (EquipmentId != issue.EquipmentId) return false;
        if (Day != issue.Day) return false;

        return base.Equals(issue);
    }

    // override object.GetHashCode
    public override int GetHashCode()
    {
        return EquipmentId.GetHashCode() ^ Day.GetHashCode() ^ base.GetHashCode();
    }
}
