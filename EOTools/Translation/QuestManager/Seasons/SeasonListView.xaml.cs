using System.Windows;

namespace EOTools.Translation.QuestManager.Seasons
{
    /// <summary>
    /// Interaction logic for SeasonManagerView.xaml
    /// </summary>
    public partial class SeasonListView : Window
    {
        public SeasonListView(SeasonListViewModel viewModel)
        {
            DataContext = viewModel;

            InitializeComponent();
        }

        private void ListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            (DataContext as SeasonListViewModel).PickedSeason = (DataContext as SeasonListViewModel).SelectedSeason?.Model;
            DialogResult = true;
        }
    }
}
