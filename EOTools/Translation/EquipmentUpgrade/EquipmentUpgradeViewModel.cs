using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOTools.DataBase;
using EOTools.Models.EquipmentUpgrade;
using EOTools.Tools.EquipmentPicker;
using System.Linq;

namespace EOTools.Translation.EquipmentUpgrade;

public partial class EquipmentUpgradeImprovmentViewModel : ObservableObject
{
    public EquipmentUpgradeImprovmentModel Model { get; set; }

    [ObservableProperty]
    private EquipmentUpgradeConversionViewModel? conversionViewModel;

    public EquipmentUpgradeImprovmentCostViewModel CostViewModel { get; set; } = new(new());

    public string AfterConversionEquipmentName => ConversionViewModel?.Equipment?.NameEN ?? "Select an equipment";

    public EquipmentUpgradeImprovmentViewModel(EquipmentUpgradeImprovmentModel model)
    {
        Model = model;
        LoadFromModel();
    }

    public void LoadFromModel()
    {
        ConversionViewModel = Model.ConversionData is null ? null : new(Model.ConversionData);
        CostViewModel = new(Model.Costs);
    }

    public void SaveChanges()
    {
        ConversionViewModel?.SaveChanges();
        CostViewModel.SaveChanges();

        Model.ConversionData = ConversionViewModel?.Model;
        Model.Costs = CostViewModel.Model;
    }

    [RelayCommand]
    private void OpenEquipmentPicker()
    {
        using EOToolsDbContext db = new();

        EquipmentPickerViewModel vm = new(db.Equipments.ToList());

        EquipmentDataPickerView picker = new(vm);

        if (picker.ShowDialog() == true && vm.SelectedEquipment != null)
        {
            ConversionViewModel = new(new()
            {
                EquipmentLevelAfter = 0,
                IdEquipmentAfter = vm.SelectedEquipment.ApiId
            });

            OnPropertyChanged(nameof(AfterConversionEquipmentName));
        }
    }


    [RelayCommand]
    private void ClearEquipment()
    {
        ConversionViewModel = null;
        OnPropertyChanged(nameof(AfterConversionEquipmentName));
    }
}
