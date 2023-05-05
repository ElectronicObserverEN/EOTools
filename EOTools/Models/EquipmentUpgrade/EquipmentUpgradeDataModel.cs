using EOTools.Tools;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EOTools.Models.EquipmentUpgrade;

public class EquipmentUpgradeDataModel
{
    [JsonIgnore]
    public int Id { get; set; }

    [JsonProperty("eq_id")]
    public int EquipmentId { get; set; }

    /// <summary>
    /// Improvments possibles for this equipment
    /// </summary>
    [JsonProperty("improvement")]
    public List<EquipmentUpgradeImprovmentModel> Improvement { get; set; } = new List<EquipmentUpgradeImprovmentModel>();

    /// <summary>
    /// This equipment can be converted to those equipments
    /// </summary>
    [JsonProperty("convert_to")]
    [NotMapped]
    public List<EquipmentUpgradeConversionModel> ConvertTo
        => Improvement
            .Where(imp => imp.ConversionData != null)
            .Select(imp => new EquipmentUpgradeConversionModel()
            {
                IdEquipmentAfter = imp.ConversionData.IdEquipmentAfter,
                EquipmentLevelAfter = imp.ConversionData.EquipmentLevelAfter,
            })
            .ToList();

    /// <summary>
    /// This equipment is use in those equipments upgrades
    /// </summary>
    [JsonProperty("upgrade_for")]
    [NotMapped]
    public List<int> UpgradeFor
        => EquipmentUpgradesService.Instance.AllUpgradeModel
            .Where(upg => upg.Improvement.Any(UseEquipmentInUpgrades))
            .Select(upg => upg.EquipmentId)
            .Where(id => id != EquipmentId)
            .ToList();


    private bool UseEquipmentInUpgrades(EquipmentUpgradeImprovmentModel improvment)
    {
        if (UseEquipmentInUpgradesCost(improvment.Costs.Cost0To5)) return true;
        if (UseEquipmentInUpgradesCost(improvment.Costs.Cost6To9)) return true;
        if (improvment.Costs.CostMax is not null && UseEquipmentInUpgradesCost(improvment.Costs.CostMax)) return true;

        return false;
    }

    private bool UseEquipmentInUpgradesCost(EquipmentUpgradeImprovmentCostDetail cost)
    {
        return cost.EquipmentDetail.Any(eq => eq.ItemId == EquipmentId);
    }
}