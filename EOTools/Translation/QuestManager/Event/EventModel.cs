using EOTools.Translation.QuestManager.Updates;

namespace EOTools.Translation.QuestManager.Event;

public class EventModel
{
    public int Id { get; set; }

    public int ApiId { get; set; }

    public string Name { get; set; } = "";

    public UpdateModel? StartOnUpdate { get; set; }
    public UpdateModel? EndOnUpdate { get; set; }
}
