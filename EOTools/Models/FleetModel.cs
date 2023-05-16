using EOTools.Models.Ships;
using System.Collections.Generic;

namespace EOTools.Models;

public class FleetModel
{
    public List<ShipDataModel> Ships { get; set; } = new();
}
