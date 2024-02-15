using Newtonsoft.Json;

namespace EOTools.Translation.FitBonus.FitBonusChecker;

public record ShipWithStatsModel
{
    [JsonProperty("shipId")] public int ShipId { get; set; }

    [JsonProperty("level")] public int Level { get; set; }

    [JsonProperty("firepower")] public int Firepower { get; set; }

    [JsonProperty("torpedo")] public int Torpedo { get; set; }

    [JsonProperty("antiAir")] public int AntiAir { get; set; }

    [JsonProperty("armor")] public int Armor { get; set; }

    [JsonProperty("evasion")] public int Evasion { get; set; }
    [JsonProperty("evasionKnown")] public bool EvasionDetermined { get; set; }

    [JsonProperty("asw")] public int ASW { get; set; }
    [JsonProperty("aswKnown")] public bool ASWDetermined { get; set; }

    [JsonProperty("los")] public int LOS { get; set; }
    [JsonProperty("losKnown")] public bool LOSDetermined { get; set; }

    [JsonProperty("accuracy")] public int Accuracy { get; set; }

    [JsonProperty("range")] public int Range { get; set; }

    public bool IsSameShip(ShipWithStatsModel? otherModel)
    {
        if (otherModel is null) return false;

        if (ShipId != otherModel.ShipId) return false;
        if (Level != otherModel.Level) return false;

        return true;
    }
}
