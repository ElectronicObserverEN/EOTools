using EOTools.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EOTools.Translation.FitBonus
{
    public class FitBonusDataViewModel
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

        public Visibility DisplayBonus => Model.Bonuses is null ? Visibility.Collapsed : Visibility.Visible;
        public Visibility DisplayBonusAirRadar => Model.BonusesIfAirRadar is null ? Visibility.Collapsed : Visibility.Visible;
        public Visibility DisplayBonusLOSRadar => Model.BonusesIfLOSRadar is null ? Visibility.Collapsed : Visibility.Visible;

        public FitBonusValueViewModel BonusViewModel => new FitBonusValueViewModel(Model.Bonuses);
        public FitBonusValueViewModel BonusesIfAirRadarViewModel => new FitBonusValueViewModel(Model.BonusesIfAirRadar);
        public FitBonusValueViewModel BonusesIfLOSRadarViewModel => new FitBonusValueViewModel(Model.BonusesIfLOSRadar);

        public FitBonusDataViewModel(FitBonusDataModel model)
        {
            Model = model;
        }
    }
}
