using System.Windows;
using System.Windows.Threading;

namespace EOTools.Translation.QuestManager.Events
{
    /// <summary>
    /// Interaction logic for EventEditView.xaml
    /// </summary>
    public partial class EventEditView : Window
    {
        public EventViewModel ViewModel { get; set; }

        public EventEditView(EventViewModel viewModel)
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
