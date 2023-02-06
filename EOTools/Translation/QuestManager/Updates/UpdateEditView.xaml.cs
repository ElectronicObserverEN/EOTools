using System.Windows;
using System.Windows.Threading;

namespace EOTools.Translation.QuestManager.Updates
{
    /// <summary>
    /// Interaction logic for UpdateEditView.xaml
    /// </summary>
    public partial class UpdateEditView : Window
    {
        public UpdateViewModel ViewModel { get; set; }

        public UpdateEditView(UpdateViewModel viewModel)
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
