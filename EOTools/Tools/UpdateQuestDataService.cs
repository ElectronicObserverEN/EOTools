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
    private string UpdateDataFilePath => Path.Combine(AppSettings.ElectronicObserverDataFolderPath, "update.json");
    public static string TrackersFilePath => Path.Combine(AppSettings.ElectronicObserverDataFolderPath, "Data", "QuestTrackers.json");
    public static string QuestsTranslationsFilePath => Path.Combine(AppSettings.ElectronicObserverDataFolderPath, "Translations", "en-US", "quest.json");


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
        List<QuestModel> questlist = db.Quests
            .AsEnumerable()
            .Where(quest => !quest.HasQuestEnded())
            .OrderBy(quest => quest.ApiId)
            .ToList();

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

        // --- Stage & push
        GitManager.Pull();

        new DatabaseSyncService().StageDatabaseChangesToGit();

        JsonHelper.WriteJson(QuestsTranslationsFilePath, toSerialize);
        JsonHelper.WriteJson(UpdateFilePath, updateJson);

        GitManager.Stage(QuestsTranslationsFilePath);
        GitManager.Stage(UpdateFilePath);

        GitManager.CommitAndPush($"Quests - {version}");

    }

    public void UpdateQuestTrackers()
    {
        // Get version
        JObject updateJson = JsonHelper.ReadJsonObject(UpdateDataFilePath);
        int version = updateJson.Value<int>("QuestTrackers") + 1;
        updateJson["QuestTrackers"] = version;

        JArray toSerialize = new();

        using EOToolsDbContext db = new();
        List<QuestModel> questlist = db.Quests
            .AsEnumerable()
            .Where(quest => !quest.HasQuestEnded())
            .Where(quest => !string.IsNullOrEmpty(quest.Tracker))
            .OrderBy(quest => quest.ApiId)
            .ToList();

        foreach (QuestModel _quest in questlist)
        {
            toSerialize.Add(JArray.Parse(_quest.Tracker));
        }

        // --- Stage & push
        GitManager.Pull();

        new DatabaseSyncService().StageDatabaseChangesToGit();

        JsonHelper.WriteJsonByOnlyIndentingOnce(TrackersFilePath, toSerialize);
        JsonHelper.WriteJson(UpdateDataFilePath, updateJson);

        GitManager.Stage(TrackersFilePath);
        GitManager.Stage(UpdateDataFilePath);

        GitManager.CommitAndPush($"Quest trackers - {version}");
    }
}
