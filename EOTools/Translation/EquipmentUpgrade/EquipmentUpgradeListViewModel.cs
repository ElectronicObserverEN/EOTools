using CommunityToolkit.Mvvm.Input;
using EOTools.Models.EquipmentUpgrade;
using EOTools.Models.EquipmentUpgrade.Source;
using EOTools.Tools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Windows.Forms;

namespace EOTools.Translation.EquipmentUpgrade;

public partial class EquipmentUpgradeListViewModel
{
    private GitManager GitManager
    {
        get
        {
            return new GitManager(ElectronicObserverDataFolderPath);
        }
    }

    private string ElectronicObserverDataFolderPath
    {
        get
        {
            return AppSettings.ElectronicObserverDataFolderPath;
        }
        set
        {
            AppSettings.ElectronicObserverDataFolderPath = value;
            LoadFile();
        }
    }

    public string EquipmentUpgradeFilePath => Path.Combine(ElectronicObserverDataFolderPath, "Data", "EquipmentUpgrades.json");
    public string UpdateFilePath => Path.Combine(ElectronicObserverDataFolderPath, "update.json");

    public ObservableCollection<EquipmentUpgradeDataModel> EquipmentUpgrades { get; set; } = new ObservableCollection<EquipmentUpgradeDataModel>();


    public EquipmentUpgradeListViewModel()
    {
        if (!string.IsNullOrEmpty(ElectronicObserverDataFolderPath) && File.Exists(EquipmentUpgradeFilePath))
        {
            LoadFile();
        }
    }

    public void LoadFile()
    {
        EquipmentUpgrades.Clear();

        List<EquipmentUpgradeDataModel> list = JsonHelper.ReadJson<List<EquipmentUpgradeDataModel>>(EquipmentUpgradeFilePath);

        foreach (EquipmentUpgradeDataModel model in list)
        {
            EquipmentUpgrades.Add(model);
        }
    }

