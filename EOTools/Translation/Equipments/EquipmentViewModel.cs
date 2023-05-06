using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOTools.DataBase;
using EOTools.Models;
using EOTools.Tools;
using EOTools.Translation.EquipmentUpgrade;
using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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

        List<EquipmentUpgradeImprovmentViewModel> upgrades =
            EquipmentUpgradesService.Instance.AllUpgradeModel
            .Where(upg => upg.EquipmentId == ApiId)
            .SelectMany(equ => equ.Improvement)
            .Select(upg => new EquipmentUpgradeImprovmentViewModel(upg, new()))
            .ToList();

        Upgrades = new(upgrades);
    }

    public void SaveChanges()
    {
        Model.NameJP = NameJP;
        Model.NameEN = NameEN;
        Model.ApiId = ApiId;

        /*using EOToolsDbContext db = new();

        foreach (EquipmentUpgradeImprovmentViewModel upg in Upgrades)
        {
            db.Update(upg.Model);
        }

        db.SaveChanges();*/
    }


    private void ShowUpgradeEditDialog(EquipmentUpgradeImprovmentViewModel vm, bool newEntity)
    {
        using EOToolsDbContext db = new();
        EquipmentUpgradeImprovmentViewModel vmEdit = new(vm.Model, db);

        bool saved = false;
        bool canceled = false;

        while (!saved && !canceled)
        {
            EquipmentUpgradeEditView view = new(vmEdit);

            if (view.ShowDialog() == true)
            {
                vmEdit.SaveChanges();
                vm.Model = vmEdit.Model;
                vm.LoadFromModel();

                if (newEntity)
                {
                    Upgrades.Add(vm);
                    db.Add(vm.Model);
                }

                try
                {
                    db.SaveChanges();
                    saved = true;
                }
                catch (Exception ex)
                {
                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }

                    ContentDialog errorDialog = new ContentDialog();
                    errorDialog.Content = $"{ex.Message}\n\n\n\n{ex.StackTrace}";
                    errorDialog.CloseButtonText = "Close";

                    errorDialog.ShowAsync();
                }
            }
            else
            {
                canceled = true;
            }
        }

        EquipmentUpgradesService.Instance.ReloadList();
    }

    [RelayCommand]
    public void ShowAddEquipmentUpgradeDialog()
    {
        EquipmentUpgradeImprovmentViewModel vm = new(new(), new());
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
        using EOToolsDbContext db = new();
        db.Remove(vm.Model);
        db.SaveChanges();
        Upgrades.Remove(vm);
        EquipmentUpgradesService.Instance.ReloadList();
    }
}
