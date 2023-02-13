using CommunityToolkit.Mvvm.ComponentModel;
using EOTools.Models;
using EOTools.Models.EquipmentUpgrade;
using EOTools.Tools;
using EOTools.Translation.EquipmentUpgrade;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;

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

    public ObservableCollection<EquipmentUpgradeImprovmentViewModel> Upgrades { get; set; }

    public EquipmentViewModel(EquipmentModel model)
    {
        Model = model;

        NameEN = model.NameEN;
        NameJP = model.NameJP;
        ApiId = model.ApiId;

        List<EquipmentUpgradeImprovmentViewModel> upgrades = model.UpgradeData switch {
            string => JsonSerializer.Deserialize<EquipmentUpgradeDataModel>(model.UpgradeData).Improvement.Select(upg => new EquipmentUpgradeImprovmentViewModel(upg)).ToList(),
            _ => new()
        };

        Upgrades = new(upgrades);
    }

    public void SaveChanges()
    {
        Model.NameJP = NameJP;
        Model.NameEN = NameEN;
        Model.ApiId = ApiId;

        EquipmentUpgradeDataModel upgrades = new()
        {
            EquipmentId = ApiId,
            ConvertTo = new(),
            Improvement = Upgrades.Select(upg => upg.Model).ToList(),
            UpgradeFor = new(),
        };

        Model.UpgradeData = JsonSerializer.Serialize(upgrades);
    }
}
