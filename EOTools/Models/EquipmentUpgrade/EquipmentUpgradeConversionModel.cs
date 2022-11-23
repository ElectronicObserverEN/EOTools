using Newtonsoft.Json;

namespace EOTools.Models.EquipmentUpgrade
{
    public class EquipmentUpgradeConversionModel
    {
        [JsonProperty("id_after")]
        public int IdEquipmentAfter { get; set; }

        [JsonProperty("lvl_after")]
        public int EquipmentLevelAfter { get; set; }
    }
}
