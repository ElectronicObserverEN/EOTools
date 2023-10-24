using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

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

        private static bool disablePush = false;
        public static bool DisablePush
        {
            get
            {
                return disablePush;
            }
            set
            {
                disablePush = value;
                WriteSettings();
            }
        }

        private static string eoDbPath = "";
        public static string EoDbPath
        {
            get
            {
                return eoDbPath;
            }
            set
            {
                eoDbPath = value;
                WriteSettings();
            }
        }

        private static string _electronicObserverApiUrl = "";
        public static string ElectronicObserverApiUrl
        {
            get => _electronicObserverApiUrl;
            set
            {
                _electronicObserverApiUrl = value;
                WriteSettings();
            }
        }

        private static string _electronicObserverApiKey = "";
        public static string ElectronicObserverApiKey
        {
            get => _electronicObserverApiKey;
            set
            {
                _electronicObserverApiKey = value;
                WriteSettings();
            }
        }
        #endregion

        private const string SettingFileName = @"Config.json";

        public static void LoadSettings()
        {
            if (!File.Exists(SettingFileName))
            {
                WriteSettings();
            }

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

            if (_jsonSettings.TryGetValue("DisablePush", out _value))
            {
                disablePush = (bool)_value;
            }

            if (_jsonSettings.TryGetValue("EoDbPath", out _value))
            {
                eoDbPath = (string)_value;
            }
            
            if(_jsonSettings.TryGetValue("EoApiUrl", out _value))
            {
                _electronicObserverApiUrl = (string)_value;
            }
            
            if(_jsonSettings.TryGetValue("EoApiKey", out _value))
            {
                _electronicObserverApiKey = (string)_value;
            }
        }

        public static void WriteSettings()
        {
            Dictionary<string, object> _jsonData = new Dictionary<string, object>
            {
                { "GetDataPath", KancolleEOAPIFolder },
                { "ShipIconFolder", ShipIconFolder },
                { "ElectronicObserverDataFolderPath", ElectronicObserverDataFolderPath },
                { "DisablePush", DisablePush },
                { "EoDbPath", EoDbPath },
                { "EoApiUrl", ElectronicObserverApiUrl },
                { "EoApiKey", ElectronicObserverApiKey },
            };

            JsonHelper.WriteJson(SettingFileName, _jsonData);
        }

        #region Set setting values
        public static string? OpenFolderDialog(string title)
        {
            // --- Load file
            using (var dialog = new FolderBrowserDialog()
            {
                Description = title,
                UseDescriptionForTitle = true
            })
            {
                DialogResult result = dialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    return dialog.SelectedPath;
                }
            }

            return null;
        }

        public static string? OpenFileDialog(string title, string extension)
        {
            // --- Load file
            using (var dialog = new System.Windows.Forms.OpenFileDialog()
            {
                Title = title,
                CheckFileExists = true,
                DefaultExt = extension
            })
            {
                DialogResult result = dialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    return dialog.FileName;
                }
            }

            return null;
        }
        #endregion
    }
}
