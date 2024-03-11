using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EOTools.Translation.FitBonus.FitBonusSourceV1;

public class FitBonusSourceV1
{
    [JsonPropertyName("types")]
    public List<int>? Types { get; set; }

    [JsonPropertyName("bonuses")] 
    public List<FitBonusSourceV1_FitBonus> Bonuses { get; set; } = new();

    [JsonPropertyName("ids")]
    public List<int>? Ids { get; set; }
}