using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EOTools.Translation.FitBonus.FitBonusSourceV1;

public class FitBonusSourceV1_FitBonus
{
    [JsonPropertyName("bonus")]
    public FitBonusSourceV1_BonusValue Bonus { get; set; }

    [JsonPropertyName("shipType")]
    public List<int>? ShipType { get; set; }

    [JsonPropertyName("num")]
    public int? Num { get; set; }

    [JsonPropertyName("level")]
    public int? Level { get; set; }

    [JsonPropertyName("shipId")]
    public List<int>? ShipId { get; set; }

    [JsonPropertyName("requiresAR")]
    public int? RequiresAR { get; set; }

    [JsonPropertyName("shipBase")]
    public List<int>? ShipBase { get; set; }

    [JsonPropertyName("requiresId")]
    public List<int>? RequiresId { get; set; }

    [JsonPropertyName("shipClass")]
    public List<int>? ShipClass { get; set; }

    [JsonPropertyName("requiresSR")]
    public int? RequiresSR { get; set; }

    [JsonPropertyName("requiresIdNum")]
    public int? RequiresIdNum { get; set; }

    [JsonPropertyName("shipCountry")]
    public List<string>? ShipCountry { get; set; }

    [JsonPropertyName("requiresType")]
    public List<int>? RequiresType { get; set; }

    [JsonPropertyName("requiresIdLevel")]
    public int? RequiresIdLevel { get; set; }

    [JsonPropertyName("requiresAccR")]
    public int? RequiresAccR { get; set; }
}