using EOTools.DataBase;
using EOTools.Models.EquipmentUpgrade;
using EOTools.Translation.Equipments.UpgradeChecker;
using System.Linq;

namespace EOTools.Extensions;

public static class EquipmentUpgradeDataExtensions
{
    public static string GetEquipmentString(this EquipmentUpgradeDataModel model)
    {
        using EOToolsDbContext db = new();
        return db.Equipments.FirstOrDefault(eq => eq.ApiId == model.EquipmentId)?.NameEN ?? "";
    }
}
