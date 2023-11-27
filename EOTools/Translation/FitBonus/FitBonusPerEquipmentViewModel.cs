using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using EOTools.DataBase;
using EOTools.Models;
using EOTools.Models.FitBonus;
using EOTools.Tools.EquipmentPicker;

namespace EOTools.Translation.FitBonus
{
    public partial class FitBonusPerEquipmentViewModel
    {
        public FitBonusPerEquipmentModel Model { get; set; }

        public ObservableCollection<FitBonusDataViewModel> FitBonusDataList { get; } = new ObservableCollection<FitBonusDataViewModel>();

        public ObservableCollection<EquipmentModel> Equipments { get; } = new();

        private EOToolsDbContext Database { get; }

        public FitBonusPerEquipmentViewModel(FitBonusPerEquipmentModel model)
        {
            Database = Ioc.Default.GetRequiredService<EOToolsDbContext>();
            Model = model;

            if (Model.Bonuses is not null)
            {
                foreach (FitBonusDataModel dataModel in Model.Bonuses)
                {
                    FitBonusDataList.Add(new FitBonusDataViewModel(dataModel));
                }
            }

            if (Model.EquipmentIds is not null)
            {
                List<EquipmentModel> equipments = Model.EquipmentIds
                    .Select(id => Database.Equipments.FirstOrDefault(eq => eq.ApiId == id))
                    .Where(eq => eq is not null)
                    .Cast<EquipmentModel>()
                    .ToList();

                equipments.ForEach(Equipments.Add);
            }
        }

        public void SaveChanges()
        {
            foreach (FitBonusDataViewModel bonus in FitBonusDataList)
            {
                bonus.SaveChanges();
            }

            Model.Bonuses = FitBonusDataList.Select(vm => vm.Model).ToList();

            Model.EquipmentIds = Equipments switch
            {
                { Count: > 0 } => Equipments.Select(eq => eq.ApiId).ToList(),
                _ => null
            };
        }

        [RelayCommand]
        private void AddBonus()
        {
            FitBonusDataList.Add(new(new()));
        }

        [RelayCommand]
        private void AddEquipment()
        {
            EquipmentPickerViewModel vm = new(Database.Equipments.ToList());
            EquipmentDataPickerView picker = new(vm);

            if (picker.ShowDialog() is not true) return;
            if (vm.SelectedEquipment is null) return;

            Equipments.Add(vm.SelectedEquipment);
        }

        [RelayCommand]
        private void RemoveEquipment(EquipmentModel model)
        {
            Equipments.Remove(model);
        }
    }
}
