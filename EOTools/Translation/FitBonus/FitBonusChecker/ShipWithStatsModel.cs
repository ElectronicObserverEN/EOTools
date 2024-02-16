using System.Text.Json.Serialization;

namespace EOTools.Translation.FitBonus.FitBonusChecker;

public record ShipWithStatsModel
{
    [JsonPropertyName("shipId")] public int ShipId { get; set; }

    [JsonPropertyName("level")] public int Level { get; set; }

    [JsonPropertyName("firepower")] public int Firepower { get; set; }

    [JsonPropertyName("torpedo")] public int Torpedo { get; set; }

    [JsonPropertyName("antiAir")] public int AntiAir { get; set; }

    [JsonPropertyName("armor")] public int Armor { get; set; }

    [JsonPropertyName("evasion")] public int Evasion { get; set; }
    [JsonPropertyName("evasionKnown")] public bool EvasionDetermined { get; set; }

    [JsonPropertyName("asw")] public int ASW { get; set; }
    [JsonPropertyName("aswKnown")] public bool ASWDetermined { get; set; }

    [JsonPropertyName("los")] public int LOS { get; set; }
    [JsonPropertyName("losKnown")] public bool LOSDetermined { get; set; }

    [JsonPropertyName("accuracy")] public int Accuracy { get; set; }

    [JsonPropertyName("range")] public int Range { get; set; }

    public bool IsSameShip(ShipWithStatsModel? otherModel)
    {
        if (otherModel is null) return false;

        if (ShipId != otherModel.ShipId) return false;
        if (Level != otherModel.Level) return false;

        return true;
    }
}
