using Newtonsoft.Json;

namespace EOTools.Models.EquipmentUpgrade;

public class EquipmentUpgradeImprovmentCostItemDetail
{
    /// <summary>
    /// Id of the item
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Number of this equipment required
    /// </summary>
    [JsonProperty("eq_count")]
    public int Count { get; set; }
}
