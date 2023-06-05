using System.Linq;
using EOTools.DataBase;
using EOTools.Models.EquipmentUpgrade;
using EOTools.Models.Ships;

namespace EOTools.Translation.EquipmentUpgrade
{
    public class EquipmentUpgradeHelpersShipViewModel
    {
        public EquipmentUpgradeHelpersShipModel Model { get; }

        public ShipModel ShipModel => DbContext.Ships.FirstOrDefault(ship => ship.ApiId == Model.ShipId) ?? new();

        private EOToolsDbContext DbContext { get; }

        public EquipmentUpgradeHelpersShipViewModel(EquipmentUpgradeHelpersShipModel model, EOToolsDbContext dbContext)
        {
            Model = model;
            DbContext = dbContext;
        }
    }
}
