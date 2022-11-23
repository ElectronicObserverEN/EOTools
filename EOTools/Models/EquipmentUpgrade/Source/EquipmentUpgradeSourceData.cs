using Newtonsoft.Json;
using System.Collections.Generic;

namespace EOTools.Models.EquipmentUpgrade.Source;

public class EquipmentUpgradeSourceData
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("improvement")]
    public List<EquipmentUpgradeSourceDataImprovement> Improvement { get; set; }

    [JsonProperty("upgrade_to")]
    public List<List<int>> UpgradeTo { get; set; }

    [JsonProperty("upgrade_for")]
    public List<int> UpgradeFor { get; set; }
}