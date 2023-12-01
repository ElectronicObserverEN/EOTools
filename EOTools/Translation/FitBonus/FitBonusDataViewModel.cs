using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using EOTools.DataBase;
using EOTools.Models;
using EOTools.Models.FitBonus;
using EOTools.Models.Ships;
using EOTools.Tools.EquipmentPicker;
using EOTools.Translation.Ships.ShipClass;
using EOTools.Translation.Ships.ShipList;
using EOTools.Translation.Ships.ShipNationality;
using EOTools.Translation.Ships.ShipType;

namespace EOTools.Translation.FitBonus
{
    public partial class FitBonusDataViewModel : ObservableObject
    {
        public FitBonusDataModel Model { get; set; }

        /*public int NumberOfEquipmentsRequired { get; set; }

        public int NumberOfEquipmentTypesRequired { get; set; }*/

        public int EquipmentRequiresLevel { get; set; }

        public int NumberOfEquipmentsRequiredAfterOtherFilters { get; set; }

        public int EquipmentLevel { get; set; }

        public Visibility BonusVisibility => DisplayBonus ? Visibility.Visible : Visibility.Collapsed;

        public Visibility BonusAirRadarVisibility => DisplayBonusAirRadar ? Visibility.Visible : Visibility.Collapsed;

        public Visibility BonusLosRadarVisibility => DisplayBonusLosRadar ? Visibility.Visible : Visibility.Collapsed;

        [ObservableProperty] private bool _displayBonus = false;
        [ObservableProperty] private bool _displayBonusAirRadar = false;
        [ObservableProperty] private bool _displayBonusLosRadar = false;

        public FitBonusValueViewModel BonusViewModel { get; set; }
        public FitBonusValueViewModel BonusesIfAirRadarViewModel { get; set; }
        public FitBonusValueViewModel BonusesIfLOSRadarViewModel { get; set; }

        public ObservableCollection<ShipModel> ShipsIds { get; set; }
        public ObservableCollection<ShipModel> ShipsMasterIds { get; set; }
        public ObservableCollection<ShipClassModel> ShipClasses { get; set; }
        public ObservableCollection<ShipTypesViewModel> ShipTypeList { get; set; }
        public ObservableCollection<ShipNationalityViewModel> ShipNationalities { get; set; }
        public ObservableCollection<EquipmentModel> EquipmentRequired { get; set; }

        private EOToolsDbContext Database { get; } = Ioc.Default.GetRequiredService<EOToolsDbContext>();

        public FitBonusDataViewModel(FitBonusDataModel model)
        {
            Model = model;

            BonusViewModel = Model.Bonuses switch
            {
                { } => new(Model.Bonuses),
                _ => new(new()),
            };

            BonusesIfAirRadarViewModel = Model.BonusesIfAirRadar switch
            {
                { } => new(Model.BonusesIfAirRadar),
                _ => new(new()),
            };

            BonusesIfLOSRadarViewModel = Model.BonusesIfLOSRadar switch
            {
                { } => new(Model.BonusesIfLOSRadar),
                _ => new(new()),
            };

            ShipsIds = model.ShipIds switch
            {
                { } ids => new(ids.Select(id => Database.Ships.First(s => s.ApiId == id))),
                _ => new()
            };

            ShipsMasterIds = model.ShipMasterIds switch
            {
                { } ids => new(ids.Select(id => Database.Ships.First(s => s.ApiId == id))),
                _ => new()
            };

            ShipClasses = model.ShipClasses switch
            {
                { } ids => new(ids.Select(id => Database.ShipClass.First(s => s.ApiId == id))),
                _ => new()
            };

            ShipNationalities = model.ShipNationalities switch
            {
                { } ids => new(ids.Select(id => new ShipNationalityViewModel() { Nationality = id })),
                _ => new()
            };

            ShipTypeList = model.ShipTypes switch
            {
                { } ids => new(ids.Select(id => new ShipTypesViewModel() { ShipType = id })),
                _ => new()
            };

            EquipmentRequired = model.EquipmentRequired switch
            {
                { } ids => new(ids.Select(id => Database.Equipments.First(eq => eq.ApiId == id))),
                _ => new()
            };

            NumberOfEquipmentsRequiredAfterOtherFilters = model.NumberOfEquipmentsRequiredAfterOtherFilters ?? 0;
            EquipmentRequiresLevel = model.EquipmentRequiresLevel ?? 0;
            EquipmentLevel = model.EquipmentLevel ?? 0;

            DisplayBonus = Model.Bonuses is not null;
            DisplayBonusAirRadar = Model.BonusesIfAirRadar is not null;
            DisplayBonusLosRadar = Model.BonusesIfLOSRadar is not null;

            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName is nameof(DisplayBonus))
                {
                    BonusViewModel.Model = new();
                    BonusViewModel.LoadFromModel();

                    OnPropertyChanged(nameof(BonusVisibility));
                }

                if (args.PropertyName is nameof(DisplayBonusAirRadar))
                {
                    BonusesIfAirRadarViewModel.Model = new();
                    BonusesIfAirRadarViewModel.LoadFromModel();

                    OnPropertyChanged(nameof(BonusAirRadarVisibility));
                }

