using EOTools.Translation.QuestManager.Updates;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace EOTools.Translation.QuestManager.Seasons;

public class SeasonModel
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; } = "";

    [JsonProperty("added_update_id")]
    [ForeignKey(nameof(UpdateModel))]
    public int? AddedOnUpdateId { get; set; }

    [JsonProperty("removed_update_id")]
    [ForeignKey(nameof(UpdateModel))]
    public int? RemovedOnUpdateId { get; set; }
}
