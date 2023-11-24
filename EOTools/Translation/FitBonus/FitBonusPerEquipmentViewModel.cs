using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using EOTools.Models.FitBonus;

namespace EOTools.Translation.FitBonus
{
    public partial class FitBonusPerEquipmentViewModel
    {
        public FitBonusPerEquipmentModel Model { get; set; }

        public ObservableCollection<FitBonusDataViewModel> FitBonusDataList { get; set; } = new ObservableCollection<FitBonusDataViewModel>();

        public FitBonusPerEquipmentViewModel(FitBonusPerEquipmentModel model)
        {
            Model = model;

            if (Model.Bonuses is not null)
            {
                foreach (FitBonusDataModel dataModel in Model.Bonuses)
                {
                    FitBonusDataList.Add(new FitBonusDataViewModel(dataModel));
                }
            }
        }

        public void SaveChanges()
        {
            foreach (FitBonusDataViewModel bonus in FitBonusDataList)
            {
                bonus.SaveChanges();
            }

            Model.Bonuses = FitBonusDataList.Select(vm => vm.Model).ToList();
        }

        [RelayCommand]
        private void AddBonus()
        {
            FitBonusDataList.Add(new(new()));
        }
    }
}