                if (args.PropertyName is nameof(DisplayBonusLosRadar))
                {
                    BonusesIfLOSRadarViewModel.Model = new();
                    BonusesIfLOSRadarViewModel.LoadFromModel();

                    OnPropertyChanged(nameof(BonusLosRadarVisibility));
                }
            };
        }

        public void SaveChanges()
        {
            if (DisplayBonus)
            {
                BonusViewModel.SaveChanges();
                Model.Bonuses = BonusViewModel.Model;
            }
            else
            {
                Model.Bonuses = null;
            }

            if (DisplayBonusAirRadar)
            {
                BonusesIfAirRadarViewModel.SaveChanges();
                Model.BonusesIfAirRadar = BonusesIfAirRadarViewModel.Model;
            }
            else
            {
                Model.BonusesIfAirRadar = null;
            }

            if (DisplayBonusLosRadar)
            {
                BonusesIfLOSRadarViewModel.SaveChanges();
                Model.BonusesIfLOSRadar = BonusesIfLOSRadarViewModel.Model;
            }
            else
            {
                Model.BonusesIfLOSRadar = null;
            }

            Model.EquipmentLevel = EquipmentLevel switch
            {
                > 0 => EquipmentLevel,
                _ => null
            };

            Model.ShipIds = ShipsIds switch
            {
                { Count: > 0 } => ShipsIds.Select(s => s.ApiId).ToList(),
                _ => null
            };

            Model.ShipMasterIds = ShipsMasterIds switch
            {
                { Count: > 0 } => ShipsMasterIds.Select(s => s.ApiId).ToList(),
                _ => null
            };

            Model.ShipClasses = ShipClasses switch
            {
                { Count: > 0 } => ShipClasses.Select(s => s.ApiId).ToList(),
                _ => null
            };

            Model.ShipNationalities = ShipNationalities switch
            {
                { Count: > 0 } => ShipNationalities.Select(s => s.Nationality).ToList(),
                _ => null
            };

            Model.ShipTypes = ShipTypeList switch
            {
                { Count: > 0 } => ShipTypeList.Select(s => s.ShipType).ToList(),
                _ => null
            };

            Model.EquipmentRequired = EquipmentRequired switch
            {
                { Count: > 0 } => EquipmentRequired.Select(s => s.ApiId).ToList(),
                _ => null
            };

            Model.NumberOfEquipmentsRequiredAfterOtherFilters = NumberOfEquipmentsRequiredAfterOtherFilters switch
            {
                > 0 => NumberOfEquipmentsRequiredAfterOtherFilters,
                _ => null
            };

            Model.EquipmentRequiresLevel = EquipmentRequiresLevel switch
            {
                > 0 => EquipmentRequiresLevel,
                _ => null
            };
        }

        [RelayCommand]
        private void AddShipMasterId()
        {
            ShipListViewModel vm = new();
            ShipListView picker = new(vm);

            if (picker.ShowDialog() is not true) return;
            if (vm.SelectedShip is null) return;

            ShipsMasterIds.Add(vm.SelectedShip.Model);
        }

        [RelayCommand]
        private void RemoveShipMasterId(ShipModel model)
        {
            ShipsMasterIds.Remove(model);
        }

        [RelayCommand]
        private void AddShipId()
        {
            ShipListViewModel vm = new();
            ShipListView picker = new(vm);

            if (picker.ShowDialog() is not true) return;
            if (vm.SelectedShip is null) return;

            ShipsIds.Add(vm.SelectedShip.Model);
        }

        [RelayCommand]
        private void RemoveShipId(ShipModel model)
        {
            ShipsIds.Remove(model);
        }

        [RelayCommand]
        private void AddShipClass()
        {
            ShipClassListViewModel vm = new();
            ShipClassListView picker = new(vm);

            if (picker.ShowDialog() is not true) return;
            if (vm.SelectedClass is null) return;

            ShipClasses.Add(vm.SelectedClass.Model);
        }

        [RelayCommand]
        private void RemoveShipClass(ShipClassModel model)
        {
            ShipClasses.Remove(model);
        }

        [RelayCommand]
        private void AddNationality()
        {
            ShipNationalityViewModel vm = new();
            ShipNationalities.Add(vm);

            vm.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName is not nameof(ShipNationalityViewModel.Nationality)) return;

                if (vm.Nationality is ShipNationality.Remove)
                {
                    ShipNationalities.Remove(vm);
                }
            };
        }

        [RelayCommand]
        private void AddShipType()
        {
            ShipTypesViewModel vm = new();
            ShipTypeList.Add(vm);

            vm.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName is not nameof(ShipTypesViewModel.ShipType)) return;

                if (vm.ShipType is ShipTypes.Remove)
                {
                    ShipTypeList.Remove(vm);
                }
            };
        }

        [RelayCommand]
        private void AddEquipment()
        {
            EquipmentPickerViewModel vm = new(Database.Equipments.ToList());
            EquipmentDataPickerView picker = new(vm);

            if (picker.ShowDialog() is not true) return;
            if (vm.SelectedEquipment is null) return;

            EquipmentRequired.Add(vm.SelectedEquipment);
        }

        [RelayCommand]
        private void RemoveEquipment(EquipmentModel model)
        {
            EquipmentRequired.Remove(model);
        }
    }
}
