using System;
using System.Text.Json.Serialization;

namespace EOTools.Translation.FitBonus.FitBonusChecker;

public record EquipmentWithStatsModel
{
    [JsonPropertyName("equipmentId")] public int EquipmentId { get; set; }

    [JsonPropertyName("level")] public int Level { get; set; }

    public virtual bool Equals(EquipmentWithStatsModel? other)
    {
        if (other is null) return false;

        if (other.EquipmentId != EquipmentId) return false;
        if (other.Level != Level) return false;

        return true;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine((int)EquipmentId, (int)Level);
    }
}
