using EOTools.Tools;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using EOTools.Models;

namespace EOTools.Translation
{
    /// <summary>
    /// Interaction logic for TranslationQuest.xaml
    /// </summary>
    public partial class QuestTrackerForm : Page, INotifyPropertyChanged
    {
        private string FilePath
        {
            get
            {
                return AppSettings.QuestTrackerFilePath;
            }
            set
            {
                AppSettings.QuestTrackerFilePath = value;
            }
        }

        private JArray JsonQuestData = new JArray();

        private GitManager GitManager
        {
            get
            {
                string _gitPath = Path.GetDirectoryName(FilePath);
                return new GitManager(_gitPath);
            }
        }

        private QuestTrackerData selectedQuest;
        public QuestTrackerData SelectedQuest
        {
            get
            {
                return selectedQuest;
            }
            set
            {
                selectedQuest = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<QuestTrackerData> JsonQuestList { get; set; } = new ObservableCollection<QuestTrackerData>();

        public ObservableCollection<QuestTrackerData> JsonQuest
        {
            get
            {
                return JsonQuestList;
            }
        }

        private string Version = "";

        public event PropertyChangedEventHandler PropertyChanged;

        public QuestTrackerForm()
        {
            this.DataContext = this;
            InitializeComponent();

            if (!string.IsNullOrEmpty(FilePath))
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
        }

        private void LoadFile()
        {
            JsonQuestData = JsonHelper.ReadJsonArray(FilePath);

            JsonQuest.Clear();

            List<QuestTrackerData> _listOfQuests = new List<QuestTrackerData>();

            foreach (JArray _questData in JsonQuestData)
            {
                QuestTrackerData _newQuest = new QuestTrackerData(_questData);
                _listOfQuests.Add(_newQuest);   
            }

            // --- Order by quest ID
            foreach (QuestTrackerData _quest in _listOfQuests.OrderBy(_q => _q.QuestID))
            {
                JsonQuest.Add(_quest);
            }

            // --- Get version
            JObject _updateJson = JsonHelper.ReadJsonObject(Path.Combine(Path.GetDirectoryName(FilePath), "..", "update.json"));
            Version = _updateJson["QuestTrackers"].ToString();
        }

        private void StageAndPushFiles()
        {
            GitManager.Stage(FilePath);

            string _updatePath = Path.Combine(Path.GetDirectoryName(FilePath), "..", "update.json");
            GitManager.Stage(_updatePath);

            GitManager.CommitAndPush($"Quest tracker - {Version}");
        }

        #region Events
        // Create the OnPropertyChanged method to raise the event
        // The calling member's name will be used as the parameter.
        protected void OnPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void buttonSelectFile_Click(object sender, System.Windows.RoutedEventArgs e)
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
        }
        #endregion

    }
}