    [RelayCommand]
    public void OpenDataFolderChoice()
    {
        // --- Load file
        using (var dialog = new FolderBrowserDialog())
        {
            DialogResult result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                ElectronicObserverDataFolderPath = dialog.SelectedPath;
            }
        }
    }

    [RelayCommand]
    public void SaveFileThenPush()
    {
        // --- Change update.json too
        JObject update = JsonHelper.ReadJsonObject(UpdateFilePath);

        JToken equipmentUpgradesVersion = update["EquipmentUpgrades"];
        int version = equipmentUpgradesVersion.Value<int>() + 1;
        update["EquipmentUpgrades"] = version;

        JsonHelper.WriteJson(UpdateFilePath, update);

        GitManager.Stage(EquipmentUpgradeFilePath);

        GitManager.Stage(UpdateFilePath);

        GitManager.CommitAndPush($"Equipment upgrades - {version}");
    }

    [RelayCommand] 
    public void OpenRaw()
    {
        Process openLink = new Process();
        openLink.StartInfo.UseShellExecute = true;
        openLink.StartInfo.FileName = "https://raw.githubusercontent.com/kcwikizh/WhoCallsTheFleet-DB/master/db/items.nedb";
        openLink.Start();
    }

    [RelayCommand]
    public async void UpdateFile()
    {
        try
        {
            using var _webClient = new HttpClient();
            string _rawJson = await _webClient.GetStringAsync("https://raw.githubusercontent.com/kcwikizh/WhoCallsTheFleet-DB/master/db/items.nedb");
            List<EquipmentUpgradeSourceData> wikiData = new();
            EquipmentUpgrades.Clear();

            foreach (string line in _rawJson.Split("\n"))
            {
                EquipmentSourceData eqData = JsonConvert.DeserializeObject<EquipmentSourceData>(line);

                if (eqData != null && eqData.Improvable) wikiData.Add(JsonConvert.DeserializeObject<EquipmentUpgradeSourceData>(line));
            }

            // Parse source data and convert it to "EO format"
            foreach (EquipmentUpgradeSourceData source in wikiData)
            {
                // Equipment Data
                EquipmentUpgradeDataModel equipment = new()
                {
                    EquipmentId = source.Id,
                };

                EquipmentUpgrades.Add(equipment);

                // Improvments
                ParseImprovment(source, equipment);

                // Upgrade to
                ParseUpgradeTo(source, equipment);

                // Used by
                equipment.UpgradeFor = source.UpgradeFor;
            }

            JsonHelper.WriteJsonByOnlyIndentingXTimes(EquipmentUpgradeFilePath, EquipmentUpgrades, 4, true);

            MessageBox.Show("Data updated");

        }
        catch (AggregateException ex)
        {
            string message = ex.Message;

            foreach (Exception innerEx in ex.InnerExceptions)
            {
                message += "\n";
                message += innerEx.Message;
            }

            MessageBox.Show(message);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"{ex.Message}\n{ex.StackTrace}");
        }
    }

    private static void ParseUpgradeTo(EquipmentUpgradeSourceData source, EquipmentUpgradeDataModel equipment)
    {
        if (source.UpgradeTo != null)
        {
            foreach (List<int> sourceImprovment in source.UpgradeTo)
            {
                EquipmentUpgradeConversionModel conversion = new();
                equipment.ConvertTo.Add(conversion);

                // [0] = equipment id
                conversion.IdEquipmentAfter = sourceImprovment[0];

                // [1] = equipment level
                conversion.EquipmentLevelAfter = sourceImprovment[1];
            }
        }
    }

    private static void ParseImprovment(EquipmentUpgradeSourceData wiki, EquipmentUpgradeDataModel equipment)
    {
        if (wiki.Improvement != null)
        {
            foreach (EquipmentUpgradeSourceDataImprovement sourceImprovment in wiki.Improvement)
            {
                EquipmentUpgradeImprovmentModel improvment = new();
                equipment.Improvement.Add(improvment);

                // Conversion possible ?
                ParseImprovmentParseConversion(sourceImprovment, improvment);

                // Helpers 
                ParseImprovmentParseHelpers(sourceImprovment, improvment);

                // Costs 

                ParseImprovmentParseCosts(sourceImprovment, improvment, wiki?.UpgradeTo?.Count > 0);
            }
        }
    }

    private static void ParseImprovmentParseCosts(EquipmentUpgradeSourceDataImprovement sourceImprovment, EquipmentUpgradeImprovmentModel improvment, bool canBeconverted)
    {
        List<int> rscCosts = sourceImprovment.Resource[0].Select(rsc => int.Parse(rsc.ToString())).ToList();

        // [0] = fuel ammo steel baux
        improvment.Costs.Fuel = rscCosts[0];
        improvment.Costs.Ammo = rscCosts[1];
        improvment.Costs.Steel = rscCosts[2];
        improvment.Costs.Bauxite = rscCosts[3];

        // [1] = 0 -> 5 Cost
        ParseImprovmentParseAnUpgradeCostDetail(improvment.Costs.Cost0To5, sourceImprovment.Resource[1]);

        // [2] = 6 -> 9 Cost
        ParseImprovmentParseAnUpgradeCostDetail(improvment.Costs.Cost6To9, sourceImprovment.Resource[2]);

        // [3] = Conversion
        if (sourceImprovment.Resource.Count >= 4 && canBeconverted)
        {
            improvment.Costs.CostMax = new EquipmentUpgradeImprovmentCostDetail();
            ParseImprovmentParseAnUpgradeCostDetail(improvment.Costs.CostMax, sourceImprovment.Resource[3]);
        }
    }

    private static void ParseImprovmentParseAnUpgradeCostDetail(EquipmentUpgradeImprovmentCostDetail costDetail, List<object> costDetailSource)
    {
        // [0] = devmat cost
        costDetail.DevmatCost = int.Parse(costDetailSource[0].ToString());
        // [1] = devmat cost but with slider
        costDetail.SliderDevmatCost = int.Parse(costDetailSource[1].ToString());

        // [2] = screw cost
        costDetail.ImproveMatCost = int.Parse(costDetailSource[2].ToString());
        // [3] = screw cost but with slider
        costDetail.SliderImproveMatCost = int.Parse(costDetailSource[3].ToString());

        // [4] = required equipments List<List<int>>
        if (costDetailSource[4] is JArray requiredEquipments)
        {
            foreach (JArray equipmentDetailSource in requiredEquipments)
            {
                EquipmentUpgradeImprovmentCostItemDetail equipmentDetail = new();

                // [1] = required equipment count
                equipmentDetail.Count = equipmentDetailSource[1].Value<int>();

                if (equipmentDetailSource[0].Type == JTokenType.Integer)
                {
                    // [0] = required equipment ID
                    equipmentDetail.Id = equipmentDetailSource[0].Value<int>();

                    if (equipmentDetail.Id > 0)
                        costDetail.EquipmentDetail.Add(equipmentDetail);
                }
                else if (equipmentDetailSource[0].Type == JTokenType.String)
                {
                    // [0] = required equipment ID
                    equipmentDetail.Id = int.Parse(equipmentDetailSource[0].Value<string>().Replace("consumable_", ""));

                    if (equipmentDetail.Id > 0)
                        costDetail.ConsumableDetail.Add(equipmentDetail);
                }

            }
        }
        else if (costDetailSource[4] is long value5 && costDetailSource.Count > 5 && costDetailSource[5] is long value6 && value5 > 0)
        {
            EquipmentUpgradeImprovmentCostItemDetail equipmentDetail = new();
            costDetail.EquipmentDetail.Add(equipmentDetail);

            // [0] = required equipment ID
            equipmentDetail.Id = (int)value5;

            // [1] = required equipment count
            equipmentDetail.Count = (int)value6;
        }
        else if (costDetailSource[4] is int requiredEquipment)
        {

            EquipmentUpgradeImprovmentCostItemDetail equipmentDetail = new();
            costDetail.EquipmentDetail.Add(equipmentDetail);

            // [0] = required equipment ID
            equipmentDetail.Id = requiredEquipment;

            // [1] = required equipment count
            equipmentDetail.Count = 1;
        }
    }

    private static void ParseImprovmentParseHelpers(EquipmentUpgradeSourceDataImprovement sourceImprovment, EquipmentUpgradeImprovmentModel improvment)
    {
        foreach (List<object> helperData in sourceImprovment.Req)
        {

            // [1] = Who can help
            if (helperData[1] is JArray ships)
            {
                EquipmentUpgradeHelpersModel helpers = new();

                // [0] = days of upgrades (starts with sunday)
                JArray days = (JArray)helperData[0];

                for (DayOfWeek day = DayOfWeek.Sunday; day <= DayOfWeek.Saturday; day++)
                {
                    if (days[(int)day].Value<bool>() is true) helpers.CanHelpOnDays.Add(day);
                }

                helpers.ShipIds = ships.Select(shipId => (int)shipId).ToList();
                improvment.Helpers.Add(helpers);
            }
            else
            {

            }
        }
    }

    private static void ParseImprovmentParseConversion(EquipmentUpgradeSourceDataImprovement sourceImprovment, EquipmentUpgradeImprovmentModel improvment)
    {
        if (sourceImprovment.Upgrade is JArray _list)
        {
            improvment.ConversionData = new EquipmentUpgradeConversionModel();
            improvment.ConversionData.IdEquipmentAfter = (int)_list[0];
            improvment.ConversionData.EquipmentLevelAfter = (int)_list[1];
        }
    }
}
