using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EOTools.Models
{
    public class QuestTrackerData
    {
        public int QuestID { get; set; }

        public JArray QuestData { get; set; }

        public QuestTrackerData(int _questID, JArray _data)
        {
            QuestID = _questID;
            QuestData = _data;
        }

        public QuestTrackerData(JArray _dataWithId)
        {
            QuestID = _dataWithId[0].First.Value<int>();
            QuestData = _dataWithId;
        }
    }
}
