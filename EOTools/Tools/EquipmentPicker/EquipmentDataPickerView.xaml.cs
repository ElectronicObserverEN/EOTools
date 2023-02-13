using EOTools.Models;
using System.Windows;
using System.Windows.Controls;

namespace EOTools.Tools.EquipmentPicker;
/// <summary>
/// Interaction logic for EquipmentPicker.xaml
/// </summary>
public partial class EquipmentDataPickerView : Window
{
	public EquipmentDataPickerView(EquipmentPickerViewModel viewModel)
	{
		DataContext = viewModel;

        InitializeComponent();

        viewModel.PropertyChanged += ViewModel_PropertyChanged;

        Closed += (_, _) => viewModel.PropertyChanged -= ViewModel_PropertyChanged;
    }

    private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(EquipmentPickerViewModel.PickedEquipment)) DialogResult = true;
    }
}
