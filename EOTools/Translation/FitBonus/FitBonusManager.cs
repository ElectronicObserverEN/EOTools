﻿using CommunityToolkit.Mvvm.Input;
using EOTools.Tools;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using EOTools.Models.FitBonus;

namespace EOTools.Translation.FitBonus
{
    public partial class FitBonusManager
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

        private FitBonusUpdaterService FitBonusUpdaterService { get; }

        public string FitBonusFilePath => Path.Combine(ElectronicObserverDataFolderPath, "Data", "FitBonuses.json");
        public string UpdateFilePath => Path.Combine(ElectronicObserverDataFolderPath, "update.json");

        public ObservableCollection<FitBonusPerEquipmentViewModel> FitBonuses { get; set; } = new();


        public FitBonusManager(FitBonusUpdaterService updateService)
        {
            FitBonusUpdaterService = updateService;

            if (!string.IsNullOrEmpty(ElectronicObserverDataFolderPath) && File.Exists(FitBonusFilePath))
            {
                LoadFile();
            }
        }

        public void LoadFile()
        {
            FitBonuses.Clear();

            List<FitBonusPerEquipmentModel> list = JsonHelper.ReadJson<List<FitBonusPerEquipmentModel>>(FitBonusFilePath);

            foreach (FitBonusPerEquipmentModel model in list)
            {
                FitBonuses.Add(new FitBonusPerEquipmentViewModel(model));
            }
        }

        public void SaveFile()
        {
            JsonHelper.WriteJsonByOnlyIndentingXTimes(FitBonusFilePath, FitBonuses.Select(vm => vm.Model), 4, true);
        }

        [RelayCommand]
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

        public async Task UpdateThenSaveFileThenPush()
        {
            List<FitBonusPerEquipmentViewModel>? bonuses = await FitBonusUpdaterService.GetFitBonuses();

            if (bonuses is null) return;
            FitBonuses.Clear();

            foreach (FitBonusPerEquipmentViewModel bonus in bonuses)
            {
                FitBonuses.Add(bonus);
            }

            SaveFileThenPush();
        }

        [RelayCommand]
        public void SaveFileThenPush()
        {
            JsonHelper.WriteJsonByOnlyIndentingXTimes(FitBonusFilePath, FitBonuses.Select(vm => vm.Model), 4, true);

            // --- Change update.json too
            JObject update = JsonHelper.ReadJsonObject(UpdateFilePath);

            JToken fitBonusUpdateVersion = update["FitBonuses"];
            int version = fitBonusUpdateVersion.Value<int>() + 1;
            update["FitBonuses"] = version;

            JsonHelper.WriteJson(UpdateFilePath, update);

            GitManager.Stage(FitBonusFilePath);

            GitManager.Stage(UpdateFilePath);

            GitManager.CommitAndPush($"Fit bonuses - {version}");
        }
    }
}
