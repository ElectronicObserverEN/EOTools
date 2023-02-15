using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;

namespace EOTools.Tools
{
    public static class AppSettings
    {
        #region Translations file paths
        private static string electronicObserverDataFolderPath = "";
        public static string ElectronicObserverDataFolderPath
        {
            get
            {
                return electronicObserverDataFolderPath;
            }
            set
            {
                electronicObserverDataFolderPath = value;
                WriteSettings();
            }
        }

        #region Kancolle cache made by EO
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

        public static string GetShipFullPath
        {
            get
            {
                return Path.Combine(KancolleEOAPIFolder, "kcs2", "resources", "ship", "full");
            }
        }

        public static string GetDataPath
        {
            get
            {
                return Path.Combine(KancolleEOAPIFolder, "kcsapi", "api_start2", "getData");
            }
        }
        #endregion

        private static string shipIconFolder = "";
        public static string ShipIconFolder
        {
            get
            {
                return shipIconFolder;
            }
            set
            {
                shipIconFolder = value;
                WriteSettings();
            }
        }

        #endregion

        private const string SettingFileName = @"Config.json";

        public static void LoadSettings()
        {
            JObject _jsonSettings = JsonHelper.ReadJsonObject(SettingFileName);

            if (_jsonSettings is null) return;

            JToken _value;

            if (_jsonSettings.TryGetValue("GetDataPath", out _value))
            {
                kancolleEOAPIFolder = _value.ToString();
            }

            if (_jsonSettings.TryGetValue("ShipIconFolder", out _value))
            {
                shipIconFolder = _value.ToString();
            }

            if (_jsonSettings.TryGetValue("ElectronicObserverDataFolderPath", out _value))
            {
                electronicObserverDataFolderPath = _value.ToString();
            }
        }

        public static void WriteSettings()
        {
            Dictionary<string, string> _jsonData = new Dictionary<string, string>();

            _jsonData.Add("GetDataPath", KancolleEOAPIFolder);
            _jsonData.Add("ShipIconFolder", ShipIconFolder);
            _jsonData.Add("ElectronicObserverDataFolderPath", ElectronicObserverDataFolderPath);

            JsonHelper.WriteJson(SettingFileName, _jsonData);
        }
    }
}
