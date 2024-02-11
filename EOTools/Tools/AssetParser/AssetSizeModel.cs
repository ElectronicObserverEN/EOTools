using System.Text.Json.Serialization;

namespace EOTools.Tools.AssetParser;

public class AssetSizeModel
{
    [JsonPropertyName("w")]
    public required int Width { get; set; }

    [JsonPropertyName("h")]
    public required int Height { get; set; }
}