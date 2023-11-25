using EOTools.DataBase;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace EOTools.Models.Ships;

public class ShipDataModel
{
    public int ApiId { get; set; }

    public int MasterShipId { get; set; }

    public ShipModel GetMasterShip()
    {
        using EOToolsDbContext db = new();
        return db.Ships
            .Include(nameof(ShipModel.ShipClass))
            .First(sh => sh.ApiId == MasterShipId);
    }
}
