using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EOTools.Translation
{
    public class QuestData
    {
        [JsonIgnore]
        public int QuestID { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name_jp")]
        public string NameJP { get; set; }

        [JsonProperty("name")]
        public string NameEN { get; set; }

        [JsonProperty("desc_jp")]
        public string DescJP { get; set; }

        [JsonProperty("desc")]
        public string DescEN { get; set; }

        [JsonIgnore]
        public string DisplayName
        {
            get
            {
                if (string.IsNullOrEmpty(NameEN)) return "NOT-TRANSLATED QUEST";

                return NameEN;
            }
        }

        public QuestData(int _questID, JObject _object)
        {
            QuestID = _questID;
            Code = _object.GetValue("code").ToString();
            NameJP = _object.GetValue("name_jp").ToString();
            NameEN = _object.GetValue("name").ToString();
            DescJP = _object.GetValue("desc_jp").ToString();
            DescEN = _object.GetValue("desc").ToString();
        }

        public QuestData(int _questID)
        {
            QuestID = _questID;
            Code = "";
            NameJP = "";
            NameEN = "";
            DescJP = "";
            DescEN = "";
        }
    }
}
