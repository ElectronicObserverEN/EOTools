using System;
using System.Collections.Generic;
using EOTools.Translation.Ships.ShipNationality;
using Newtonsoft.Json;

namespace EOTools.Models.FitBonus
{
    public class FitBonusDataModel
    {
        [JsonProperty("shipClass")]
        public List<int>? ShipClasses { get; set; } = null;

        /// <summary>
        /// Master id = exact id of the ship
        /// </summary>
        [JsonProperty("shipX")]
        public List<int>? ShipMasterIds { get; set; } = null;

        /// <summary>
        /// Base id of the ship (minimum remodel), bonus applies to all of the ship forms
        /// </summary>
        [JsonProperty("shipS")]
        public List<int>? ShipIds { get; set; } = null;

        [JsonProperty("shipType")]
        public List<int?>? ShipTypes { get; set; } = null;

        [JsonProperty("shipNationality")] public List<ShipNationality>? ShipNationalities { get; set; }

        [JsonProperty("requires")]
        public List<int>? EquipmentRequired { get; set; } = null;

        [JsonProperty("requiresLevel")] public int? EquipmentRequiresLevel { get; set; } = null;

        [Obsolete("Use NumberOfEquipmentsRequiredAfterOtherFilters instead", true)]
        [JsonProperty("requiresNum")]
        public int? NumberOfEquipmentsRequired { get; set; }


        [JsonProperty("requiresType")]
        public List<int?>? EquipmentTypesRequired { get; set; } = null;

        [JsonProperty("requiresNumType")]
        public int? NumberOfEquipmentTypesRequired { get; set; }

        /// <summary>
        /// Improvment level of the equipment required
        /// </summary>
        [JsonProperty("level")]
        public int? EquipmentLevel { get; set; }

        /// <summary>
        /// Number Of Equipments Required after applying the improvment filter
        /// </summary>
        [JsonProperty("num")]
        public int? NumberOfEquipmentsRequiredAfterOtherFilters { get; set; }

        /// <summary>
        /// Bonuses to apply
        /// Applied x times, x being the number of equipment matching the conditions of the bonus fit 
        /// If NumberOfEquipmentsRequiredAfterOtherFilters or EquipmentRequired or EquipmentTypesRequired, bonus is applied only once
        /// </summary>
        [JsonProperty("bonus")]
        public FitBonusValueModel? Bonuses { get; set; } = null;

        /// <summary>
        /// Bonuses to apply if ship had a radar with LOS >= 5
        /// </summary>
        [JsonProperty("bonusSR")]
        public FitBonusValueModel? BonusesIfLOSRadar { get; set; } = null;

        /// <summary>
        /// Bonuses to apply if ship had a radar with AA >= 2
        /// </summary>
        [JsonProperty("bonusAR")]
        public FitBonusValueModel? BonusesIfAirRadar { get; set; } = null;
    }
}