using EOTools.Translation.QuestManager.Seasons;
using EOTools.Translation.QuestManager.Updates;

namespace EOTools.Translation.QuestManager.Quests
{
    public class QuestModel
    {
        public int Id { get; set; }

        public int ApiId { get; set; }

        public string Code { get; set; } = "";

        public string NameJP { get; set; } = "";

        public string NameEN { get; set; } = "";

        public string DescJP { get; set; } = "";

        public string DescEN { get; set; } = "";

        public UpdateModel? AddedOnUpdate { get; set; }
        public UpdateModel? RemovedOnUpdate { get; set; }

        public SeasonModel? Season { get; set; }
    }
}

