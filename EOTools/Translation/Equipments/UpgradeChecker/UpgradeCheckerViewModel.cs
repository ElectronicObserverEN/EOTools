using CommunityToolkit.Mvvm.ComponentModel;
using EOTools.DataBase;
using EOTools.Models.Ships;
using System.Collections.Generic;
using System.Linq;

namespace EOTools.Translation.Equipments.UpgradeChecker;

public partial class UpgradeCheckerViewModel : ObservableObject
{
    [ObservableProperty]
    private List<TooManyUpgradePerShipViewModel> tooManyUpgradePerShipList = new();

    [ObservableProperty]
    private TooManyUpgradePerShipViewModel? selectedIssue;

    private List<UpgradeDataPerShipViewModel> UpgradesPerShip { get; set; }

    public List<UpgradeDataPerDayAndShipViewModel> UpgradesForSelectedShip => SelectedIssue switch
    {
        TooManyUpgradePerShipViewModel issue => GetUpgradesPerShip(issue.Ship),
        _ => new()
    };

    public UpgradeCheckerViewModel()
    {
        using EOToolsDbContext db = new();
        UpgradesPerShip = db.Ships.Select(ship => new UpgradeDataPerShipViewModel(ship)).ToList();

        LoadUpgradeIssuesView();

        PropertyChanged += UpgradeCheckerViewModel_PropertyChanged;
    }

    private void UpgradeCheckerViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName is not nameof(SelectedIssue)) return;

        OnPropertyChanged(nameof(UpgradesForSelectedShip));
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
}
