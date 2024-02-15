using EOTools.Models.FitBonus;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace EOTools.Translation.FitBonus.FitBonusChecker;

public record FitBonusIssueModel
{
    [JsonProperty("software_version")] public string SoftwareVersion { get; set; } = "";

    [JsonProperty("data_version")] public int DataVersion { get; set; }

    [JsonProperty("expected")] public FitBonusValueModel ExpectedBonus { get; set; } = new();

    [JsonProperty("actual")] public FitBonusValueModel ActualBonus { get; set; } = new();

    [JsonProperty("ship")] public ShipWithStatsModel Ship { get; set; } = new();

    [JsonProperty("equipments")] public List<EquipmentWithStatsModel> Equipments { get; set; } = new();
}