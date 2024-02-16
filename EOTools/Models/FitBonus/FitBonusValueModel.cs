using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace EOTools.Models.FitBonus
{
    public class FitBonusValueModel
    {
        [JsonProperty("houg")]
        [JsonPropertyName("houg")]
        public int? Firepower { get; set; }

        [JsonProperty("raig")]
        [JsonPropertyName("raig")]
        public int? Torpedo { get; set; }

        [JsonProperty("tyku")]
        [JsonPropertyName("tyku")]
        public int? AntiAir { get; set; }

        [JsonProperty("souk")]
        [JsonPropertyName("souk")]
        public int? Armor { get; set; }

        [JsonProperty("kaih")]
        [JsonPropertyName("kaih")]
        public int? Evasion { get; set; }

        [JsonProperty("tais")]
        [JsonPropertyName("tais")]
        public int? ASW { get; set; }

        [JsonProperty("saku")]
        [JsonPropertyName("saku")]
        public int? LOS { get; set; }

        [JsonProperty("baku")] 
        [JsonPropertyName("baku")]
        public int? Bombing { get; set; }

        /// <summary>
        /// Visible acc fit actually doesn't work according to some studies
        /// </summary>
        [JsonProperty("houm")]
        [JsonPropertyName("houm")]
        public int? Accuracy { get; set; }

        [JsonProperty("leng")]
        [JsonPropertyName("leng")]
        public int? Range { get; set; }
    }
}
