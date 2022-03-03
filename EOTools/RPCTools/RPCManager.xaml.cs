using EOTools.Tools;
using EOTools.Models;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Diagnostics;

namespace EOTools.RPCTools
{
    /// <summary>
    /// Interaction logic for RPCManager.xaml
    /// </summary>
    public partial class RPCManager : Page, INotifyPropertyChanged
    {
        private string FilePath
        {
            get
            {
                return AppSettings.GetDataPath;
            }
            set
            {
                AppSettings.KancolleEOAPIFolder = value;
            }
        }

        private GitManager GitManager
        {
            get
            {
                string _gitPath = Path.GetDirectoryName(FilePath);
                return new GitManager(_gitPath);
            }
        }

        public ObservableCollection<ShipData> ShipList { get; set; } = new ObservableCollection<ShipData>();

        public event PropertyChangedEventHandler PropertyChanged;

        public RPCManager()
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
            try
            {
                JObject _jsonShips = JsonHelper.ReadKCJson(FilePath);
                LoadShipList(_jsonShips);
            }
            catch (Exception _ex)
            {
                MessageBox.Show("Error loading ship data : "+ _ex);
            }
        }

        private void LoadShipList(JObject _object)
        {
            ShipList.Clear();

            JArray _shipJson = (JArray)_object["api_data"]["api_mst_ship"];

            Dictionary<int, ShipData> _shipDictionary = new Dictionary<int, ShipData>();

            foreach (JObject _shipData in _shipJson)
            {
                int _id = (int)_shipData["api_id"];

                // --- Skip abyssals
                if (_id > 1500) continue;

                string _nameJP = _shipData["api_name"].ToString();

                ShipData _newShip = new ShipData( _nameJP, "")
                {
                    ShipId = _id
                };

                if (_newShip.RPCExists) continue;

                ShipList.Add(_newShip);

                if (_shipData.TryGetValue("api_sortno", out JToken _sortNumber))
                {
                    _shipDictionary.Add((int)_sortNumber, _newShip);
                }
            }

            JArray _shipGraphArray = (JArray)_object["api_data"]["api_mst_shipgraph"];

            foreach (JObject _shipGraph in _shipGraphArray)
            {
                if (!_shipGraph.TryGetValue("api_sortno", out JToken _sortNumber)) continue;
                if ((int)_sortNumber == 0) continue;
                if (!_shipDictionary.ContainsKey((int)_sortNumber)) continue;

                ShipData _ship = _shipDictionary[(int)_sortNumber];

                int _x1 = (int)_shipGraph["api_weda"][0];
                int _y1 = (int)_shipGraph["api_weda"][1];
                int _x2 = (int)_shipGraph["api_wedb"][0];
                int _y2 = (int)_shipGraph["api_wedb"][1];

                (int, int, int, int) _start = (_x1, _y1, _x2, _y2);

                _ship.FaceData = _start;

                _ship.RessourceName = _shipGraph["api_filename"].ToString();
            }

            foreach (ShipData _ship in ShipList)
            {
                ListShipPicture.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
                {
                    // --- Load icon
                    _ship.ShipIcon = _ship.GetShipIcon();

                    // --- if no icon, copy the full picture of the ship 
                    if (_ship.ShipIcon is null)
                    {
                        string _tempFile = Path.Combine(AppSettings.ShipIconFolder, $"{_ship.ShipId}_temp.png");

                        string _iconFileName = _ship.GetIconFileName();
                        if (!File.Exists(_tempFile) && !string.IsNullOrEmpty(_iconFileName))
                        {
                            File.Copy(_iconFileName, _tempFile);
                        }
                    }

                    // --- Load temp icon
                    _ship.ShipIcon = _ship.GetTempShipIcon();
                }));                
            }
        }

        private void StageAndPushFiles()
        {
            GitManager.Stage(FilePath);

            string _updatePath = Path.Combine(Path.GetDirectoryName(FilePath), "update.json");
            GitManager.Stage(_updatePath);

            GitManager.CommitAndPush($"");
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
            var _dialog = new SaveFileDialog();
            _dialog.Title = "Select a Directory";
            _dialog.Filter = "Directory|*.this.directory";
            _dialog.FileName = "select";

            if (_dialog.ShowDialog() != true) return;

            string _path = _dialog.FileName;

            _path = _path.Replace("\\select.this.directory", "");
            _path = _path.Replace(".this.directory", "");

            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }

            // --- Load file
            FilePath = _path;

            try
            {
                LoadFile();
            }
            catch
            {
                MessageBox.Show("Error parsing Json");
            }
        }


        private void buttonExport_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            return;

            Dictionary<string, object> _toSerialize = new Dictionary<string, object>();

            JsonHelper.WriteJson(FilePath, _toSerialize);

            // --- Stage & push
            StageAndPushFiles();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var _dialog = new SaveFileDialog();
            _dialog.Title = "Select a Directory";
            _dialog.Filter = "Directory|*.this.directory"; 
            _dialog.FileName = "select";

            if (_dialog.ShowDialog() != true) return;

            string _path = _dialog.FileName;

            _path = _path.Replace("\\select.this.directory", "");
            _path = _path.Replace(".this.directory", "");

            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }

            // --- Load file
            AppSettings.ShipIconFolder = _path;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ShipData _ship = ListShipPicture.SelectedItem as ShipData;
            string _imagePath = Path.Combine(AppSettings.ShipIconFolder, _ship.ShipId + "_temp.png");

            if (!File.Exists(_imagePath)) return;

            (_ship.ShipIcon as BitmapImage).StreamSource.Close();

            ProcessStartInfo procInfo = new ProcessStartInfo();
            procInfo.FileName = "mspaint.exe";
            procInfo.Arguments = $"\"{_imagePath}\" /ForceBootstrapPaint3D";

            Process _process = new Process();
            _process.StartInfo = procInfo;
            _process.Start();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            foreach (ShipData _ship in ShipList)
            {
                (_ship.ShipIcon as BitmapImage).StreamSource.Close();

                _ship.ShipIcon = _ship.GetTempShipIcon();
            }
        }

        /*private void ListEquipment_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
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
        }*/
        #endregion

    }
}
