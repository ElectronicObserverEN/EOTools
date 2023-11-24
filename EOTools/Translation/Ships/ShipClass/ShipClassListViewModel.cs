using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOTools.DataBase;
using EOTools.Models.Ships;
using System.Collections.Generic;
using System.Linq;

namespace EOTools.Translation.Ships.ShipClass;

public partial class ShipClassListViewModel : ObservableObject
{
    private List<ShipClassViewModel> ClassList { get; set; }

    [ObservableProperty]
    private List<ShipClassViewModel> _shipListFiltered = new();

    [ObservableProperty]
    private string _filter = "";

    public ShipClassViewModel? SelectedClass { get; set; }
    public ShipClassModel? PickedClass { get; set; }

    public ShipClassListViewModel()
    {
        using EOToolsDbContext db = new();

        ClassList = new(db.ShipClass
            .Select(ship => new ShipClassViewModel(ship))
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
        ShipListFiltered = ClassList
            .Where(ship => string.IsNullOrEmpty(Filter) || ship.NameEnglish.ToUpperInvariant().Contains(Filter.ToUpperInvariant()))
            .OrderBy(ship => ship.ApiId)
            .ToList();
    }

    [RelayCommand]
    public void SelectShip()
    {
        PickedClass = SelectedClass?.Model;
        OnPropertyChanged(nameof(PickedClass));
    }
}
