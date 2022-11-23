using Newtonsoft.Json;
using System.Collections.Generic;

namespace EOTools.Models.EquipmentUpgrade;

public class EquipmentUpgradeImprovmentCostDetail
{
    /// <summary>
    /// Devmat cost
    /// </summary>
    [JsonProperty("devmats")]
    public int DevmatCost { get; set; }

    /// <summary>
    /// Devmat cost if slider is used
    /// </summary>
    [JsonProperty("devmats_sli")]
    public int SliderDevmatCost { get; set; }

    /// <summary>
    /// Screw cost
    /// </summary>
    [JsonProperty("screws")]
    public int ImproveMatCost { get; set; }

    /// <summary>
    /// Screw cost if slider is used
    /// </summary>
    [JsonProperty("screws_sli")]
    public int SliderImproveMatCost { get; set; }

    [JsonProperty("equips")]
    public List<EquipmentUpgradeImprovmentCostItemDetail> EquipmentDetail { get; set; } = new List<EquipmentUpgradeImprovmentCostItemDetail>();

    [JsonProperty("consumable")]
    public List<EquipmentUpgradeImprovmentCostItemDetail> ConsumableDetail { get; set; } = new List<EquipmentUpgradeImprovmentCostItemDetail>();

}