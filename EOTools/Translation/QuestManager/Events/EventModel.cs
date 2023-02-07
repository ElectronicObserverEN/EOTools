using EOTools.Translation.QuestManager.Updates;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EOTools.Translation.QuestManager.Events;

public class EventModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("api_id")]
    public int ApiId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("start_update_id")]
    [ForeignKey(nameof(UpdateModel))]
    public int? StartOnUpdateId { get; set; }

    [JsonPropertyName("end_update_id")]
    [ForeignKey(nameof(UpdateModel))]
    public int? EndOnUpdateId { get; set; }
}
