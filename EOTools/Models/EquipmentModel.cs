using Newtonsoft.Json;

namespace EOTools.Models;

public class EquipmentModel
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("api_id")]
    public int ApiId { get; set; }

    [JsonProperty("name_jp")]
    public string NameJP { get; set; } = "";

    [JsonProperty("name_en")]
    public string NameEN { get; set; } = "";

    /// <summary>
    /// Stored as json for now
    /// </summary>
    [JsonProperty("upgrade_data")]
    public string? UpgradeData { get; set; }
}
