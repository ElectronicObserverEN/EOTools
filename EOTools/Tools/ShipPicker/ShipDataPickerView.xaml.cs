using System.Windows;

namespace EOTools.Tools.ShipPicker;

/// <summary>
/// Interaction logic for ShipPicker.xaml
/// </summary>
public partial class ShipDataPickerView : Window
{
	public ShipDataPickerView(ShipPickerViewModel viewModel)
	{
		DataContext = viewModel;

        InitializeComponent();

        viewModel.PropertyChanged += ViewModel_PropertyChanged;

        Closed += (_, _) => viewModel.PropertyChanged -= ViewModel_PropertyChanged;
    }

    private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ShipPickerViewModel.PickedShip)) DialogResult = true;
    }
}
