using System;
using EOTools.DataBase;
using EOTools.Models;
using EOTools.Models.EquipmentUpgrade;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EOTools.Extensions;

namespace EOTools.Tools;

public class UpdateEquipmentDataService
{
    private GitManager GitManager => new GitManager(AppSettings.ElectronicObserverDataFolderPath);

    private string UpdateFilePath => Path.Combine(AppSettings.ElectronicObserverDataFolderPath, "Translations", "en-US", "update.json");
    private string UpdateDataFilePath => Path.Combine(AppSettings.ElectronicObserverDataFolderPath, "update.json");
    private string EquipmentUpgradesFilePath => Path.Combine(AppSettings.ElectronicObserverDataFolderPath, "Data", "EquipmentUpgrades.json");
    public static string EquipmentTranslationsFilePath => Path.Combine(AppSettings.ElectronicObserverDataFolderPath, "Translations", "en-US", "equipment.json");


    public void UpdateEquipmentTranslations()
    {
        // Get version
        JObject updateJson = JsonHelper.ReadJsonObject(UpdateFilePath);
        string version = (int.Parse(updateJson.Value<string>("equipment")) + 1).ToString();

        Dictionary<string, object> toSerialize = new()
        {
            { "version", version }
        };

        updateJson["equipment"] = version;

        using EOToolsDbContext db = new();
        List<EquipmentModel> equipments = db.Equipments
            .AsEnumerable()
            .OrderBy(quest => quest.ApiId)
            .ToList();

        Dictionary<string, string> translations = new();
        toSerialize.Add("equipment", translations);

        // TODO : eqtype manager
        JObject prevTranslations = JsonHelper.ReadJsonObject(EquipmentTranslationsFilePath);
        toSerialize.Add("equiptype", prevTranslations["equiptype"]);

        foreach (EquipmentModel model in equipments)
        {
            if (!translations.ContainsKey(model.NameJP))
                translations.Add(model.NameJP, model.NameEN);
        }

        // --- Stage & push
        GitManager.Pull();

        new DatabaseSyncService().StageDatabaseChangesToGit();

        JsonHelper.WriteJson(EquipmentTranslationsFilePath, toSerialize);
        JsonHelper.WriteJson(UpdateFilePath, updateJson);

        GitManager.Stage(EquipmentTranslationsFilePath);
        GitManager.Stage(UpdateFilePath);

        GitManager.CommitAndPush($"Equipments - {version}");

    }

    public void UpdateEquipmentUpgrades()
    {
        // Get version
        JObject updateJson = JsonHelper.ReadJsonObject(UpdateDataFilePath);
        int version = updateJson.Value<int>("EquipmentUpgrades") + 1;
        updateJson["EquipmentUpgrades"] = version;

        CleanUpDbBeforePush();

        List<EquipmentUpgradeDataModel> upgradesJson = EquipmentUpgradesService.Instance.AllUpgradeModel
            .AsEnumerable()
            .OrderBy(eq => eq.EquipmentId)
            .Where(eq => eq.UpgradeFor.Any() || eq.Improvement.Any())
            .ToList();

        // --- Stage & push
        GitManager.Pull();

        new DatabaseSyncService().StageDatabaseChangesToGit();

        JsonHelper.WriteJsonByOnlyIndentingXTimes(EquipmentUpgradesFilePath, upgradesJson, 4);
        JsonHelper.WriteJson(UpdateDataFilePath, updateJson);

        GitManager.Stage(EquipmentUpgradesFilePath);
        GitManager.Stage(UpdateDataFilePath);

        GitManager.CommitAndPush($"Equipment upgrades - {version}");
    }

    private void CleanUpDbBeforePush()
    {
        // Noticed duplicate entries for some equipments ...
        List<EquipmentUpgradeDataModel> duplicates = EquipmentUpgradesService.Instance.AllUpgradeModel
            .Where(elementInList =>
                EquipmentUpgradesService.Instance.AllUpgradeModel.Count(element =>
                    elementInList.EquipmentId == element.EquipmentId) > 1)
            .ToList();

        if (duplicates.Count > 0)
        {
            // do the cleanup
            foreach (IGrouping<int, EquipmentUpgradeDataModel> model in duplicates.GroupBy(upgrade => upgrade.EquipmentId))
            {
                EquipmentUpgradeDataModel? firstUpgrade = model.FirstOrDefault(upg => upg.Improvement.Any());
                if (firstUpgrade is null) firstUpgrade = model.First();

                // delete everything but the "first upgrade"
                foreach (EquipmentUpgradeDataModel upg in model)
                {
                    if (upg != firstUpgrade)
                    {
                        EquipmentUpgradesService.Instance.DbContext.Remove(upg);
                        EquipmentUpgradesService.Instance.AllUpgradeModel.Remove(upg);
                    }
                }
            }
            
            // Then do another check
            duplicates = EquipmentUpgradesService.Instance.AllUpgradeModel
                .Where(elementInList =>
                    EquipmentUpgradesService.Instance.AllUpgradeModel.Count(element =>
                        elementInList.EquipmentId == element.EquipmentId) > 1)
                .ToList();

            if (duplicates.Count > 0)
            {
                throw new Exception($"Duplicate entries found for equipment {duplicates[0].GetEquipmentString()}");
            }
        }
    }
}
