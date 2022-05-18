using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOTools.Models;
using EOTools.Tools;
using Microsoft.Win32;
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
        private string DataFilePath => Path.Combine(ElectronicObserverDataFolderPath, "Data", "Locks.json");

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

            AddLockCommand = new RelayCommand(() => LocksViewModel.Locks.Add(new LockData()) );
            ResetLocksCommand = new RelayCommand(() => LocksViewModel.Locks.Clear() );
            SaveLocksCommand = new RelayCommand(() => SaveToFile() );
            DeleteLockCommand = new RelayCommand<LockData>((lockData) => LocksViewModel.Locks.Remove(lockData) );
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

    }

    internal class LocksPhasesModel
    {
        public List<LockData> Locks { get; set; } = new List<LockData>();

        public List<LockPhaseData> Phases { get; set; } = new List<LockPhaseData>();
    }

    public class LocksViewModel
    {
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

        public ObservableCollection<int> TagIds { get; set; } = new ObservableCollection<int>();

            

            
        public PhaseViewModel(LockPhaseData phase)
        {
            Phase = phase; 
            
            foreach (int lockId in Phase.LockGroups)
            {
                TagIds.Add(lockId);
            }

            TagIds.CollectionChanged += TagIds_CollectionChanged;
        }

        private void TagIds_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Phase.LockGroups = new List<int>(TagIds);
        }
    }

}
