using Newtonsoft.Json.Linq;

namespace ReadBattleResultService
{
    public static class AppSettings
    {
        private static string parsedFleetFile = "";
        public static string ParsedFleetFile
        {
            get
            {
                return parsedFleetFile;
            }
            set
            {
                parsedFleetFile = value;
                WriteSettings();
            }
        }

        private static string kancolleEOAPIFolder = "";

        public static string KancolleEOAPIFolder
        {
            get
            {
                return kancolleEOAPIFolder;
            }
            set
            {
                kancolleEOAPIFolder = value;
                WriteSettings();
            }
        }

        private const string SettingFileName = @"Config.json";

        public static void LoadSettings()
        {
            JObject _jsonSettings = JsonHelper.ReadJsonObject(SettingFileName);

            if (_jsonSettings is null) return;

            JToken _value;

            if (_jsonSettings.TryGetValue("ParsedFleetFile", out _value))
            {
                parsedFleetFile = _value.ToString();
            }
            if (_jsonSettings.TryGetValue("GetDataPath", out _value))
            {
                kancolleEOAPIFolder = _value.ToString();
            }
        }

        public static void WriteSettings()
        {
            Dictionary<string, string> _jsonData = new Dictionary<string, string>();

            _jsonData.Add("GetDataPath", KancolleEOAPIFolder);
            _jsonData.Add("ParsedFleetFile", ParsedFleetFile);

            JsonHelper.WriteJson(SettingFileName, _jsonData);
        }
    }
}
