using CommunityToolkit.Mvvm.ComponentModel;
using EOTools.DataBase;
using EOTools.Models.Ships;
using System.Linq;
using EOTools.Control.Grid;

namespace EOTools.Translation.FitBonus.FitBonusChecker;

public partial class FitBonusIssueViewModel : ObservableObject, IGridRowFetched
{
    private FitBonusIssueModel Model { get; }

    private EOToolsDbContext Database { get; }

    [ObservableProperty] 
    private ShipModel _ship = new();

    public FitBonusIssueViewModel(FitBonusIssueModel model, EOToolsDbContext db)
    {
        Model = model;
        Database = db;

        LoadFromModel();
    }

    private void LoadFromModel()
    {
        Ship = Database.Ships
            .FirstOrDefault(ship => ship.ApiId == Model.Ship.ShipId) ?? new ShipModel();
    }
}
