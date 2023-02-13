using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOTools.Models;

namespace EOTools.Tools.EquipmentPicker;

public partial class EquipmentPickerViewModel : ObservableObject
{
	[ObservableProperty]
	private string nameFilter = "";


    private List<EquipmentModel> AllEquipments { get; }

    [ObservableProperty]
    private List<EquipmentModel> equipmentsFiltered = new();

	public EquipmentModel? SelectedEquipment { get; set; }
	public EquipmentModel? PickedEquipment { get; set; }

    public EquipmentPickerViewModel(List<EquipmentModel> allEquips)
	{
        AllEquipments = allEquips;

        RefreshList();

        PropertyChanged += EquipmentPickerViewModel_PropertyChanged;
	}

    private void EquipmentPickerViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(NameFilter)) return;
        RefreshList();
    }

    private void RefreshList()
        => EquipmentsFiltered = new(AllEquipments.Where(eq => string.IsNullOrEmpty(NameFilter) || eq.NameEN.ToUpper().Contains(NameFilter.ToUpper())).OrderBy(s => s.ApiId));


    [RelayCommand]
    public void SelectEquipment()
    {
        PickedEquipment = SelectedEquipment;
        OnPropertyChanged(nameof(PickedEquipment));
    }

}
