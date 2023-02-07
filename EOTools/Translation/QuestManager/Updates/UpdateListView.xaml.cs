using System.Windows;

namespace EOTools.Translation.QuestManager.Updates
{
    /// <summary>
    /// Interaction logic for UpdateManagerView.xaml
    /// </summary>
    public partial class UpdateListView : Window
    {
        public UpdateListView(UpdateListViewModel viewModel)
        {
            DataContext = viewModel;

            InitializeComponent();
        }

        private void ListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            (DataContext as UpdateListViewModel).PickedUpdate = (DataContext as UpdateListViewModel).SelectedUpdate?.Model;
            DialogResult = true;
        }
    }
}
