using CommunityToolkit.Mvvm.ComponentModel;
using EOTools.DataBase;
using EOTools.Models;
using EOTools.Models.EquipmentUpgrade;
using System.Linq;

namespace EOTools.Translation.EquipmentUpgrade;

public partial class EquipmentUpgradeConversionViewModel : ObservableObject
{
    [ObservableProperty]
    private int equipmentLevelAfter;

    [ObservableProperty]
    private EquipmentModel equipment = new();

    public EquipmentUpgradeConversionModel Model { get; set; }

    public EquipmentUpgradeConversionViewModel(EquipmentUpgradeConversionModel model)
    {
        Model = model;

        using EOToolsDbContext db = new();
        Equipment = db.Equipments.Where(eq => eq.ApiId == model.IdEquipmentAfter).First();

        EquipmentLevelAfter = model.EquipmentLevelAfter;
    }

    public void SaveChanges()
    {
        Model.EquipmentLevelAfter = EquipmentLevelAfter;
        Model.IdEquipmentAfter = Equipment.ApiId;
    }
}
