using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EOTools.ElectronicObserverApi.Models;

public record UserReportedEquipmentUpgradeIssueModel
{
    [JsonPropertyName("software_version")] public string SoftwareVersion { get; set; } = "";

    [JsonPropertyName("data_version")] public int DataVersion { get; set; }
    
    [JsonPropertyName("expected")] public List<int> ExpectedUpgrades { get; set; } = new();

    [JsonPropertyName("actual")] public List<int> ActualUpgrades { get; set; } = new();

    [JsonPropertyName("day")] public DayOfWeek Day { get; set; }

    [JsonPropertyName("helperId")] public int HelperId { get; set; }

    [JsonPropertyName("state")] public IssueState IssueState { get; set; }

    [JsonIgnore] public int Id { get; set; }

    [JsonIgnore] public DateTime AddedOn { get; set; }

}