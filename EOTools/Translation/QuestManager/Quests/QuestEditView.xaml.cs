using System.Windows;
using System.Windows.Threading;

namespace EOTools.Translation.QuestManager.Quests
{
    /// <summary>
    /// Interaction logic for QuestEditView.xaml
    /// </summary>
    public partial class QuestEditView : Window
    {
        public QuestViewModel ViewModel { get; set; }

        public QuestEditView(QuestViewModel viewModel)
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
