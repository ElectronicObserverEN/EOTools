using Newtonsoft.Json;
using System.Collections.Generic;

namespace EOTools.Models.EquipmentUpgrade;

public class EquipmentUpgradeDataModel
{
    [JsonProperty("eq_id")]
    public int EquipmentId { get; set; }

    /// <summary>
    /// Improvments possibles for this equipment
    /// </summary>
    [JsonProperty("improvement")]
    public List<EquipmentUpgradeImprovmentModel> Improvement { get; set; } = new List<EquipmentUpgradeImprovmentModel>();

    /// <summary>
    /// This equipment can be converted to those equipments
    /// </summary>
    [JsonProperty("convert_to")]
    public List<EquipmentUpgradeConversionModel> ConvertTo { get; set; } = new List<EquipmentUpgradeConversionModel>();

    /// <summary>
    /// This equipment is use in those equipments upgrades
    /// </summary>
    [JsonProperty("upgrade_for")]
    public List<int> UpgradeFor { get; set; } = new List<int>();
}