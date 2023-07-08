using EOTools.DataBase;
using EOTools.Models.EquipmentUpgrade;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace EOTools.Tools;

public class EquipmentUpgradesService
{
    private static EquipmentUpgradesService? instance;
    public static EquipmentUpgradesService Instance
    {
        get
        {
            if (instance is null) instance = new();
            return instance;
        }
    }

    public List<EquipmentUpgradeDataModel> AllUpgradeModel { get; set; }

    private EquipmentUpgradesService()
    {
        ReloadList();
    }

    public EOToolsDbContext DbContext { get; set; } = new();

    public void ReloadList()
    {
        AllUpgradeModel = DbContext.EquipmentUpgrades
            .Include("Improvement.ConversionData")
            .Include("Improvement.Helpers.CanHelpOnDays")
            .Include("Improvement.Helpers.ShipIds")
            .Include("Improvement.Costs.Cost0To5.ConsumableDetail")
            .Include("Improvement.Costs.Cost0To5.EquipmentDetail")
            .Include("Improvement.Costs.Cost6To9.ConsumableDetail")
            .Include("Improvement.Costs.Cost6To9.EquipmentDetail")
            .Include("Improvement.Costs.CostMax.ConsumableDetail")
            .Include("Improvement.Costs.CostMax.EquipmentDetail")
            //.SelectMany(equ => equ.Improvement)
            //.Select(upg => new EquipmentUpgradeImprovmentViewModel(upg))
            .ToList();
    }
}
