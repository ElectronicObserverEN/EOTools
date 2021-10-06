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

namespace EOTools.Translation
{
    /// <summary>
    /// Interaction logic for TranslationQuest.xaml
    /// </summary>
    public partial class TranslationQuestForm : Page, INotifyPropertyChanged
    {
        private string FilePath
        {
            get
            {
                return AppSettings.QuestTLFilePath;
            }
            set
            {
                AppSettings.QuestTLFilePath = value;
            }
        }

        private JObject JsonQuestData = new JObject();

        private GitManager GitManager
        {
            get
            {
                string _gitPath = Path.GetDirectoryName(FilePath);
                return new GitManager(_gitPath);
            }
        }

        private QuestData selectedQuest;
        public QuestData SelectedQuest
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

        public ObservableCollection<QuestData> JsonQuestList { get; set; } = new ObservableCollection<QuestData>();

        public ObservableCollection<QuestData> JsonQuest
        {
            get
            {
                return JsonQuestList;
            }
        }

        private string Version = "";

        public event PropertyChangedEventHandler PropertyChanged;

        public TranslationQuestForm()
        {
            this.DataContext = this;
            InitializeComponent();

            if (!string.IsNullOrEmpty(FilePath))
            {
                LoadFile();
            }
        }

        private void LoadFile()
        {
            JsonQuestData = JsonHelper.ReadJson(FilePath);

            JsonQuest.Clear();

            List<QuestData> _listOfQuests = new List<QuestData>();

            foreach (JProperty _questKey in JsonQuestData.Properties())
            {
                QuestData _newQuest = ParseJsonQuest(_questKey, JsonQuestData);
                if (_newQuest != null)
                    _listOfQuests.Add(_newQuest);
            }

            // --- Order by quest ID
            foreach (QuestData _quest in _listOfQuests.OrderBy(_q => _q.QuestID))
            {
                JsonQuest.Add(_quest);
            }
        }

        private QuestData ParseJsonQuest(JProperty _questKey, JObject _jsonObject)
        {
            if (_questKey.Name == "version")
            {
                Version = _jsonObject[_questKey.Name].ToString();
                return null;
            }

            JObject _questData = (JObject)_jsonObject[_questKey.Name];

            return new QuestData(int.Parse(_questKey.Name), _questData);
        }

        private void StageAndPushFiles()
        {
            GitManager.Stage(FilePath);

            string _updatePath = Path.Combine(Path.GetDirectoryName(FilePath), "update.json");
            GitManager.Stage(_updatePath);

            GitManager.CommitAndPush($"Quests - {Version}");
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

            SelectedQuest = (QuestData)e.AddedItems[0];
        }

        private void buttonAddQuestTL_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            QuestData _newQuest = new QuestData(JsonQuest.Max(_q => _q.QuestID));

            JsonQuest.Add(_newQuest);
            SelectedQuest = _newQuest;
        }

        private void buttonAddQuestFromEO_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Prompt _prompt = new Prompt("Import quest data from EO", "Enter quest data");

            if (_prompt.ShowDialog() == true)
            {
                JObject _newQuests = (JObject)JsonConvert.DeserializeObject(_prompt.ResultText);
                QuestData _newQuest = null;

                foreach (JProperty _questKey in _newQuests.Properties())
                {
                    _newQuest = ParseJsonQuest(_questKey, _newQuests);
                    if (_newQuest != null)
                        JsonQuest.Add(_newQuest);
                }

                if (_newQuest != null)
                    SelectedQuest = _newQuest;
            }
        }

        private void buttonExport_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Version = (int.Parse(Version) + 1).ToString();

            Dictionary<string, object> _toSerialize = new Dictionary<string, object>();

            _toSerialize.Add("version", Version);

            foreach (QuestData _quest in JsonQuestList.OrderBy(_q => _q.QuestID))
            {
                _toSerialize.Add(_quest.QuestID.ToString(), _quest);
            }

            JsonHelper.WriteJson(FilePath, _toSerialize);

            // --- Change update.json too
            string _updatePath = Path.Combine(Path.GetDirectoryName(FilePath), "update.json");
            JObject _update = JsonHelper.ReadJson(_updatePath);

            _update["tl_ver"]["quest"] = Version;

            JsonHelper.WriteJson(_updatePath, _update);

            // --- Stage & push
            StageAndPushFiles();
        }

        private void ListQuests_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Delete)
            {
                JsonQuest.Remove((QuestData)ListQuests.SelectedItem);
            }
        }
        #endregion

    }
}
