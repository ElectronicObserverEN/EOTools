using System;
using System.Text.Json.Serialization;

namespace EOTools.Translation.QuestManager.Updates;

public class UpdateModel
{

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("start_date")]
    public DateTime UpdateDate { get; set; } = DateTime.Now;

    [JsonPropertyName("start_time")]
    public TimeSpan UpdateStartTime { get; set; } = TimeSpan.Zero;

    [JsonPropertyName("end_time")]
    public TimeSpan UpdateEndTime { get; set; } = TimeSpan.Zero;

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("description")]
    public string Description { get; set; } = "";

    [JsonPropertyName("live_update")]
    public bool WasLiveUpdate { get; set; } = false;
}
