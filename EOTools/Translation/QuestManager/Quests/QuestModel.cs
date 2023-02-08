using EOTools.Translation.QuestManager.Seasons;
using EOTools.Translation.QuestManager.Updates;
using System.ComponentModel.DataAnnotations.Schema;

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

        [ForeignKey(nameof(UpdateModel))]
        public int? AddedOnUpdateId { get; set; }

        [ForeignKey(nameof(UpdateModel))]
        public int? RemovedOnUpdateId { get; set; }

        [ForeignKey(nameof(SeasonModel))]
        public int? SeasonId { get; set; }
    }
}

