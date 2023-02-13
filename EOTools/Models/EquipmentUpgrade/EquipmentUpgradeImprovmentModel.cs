using Newtonsoft.Json;
using System.Collections.Generic;

namespace EOTools.Models.EquipmentUpgrade
{
    public class EquipmentUpgradeImprovmentModel
    {
        [JsonProperty("convert", NullValueHandling = NullValueHandling.Ignore)]
        public EquipmentUpgradeConversionModel? ConversionData { get; set; }

        [JsonProperty("helpers")]
        public List<EquipmentUpgradeHelpersModel> Helpers { get; set; } = new List<EquipmentUpgradeHelpersModel>();

        [JsonProperty("costs")]
        public EquipmentUpgradeImprovmentCost Costs { get; set; } = new EquipmentUpgradeImprovmentCost();
    }
}
