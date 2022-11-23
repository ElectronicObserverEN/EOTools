using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EOTools.Models.EquipmentUpgrade
{
    public class EquipmentUpgradeHelpersModel
    {
        /// <summary>
        /// Ids of the helpers
        /// </summary>
        [JsonProperty("ship_ids")]
        public List<int> ShipIds { get; set; } = new List<int>();

        /// <summary>
        /// Days those helpers can help
        /// </summary>
        [JsonProperty("days")]
        public List<DayOfWeek> CanHelpOnDays { get; set; } = new List<DayOfWeek>();
    }
}
