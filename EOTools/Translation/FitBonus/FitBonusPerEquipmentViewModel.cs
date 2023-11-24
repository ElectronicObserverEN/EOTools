using EOTools.Models;
using System.Collections.ObjectModel;
using EOTools.Models.FitBonus;

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

        public void SaveChanges()
        {
            foreach (FitBonusDataViewModel bonus in FitBonusDataList)
            {
                bonus.SaveChanges();
            }
        }
    }
}
