using Newtonsoft.Json;

namespace EOTools.Models
{
    public class FitBonusValueModel
    {

        [JsonProperty("houg")]
        public int? Firepower { get; set; }

        [JsonProperty("raig")]
        public int? Torpedo { get; set; }

        [JsonProperty("tyku")]
        public int? AntiAir { get; set; }

        [JsonProperty("souk")]
        public int? Armor { get; set; }

        [JsonProperty("kaih")]
        public int? Evasion { get; set; }

        [JsonProperty("tais")]
        public int? ASW { get; set; }

        [JsonProperty("saku")]
        public int? LOS { get; set; }

        /// <summary>
        /// Visible acc fit actually doesn't work according to some studies
        /// </summary>
        [JsonProperty("houm")]
        public int? Accuracy { get; set; }

        [JsonProperty("leng")]
        public int? Range { get; set; }

    }
}
