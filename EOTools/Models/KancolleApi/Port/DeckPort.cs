using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EOTools.Models.KancolleApi.Port;

public class DeckPort
{
    [JsonPropertyName("api_ship")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [Required]
    public List<int> ApiShip { get; set; } = new();
}
