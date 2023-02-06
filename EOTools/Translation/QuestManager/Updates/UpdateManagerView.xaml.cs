using System.Windows.Controls;

namespace EOTools.Translation.QuestManager.Updates
{
    /// <summary>
    /// Interaction logic for UpdateManagerView.xaml
    /// </summary>
    public partial class UpdateManagerView : Page
    {
        public UpdateManagerView()
        {
            DataContext = new UpdateManagerViewModel();

            InitializeComponent();
        }
    }
}
