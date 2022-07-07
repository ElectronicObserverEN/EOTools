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
    public class MapNameViewModel : ObservableObject
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

        public ObservableCollection<MapTranslationModel> MapTranslationData { get; set; } = new ObservableCollection<MapTranslationModel>();
        public ObservableCollection<MapTranslationModel> FleetsTranslationData { get; set; } = new ObservableCollection<MapTranslationModel>();

        private string TranslationFilePath => Path.Combine(ElectronicObserverDataFolderPath, "Translations", "en-US", "operation.json");
        private string UpdateFilePath => Path.Combine(ElectronicObserverDataFolderPath, "Translations", "en-US", "update.json");

        private GitManager GitManager
        {
            get
            {
                string _gitPath = Path.GetDirectoryName(ElectronicObserverDataFolderPath);
                return new GitManager(_gitPath);
            }
        }

        private JObject RawJson { get; set; }

        public MapTranslationModel SelectedTranslation { get; set; }

        private string Version = "";

        public RelayCommand ChooseDataFolder { get; set; }
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
            SaveFileThenPush = new RelayCommand(() => WriteFile());
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

        private void WriteFile()
        {
            Version = (int.Parse(Version) + 1).ToString();

            JObject _toSerialize = new JObject();
            JObject _mapList = new JObject();
            JObject _fleetList = new JObject();

            _toSerialize["version"] = Version;
            _toSerialize["maps"] = _mapList;
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
        }

        private void StageAndPushFiles()
        {
            GitManager.Stage(ElectronicObserverDataFolderPath);

            string _updatePath = Path.Combine(Path.GetDirectoryName(ElectronicObserverDataFolderPath), "..", "update.json");
            GitManager.Stage(_updatePath);

            GitManager.CommitAndPush($"Quest tracker - {Version}");
        }


        #region Events
        /*private void buttonSelectFile_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() != true) return;

            // --- Load file
            FilePath = openFileDialog.FileName;

            try
            {
                LoadFile();
            }
            catch
            {
                MessageBox.Show("Error parsing Json");
            }
        }

        private void ListQuests_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count != 1) return;

            SelectedQuest = (QuestTrackerData)e.AddedItems[0];
        }

        private void buttonAddQuestTL_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            JArray _quests = JsonHelper.ReadJsonFromString(Clipboard.GetText()) as JArray;

            foreach (JArray _quest in _quests)
            {
                QuestTrackerData _questData = new QuestTrackerData(_quest);

                QuestTrackerData _questFound = JsonQuestList.FirstOrDefault((_q1) => _q1.QuestID == _questData.QuestID);

                if (_questFound is QuestTrackerData)
                {
                    _questFound.QuestData = _questData.QuestData;
                }
                else
                {
                    JsonQuestList.Add(_questData);
                }
            }
        }

        private void buttonExport_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Version = (int.Parse(Version) + 1).ToString();

            JArray _toSerialize = new JArray();

            foreach (QuestTrackerData _quest in JsonQuestList.OrderBy(_q => _q.QuestID))
            {

                _toSerialize.Add(_quest.QuestData);
            }

            JsonHelper.WriteJsonByOnlyIndentingOnce(FilePath, _toSerialize);

            // --- Change update.json too
            string _updatePath = Path.Combine(Path.GetDirectoryName(FilePath), "..", "update.json");
            JObject _update = JsonHelper.ReadJsonObject(_updatePath);

            _update["QuestTrackers"] = int.Parse(Version);

            JsonHelper.WriteJson(_updatePath, _update);

            // --- Stage & push
            StageAndPushFiles();
        }

        private void ListQuests_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Delete)
            {
                JsonQuest.Remove((QuestTrackerData)ListQuests.SelectedItem);
            }
        }*/
        #endregion
    }
}


