using EOTools.Models.FitBonus;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EOTools.Translation.FitBonus.FitBonusChecker;

public record FitBonusIssueModel
{
    [JsonPropertyName("id")] public int Id { get; set; }

    [JsonPropertyName("software_version")] public string SoftwareVersion { get; set; } = "";

    [JsonPropertyName("data_version")] public int DataVersion { get; set; }

    [JsonPropertyName("expected")] public required FitBonusValueModel ExpectedBonus { get; set; }

    [JsonPropertyName("actual")] public required FitBonusValueModel ActualBonus { get; set; }

    [JsonPropertyName("ship")] public required ShipWithStatsModel Ship { get; set; }

    [JsonPropertyName("equipments")] public required List<EquipmentWithStatsModel> Equipments { get; set; }
}