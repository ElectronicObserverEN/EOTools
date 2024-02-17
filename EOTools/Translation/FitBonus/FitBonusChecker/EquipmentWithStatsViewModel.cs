using EOTools.DataBase;
using EOTools.Models;
using System.Linq;

namespace EOTools.Translation.FitBonus.FitBonusChecker;

public class EquipmentWithStatsViewModel
{
    private EquipmentWithStatsModel Model { get; }

    private EOToolsDbContext Database { get; }

    public int Level { get; set; }

    public EquipmentModel Equipment { get; set; } = new();

    public EquipmentWithStatsViewModel(EquipmentWithStatsModel model, EOToolsDbContext db)
    {
        Model = model;
        Database = db;

        LoadFromModel();
    }

    private void LoadFromModel()
    {
        Level = Model.Level;
        Equipment = Database.Equipments.FirstOrDefault(eqDb => eqDb.ApiId == Model.EquipmentId) ?? new();
    }
}