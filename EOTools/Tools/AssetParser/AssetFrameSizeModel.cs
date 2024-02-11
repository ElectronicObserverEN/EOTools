using System.Text.Json.Serialization;

namespace EOTools.Tools.AssetParser;

public class AssetFrameSizeModel : AssetSizeModel
{
    [JsonPropertyName("x")]
    public required int PositionX { get; set; }

    [JsonPropertyName("y")]
    public required int PositionY { get; set; }
}