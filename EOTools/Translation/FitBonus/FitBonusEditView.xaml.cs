using System.Windows.Threading;

namespace EOTools.Translation.FitBonus
{
    /// <summary>
    /// Interaction logic for FitBonusView.xaml
    /// </summary>
    public partial class FitBonusEditView
    {
        public FitBonusPerEquipmentViewModel ViewModel { get; set; }

        public FitBonusEditView(FitBonusPerEquipmentViewModel viewModel)
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
