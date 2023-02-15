using System.Windows;

namespace EOTools.Translation.QuestManager.Quests
{
    /// <summary>
    /// Interaction logic for QuestManagerView.xaml
    /// </summary>
    public partial class QuestManagerWindowView : Window
    {
        public QuestManagerWindowView(QuestManagerViewModel vm)
        {
            DataContext = vm;

            InitializeComponent();
        }
    }
}
