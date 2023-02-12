using System.Windows;
using System.Windows.Threading;

namespace EOTools.Translation.Equipments
{
    /// <summary>
    /// Interaction logic for EquipmentEditView.xaml
    /// </summary>
    public partial class EquipmentEditView : Window
    {
        public EquipmentViewModel ViewModel { get; set; }

        public EquipmentEditView(EquipmentViewModel viewModel)
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
