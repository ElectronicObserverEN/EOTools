using EOTools.Tools;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace EOTools.Translation
{
    /// <summary>
    /// Interaction logic for TranslationQuest.xaml
    /// </summary>
    public partial class TranslationEquipForm : Page, INotifyPropertyChanged
    {
        private string FilePath
        {
            get
            {
                return AppSettings.EquipmentTLFilePath;
            }
            set
            {
                AppSettings.EquipmentTLFilePath = value;
            }
        }

        private JObject JsonEquipData = new JObject();

        private GitManager GitManager
        {
            get
            {
                string _gitPath = Path.GetDirectoryName(FilePath);
                return new GitManager(_gitPath);
            }
        }

        private EquipData selectedEquip;
        public EquipData SelectedEquip
        {
            get
            {
                return selectedEquip;
            }
            set
            {
                selectedEquip = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<EquipData> JsonEquip { get; set; } = new ObservableCollection<EquipData>();
        public ObservableCollection<EquipData> JsonEquipType { get; set; } = new ObservableCollection<EquipData>();

        private string Version = "";

        public event PropertyChangedEventHandler PropertyChanged;

        public TranslationEquipForm()
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
            JsonEquipData = JsonHelper.ReadJson(FilePath);

            // --- Equips
            Version = JsonEquipData["version"].ToString();

            LoadEquipDataFromJObject(JsonEquip, (JObject)JsonEquipData["equipment"]);
            LoadEquipDataFromJObject(JsonEquipType, (JObject)JsonEquipData["equiptype"]);
        }

        private void LoadEquipDataFromJObject(ObservableCollection<EquipData> _list, JObject _object)
        {
            _list.Clear();

            foreach (JProperty _EquipKey in _object.Properties())
            {
                string _nameEN = _EquipKey.Name;
                string _nameJP = _object[_nameEN].ToString();

                EquipData _newEquip = new EquipData(_nameEN, _nameJP);

                _list.Add(_newEquip);
            }
        }

        private void StageAndPushFiles()
        {
            GitManager.Stage(FilePath);

            string _updatePath = Path.Combine(Path.GetDirectoryName(FilePath), "update.json");
            GitManager.Stage(_updatePath);

            GitManager.CommitAndPush($"Equipment - {Version}");
        }

        private Dictionary<string, string> DictionnaryFromEquipDataList(IEnumerable<EquipData> _EquipList)
        {
            Dictionary<string, string> _dictionary = new Dictionary<string, string>();

            foreach (EquipData _EquipData in _EquipList)
            {
                if (!_dictionary.ContainsKey(_EquipData.NameJP))
                    _dictionary.Add(_EquipData.NameJP, _EquipData.NameEN);
            }

            return _dictionary;
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

            SelectedEquip = (EquipData)e.AddedItems[0];
        }

        private void buttonAddQuestTL_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            EquipData _newEquip = new EquipData("", "");

            JsonEquip.Add(_newEquip);
            SelectedEquip = _newEquip;
        }

        private void buttonAddSuffix_Click(object sender, RoutedEventArgs e)
        {
            EquipData _newEquip = new EquipData("", "");

            JsonEquipType.Add(_newEquip);
            SelectedEquip = _newEquip;
        }

        private void buttonExport_Click(object sender, System.Windows.RoutedEventArgs e)
        {

            Version = (int.Parse(Version) + 1).ToString();

            Dictionary<string, object> _toSerialize = new Dictionary<string, object>();

            _toSerialize.Add("version", Version);

            _toSerialize.Add("equipment", DictionnaryFromEquipDataList(JsonEquip));
            _toSerialize.Add("equiptype", DictionnaryFromEquipDataList(JsonEquipType));

            JsonHelper.WriteJson(FilePath, _toSerialize);

            // --- Change update.json too
            string _updatePath = Path.Combine(Path.GetDirectoryName(FilePath), "update.json");
            JObject _update = JsonHelper.ReadJson(_updatePath);

            _update["tl_ver"]["equipment"] = Version;

            JsonHelper.WriteJson(_updatePath, _update);

            // --- Stage & push
            StageAndPushFiles();
        }

        private void ListEquipment_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Delete)
            {
                JsonEquip.Remove((EquipData)ListEquipment.SelectedItem);
            }
        }

        private void ListEquipType_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Delete)
            {
                JsonEquipType.Remove((EquipData)ListEquipType.SelectedItem);
            }
        }
        #endregion

    }
}
