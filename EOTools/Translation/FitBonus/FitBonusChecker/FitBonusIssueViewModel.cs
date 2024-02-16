using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using EOTools.DataBase;
using EOTools.Models.Ships;
using System.Linq;
using EOTools.Control.Grid;
using EOTools.Models;
using EOTools.Models.FitBonus;

namespace EOTools.Translation.FitBonus.FitBonusChecker;

public partial class FitBonusIssueViewModel : ObservableObject, IGridRowFetched
{
    private FitBonusIssueModel Model { get; }

    private EOToolsDbContext Database { get; }

    [ObservableProperty] 
    private ShipModel _ship = new();

    [ObservableProperty]
    private List<EquipmentModel> _equipments = new();

    [ObservableProperty]
    private FitBonusValueModel _expectedValue = new();

    [ObservableProperty]
    private FitBonusValueModel _actualValue = new ();

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
            .Select(eq => Database.Equipments.FirstOrDefault(eqDb => eqDb.ApiId == eq.EquipmentId) ?? new())
            .ToList();

        ExpectedValue = Model.ExpectedBonus;
        ActualValue = Model.ActualBonus;
    }
}
