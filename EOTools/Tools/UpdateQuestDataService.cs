﻿using EOTools.DataBase;
using EOTools.Models;
using EOTools.Translation.QuestManager.Quests;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EOTools.Tools;

public class UpdateQuestDataService
{
    private GitManager GitManager => new GitManager(AppSettings.ElectronicObserverDataFolderPath);

    private string UpdateFilePath => Path.Combine(AppSettings.ElectronicObserverDataFolderPath, "Translations", "en-US", "update.json");
    private string QuestsDataFilePath => Path.Combine(AppSettings.ElectronicObserverDataFolderPath, "Data", "Quests.json");
    private string QuestsTranslationsFilePath => Path.Combine(AppSettings.ElectronicObserverDataFolderPath, "Translations", "en-US", "quest.json");


    public void UpdateQuestTranslations()
    {
        // Get version
        JObject updateJson = JsonHelper.ReadJsonObject(UpdateFilePath);
        string version = (int.Parse(updateJson.Value<string>("quest")) + 1).ToString();

        Dictionary<string, object> toSerialize = new()
        {
            { "version", version }
        };

        updateJson["quest"] = version;

        using EOToolsDbContext db = new();
        List<QuestModel> questlist = db.Quests.AsEnumerable().OrderBy(quest => quest.ApiId).ToList();

        foreach (QuestModel _quest in questlist)
        {
            toSerialize.Add(_quest.ApiId.ToString(), new QuestData(_quest.ApiId)
            {
                Code = _quest.Code,
                DescEN = _quest.DescEN,
                DescJP = _quest.DescJP,
                NameEN = _quest.NameEN,
                NameJP = _quest.NameJP
            });
        }

        new DatabaseSyncService().PushDatabaseChangesToGit();

        JsonHelper.WriteJson(QuestsTranslationsFilePath, toSerialize);
        JsonHelper.WriteJson(UpdateFilePath, updateJson);

        // --- Stage & push
        //StageAndPushFiles();
    }
}
