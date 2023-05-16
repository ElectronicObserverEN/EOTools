using EOTools.Models.Ships;

namespace EOTools.Translation.Equipments.UpgradeChecker;

public class TooManyUpgradePerShipViewModel : UpgradeIssueViewModel
{
    public override string Message => "Too many upgrades for this ship";

    public TooManyUpgradePerShipViewModel(ShipModel ship) : base(ship)
    {

    }

}
