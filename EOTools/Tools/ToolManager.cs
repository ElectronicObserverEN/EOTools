using System.IO;
using System.Threading.Tasks;
using EOTools.Tools.AssetParser;
using Microsoft.Win32;

namespace EOTools.Tools;
public class ToolManager(AssetReader assetReader)
{
    public async Task OpenAssetViewer()
    {
        OpenFileDialog dialog = new()
        {
            Filter = "Json files (.json)|*.json",
            DefaultDirectory = Path.Combine(AppSettings.KancolleEOAPIFolder, "kcs2"),
        };

        // Show open file dialog box
        bool? result = dialog.ShowDialog();

        if (result is not true)
        {
            return;
        }

        if (dialog.CheckFileExists)
        {
            AssetViewModel? asset = assetReader.ReadAsset(dialog.FileName);

            if (asset is null)
            {
                App.ShowErrorMessage("Failed reading asset files");
                return;
            }

            AssetViewerView view = new(asset);
            view.Show();
        }
    }
}
