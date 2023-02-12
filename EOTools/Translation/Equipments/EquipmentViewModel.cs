using CommunityToolkit.Mvvm.ComponentModel;
using EOTools.Models;
using EOTools.Models.EquipmentUpgrade;
using EOTools.Tools;

namespace EOTools.Translation.Equipments;

public partial class EquipmentViewModel : ObservableObject
{
    [ObservableProperty]
    private string nameEN = "";

    [ObservableProperty]
    private string nameJP = "";

    [ObservableProperty]
    private int apiId;

    public EquipmentModel Model { get; set; }

    public EquipmentUpgradeViewModel Upgrade { get; set; }

    public EquipmentViewModel(EquipmentModel model)
    {
        Model = model;

        NameEN = model.NameEN;
        NameJP = model.NameJP;
        ApiId = model.ApiId;

        EquipmentUpgradeDataModel upgradeModel = model.UpgradeData is null ? new() : JsonHelper.ReadJsonFromString<EquipmentUpgradeDataModel>(model.UpgradeData);

        Upgrade = new(upgradeModel);
    }

    public void SaveChanges()
    {
        Model.NameJP = NameJP;
        Model.NameEN = NameEN;
        Model.ApiId = ApiId;
    }
}
