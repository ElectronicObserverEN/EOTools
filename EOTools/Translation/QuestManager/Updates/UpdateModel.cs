using Newtonsoft.Json;
using System;

namespace EOTools.Translation.QuestManager.Updates;

public class UpdateModel
{

    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("start_date")]
    public DateTime UpdateDate { get; set; } = DateTime.Now;

    [JsonProperty("start_time")]
    public TimeSpan UpdateStartTime { get; set; } = TimeSpan.Zero;

    [JsonProperty("end_time")]
    public TimeSpan? UpdateEndTime { get; set; } = null;

    [JsonProperty("name")]
    public string Name { get; set; } = "";

    [JsonProperty("description")]
    public string Description { get; set; } = "";

    [JsonProperty("live_update")]
    public bool WasLiveUpdate { get; set; } = false;
}
