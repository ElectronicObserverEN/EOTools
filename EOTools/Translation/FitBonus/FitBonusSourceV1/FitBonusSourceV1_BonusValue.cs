using System.Text.Json.Serialization;

namespace EOTools.Translation.FitBonus.FitBonusSourceV1;

public class FitBonusSourceV1_BonusValue
{
    [JsonPropertyName("saku")]
    public int? Saku { get; set; }

    [JsonPropertyName("houg")]
    public int? Houg { get; set; }

    [JsonPropertyName("tais")]
    public int? Tais { get; set; }

    [JsonPropertyName("kaih")]
    public int? Kaih { get; set; }

    [JsonPropertyName("tyku")]
    public int? Tyku { get; set; }

    [JsonPropertyName("raig")]
    public int? Raig { get; set; }

    [JsonPropertyName("souk")]
    public int? Souk { get; set; }

    [JsonPropertyName("houm")]
    public int? Houm { get; set; }

    [JsonPropertyName("leng")]
    public int? Leng { get; set; }

    [JsonPropertyName("baku")]
    public int? Baku { get; set; }
}