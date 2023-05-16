using EOTools.DataBase;
using System.Linq;

namespace EOTools.Models.Ships;

public class ShipDataModel
{
    public int ApiId { get; set; }

    public int MasterShipId { get; set; }

    public ShipModel GetMasterShip()
    {
        using EOToolsDbContext db = new();
        return db.Ships.First(sh => sh.ApiId == MasterShipId);
    }
}
