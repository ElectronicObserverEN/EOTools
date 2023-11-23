using EOTools.Models;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;

namespace EOTools.Translation.FitBonus
{
    public partial class FitBonusDataViewModel : ObservableObject
    {
        public FitBonusDataModel Model { get; set; }

        public int NumberOfEquipmentsRequired
        {
            get
            {
                return Model.NumberOfEquipmentsRequired ?? 0;
            }
            set
            {
                Model.NumberOfEquipmentsRequired = value == 0 ? null : value;
            }
        }

        public int NumberOfEquipmentTypesRequired
        {
            get
            {
                return Model.NumberOfEquipmentTypesRequired ?? 0;
            }
            set
            {
                Model.NumberOfEquipmentTypesRequired = value == 0 ? null : value;
            }
        }

        public int EquipmentLevel
        {
            get
            {
                return Model.EquipmentLevel ?? 0;
            }
            set
            {
                Model.EquipmentLevel = value == 0 ? null : value;
            }
        }

        public int NumberOfEquipmentsRequiredAfterOtherFilters
        {
            get
            {
                return Model.NumberOfEquipmentsRequiredAfterOtherFilters ?? 0;
            }
            set
            {
                Model.NumberOfEquipmentsRequiredAfterOtherFilters = value == 0 ? null : value;
            }
        }

        public Visibility BonusVisibility => BonusViewModel is null ? Visibility.Collapsed : Visibility.Visible;
        public Visibility BonusAirRadarVisibility => BonusesIfAirRadarViewModel is null ? Visibility.Collapsed : Visibility.Visible;
        public Visibility BonusLosRadarVisibility => BonusesIfLOSRadarViewModel is null ? Visibility.Collapsed : Visibility.Visible;

        [ObservableProperty] private bool _displayBonus = false;
        [ObservableProperty] private bool _displayBonusAirRadar = false;
        [ObservableProperty] private bool _displayBonusLosRadar = false;

        public FitBonusValueViewModel? BonusViewModel { get; set; }
        public FitBonusValueViewModel? BonusesIfAirRadarViewModel { get; set; }
        public FitBonusValueViewModel? BonusesIfLOSRadarViewModel { get; set; }

        public FitBonusDataViewModel(FitBonusDataModel model)
        {
            Model = model;

            BonusViewModel = Model.Bonuses switch
            {
                { } => new(Model.Bonuses),
                _ => null
            };

            BonusesIfAirRadarViewModel = Model.BonusesIfAirRadar switch
            {
                { } => new(Model.BonusesIfAirRadar),
                _ => null
            };

            BonusesIfLOSRadarViewModel = Model.BonusesIfLOSRadar switch
            {
                { } => new(Model.BonusesIfLOSRadar),
                _ => null
            };

            DisplayBonus = Model.Bonuses is not null;
            DisplayBonusAirRadar = Model.BonusesIfAirRadar is not null;
            DisplayBonusLosRadar = Model.BonusesIfLOSRadar is not null;

            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName is nameof(DisplayBonus))
                {
                    BonusViewModel = DisplayBonus switch
                    {
                        true => new FitBonusValueViewModel(new FitBonusValueModel()),
                        _ => null
                    };

                    OnPropertyChanged(nameof(BonusVisibility));
                }

                if (args.PropertyName is nameof(DisplayBonusAirRadar))
                {
                    BonusesIfAirRadarViewModel = DisplayBonusAirRadar switch
                    {
                        true => new FitBonusValueViewModel(new FitBonusValueModel()),
                        _ => null
                    };

                    OnPropertyChanged(nameof(BonusAirRadarVisibility));
                }

                if (args.PropertyName is nameof(DisplayBonusLosRadar))
                {
                    BonusesIfLOSRadarViewModel = DisplayBonusLosRadar switch
                    {
                        true => new FitBonusValueViewModel(new FitBonusValueModel()),
                        _ => null
                    };

                    OnPropertyChanged(nameof(BonusLosRadarVisibility));
                }
            };
        }
    }
}
