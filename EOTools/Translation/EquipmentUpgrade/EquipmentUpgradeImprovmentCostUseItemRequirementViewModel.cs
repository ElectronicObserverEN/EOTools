using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOTools.DataBase;
using EOTools.Models;
using EOTools.Models.EquipmentUpgrade;
using EOTools.Tools.EquipmentPicker;
using System.Collections.Generic;
using System;
using System.Linq;

namespace EOTools.Translation.EquipmentUpgrade;

public partial class EquipmentUpgradeImprovmentCostUseItemRequirementViewModel : ObservableObject
{
    private EOToolsDbContext DbContext { get; } = new();

    public List<UseItemModel> UseItems { get; } 

    public UseItemModel Item => UseItems.FirstOrDefault(eq => eq.ApiId == Id)!;

    [ObservableProperty]
    private int id;
    [ObservableProperty]
    private int count;

    public EquipmentUpgradeImprovmentCostItemDetail Model { get; set; }

    public EquipmentUpgradeImprovmentCostUseItemRequirementViewModel(EquipmentUpgradeImprovmentCostItemDetail model)
    {
        UseItems = Enum.GetValues<UseItemId>().Select(enu => new UseItemModel()
        {
            ApiId = (int)enu,
            NameEN = enu.ToString()
        }).ToList();

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
        Model.Id = Item.ApiId;
        Model.Count = Count;
    }

    [RelayCommand]
    public void OpenEquipmentPicker()
    {
        EquipmentPickerViewModel vm = new(UseItems.Select(item => new EquipmentModel()
        {
            ApiId = item.ApiId,
            NameEN = item.NameEN
        }).ToList());

        EquipmentDataPickerView picker = new(vm);

        if (picker.ShowDialog() == true && vm.SelectedEquipment != null)
        {
            Id = vm.SelectedEquipment.ApiId;

            OnPropertyChanged(nameof(Item));
        }
    }
}
