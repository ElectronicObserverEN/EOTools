using EOTools.DataBase;
using EOTools.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EOTools.Tools;

public class UpdateEquipmentDataService
{
    private GitManager GitManager => new GitManager(AppSettings.ElectronicObserverDataFolderPath);

    private string UpdateFilePath => Path.Combine(AppSettings.ElectronicObserverDataFolderPath, "Translations", "en-US", "update.json");
    private string UpdateDataFilePath => Path.Combine(AppSettings.ElectronicObserverDataFolderPath, "update.json");
    private string EquipmentUpgradesFilePath => Path.Combine(AppSettings.ElectronicObserverDataFolderPath, "Data", "EquipmentUpgrades.json");
    private string EquipmentTranslationsFilePath => Path.Combine(AppSettings.ElectronicObserverDataFolderPath, "Translations", "en-US", "equipment.json");


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

        new DatabaseSyncService().PushDatabaseChangesToGit();

        JsonHelper.WriteJson(EquipmentTranslationsFilePath, toSerialize);
        JsonHelper.WriteJson(UpdateFilePath, updateJson);

        // --- Stage & push
        GitManager.Pull();

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

        // TODO
    }
}
