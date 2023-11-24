using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOTools.DataBase;
using EOTools.Models.Ships;
using EOTools.Tools;
using ModernWpf.Controls;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EOTools.Translation.Ships;

public partial class ShipManagerViewModel : ObservableObject
{
    [ObservableProperty]
    private string filter = "";

    private List<ShipViewModel> Ships { get; set; } = new();

    [ObservableProperty]
    private List<ShipViewModel> shipsFiltered = new();

    public ShipManagerViewModel()
    {
        using EOToolsDbContext db = new();
        Ships = new(db.Ships.Select(sh => new ShipViewModel(sh)));

        ReloadShipList();

        PropertyChanged += (_, e) =>
        {
            if (e.PropertyName is nameof(filter))
            {
                ReloadShipList();
            }
        };
    }


    private void ReloadShipList()
    {
        ShipsFiltered = Ships.Where(ship => string.IsNullOrEmpty(Filter) || ship.NameEN.Contains(Filter)).ToList();
    }

    private async Task ShowEditDialog(ShipViewModel vm, bool newEntity)
    {
        ShipViewModel vmEdit = new(vm.Model);

        ShipEditView view = new(vmEdit);

        if (view.ShowDialog() == true)
        {
            vm.ApiId = vmEdit.ApiId;
            vm.NameJP = vmEdit.NameJP;
            vm.NameEN = vmEdit.NameEN;

            try
            {
                vm.SaveChanges();
                using EOToolsDbContext db = new();

                if (newEntity)
                {
                    db.Ships.Add(vm.Model);
                }
                else
                {
                    db.Update(vm.Model);
                }

                db.SaveChanges();

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
        ShipViewModel vm = new(new());
        await ShowEditDialog(vm, true);
    }

    [RelayCommand]
    public async Task EditShip(ShipViewModel vm)
    {
        await ShowEditDialog(vm, false);
    }

    [RelayCommand]
    public void RemoveShip(ShipViewModel vm)
    {
        using EOToolsDbContext db = new();
        db.Remove(vm.Model);
        db.SaveChanges();
        Ships.Remove(vm);
        ReloadShipList();
    }

    [RelayCommand]
    public async Task ImportFromAPI()
    {
        if (string.IsNullOrEmpty(AppSettings.KancolleEOAPIFolder)) return;

        string importPath = Path.Combine(AppSettings.KancolleEOAPIFolder, "kcsapi", "api_start2", "getData");

        JObject data = JsonHelper.ReadKCJson(importPath);
        
        using EOToolsDbContext db = new();

        foreach (JObject shipJson in data["api_data"]["api_mst_ship"].Children<JObject>())
        {
            int apiId = int.Parse(shipJson["api_id"].ToString());
            string nameJp = shipJson["api_name"].ToString();
            int? classId = null;

            if (shipJson.ContainsKey("api_ctype") && apiId < 1500)
            {
                classId = int.Parse(shipJson["api_ctype"].ToString());
            }

            ShipViewModel? vm = Ships.Find(sh => sh.Model.ApiId == apiId);

            if (vm is null)
            {
                ShipModel model = new()
                {
                    ApiId = apiId,
                    NameJP = nameJp
                };

                model.NameEN = model.GetNameEN();

                db.Add(model);

                Ships.Add(new(model));
            }
            else
            {
                // Update ship class id
                vm.NameEN = vm.Model.GetNameEN();
                vm.ClassId = classId;
                vm.SaveChanges();

                db.Ships.Update(vm.Model);
            }
        }

        db.SaveChanges();

        ReloadShipList();
    }
}
