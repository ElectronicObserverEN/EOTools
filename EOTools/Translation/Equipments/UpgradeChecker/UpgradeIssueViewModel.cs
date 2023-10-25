using System.Collections.Generic;
using EOTools.ElectronicObserverApi.Models;
using EOTools.Models.Ships;

namespace EOTools.Translation.Equipments.UpgradeChecker;

public abstract class UpgradeIssueViewModel
{
    public ShipModel Ship { get; set; }

    public abstract string Message { get; }

    public List<UserReportedEquipmentUpgradeIssueModel> UserReportedIssues { get; set; } = new();

    protected UpgradeIssueViewModel(ShipModel ship)
    {
        Ship = ship;
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

        UpgradeIssueViewModel issue = (obj as UpgradeIssueViewModel)!;

        if (Ship.ApiId != issue.Ship.ApiId) return false;

        return true;
    }

    // override object.GetHashCode
    public override int GetHashCode()
    {
        return Ship.ApiId.GetHashCode();
    }
}
