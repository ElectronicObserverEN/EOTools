using EOTools.Translation.QuestManager.Updates;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace EOTools.Translation.QuestManager.Events;

public class EventModel
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("api_id")]
    public int ApiId { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; } = "";

    [JsonProperty("start_update_id")]
    [ForeignKey(nameof(UpdateModel))]
    public int? StartOnUpdateId { get; set; }

    [JsonProperty("end_update_id")]
    [ForeignKey(nameof(UpdateModel))]
    public int? EndOnUpdateId { get; set; }
}
