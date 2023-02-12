using EOTools.Models.EquipmentUpgrade;

namespace EOTools.Translation.Equipments;

public class EquipmentUpgradeViewModel
{
    public EquipmentUpgradeDataModel Model { get; set; }

    public EquipmentUpgradeViewModel(EquipmentUpgradeDataModel model)
    {
        Model = model;
    }
}
