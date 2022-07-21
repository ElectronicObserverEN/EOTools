using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EOTools.Models
{
    public class FitBonusPerEquipmentModel
    {
        [JsonPropertyName("types")]
        public List<int?> EquipmentTypes { get; set; } = null;

        [JsonPropertyName("ids")]
        public List<int?> EquipmentIds { get; set; } = null;

        [JsonPropertyName("bonuses")]
        public List<FitBonusDataModel> Bonuses { get; set; } = null;
    }
}
