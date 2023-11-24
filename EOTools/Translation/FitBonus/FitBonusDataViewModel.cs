using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using EOTools.DataBase;
using EOTools.Models.FitBonus;
using EOTools.Models.Ships;

namespace EOTools.Translation.FitBonus
{
    public partial class FitBonusDataViewModel : ObservableObject
    {
        public FitBonusDataModel Model { get; set; }

        /*public int NumberOfEquipmentsRequired { get; set; }

        public int NumberOfEquipmentTypesRequired { get; set; }

        public int NumberOfEquipmentsRequiredAfterOtherFilters { get; set; }*/
           
        public int EquipmentLevel { get; set; }

        public Visibility BonusVisibility => BonusViewModel is null ? Visibility.Collapsed : Visibility.Visible;
        public Visibility BonusAirRadarVisibility => BonusesIfAirRadarViewModel is null ? Visibility.Collapsed : Visibility.Visible;
        public Visibility BonusLosRadarVisibility => BonusesIfLOSRadarViewModel is null ? Visibility.Collapsed : Visibility.Visible;

        [ObservableProperty] private bool _displayBonus = false;
        [ObservableProperty] private bool _displayBonusAirRadar = false;
        [ObservableProperty] private bool _displayBonusLosRadar = false;

        public FitBonusValueViewModel? BonusViewModel { get; set; }
        public FitBonusValueViewModel? BonusesIfAirRadarViewModel { get; set; }
        public FitBonusValueViewModel? BonusesIfLOSRadarViewModel { get; set; }

        public ObservableCollection<ShipModel> ShipsIds { get; set; }

        private EOToolsDbContext Database { get; } = Ioc.Default.GetRequiredService<EOToolsDbContext>();

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

            ShipsIds = model.ShipIds switch
            {
                {} ids => new(ids.Select(id => Database.Ships.First(s => s.ApiId == id))),
                _ => new()
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

        public void SaveChanges()
        {
            if (BonusViewModel is not null)
            {
                BonusViewModel.SaveChanges();
                Model.Bonuses = BonusViewModel.Model;
            }

            if (BonusesIfAirRadarViewModel is not null)
            {
                BonusesIfAirRadarViewModel.SaveChanges();
                Model.BonusesIfAirRadar = BonusesIfAirRadarViewModel.Model;
            }

            if (BonusesIfLOSRadarViewModel is not null)
            {
                BonusesIfLOSRadarViewModel.SaveChanges();
                Model.BonusesIfLOSRadar = BonusesIfLOSRadarViewModel.Model;
            }


            Model.EquipmentLevel = EquipmentLevel switch
            {
                > 0 => EquipmentLevel,
                _ => null
            };

            Model.ShipIds = ShipsIds switch
            {
                not null => ShipsIds.Select(s => s.ApiId).ToList(),
                _ => null
            };
        }
    }
}
