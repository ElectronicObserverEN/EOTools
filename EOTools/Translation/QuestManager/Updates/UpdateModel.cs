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

    [JsonProperty("maint_start_tweet")]
    public string StartTweetLink { get; set; } = "";

    [JsonProperty("maint_end_tweet")]
    public string EndTweetLink { get; set; } = "";

    /// <summary>
    /// Returns true if update is coming
    /// </summary>
    /// <returns></returns>
    public bool UpdateIsComing()
    {
        DateTime dateNowJST = DateTime.UtcNow + new TimeSpan(9, 0, 0);
        DateTime start = UpdateDate.Date.Add(UpdateStartTime);

        // Update has started ?
        return start > dateNowJST;
    }

    /// <summary>
    /// Returns true if update is in progress
    /// </summary>
    /// <returns></returns>
    public bool UpdateInProgress()
    {
        DateTime dateNowJST = DateTime.UtcNow + new TimeSpan(9, 0, 0);
        DateTime start = UpdateDate.Date.Add(UpdateStartTime);

        // Update has started and no end time => update in progress
        if (start < dateNowJST && UpdateEndTime is null) return true;

        // Update has started and end time => update could be in progress
        if (start < dateNowJST && UpdateEndTime is TimeSpan endTime)
        {
            DateTime end = UpdateDate.Date.Add(endTime);

            // End didn't happen yet => Update is in progress
            if (end > dateNowJST) return true;
        }
        return false;
    }
}
