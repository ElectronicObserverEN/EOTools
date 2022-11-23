using Newtonsoft.Json;

namespace EOTools.Models.EquipmentUpgrade;

public class EquipmentUpgradeImprovmentCost
{
    [JsonProperty("fuel")]
    public int Fuel { get; set; }

    [JsonProperty("ammo")]
    public int Ammo { get; set; }

    [JsonProperty("steel")]
    public int Steel { get; set; }

    [JsonProperty("baux")]
    public int Bauxite { get; set; }

    /// <summary>
    /// Costs for level 0 -> 6
    /// </summary>
    [JsonProperty("p1")]
    public EquipmentUpgradeImprovmentCostDetail Cost0To5 { get; set; } = new();

    /// <summary>
    /// Costs for level 7 -> 10
    /// </summary>
    [JsonProperty("p2")]
    public EquipmentUpgradeImprovmentCostDetail Cost6To9 { get; set; } = new ();

    /// <summary>
    /// Costs for conversion
    /// </summary>
    [JsonProperty("conv", NullValueHandling = NullValueHandling.Ignore)]
    public EquipmentUpgradeImprovmentCostDetail? CostMax { get; set; } = null;
}
