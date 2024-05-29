using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using EOTools.DataBase;
using EOTools.Migrations;
using EOTools.Models.Ships;
using EOTools.Tools;
using EOTools.Tools.Translations;
using Microsoft.EntityFrameworkCore;
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
        Ships = new(db.Ships
            .Include(nameof(ShipModel.ShipClass))
            .Select(sh => new ShipViewModel(sh)));

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
            vm.ShipClass = vmEdit.ShipClass;

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
        ShipTranslationService translationService = Ioc.Default.GetRequiredService<ShipTranslationService>();

        foreach (JObject shipJson in data["api_data"]["api_mst_ship"].Children<JObject>())
        {
            int apiId = int.Parse(shipJson["api_id"].ToString());
            string nameJp = shipJson["api_name"].ToString();
            ShipClassModel? shipClass = null;

            if (shipJson.ContainsKey("api_ctype") && apiId < 1500)
            {
                int classId = int.Parse(shipJson["api_ctype"].ToString());

                string nameJapanese = GetShipClassUntranslated(classId, apiId);
                shipClass = db.ShipClass.FirstOrDefault(sc => sc.ApiId == classId && sc.NameJapanese == nameJapanese);

                if (shipClass is null)
                {
                    shipClass = new()
                    {
                        ApiId = classId,
                        NameJapanese = nameJapanese,
                        NameEnglish = translationService.Class(nameJapanese)
                    };

                    await db.ShipClass.AddAsync(shipClass);
                }
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
                vm.ShipClass = shipClass;
                vm.SaveChanges();

                db.Ships.Update(vm.Model);
            }
        }

        await db.SaveChangesAsync();

        ReloadShipList();
    }

    /// <summary>
    /// 艦型を表す文字列を取得します。
    /// </summary>
    private string GetShipClassUntranslated(int id, int shipId) => id switch
    {
        1 => "綾波型",
        2 => "伊勢型",
        3 => "加賀型",
        4 => "球磨型",
        5 => "暁型",
        6 => "金剛型",
        7 => "古鷹型",
        8 => "高雄型",
        9 => "最上型",
        10 => "初春型",
        11 => "祥鳳型",
        12 => "吹雪型",
        13 => "青葉型",
        14 => "赤城型",
        15 => "千歳型",
        16 => "川内型",
        17 => "蒼龍型",
        18 => "朝潮型",
        19 => "長門型",
        20 => "長良型",
        21 => "天龍型",
        22 => "島風型",
        23 => "白露型",
        24 => "飛鷹型",
        25 => "飛龍型",
        26 => "扶桑型",
        27 => "鳳翔型",
        28 => "睦月型",
        29 => "妙高型",
        30 => "陽炎型",
        31 => "利根型",
        32 => "龍驤型",
        33 => "翔鶴型",
        34 => "夕張型",
        35 => "海大VI型",
        36 => "巡潜乙型改二",
        37 => "大和型",
        38 => "夕雲型",
        39 => "巡潜乙型",
        40 => "巡潜3型",
        41 => "阿賀野型",
        42 => "「霧」",
        43 => "大鳳型",
        44 => "潜特型(伊400型潜水艦)",
        45 => "特種船丙型",
        46 => "三式潜航輸送艇",
        47 => "Bismarck級",
        48 => "Z1型",
        49 => "工作艦",
        50 => "大鯨型",
        51 => "龍鳳型",
        52 => "大淀型",
        53 => "雲龍型",
        54 => "秋月型",
        55 => "Admiral Hipper級",
        56 => "香取型",
        57 => "UボートIXC型",
        58 => "V.Veneto級",
        59 => "秋津洲型",
        60 => "改風早型",
        61 => "Maestrale級",
        62 => "瑞穂型",
        63 => "Graf Zeppelin級",
        64 => "Zara級",
        65 => "Iowa級",
        66 => "神風型",
        67 => "Queen Elizabeth級",
        68 => "Aquila級",
        69 => "Lexington級",
        70 => "C.Teste級",
        71 => "巡潜甲型改二",
        72 => "神威型",
        73 => "Гангут級",
        74 => "占守型",
        75 => "春日丸級",
        76 => "大鷹型",
        77 => "択捉型",
        78 => "Ark Royal級",
        79 => "Richelieu級",
        80 => "Guglielmo Marconi級",
        81 => "Ташкент級",
        82 => "J級",
        83 => "Casablanca級",
        84 => "Essex級",
        85 => "日振型",
        86 => "呂号潜水艦",
        87 => "John C.Butler級",
        88 => "Nelson級",
        89 => "Gotland級",
        90 => "日進型",
        91 => "Fletcher級",
        92 => "L.d.S.D.d.Abruzzi級",
        93 => "Colorado級",
        94 => "御蔵型",
        95 => "Northampton級",
        96 => "Perth級",
        97 => "陸軍特種船(R1)",
        98 => "De Ruyter級",
        99 => "Atlanta級",
        100 => "迅鯨型",
        101 => "松型",
        102 => "South Dakota級",
        103 => "巡潜丙型",
        104 => "丁型海防艦",
        105 => "Yorktown級",
        106 => "St. Louis級",
        107 => "North Carolina級",
        108 => "Town級",
        109 => "潜高型",
        110 => "Brooklyn級",
        111 when shipId is 699 => "耐氷型雑用運送艦",
        111 when shipId is 645 => "LL01",
        111 when shipId is 650 => "PL107",
        112 => "Illustrious級",
        113 => "Conte di Cavour級",
        114 => "Gato級",
        115 => "特2TL型",
        116 => "Independence級",
        117 => "鵜来型",
        118 => "Ranger級",
        119 => "特種船M丙型",
        120 => "二等輸送艦",
        121 => "New Orleans級",
        122 => "Salmon級",
        123 => "改敷島型",
        124 => "Marcello級",
        125 => "Nevada級",
        126 => "改氷川丸級",
        127 => "巡潜乙型改一",
        _ => "不明",
    };

    [RelayCommand]
    private void PushTranslations()
    {
        UpdateShipDataService service = Ioc.Default.GetRequiredService<UpdateShipDataService>();
        service.UpdateShipTranslations();
    }
}
