using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;

namespace EOTools.Models
{
    public class QuestData : INotifyPropertyChanged
    {
        [JsonIgnore]
        public int QuestID { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name_jp")]
        public string NameJP { get; set; }

        private string nameEN;

        [JsonProperty("name")]
        public string NameEN
        {
            get { return nameEN; }
            set { nameEN = value; OnPropertyChanged(); }
        }

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

        public QuestData()
        {
            Code = "";
            NameJP = "";
            NameEN = "";
            nameEN = "";
            DescJP = "";
            DescEN = "";
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
