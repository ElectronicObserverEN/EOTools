using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EOTools.Models.KancolleApi.Port;
public class ApiReqHenseiChangeRequest
{
    /// <summary>
    /// Fleet Id
    /// </summary>
    [JsonPropertyName("api_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [Required(AllowEmptyStrings = true)]
    public string ApiId { get; set; } = default!;

    /// <summary>
    /// Ship choosen to go in destination slot
    /// </summary>
    [JsonPropertyName("api_ship_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [Required(AllowEmptyStrings = true)]
    public string ApiShipId { get; set; } = default!;

    /// <summary>
    /// Position in the fleet of destination slot
    /// </summary>
    [JsonPropertyName("api_ship_idx")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [Required(AllowEmptyStrings = true)]
    public string ApiShipIdx { get; set; } = default!;

    [JsonPropertyName("api_verno")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [Required(AllowEmptyStrings = true)]
    public string ApiVerno { get; set; } = default!;
}
