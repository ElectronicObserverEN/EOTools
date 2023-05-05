using Newtonsoft.Json;

namespace EOTools.Models.EquipmentUpgrade;

public class EquipmentUpgradeImprovmentCostItemDetail
{
    [JsonIgnore]
    public int Id { get; set; }

    /// <summary>
    /// Id of the item
    /// </summary>
    [JsonProperty("id")]
    public int ItemId { get; set; }

    /// <summary>
    /// Number of this equipment required
    /// </summary>
    [JsonProperty("eq_count")]
    public int Count { get; set; }
}
