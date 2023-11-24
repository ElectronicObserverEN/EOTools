using System.Windows;

namespace EOTools.Translation.Ships.ShipClass
{
    /// <summary>
    /// Interaction logic for UpdateManagerView.xaml
    /// </summary>
    public partial class ShipClassListView : Window
    {
        public ShipClassListView(ShipClassListViewModel viewModel)
        {
            DataContext = viewModel;

            InitializeComponent();

            viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName is nameof(ShipClassListViewModel.PickedClass))
            {
                DialogResult = true;
            }
        }
    }
}
