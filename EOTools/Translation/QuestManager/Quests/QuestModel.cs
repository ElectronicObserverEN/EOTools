using EOTools.Translation.QuestManager.Seasons;
using EOTools.Translation.QuestManager.Updates;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace EOTools.Translation.QuestManager.Quests
{
    [Index(nameof(Code), nameof(ApiId), IsUnique = true)]
    public class QuestModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("api_id")]
        public int ApiId { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; } = "";

        [JsonProperty("name_jp")]
        public string NameJP { get; set; } = "";

        [JsonProperty("name_en")]
        public string NameEN { get; set; } = "";

        [JsonProperty("desc_jp")]
        public string DescJP { get; set; } = "";

        [JsonProperty("desc_en")]
        public string DescEN { get; set; } = "";

        [JsonProperty("added_update_id")]
        [ForeignKey(nameof(UpdateModel))]
        public int? AddedOnUpdateId { get; set; }

        [JsonProperty("removed_update_id")]
        [ForeignKey(nameof(UpdateModel))]
        public int? RemovedOnUpdateId { get; set; }

        [JsonProperty("season_id")]
        [ForeignKey(nameof(SeasonModel))]
        public int? SeasonId { get; set; }

        [JsonProperty("tracker")]
        public string Tracker { get; set; } = "";
    }
}

