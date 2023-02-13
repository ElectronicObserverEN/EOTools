using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOTools.DataBase;
using EOTools.Models;
using EOTools.Models.EquipmentUpgrade;
using EOTools.Tools.EquipmentPicker;
using System.Linq;

namespace EOTools.Translation.EquipmentUpgrade;

public partial class EquipmentUpgradeImprovmentCostEquipmentRequirementViewModel : ObservableObject
{
    private EOToolsDbContext DbContext { get; } = new();

    public EquipmentModel Equipment => DbContext.Equipments.FirstOrDefault(eq => eq.ApiId == Id)!;

    [ObservableProperty]
    private int id;
    [ObservableProperty]
    private int count;

    public EquipmentUpgradeImprovmentCostItemDetail Model { get; set; }

    public EquipmentUpgradeImprovmentCostEquipmentRequirementViewModel(EquipmentUpgradeImprovmentCostItemDetail model)
    {
        Model = model;
        LoadFromModel();
    }

    public void LoadFromModel()
    {
        Id = Model.Id;
        Count = Model.Count;
    }

    public void SaveChanges()
    {
        Model.Id = Equipment.ApiId;
        Model.Count = Count;
    }

    [RelayCommand]
    public void OpenEquipmentPicker()
    {
        EquipmentPickerViewModel vm = new(DbContext.Equipments.ToList());

        EquipmentDataPickerView picker = new(vm);

        if (picker.ShowDialog() == true && vm.SelectedEquipment != null)
        {
            Id = vm.SelectedEquipment.ApiId;

            OnPropertyChanged(nameof(Equipment));
        }
    }
}
