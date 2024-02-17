using CommunityToolkit.Mvvm.ComponentModel;
using EOTools.Control.Grid;
using EOTools.DataBase;
using EOTools.Models.FitBonus;
using EOTools.Models.Ships;
using System.Collections.Generic;
using System.Linq;

namespace EOTools.Translation.FitBonus.FitBonusChecker;

public partial class FitBonusIssueViewModel : ObservableObject, IGridRowFetched
{
    private FitBonusIssueModel Model { get; }

    private EOToolsDbContext Database { get; }

    [ObservableProperty]
    private ShipModel _ship = new();

    [ObservableProperty]
    private List<EquipmentWithStatsViewModel> _equipments = new();

    [ObservableProperty]
    private FitBonusValueModel _expectedValue = new();

    [ObservableProperty]
    private FitBonusValueModel _actualValue = new();

    public int Id => Model.Id;

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

        Equipments = Model.Equipments
            .Select(eq => new EquipmentWithStatsViewModel(eq, Database))
            .ToList();

        ExpectedValue = Model.ExpectedBonus;
        ActualValue = Model.ActualBonus;
    }
}