using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOTools.DataBase;
using EOTools.Models.Ships;
using EOTools.Tools;
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
        if (string.IsNullOrEmpty(AppSettings.KancolleEOAPIFolder)) return;

        string importPath = Path.Combine(AppSettings.KancolleEOAPIFolder, "kcsapi", "api_start2", "getData");

        JObject data = JsonHelper.ReadKCJson(importPath);

        List<JToken> ships = data["api_data"]["api_mst_ship"].AsJEnumerable().ToList();

        using EOToolsDbContext db = new();

        foreach (JToken shipJson in ships)
        {
            ShipClassModel model = new()
            {
                ApiId = int.Parse(shipJson["api_id"].ToString()),
                NameJapanese = shipJson["api_name"].ToString()
            };

            if (!ShipClass.Any(sh => sh.Model.ApiId == model.ApiId))
            {
                //model.NameEnglish = model.GetNameEN();
                db.Add(model);

                ShipClass.Add(new(model));
            }
        }

        db.SaveChanges();

        ReloadShipList();
    }
}
