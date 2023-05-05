using Newtonsoft.Json;

namespace EOTools.Models.EquipmentUpgrade
{
    public class EquipmentUpgradeConversionModel
    {
        [JsonIgnore]
        public EquipmentUpgradeImprovmentModel ImprovmentModel { get; set; }

        [JsonIgnore]
        public int ImprovmentModelId { get; set; }

        [JsonIgnore]
        public int Id { get; set; }

        [JsonProperty("id_after")]
        public int IdEquipmentAfter { get; set; }

        [JsonProperty("lvl_after")]
        public int EquipmentLevelAfter { get; set; }
    }
}
