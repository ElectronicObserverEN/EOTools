using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOTools.DataBase;
using EOTools.Models.Ships;
using EOTools.Translation.Ships.ShipList;
using System.Collections.Generic;
using System.Linq;

namespace EOTools.Translation.Equipments.UpgradeChecker;

public partial class UpgradeCheckerViewModel : ObservableObject
{
    [ObservableProperty]
    private List<TooManyUpgradePerShipViewModel> tooManyUpgradePerShipList = new();

    [ObservableProperty]
    private TooManyUpgradePerShipViewModel? selectedIssue;

    [ObservableProperty]
    private ShipModel? selectedShip;

    public string SelectedShipString => SelectedShip?.NameEN ?? "Select a ship";

    private List<UpgradeDataPerShipViewModel> UpgradesPerShip { get; set; }

    public List<UpgradeDataPerDayAndShipViewModel> UpgradesForSelectedShip => SelectedShip switch
    {
        ShipModel ship => GetUpgradesPerShip(ship),
        _ => new()
    };

    public UpgradeCheckerViewModel()
    {
        using EOToolsDbContext db = new();
        UpgradesPerShip = db.Ships.Select(ship => new UpgradeDataPerShipViewModel(ship)).ToList();

        LoadUpgradeIssuesView();

        PropertyChanged += UpgradeCheckerViewModel_PropertyChanged;
        PropertyChanged += UpgradeCheckerViewModel_PropertyChanged2;
    }

    private void UpgradeCheckerViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName is not nameof(SelectedIssue)) return;

        SelectedShip = SelectedIssue.Ship;
    }

    private void UpgradeCheckerViewModel_PropertyChanged2(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName is not nameof(SelectedShip)) return;

        OnPropertyChanged(nameof(UpgradesForSelectedShip));
        OnPropertyChanged(nameof(SelectedShipString)); 
    }

    private void LoadUpgradeIssuesView()
    {
        TooManyUpgradePerShipList = UpgradesPerShip
            .Where(upg => upg.Days.Any(day => day.Improvments.Count > 3))
            .Select(upg => new TooManyUpgradePerShipViewModel(upg.ShipModel))
            .ToList();
    }

    private List<UpgradeDataPerDayAndShipViewModel> GetUpgradesPerShip(ShipModel ship)
    {
        UpgradeDataPerShipViewModel? shipUpgrades = UpgradesPerShip
           .FirstOrDefault(upg => upg.ShipModel.ApiId == ship.ApiId);

        if (shipUpgrades is null) return new();

        return shipUpgrades.Days.Select(day => new UpgradeDataPerDayAndShipViewModel(day.Day, ship, day.Improvments)).ToList();
    }

    [RelayCommand]
    private void OpenShipSelection()
    {
        ShipListViewModel vm = new();
        ShipListView view = new(vm);

        if (view.ShowDialog() is true)
        {
            SelectedShip = vm.PickedShip;
        }
    }
}
