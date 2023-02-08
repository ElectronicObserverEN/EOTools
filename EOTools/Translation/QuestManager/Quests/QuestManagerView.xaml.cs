using System.Windows.Controls;

namespace EOTools.Translation.QuestManager.Quests
{
    /// <summary>
    /// Interaction logic for QuestManagerView.xaml
    /// </summary>
    public partial class QuestManagerView : Page
    {
        public QuestManagerView()
        {
            DataContext = new QuestManagerViewModel();

            InitializeComponent();
        }
    }
}
