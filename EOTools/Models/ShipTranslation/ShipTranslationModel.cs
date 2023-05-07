using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EOTools.Models.ShipTranslation;

public class ShipTranslationModel
{
    [JsonPropertyName("version")]
    public string Version { get; set; } = "";

    [JsonPropertyName("ship")]
    public Dictionary<string, string> Ships { get; set; } = new();

    [JsonPropertyName("class")]
    public Dictionary<string, string> Classes { get; set; } = new();

    [JsonPropertyName("suffix")]
    public Dictionary<string, string> Suffixes { get; set; } = new();

    [JsonPropertyName("stype")]
    public Dictionary<string, string> Types { get; set; } = new();
}
