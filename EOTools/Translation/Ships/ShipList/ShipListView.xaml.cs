using System.Windows;

namespace EOTools.Translation.Ships.ShipList
{
    /// <summary>
    /// Interaction logic for UpdateManagerView.xaml
    /// </summary>
    public partial class ShipListView : Window
    {
        public ShipListView(ShipListViewModel viewModel)
        {
            DataContext = viewModel;

            InitializeComponent();

            viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName is nameof(ShipListViewModel.PickedShip))
            {
                DialogResult = true;
            }
        }
    }
}
