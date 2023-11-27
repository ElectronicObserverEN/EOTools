using System.Collections.Generic;
using Newtonsoft.Json;

namespace EOTools.Models.FitBonus
{
    public class FitBonusPerEquipmentModel
    {
        [JsonProperty("types")]
        public List<int?>? EquipmentTypes { get; set; } = null;

        [JsonProperty("ids")]
        public List<int>? EquipmentIds { get; set; } = null;

        [JsonProperty("bonuses")]
        public List<FitBonusDataModel>? Bonuses { get; set; } = null;
    }
}
