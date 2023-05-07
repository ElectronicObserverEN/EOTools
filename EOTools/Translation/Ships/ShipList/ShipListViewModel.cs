using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOTools.DataBase;
using EOTools.Models.Ships;
using System.Collections.Generic;
using System.Linq;

namespace EOTools.Translation.Ships.ShipList;

public partial class ShipListViewModel : ObservableObject
{
    private List<ShipViewModel> ShipList { get; set; }

    [ObservableProperty]
    private List<ShipViewModel> shipListFiltered = new();

    [ObservableProperty]
    public string filter = "";

    public ShipViewModel? SelectedShip { get; set; }
    public ShipModel? PickedShip { get; set; }

    public ShipListViewModel()
    {
        // Load updates
        using EOToolsDbContext db = new();

        ShipList = new(db.Ships
            .Select(ship => new ShipViewModel(ship))
            .ToList());

        RefreshList();

        PropertyChanged += ShipListViewModel_PropertyChanged;
    }

    private void ShipListViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName is not nameof(Filter)) return;

        RefreshList();
    }

    private void RefreshList()
    {
        ShipListFiltered = ShipList
            .Where(ship => ship.IsFriendly)
            .Where(ship => string.IsNullOrEmpty(Filter) || ship.NameEN.ToUpperInvariant().Contains(Filter.ToUpperInvariant()))
            .OrderBy(ship => ship.ApiId)
            .ToList();
    }

    [RelayCommand]
    public void SelectShip()
    {
        PickedShip = SelectedShip?.Model;
        OnPropertyChanged(nameof(PickedShip));
    }
}
