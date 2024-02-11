using System.Text.Json.Serialization;

namespace EOTools.Tools.AssetParser;

public class AssetMetaDataModel
{
    [JsonPropertyName("app")]
    public required string App { get; set; }

    [JsonPropertyName("image")]
    public required string Image { get; set; }

    [JsonPropertyName("format")]
    public required string Format { get; set; }

    [JsonPropertyName("size")]
    public required AssetSizeModel Size { get; set; }

    [JsonPropertyName("scale")]
    public required int Scale { get; set; }
}

