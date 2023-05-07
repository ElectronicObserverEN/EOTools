using EOTools.Translation.Equipments;
using System.Windows;
using System.Windows.Threading;

namespace EOTools.Translation.Ships
{
    /// <summary>
    /// Interaction logic for ShipView.xaml
    /// </summary>
    public partial class ShipEditView : Window
    {
        public ShipViewModel ViewModel { get; set; }

        public ShipEditView(ShipViewModel viewModel)
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
}
