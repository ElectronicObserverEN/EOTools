using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace EOTools.Tools
{
    public static class AppSettings
    {
        #region Translations file paths
        private static string questTLFilePath = "";
        public static string QuestTLFilePath
        {
            get
            {
                return questTLFilePath;
            }
            set
            {
                questTLFilePath = value;
                WriteSettings();
            }
        }

        private static string equipmentTLFilePath = "";
        public static string EquipmentTLFilePath
        {
            get
            {
                return equipmentTLFilePath;
            }
            set
            {
                equipmentTLFilePath = value;
                WriteSettings();
            }
        }

        private static string shipTLFilePath = "";
        public static string ShipTLFilePath
        {
            get
            {
                return shipTLFilePath;
            }
            set
            {
                shipTLFilePath = value;
                WriteSettings();
            }
        }
        #endregion

        private const string SettingFileName = @"Config.json";

        public static void LoadSettings()
        {
            JObject _jsonSettings = JsonHelper.ReadJson(SettingFileName);

            if (_jsonSettings is null) return;

            JToken _value;

            if (_jsonSettings.TryGetValue("QuestTLFilePath", out _value)) 
            {
                questTLFilePath = _value.ToString();
            }

            if (_jsonSettings.TryGetValue("EquipmentTLFilePath", out _value))
            {
                equipmentTLFilePath = _value.ToString();
            }

            if (_jsonSettings.TryGetValue("ShipTLFilePath", out _value))
            {
                shipTLFilePath = _value.ToString();
            }

        }

        public static void WriteSettings()
        {
            Dictionary<string, string> _jsonData = new Dictionary<string, string>();

            _jsonData.Add("QuestTLFilePath", QuestTLFilePath);
            _jsonData.Add("EquipmentTLFilePath", EquipmentTLFilePath);
            _jsonData.Add("ShipTLFilePath", ShipTLFilePath);

            JsonHelper.WriteJson(SettingFileName, _jsonData);
        }
    }
}
