using System.Windows;

namespace EOTools.Tools.AssetParser;

public partial class AssetViewerView : Window
{
    public AssetViewerView(AssetViewModel viewModel)
    {
        DataContext = viewModel;

        InitializeComponent();
    }
}
