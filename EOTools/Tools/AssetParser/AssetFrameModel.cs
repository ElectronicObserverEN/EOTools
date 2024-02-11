using System.Text.Json.Serialization;

namespace EOTools.Tools.AssetParser;

public class AssetFrameModel
{
    [JsonPropertyName("frame")]
    public required AssetFrameSizeModel FrameDimensions { get; set; }

    [JsonPropertyName("rotated")]
    public required bool Rotated { get; set; }

    [JsonPropertyName("trimmed")]
    public required bool Trimmed { get; set; }

    [JsonPropertyName("spriteSourceSize")]
    public required AssetFrameSizeModel SpriteSourceSize { get; set; }

    [JsonPropertyName("sourceSize")]
    public required AssetSizeModel SourceSize { get; set; }

    public string Name { get; set; } = "";
}