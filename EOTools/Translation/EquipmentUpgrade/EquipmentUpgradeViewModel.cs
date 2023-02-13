using EOTools.Models.EquipmentUpgrade;

namespace EOTools.Translation.EquipmentUpgrade;

public class EquipmentUpgradeImprovmentViewModel
{
    public EquipmentUpgradeImprovmentModel Model { get; set; }

    public EquipmentUpgradeImprovmentViewModel(EquipmentUpgradeImprovmentModel model)
    {
        Model = model;
    }

    public void SaveChanges()
    {

    }
}
