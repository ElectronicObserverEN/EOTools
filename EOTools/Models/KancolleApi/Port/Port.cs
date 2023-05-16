using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EOTools.Models.KancolleApi.Port
{
    public class Port
    {
        [JsonPropertyName("api_deck_port")]
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        [Required]
        public List<DeckPort> ApiDeckPort { get; set; } = new();

        [JsonPropertyName("api_ship")]
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        [Required]
        public List<ApiShip> ApiShip { get; set; } = new();
    }
}
