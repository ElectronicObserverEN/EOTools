using System.Windows.Controls;

namespace EOTools.Translation.QuestManager.Seasons
{
    /// <summary>
    /// Interaction logic for SeasonManagerView.xaml
    /// </summary>
    public partial class SeasonManagerView : Page
    {
        public SeasonManagerView()
        {
            DataContext = new SeasonManagerViewModel();

            InitializeComponent();
        }
    }
}
