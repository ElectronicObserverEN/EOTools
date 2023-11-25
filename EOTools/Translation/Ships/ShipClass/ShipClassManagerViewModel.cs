using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using EOTools.DataBase;
using EOTools.Migrations;
using EOTools.Models;
using EOTools.Models.Ships;
using EOTools.Tools;
using EOTools.Tools.Translations;
using ModernWpf.Controls;
using Newtonsoft.Json.Linq;

namespace EOTools.Translation.Ships.ShipClass;

public partial class ShipClassManagerViewModel : ObservableObject
{
    [ObservableProperty]
    private string _filter = "";

    private List<ShipClassViewModel> ShipClass { get; }

    [ObservableProperty]
    private List<ShipClassViewModel> _shipClassListFiltered = new();

    public ShipClassManagerViewModel()
    {
        using EOToolsDbContext db = new();
        ShipClass = new(db.ShipClass.Select(sh => new ShipClassViewModel(sh)));

        ReloadShipList();

        PropertyChanged += (_, e) =>
        {
            if (e.PropertyName is nameof(Filter))
            {
                ReloadShipList();
            }
        };
    }


    private void ReloadShipList()
    {
        ShipClassListFiltered = ShipClass.Where(ship => string.IsNullOrEmpty(Filter) || ship.NameEnglish.Contains(Filter)).ToList();
    }

    private async Task ShowEditDialog(ShipClassViewModel vm, bool newEntity)
    {
        ShipClassViewModel vmEdit = new(vm.Model);

        ShipClassEditView view = new(vmEdit);

        if (view.ShowDialog() == true)
        {
            vm.ApiId = vmEdit.ApiId;
            vm.NameJapanese = vmEdit.NameJapanese;
            vm.NameEnglish = vmEdit.NameEnglish;

            try
            {
                vm.SaveChanges();
                await using EOToolsDbContext db = new();

                if (newEntity)
                {
                    db.ShipClass.Add(vm.Model);
                }
                else
                {
                    db.Update(vm.Model);
                }

                await db.SaveChangesAsync();

                ReloadShipList();
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

                await errorDialog.ShowAsync();

                await ShowEditDialog(vm, newEntity);
            }
        }
    }

    [RelayCommand]
    public async Task ShowAddShipDialog()
    {
        ShipClassViewModel vm = new(new());
        await ShowEditDialog(vm, true);
    }

    [RelayCommand]
    public async Task EditShip(ShipClassViewModel vm)
    {
        await ShowEditDialog(vm, false);
    }

    [RelayCommand]
    public void RemoveShip(ShipClassViewModel vm)
    {
        using EOToolsDbContext db = new();
        db.Remove(vm.Model);
        db.SaveChanges();
        ShipClass.Remove(vm);
        ReloadShipList();
    }

    [RelayCommand]
    public async Task ImportFromAPI()
    {
        await using EOToolsDbContext db = new();
        ShipTranslationService translationService = Ioc.Default.GetRequiredService<ShipTranslationService>();

        List<ShipClassModel> shipClass = db.ShipClass.ToList();

        foreach (ShipClassModel classModel in shipClass)
        {
            classModel.NameEnglish = translationService.Class(classModel.NameJapanese);
            db.Update(classModel);
        }

        await db.SaveChangesAsync();

        ReloadShipList();
    }
}
