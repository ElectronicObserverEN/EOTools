using System.Windows;
using System.Windows.Threading;

namespace EOTools.Translation.QuestManager.Seasons
{
    /// <summary>
    /// Interaction logic for SeasonEditView.xaml
    /// </summary>
    public partial class SeasonEditView : Window
    {
        public SeasonViewModel ViewModel { get; set; }

        public SeasonEditView(SeasonViewModel viewModel)
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
