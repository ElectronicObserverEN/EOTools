using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace EOTools.Tools.AssetParser;

public class AssetStructureReader
{
    public AssetStructureModel? ReadAssetStructure(string filePath)
    {
        if (!Path.Exists(filePath)) return null;

        JsonElement parsedJson = JsonSerializer.Deserialize<JsonElement>(File.ReadAllText(filePath));

        AssetMetaDataModel? meta = parsedJson.GetProperty("meta").Deserialize<AssetMetaDataModel>();

        if (meta is null) return null;

        List<AssetFrameModel> frames = new();

        foreach (JsonProperty property in parsedJson.GetProperty("frames").EnumerateObject())
        {
            AssetFrameModel? frame = property.Value.Deserialize<AssetFrameModel>();

            if (frame is not null)
            {
                frame.Name = property.Name;
                frames.Add(frame);
            }
        }

        return new AssetStructureModel()
        {
            MetaData = meta,
            Frames = frames,
        };
    }
}
