using EOTools.Translation.QuestManager.Updates;
using System.ComponentModel.DataAnnotations.Schema;

namespace EOTools.Translation.QuestManager.Events;

public class EventModel
{
    public int Id { get; set; }

    public int ApiId { get; set; }

    public string Name { get; set; } = "";

    [ForeignKey(nameof(UpdateModel))]
    public int? StartOnUpdateId { get; set; }

    [ForeignKey(nameof(UpdateModel))]
    public int? EndOnUpdateId { get; set; }
}
