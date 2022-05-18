using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOTools.Models;
using EOTools.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOTools.Translation
{
    public class TagTranslationViewModel : ObservableObject
    {

        private string DataFilePath => Path.Combine(ElectronicObserverDataFolderPath, "Translations", "en-US", "Locks.json");
        private string UpdateFilePath => Path.Combine(ElectronicObserverDataFolderPath, "Translations", "en-US", "update.json");

        private string TagDataFilePath => Path.Combine(ElectronicObserverDataFolderPath, "Data", "Locks.json");

        public string ElectronicObserverDataFolderPath
        {
            get
            {
                return AppSettings.ElectronicObserverDataFolderPath;
            }
            set
            {
                AppSettings.ElectronicObserverDataFolderPath = value;
                LoadFromFile();
            }
        }

        public ObservableCollection<TagTranslationData> Translations { get; private set; } = new ObservableCollection<TagTranslationData>();

        #region Commands
        public RelayCommand AddLockCommand { get; private set; }
        public RelayCommand SaveLocksCommand { get; private set; }
        public RelayCommand<TagTranslationData> DeleteLockCommand { get; private set; }
        #endregion

        public TagTranslationViewModel()
        {
            // --- Load from file
            if (!string.IsNullOrEmpty(ElectronicObserverDataFolderPath))
            {
                LoadFromFile();
            }

            AddLockCommand = new RelayCommand(() => Translations.Insert(0, new TagTranslationData()));
            SaveLocksCommand = new RelayCommand(() => SaveToFile());
            DeleteLockCommand = new RelayCommand<TagTranslationData>((translation) => Translations.Remove(translation));
        }

        public void SaveToFile()
        {
            JsonHelper.WriteJsonByOnlyIndentingXTimes(DataFilePath, Translations, 1);
        }

        public void LoadFromFile()
        {
            Translations.Clear();
            if (!File.Exists(DataFilePath)) return;

            List<TagTranslationData> locks = JsonHelper.ReadJson<List<TagTranslationData>>(DataFilePath);

            Translations.Clear();

            foreach (TagTranslationData lockData in locks)
            {
                Translations.Add(lockData);
            }

            // --- Look for untranslated tags

            if (File.Exists(TagDataFilePath))
            {
                LocksPhasesModel lockAndPhases = JsonHelper.ReadJson<LocksPhasesModel>(TagDataFilePath);

                foreach (LockData lockData in lockAndPhases.Locks)
                {
                    if (!locks.Exists(tagData => tagData.NameJP == lockData.Name))
                    {
                        Translations.Add(new TagTranslationData()
                        {
                            NameJP = lockData.Name,
                            NameTranslated = "Not translated"
                        });
                    }
                }
            }
        }
    }
}
