using EOTools.Models;
using EOTools.Models.EquipmentUpgrade;
using EOTools.Tools;

namespace EOTools.Translation.Equipments;

public class EquipmentViewModel
{
    public EquipmentModel Model { get; set; }

    public EquipmentUpgradeViewModel Upgrade { get; set; }

    public EquipmentViewModel(EquipmentModel model)
    {
        Model = model;

        EquipmentUpgradeDataModel upgradeModel = model.UpgradeData is null ? new() : JsonHelper.ReadJsonFromString<EquipmentUpgradeDataModel>(model.UpgradeData);

        Upgrade = new(upgradeModel);
    }
}
