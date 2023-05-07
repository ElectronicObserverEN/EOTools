using EOTools.Models.Ships;

namespace EOTools.Translation.Equipments.UpgradeChecker;

public class TooManyUpgradePerShipViewModel
{
    public ShipModel Ship { get; set; }

    public string Message => "Too many upgrades for this ship";

    public TooManyUpgradePerShipViewModel(ShipModel ship)
    {
        Ship = ship;
    }
}
