using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EOTools.Models.EquipmentUpgrade
{
    public class EquipmentUpgradeHelpersModel
    {
        [JsonIgnore]
        public int EquipmentUpgradeImprovmentModelId { get; set; }

        [JsonIgnore]
        public EquipmentUpgradeImprovmentModel Improvment { get; set; }

        [JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// Ids of the helpers
        /// </summary>
        [JsonProperty("ship_ids")]
        [NotMapped]
        public List<int> ShipIdsList => ShipIds.Select(m => m.ShipId).ToList();
        //public List<int> ShipIdsList { get; set; } = new();

        [JsonIgnore]
        public List<EquipmentUpgradeHelpersShipModel> ShipIds { get; set; } = new List<EquipmentUpgradeHelpersShipModel>();

        /// <summary>
        /// Days those helpers can help
        /// </summary>
        [JsonProperty("days")]
        [NotMapped]
        //public List<DayOfWeek> CanHelpOnDaysList { get; set; } = new();
        public List<DayOfWeek> CanHelpOnDaysList => CanHelpOnDays.Select(m => m.Day).ToList();

        [JsonIgnore]
        public List<EquipmentUpgradeHelpersDayModel> CanHelpOnDays { get; set; } = new();
    }
}
