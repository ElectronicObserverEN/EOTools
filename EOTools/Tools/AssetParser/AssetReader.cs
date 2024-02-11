using System.Collections.Generic;
using System.IO;

namespace EOTools.Tools.AssetParser;

public class AssetReader(AssetStructureReader structureReader)
{
    /// <summary>
    /// Read assets from files
    /// </summary>
    /// <param name="jsonPath">Path of the json file</param>
    /// <returns></returns>
    public AssetViewModel? ReadAsset(string jsonPath)
    {
        string imagePath = Path.GetDirectoryName(jsonPath) switch
        {
            { } dir => Path.Combine(dir, Path.GetFileNameWithoutExtension(jsonPath) + ".png"),
            _ => ""
        };

        return ReadAsset(jsonPath, imagePath);
    }

    /// <summary>
    /// Read assets from files
    /// </summary>
    /// <param name="jsonPath">Path of the json file</param>
    /// <param name="imagePath">Path of the image file</param>
    /// <returns></returns>
    private AssetViewModel? ReadAsset(string jsonPath, string imagePath)
    {
        AssetStructureModel? structure = structureReader.ReadAssetStructure(jsonPath);

        if (structure is null) return null;

        return ReadAsset(imagePath, structure);
    }

    /// <summary>
    /// Read assets from files
    /// </summary>
    /// <param name="path">Path of the image</param>
    /// <param name="structure"></param>
    /// <returns></returns>
    private AssetViewModel ReadAsset(string path, AssetStructureModel structure)
    {
        List<AssetPartViewModel> parts = new();

        foreach (AssetFrameModel frame in structure.Frames)
        {
            AssetPartViewModel part = new()
            {
                FrameData = frame,
                SourcePath = path,
            };

            part.Load();

            parts.Add(part);
        }

        return new AssetViewModel()
        {
            Parts = parts,
        };
    }
}
