using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOTools.Models;
using EOTools.Tools;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace EOTools.Translation
{
    public class TagListViewModel : ObservableObject
    {
        public ObservableCollection<int> TagIdsThatCanBeUsed => LocksViewModel.TagIdsThatCanBeUsed;

        private string DataFilePath => Path.Combine(ElectronicObserverDataFolderPath, "Data", "Locks.json");

        private string UpdateFilePath => Path.Combine(ElectronicObserverDataFolderPath, "update.json");

        private GitManager GitManager => new GitManager(ElectronicObserverDataFolderPath);

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


        public LocksViewModel LocksViewModel { get; private set; } = new LocksViewModel();

        public PhasesViewModel PhasesViewModel { get; private set; } = new PhasesViewModel();


        #region Commands
        public RelayCommand AddLockCommand { get; private set; }
        public RelayCommand ResetLocksCommand { get; private set; }
        public RelayCommand SaveLocksCommand { get; private set; }
        public RelayCommand<LockData> DeleteLockCommand { get; private set; }
        #endregion

        public TagListViewModel()
        {
            // --- Load from file
            if (!string.IsNullOrEmpty(ElectronicObserverDataFolderPath))
            {
                LoadFromFile();
            }

            AddLockCommand = new RelayCommand(() => LocksViewModel.Locks.Add(new LockData()));
            ResetLocksCommand = new RelayCommand(() => LocksViewModel.Locks.Clear());
            SaveLocksCommand = new RelayCommand(() => SaveFileStageCommitAndPush());
            DeleteLockCommand = new RelayCommand<LockData>((lockData) => LocksViewModel.Locks.Remove(lockData));
        }

        public void LoadFromFile()
        {
            LocksPhasesModel lockAndPhases = JsonHelper.ReadJson<LocksPhasesModel>(DataFilePath);

            LocksViewModel.Locks.Clear();

            foreach (LockData lockData in lockAndPhases.Locks)
            {
                LocksViewModel.Locks.Add(lockData);
            }

            PhasesViewModel.Phases.Clear();

            foreach (LockPhaseData phaseData in lockAndPhases.Phases)
            {
                PhasesViewModel.Phases.Add(new PhaseViewModel(phaseData));
            }
        }

        public void SaveToFile()
        {
            LocksPhasesModel lockAndPhases = new LocksPhasesModel()
            {
                Locks = new List<LockData>(LocksViewModel.Locks),
                Phases = PhasesViewModel.Phases.Select(phase => phase.Phase).ToList()
            };

            JsonHelper.WriteJsonByOnlyIndentingXTimes(DataFilePath, lockAndPhases, 2);
        }

        private void SaveFileStageCommitAndPush()
        {
            
            SaveToFile();

            // --- Change update.json too
            JObject update = JsonHelper.ReadJsonObject(UpdateFilePath);

            int version = int.Parse(update["Locks"].ToString()) + 1;

            update["Locks"] = version;

            JsonHelper.WriteJson(UpdateFilePath, update);

            // --- Stage & push
            GitManager.Stage(DataFilePath);

            string _updatePath = Path.Combine(UpdateFilePath);
            GitManager.Stage(_updatePath);

            GitManager.CommitAndPush($"Locks data - {version}");
        }
    }

    public class LocksViewModel
    {
        public LocksViewModel()
        {
            Locks.CollectionChanged += Locks_CollectionChanged;
        }

        private void Locks_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            TagIdsThatCanBeUsed.Clear();

            foreach (LockData lockData in Locks)
            {
                TagIdsThatCanBeUsed.Add(lockData.Id);
            }
        }

        public ObservableCollection<int> TagIdsThatCanBeUsed {get; set;}= new ObservableCollection<int>();

        public ObservableCollection<LockData> Locks { get; private set; } = new ObservableCollection<LockData>();
    }

    public class PhasesViewModel
    {
        public ObservableCollection<PhaseViewModel> Phases { get; private set; } = new ObservableCollection<PhaseViewModel>();


    }

    public class PhaseViewModel : ObservableObject
    {
        public LockPhaseData Phase { get; private set; } = new LockPhaseData();

        public string Name
        {
            get
            {
                return Phase.Name;
            }
            set
            {
                Phase.Name = value;
            }
        }

        public ObservableCollection<PhaseTagListItemViewModel> TagIds { get; set; } = new ObservableCollection<PhaseTagListItemViewModel>();


        public RelayCommand AddElementCommand { get; set; }

        public int ComboBoxValue { get; set; }


        public PhaseViewModel(LockPhaseData phase)
        {
            Phase = phase;

            foreach (int lockId in Phase.LockGroups)
            {
                AddElement(lockId);
            }

            TagIds.CollectionChanged += TagIds_CollectionChanged;

            AddElementCommand = new RelayCommand(() => {
                
                if (ComboBoxValue > 0) AddElement(ComboBoxValue);              
                
            });
        }

        private void AddElement(int lockId)
        {
            PhaseTagListItemViewModel vm = new PhaseTagListItemViewModel(lockId);
            vm.DeleteClickedEvent += PhaseTagListItemViewModel_DeleteElement;
            TagIds.Add(vm);
        }

        private void PhaseTagListItemViewModel_DeleteElement(PhaseTagListItemViewModel deletedElement)
        {
            TagIds.Remove(deletedElement);
        }

        private void TagIds_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Phase.LockGroups = new List<int>(TagIds.Select(tagVm => tagVm.TagId));
        }
    }

    public class PhaseTagListItemViewModel : ObservableObject
    {
        public int TagId { get; set; }

        //public int CurrentNumberBoxValue { get; set; }

        public RelayCommand DeleteCommand { get; private set; }

        public PhaseTagListItemViewModel(int tagId)
        {
            TagId = tagId;

            DeleteCommand = new RelayCommand(() => DeleteClickedEvent?.Invoke(this) );
        }


        public delegate void DeleteClickedEventHandler(PhaseTagListItemViewModel deletedElement);

        public event DeleteClickedEventHandler DeleteClickedEvent;

    }
}
