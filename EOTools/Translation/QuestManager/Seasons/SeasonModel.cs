using EOTools.Translation.QuestManager.Updates;
using System.ComponentModel.DataAnnotations.Schema;

namespace EOTools.Translation.QuestManager.Seasons;

public class SeasonModel
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    [ForeignKey(nameof(UpdateModel))]
    public int? AddedOnUpdateId { get; set; }

    [ForeignKey(nameof(UpdateModel))]
    public int? RemovedOnUpdateId { get; set; }
}
