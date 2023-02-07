using System.Windows.Controls;

namespace EOTools.Translation.QuestManager.Events
{
    /// <summary>
    /// Interaction logic for EventManagerView.xaml
    /// </summary>
    public partial class EventManagerView : Page
    {
        public EventManagerView()
        {
            DataContext = new EventManagerViewModel();

            InitializeComponent();
        }
    }
}
