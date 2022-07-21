using CommunityToolkit.Mvvm.Input;
using EOTools.Models;
using EOTools.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EOTools.Translation.FitBonus
{
    public partial class FitBonusListViewModel
    {
        private GitManager GitManager
        {
            get
            {
                return new GitManager(ElectronicObserverDataFolderPath);
            }
        }

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

        public string FitBonusFilePath => Path.Combine(ElectronicObserverDataFolderPath, "Data", "FitBonuses.json");
        public string UpdateFilePath => Path.Combine(ElectronicObserverDataFolderPath, "update.json");

        public ObservableCollection<FitBonusPerEquipmentViewModel> FitBonuses { get; set; } = new ObservableCollection<FitBonusPerEquipmentViewModel>();


        public FitBonusListViewModel()
        {
            if (!string.IsNullOrEmpty(ElectronicObserverDataFolderPath) && File.Exists(FitBonusFilePath))
            {
                LoadFile();
            }
        }

        public void LoadFile()
        {
            FitBonuses.Clear();

            List<FitBonusPerEquipmentModel> list = JsonSerializer.Deserialize<List<FitBonusPerEquipmentModel>>(File.ReadAllText(FitBonusFilePath));

            foreach (FitBonusPerEquipmentModel model in list)
            {
                FitBonuses.Add(new FitBonusPerEquipmentViewModel(model));
            }
        }

        [ICommand]
        public void OpenDataFolderChoice()
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

        [ICommand]
        public void SaveFileThenPush()
        {
            using (var fileStream = File.Create(FitBonusFilePath))
                JsonSerializer.Serialize(fileStream, FitBonuses.Select(vm => vm.Model), typeof(IEnumerable<FitBonusPerEquipmentModel>), new JsonSerializerOptions()
                {
                    WriteIndented = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                });

            // --- Change update.json too
            JsonObject update = JsonSerializer.Deserialize<JsonObject>(File.ReadAllText(UpdateFilePath));

            JsonNode fitBonusUpdateVersion = update["FitBonuses"];
            int version = fitBonusUpdateVersion.GetValue<int>() + 1;
            update["FitBonuses"] = version;

            using (var fileStream = File.Create(UpdateFilePath))
                JsonSerializer.Serialize(fileStream, update, typeof(JsonObject), new JsonSerializerOptions()
                {
                    WriteIndented = true,
                });


            GitManager.Stage(FitBonusFilePath);

            GitManager.Stage(UpdateFilePath);

            GitManager.CommitAndPush($"Fit bonuses - {version}");
        }
    }
}
