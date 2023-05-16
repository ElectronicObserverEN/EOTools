using System.Text.Json.Serialization;

namespace EOTools.Models.KancolleApi.Port;

public class ApiShip
{
    [JsonPropertyName("api_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public int ApiId { get; set; } = default!;

    [JsonPropertyName("api_ship_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public int ApiShipId { get; set; } = default!;
}
