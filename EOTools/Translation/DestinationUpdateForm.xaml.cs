using EOTools.Tools;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace EOTools.Translation
{
    /// <summary>
    /// Interaction logic for DestinationUpdateForm.xaml
    /// </summary>
    public partial class DestinationUpdateForm : Page, INotifyPropertyChanged
    {
        private string FilePath => Path.Combine(AppSettings.ElectronicObserverDataFolderPath, "Data", "destination.json");
        private string UpdateFilePath => Path.Combine(AppSettings.ElectronicObserverDataFolderPath, "update.json");

        private JObject JsonDestinationData = new JObject();

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
            JsonDestinationData = JsonHelper.ReadJsonObject(FilePath);
            //JsonNodeData = JsonHelper.ReadJsonObject(NodeFilePath);

            // --- Get version
            if (JsonDestinationData != null)
                Version = JsonDestinationData["version"].ToString();
        }

        private void StageAndPushFiles()
        {
            GitManager.Stage(FilePath);
            //GitManager.Stage(NodeFilePath);
            GitManager.Stage(UpdateFilePath);

            GitManager.CommitAndPush($"Destination - {Version}");
        }

        #region Events
        // Create the OnPropertyChanged method to raise the event
        // The calling member's name will be used as the parameter.
        protected void OnPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void buttonExport_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Version = (int.Parse(Version) + 1).ToString();

            JsonDestinationData["version"] = Version;
            JsonHelper.WriteJsonByOnlyIndentingXTimes(FilePath, JsonDestinationData, 2);

            //JsonNodeData["Revision"] = int.Parse(Version);
            //JsonHelper.WriteJsonByOnlyIndentingXTimes(NodeFilePath, JsonNodeData, 2);

            // --- Change update.json too
            JObject _update = JsonHelper.ReadJsonObject(UpdateFilePath);

            _update["nodes"] = int.Parse(Version);

            JsonHelper.WriteJson(UpdateFilePath, _update);

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
                //JsonNodeData = new JObject();

                // --- Add version property
                JsonDestinationData.Add("version", Version);
                //JsonNodeData.Add("Revision", int.Parse(Version));

                foreach (JProperty _property in _wikiData.Properties())
                {
                    /*JObject _nodeFormatObject = new JObject();

                    foreach (JProperty _nodeProperty in (_property.Value as JObject).Properties())
                    {
                        _nodeFormatObject.Add($"N{_nodeProperty.Name}", _nodeProperty.Value);
                    }*/

                    //JsonNodeData.Add($"W{_property.Name}", _nodeFormatObject);

                    string _world = _property.Name[0..^1];
                    string _map = _property.Name[^1..];

                    JsonDestinationData.Add($"World {_world}-{_map}", _property.Value);
                }

                MessageBox.Show("Data updated");

            }
            catch (Exception _ex)
            {
                MessageBox.Show(_ex.Message);
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Process openLink = new Process();
            openLink.StartInfo.UseShellExecute = true;
            openLink.StartInfo.FileName = "https://raw.githubusercontent.com/kcwiki/kancolle-data/master/map/edge.json";
            openLink.Start();
        }
        #endregion

    }
}
