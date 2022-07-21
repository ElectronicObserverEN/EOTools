using EOTools.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOTools.Translation.FitBonus
{
    public class FitBonusPerEquipmentViewModel
    {
        public FitBonusPerEquipmentModel Model { get; set; }

        public ObservableCollection<FitBonusDataViewModel> FitBonusDataList { get; set; } = new ObservableCollection<FitBonusDataViewModel>();

        public FitBonusPerEquipmentViewModel(FitBonusPerEquipmentModel model)
        {
            Model = model;

            foreach (FitBonusDataModel dataModel in Model.Bonuses)
            {
                FitBonusDataList.Add(new FitBonusDataViewModel(dataModel));
            }
        }
    }
}
