﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using EOTools.DataBase;
using EOTools.Models.FitBonus;
using EOTools.Models.Ships;
using EOTools.Tools.ShipPicker;

namespace EOTools.Translation.FitBonus
{
    public partial class FitBonusDataViewModel : ObservableObject
    {
        public FitBonusDataModel Model { get; set; }

        /*public int NumberOfEquipmentsRequired { get; set; }

        public int NumberOfEquipmentTypesRequired { get; set; }

        public int NumberOfEquipmentsRequiredAfterOtherFilters { get; set; }*/

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
        }

        [RelayCommand]
        private void AddShipMasterId()
        {
            ShipPickerViewModel vm = new();
            ShipDataPickerView picker = new(vm);

            if (picker.ShowDialog() is not true) return;
            if (vm.SelectedShip is null) return;

            ShipsMasterIds.Add(vm.SelectedShip);
        }

        [RelayCommand]
        private void AddShipId()
        {
            ShipPickerViewModel vm = new();
            ShipDataPickerView picker = new(vm);

            if (picker.ShowDialog() is not true) return;
            if (vm.SelectedShip is null) return;

            ShipsIds.Add(vm.SelectedShip);
        }
    }
}
