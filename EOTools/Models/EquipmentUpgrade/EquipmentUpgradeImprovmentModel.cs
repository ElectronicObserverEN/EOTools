using Newtonsoft.Json;
using System.Collections.Generic;

namespace EOTools.Models.EquipmentUpgrade
{
    public class EquipmentUpgradeImprovmentModel
    {
        [JsonIgnore]
        public int Id { get; set; }

        [JsonProperty("convert")]
        public EquipmentUpgradeConversionModel? ConversionData { get; set; }

        [JsonProperty("helpers")]
        public List<EquipmentUpgradeHelpersModel> Helpers { get; set; } = new List<EquipmentUpgradeHelpersModel>();

        [JsonProperty("costs")]
        public EquipmentUpgradeImprovmentCost Costs { get; set; } = new EquipmentUpgradeImprovmentCost();
    }
}
