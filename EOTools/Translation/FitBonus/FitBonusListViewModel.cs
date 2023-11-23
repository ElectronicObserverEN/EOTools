using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using EOTools.DataBase;
using EOTools.Tools.EquipmentPicker;

namespace EOTools.Translation.FitBonus;

public partial class FitBonusListViewModel : ObservableObject
{
    public string SelectedEquipmentName => SelectedEquipment switch
    {
        {} id => DbContext.Equipments.FirstOrDefault(eq => eq.ApiId == id)?.NameEN ?? "Select an equipment",
        _ => "Select an equipment",
    };

    private int? SelectedEquipment { get; set; }

    private EOToolsDbContext DbContext { get; } = new();

    public FitBonusManager BonusManager { get; }

    [ObservableProperty]
    private List<FitBonusPerEquipmentViewModel> _fitBonuses = new();

    public FitBonusListViewModel()
    {
        BonusManager = Ioc.Default.GetRequiredService<FitBonusManager>();
    }

    [RelayCommand]
    private void OpenEquipmentPicker()
    {
        EquipmentPickerViewModel vm = new(DbContext.Equipments.ToList());

        EquipmentDataPickerView picker = new(vm);

        if (picker.ShowDialog() == true && vm.SelectedEquipment != null)
        {
            SelectedEquipment = vm.SelectedEquipment.ApiId;
            OnPropertyChanged(nameof(SelectedEquipmentName));
            FitBonuses = BonusManager.FitBonuses
                .Where(fit => fit.Model.EquipmentIds?.Contains(SelectedEquipment) is true)
                .ToList();
        }
    }
}

