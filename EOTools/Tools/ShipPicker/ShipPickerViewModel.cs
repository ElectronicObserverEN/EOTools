using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.DependencyInjection;
using EOTools.DataBase;
using EOTools.Models.Ships;

namespace EOTools.Tools.ShipPicker;

public partial class ShipPickerViewModel : ObservableObject
{
	[ObservableProperty]
	private string _nameFilter = "";

    private List<ShipModel> AllShips { get; }

    [ObservableProperty]
    private List<ShipModel> _shipsFiltered = new();

	public ShipModel? SelectedShip { get; set; }
	public ShipModel? PickedShip { get; set; }

    public ShipPickerViewModel()
    {
        AllShips = Ioc.Default.GetRequiredService<EOToolsDbContext>().Ships.ToList();

        RefreshList();

        PropertyChanged += ShipPickerViewModel_PropertyChanged;
    }

    public ShipPickerViewModel(List<ShipModel> allShips)
    {
        AllShips = allShips;

        RefreshList();

        PropertyChanged += ShipPickerViewModel_PropertyChanged;
    }

    private void ShipPickerViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(NameFilter)) return;
        RefreshList();
    }

    private void RefreshList()
        => ShipsFiltered = new(AllShips.Where(eq => string.IsNullOrEmpty(NameFilter) || eq.NameEN.ToUpper().Contains(NameFilter.ToUpper())).OrderBy(s => s.ApiId));

    [RelayCommand]
    public void SelectShip()
    {
        PickedShip = SelectedShip;
        OnPropertyChanged(nameof(PickedShip));
    }
}
