using System.Windows;
using System.Windows.Threading;

namespace EOTools.Translation.EquipmentUpgrade;

/// <summary>
/// Interaction logic for EquipmentUpgradeEditView.xaml
/// </summary>
public partial class EquipmentUpgradeEditView : Window
{
    public EquipmentUpgradeImprovmentViewModel ViewModel { get; set; }

    public EquipmentUpgradeEditView(EquipmentUpgradeImprovmentViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = viewModel;

        // https://github.com/Kinnara/ModernWpf/issues/378
        SourceInitialized += (s, a) =>
        {
            Dispatcher.Invoke(InvalidateVisual, DispatcherPriority.Input);
        };

        InitializeComponent();
    }

    private void OnConfirmClick(object sender, System.Windows.RoutedEventArgs e)
    {
        DialogResult = true;
    }

    private void OnCancelClick(object sender, System.Windows.RoutedEventArgs e)
    {
        DialogResult = false;
    }
}