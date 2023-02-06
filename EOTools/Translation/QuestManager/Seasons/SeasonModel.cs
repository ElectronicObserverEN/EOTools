using EOTools.Translation.QuestManager.Updates;

namespace EOTools.Translation.QuestManager.Seasons;

public class SeasonModel
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public UpdateModel? AddedOnUpdate { get; set; }
    public UpdateModel? RemovedOnUpdate { get; set; }
}
