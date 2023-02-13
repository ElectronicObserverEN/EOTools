using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOTools.Models;
using EOTools.Models.EquipmentUpgrade;
using EOTools.Translation.EquipmentUpgrade;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;

namespace EOTools.Translation.Equipments;

public partial class EquipmentViewModel : ObservableObject
{
    [ObservableProperty]
    private string nameEN = "";

    [ObservableProperty]
    private string nameJP = "";

    [ObservableProperty]
    private int apiId;

    public EquipmentModel Model { get; set; }

    public ObservableCollection<EquipmentUpgradeImprovmentViewModel> Upgrades { get; set; }

    public EquipmentViewModel(EquipmentModel model)
    {
        Model = model;

        NameEN = model.NameEN;
        NameJP = model.NameJP;
        ApiId = model.ApiId;

        List<EquipmentUpgradeImprovmentViewModel> upgrades = model.UpgradeData switch
        {
            string => JsonSerializer.Deserialize<EquipmentUpgradeDataModel>(model.UpgradeData).Improvement.Select(upg => new EquipmentUpgradeImprovmentViewModel(upg)).ToList(),
            _ => new()
        };

        Upgrades = new(upgrades);
    }

    public void SaveChanges()
    {
        Model.NameJP = NameJP;
        Model.NameEN = NameEN;
        Model.ApiId = ApiId;

        EquipmentUpgradeDataModel upgrades = new()
        {
            EquipmentId = ApiId,
            ConvertTo = new(),
            Improvement = Upgrades.Select(upg => upg.Model).ToList(),
            UpgradeFor = new(),
        };

        Model.UpgradeData = JsonSerializer.Serialize(upgrades);
    }


    private void ShowUpgradeEditDialog(EquipmentUpgradeImprovmentViewModel vm, bool newEntity)
    {
        EquipmentUpgradeImprovmentViewModel vmEdit = new(vm.Model);

        EquipmentUpgradeEditView view = new(vmEdit);

        if (view.ShowDialog() == true)
        {
            vmEdit.SaveChanges();
            vm.Model = vmEdit.Model;
            vm.LoadFromModel();

            if (newEntity)
            {
                Upgrades.Add(vm);
            }
        }
    }

    [RelayCommand]
    public void ShowAddEquipmentUpgradeDialog()
    {
        EquipmentUpgradeImprovmentViewModel vm = new(new());
        ShowUpgradeEditDialog(vm, true);
    }

    [RelayCommand]
    public void EditEquipmentUpgrade(EquipmentUpgradeImprovmentViewModel vm)
    {
        ShowUpgradeEditDialog(vm, false);
    }

    [RelayCommand]
    public void RemoveEquipmentUpgrade(EquipmentUpgradeImprovmentViewModel vm)
    {
        Upgrades.Remove(vm);
    }
}
