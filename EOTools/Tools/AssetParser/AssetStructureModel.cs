using System.Collections.Generic;

namespace EOTools.Tools.AssetParser;

public class AssetStructureModel
{
    public required AssetMetaDataModel MetaData { get; set; }

    public required List<AssetFrameModel> Frames { get; set; }
}
