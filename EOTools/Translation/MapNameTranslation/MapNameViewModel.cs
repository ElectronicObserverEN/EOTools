using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOTools.Models;
using EOTools.Tools;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EOTools.Translation
{
    public partial class MapNameViewModel : ObservableObject
    {
        private string ElectronicObserverDataFolderPath
        {
            get
            {
                return AppSettings.ElectronicObserverDataFolderPath;
            }
            set
            {
                AppSettings.ElectronicObserverDataFolderPath = value;
                LoadFile();
            }
        }

        private string KancolleAPIFolder
        {
            get
            {
                return AppSettings.KancolleEOAPIFolder;
            }
            set
            {
                AppSettings.KancolleEOAPIFolder = value;
                LoadFile();
            }
        }

        public ObservableCollection<MapTranslationModel> MapTranslationData { get; set; } = new ObservableCollection<MapTranslationModel>();
        public ObservableCollection<MapTranslationModel> FleetsTranslationData { get; set; } = new ObservableCollection<MapTranslationModel>();

        private string TranslationFilePath => Path.Combine(ElectronicObserverDataFolderPath, "Translations", "en-US", "operation.json");
        private string UpdateFilePath => Path.Combine(ElectronicObserverDataFolderPath, "Translations", "en-US", "update.json");

        private string ApiDataFilePath => AppSettings.GetDataPath;

        private GitManager GitManager
        {
            get
            {
                return new GitManager(ElectronicObserverDataFolderPath);
            }
        }

        private JObject RawJson { get; set; }

        [ObservableProperty]
        private MapTranslationModel _selectedTranslation;

        private string Version = "";

        public RelayCommand ChooseDataFolder { get; set; }
        public RelayCommand ChooseAPIFolder { get; set; }
        public RelayCommand SaveFileThenPush { get; set; }

        public MapNameViewModel()
        {
            if (!string.IsNullOrEmpty(ElectronicObserverDataFolderPath))
            {
                try
                {
                    LoadFile();
                }
                catch
                {
                    MessageBox.Show("Error parsing Json");
                }
            }

            ChooseDataFolder = new RelayCommand(() => OpenDataFolderChoice());
            SaveFileThenPush = new RelayCommand(() => { WriteFile(); StageAndPushFiles(); });
            ChooseAPIFolder = new RelayCommand(() => OpenAPIFolderChoice());
        }

        private void OpenDataFolderChoice()
        {
            // --- Load file
            using (var dialog = new FolderBrowserDialog())
            {
                DialogResult result = dialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    ElectronicObserverDataFolderPath = dialog.SelectedPath;
                }
            }
        }

        private void OpenAPIFolderChoice()
        {
            // --- Load file
            using (var dialog = new FolderBrowserDialog())
            {
                DialogResult result = dialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    KancolleAPIFolder = dialog.SelectedPath;
                }
            }
        }


        private void WriteFile()
        {
            Version = (int.Parse(Version) + 1).ToString();

            JObject _toSerialize = new JObject();
            JObject _mapList = new JObject();
            JObject _fleetList = new JObject();

            _toSerialize["version"] = Version;
            _toSerialize["map"] = _mapList;
            _toSerialize["fleet"] = _fleetList;

            foreach (var _map in MapTranslationData)
            {
                _mapList[_map.NameJP] = _map.NameTranslated;
            }

            foreach (var _fleet in FleetsTranslationData)
            {
                _fleetList[_fleet.NameJP] = _fleet.NameTranslated;
            }
            

            JsonHelper.WriteJson(TranslationFilePath, _toSerialize);

            // --- Change update.json too
            string _updatePath = Path.Combine(UpdateFilePath);
            JObject _update = JsonHelper.ReadJsonObject(_updatePath);

            _update["operation"] = Version;

            JsonHelper.WriteJson(_updatePath, _update);
        }

        private void LoadFile()
        {
            RawJson = JsonHelper.ReadJsonObject(TranslationFilePath);

            MapTranslationData.Clear();
            FleetsTranslationData.Clear();

            JObject _maps = RawJson["map"] as JObject;
            JObject _fleets = RawJson["fleet"] as JObject;
            Version = RawJson.Value<string>("version");

            foreach (JProperty _map in _maps.Properties())
            {
                MapTranslationModel _newMap = new MapTranslationModel()
                {
                    NameJP = _map.Name,
                    NameTranslated = _maps.Value<string>(_map.Name),
                };

                MapTranslationData.Add(_newMap);
            }

            foreach (JProperty _fleet in _fleets.Properties())
            {
                MapTranslationModel _newFleet = new MapTranslationModel()
                {
                    NameJP = _fleet.Name,
                    NameTranslated = _fleets.Value<string>(_fleet.Name),
                };

                FleetsTranslationData.Add(_newFleet);
            }

            // --- Read untranslated stuff : 
            JObject _mapApi = JsonHelper.ReadKCJson(ApiDataFilePath);
            if (_mapApi != null)
            {
                JArray _mapsFromAPI = (JArray)_mapApi["api_data"]["api_mst_mapinfo"];
                List<string> _translations = MapTranslationData.Select(_m => _m.NameJP).ToList();

                foreach (JObject _mapInfo in _mapsFromAPI)
                {
                    string _mapName = _mapInfo.Value<string>("api_name");
                    string _mapId = _mapInfo.Value<string>("api_no");
                    string _worldId = _mapInfo.Value<string>("api_maparea_id");
                    if (!_translations.Contains(_mapName))
                    {
                        MapTranslationData.Add(new MapTranslationModel()
                        {
                            NameJP = _mapName,
                            NameTranslated = $"{_mapName} ({_worldId}-{_mapId})",
                        });
                    }
                }
            }
        }

        private void StageAndPushFiles()
        {
            GitManager.Stage(TranslationFilePath);

            GitManager.Stage(UpdateFilePath);

            GitManager.CommitAndPush($"Map translations - {Version}");
        }
    }
}


