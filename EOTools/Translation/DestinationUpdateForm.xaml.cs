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
using System;

namespace EOTools.Translation
{
    /// <summary>
    /// Interaction logic for DestinationUpdateForm.xaml
    /// </summary>
    public partial class DestinationUpdateForm : Page, INotifyPropertyChanged
    {
        private string FilePath
        {
            get
            {
                return AppSettings.DestinationFilePath;
            }
            set
            {
                AppSettings.DestinationFilePath = value;
            }
        }

        private string NodeFilePath => Path.Combine(Path.GetDirectoryName(FilePath), "nodes.json");

        private JObject JsonDestinationData = new JObject();
        private JObject JsonNodeData = new JObject();

        private GitManager GitManager
        {
            get
            {
                string _gitPath = Path.GetDirectoryName(FilePath);
                return new GitManager(_gitPath);
            }
        }

        private string Version = "";

        public event PropertyChangedEventHandler PropertyChanged;

        public DestinationUpdateForm()
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
            JsonDestinationData = JsonHelper.ReadJsonObject(FilePath);
            JsonNodeData = JsonHelper.ReadJsonObject(NodeFilePath);

            // --- Get version
            if (JsonDestinationData != null)
                Version = JsonDestinationData["version"].ToString();
        }

        private void StageAndPushFiles()
        {
            GitManager.Stage(FilePath);
            GitManager.Stage(NodeFilePath);

            string _updatePath = Path.Combine(Path.GetDirectoryName(FilePath), "update.json");
            GitManager.Stage(_updatePath);

            GitManager.CommitAndPush($"Destination - {Version}");
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

        private void buttonExport_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Version = (int.Parse(Version) + 1).ToString();

            JsonDestinationData["version"] = Version;
            JsonHelper.WriteJsonByOnlyIndentingXTimesWidePeepoHappy(FilePath, JsonDestinationData, 2);

            JsonNodeData["Revision"] = int.Parse(Version);
            JsonHelper.WriteJsonByOnlyIndentingXTimesWidePeepoHappy(NodeFilePath, JsonNodeData, 2);

            // --- Change update.json too
            string _updatePath = Path.Combine(Path.GetDirectoryName(FilePath), "update.json");
            JObject _update = JsonHelper.ReadJsonObject(_updatePath);

            _update["tl_ver"]["nodes"] = int.Parse(Version);

            JsonHelper.WriteJson(_updatePath, _update);

            // --- Stage & push
            StageAndPushFiles();
        }
        
        private void buttonUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                JObject _wikiData = JsonHelper.ReadJsonFromUrl("https://raw.githubusercontent.com/kcwiki/kancolle-data/master/map/edge.json") as JObject;

                // --- Convert to EO data
                JsonDestinationData = new JObject();
                JsonNodeData = new JObject();

                // --- Add version property
                JsonDestinationData.Add("version", Version);
                JsonNodeData.Add("Revision", int.Parse(Version));

                foreach (JProperty _property in _wikiData.Properties())
                {
                    JObject _nodeFormatObject = new JObject();

                    foreach (JProperty _nodeProperty in (_property.Value as JObject).Properties())
                    {
                        _nodeFormatObject.Add($"N{_nodeProperty.Name}", _nodeProperty.Value);
                    }

                    JsonNodeData.Add($"W{_property.Name}", _nodeFormatObject);

                    string _world = _property.Name[0..^1];
                    string _map = _property.Name[^1..];

                    JsonDestinationData.Add($"World {_world}-{_map}", _property.Value);
                }
            }
            catch (Exception _ex)
            {
                MessageBox.Show(_ex.Message);
            }

        }
        #endregion
    }
}
