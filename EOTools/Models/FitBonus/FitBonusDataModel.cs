using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace EOTools.Models
{
    public class FitBonusDataModel
    {
        [JsonPropertyName("shipClass")]
        public List<int?> ShipClasses { get; set; } = null;

        /// <summary>
        /// Master id = exact id of the ship
        /// </summary>
        [JsonPropertyName("shipX")]
        public List<int?> ShipMasterIds { get; set; } = null;

        /// <summary>
        /// Base id of the ship (minimum remodel), bonus applies to all of the ship forms
        /// </summary>
        [JsonPropertyName("shipS")]
        public List<int?> ShipIds { get; set; } = null;

        [JsonPropertyName("shipType")]
        public List<int?> ShipTypes { get; set; } = null;


        [JsonPropertyName("requires")]
        public List<int?> EquipmentRequired { get; set; } = null;

        [JsonPropertyName("requiresNum")]
        public int? NumberOfEquipmentsRequired { get; set; }


        [JsonPropertyName("requiresType")]
        public List<int?> EquipmentTypesRequired { get; set; } = null;

        [JsonPropertyName("requiresNumType")]
        public int? NumberOfEquipmentTypesRequired { get; set; }

        /// <summary>
        /// Improvment level of the equipment required
        /// </summary>
        [JsonPropertyName("level")]
        public int? EquipmentLevel { get; set; }

        /// <summary>
        /// Number Of Equipments Required after applying the improvment filter
        /// </summary>
        [JsonPropertyName("num")]
        public int? NumberOfEquipmentsRequiredAfterOtherFilters { get; set; }

        /// <summary>
        /// Bonuses to apply
        /// Applied x times, x being the number of equipment matching the conditions of the bonus fit 
        /// If NumberOfEquipmentsRequiredAfterOtherFilters or EquipmentRequired or EquipmentTypesRequired, bonus is applied only once
        /// </summary>
        [JsonPropertyName("bonus")]
        public FitBonusValueModel Bonuses { get; set; } = null;

        /// <summary>
        /// Bonuses to apply if ship had a radar with LOS >= 5
        /// </summary>
        [JsonPropertyName("bonusSR")]
        public FitBonusValueModel BonusesIfLOSRadar { get; set; } = null;

        /// <summary>
        /// Bonuses to apply if ship had a radar with AA >= 2
        /// </summary>
        [JsonPropertyName("bonusAR")]
        public FitBonusValueModel BonusesIfAirRadar { get; set; } = null;

    }
}