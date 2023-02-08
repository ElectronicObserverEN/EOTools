using EOTools.DataBase;
using EOTools.Translation.QuestManager.Events;
using EOTools.Translation.QuestManager.Quests;
using EOTools.Translation.QuestManager.Seasons;
using EOTools.Translation.QuestManager.Updates;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EOTools.Tools;

public class DatabaseSyncService
{
    private GitManager GitManager => new(AppSettings.ElectronicObserverDataFolderPath);

    private string UpdatesFilePath => Path.Combine(AppSettings.ElectronicObserverDataFolderPath, "Data", "Updates.json");
    private string EventsFilePath => Path.Combine(AppSettings.ElectronicObserverDataFolderPath, "Data", "Events.json");
    private string QuestsFilePath => Path.Combine(AppSettings.ElectronicObserverDataFolderPath, "Data", "Quests.json");
    private string SeasonsFilePath => Path.Combine(AppSettings.ElectronicObserverDataFolderPath, "Data", "Seasons.json");

    public void PushDatabaseChangesToGit()
    {
        using EOToolsDbContext db = new();

        JsonHelper.WriteJson(UpdatesFilePath, db.Updates.ToList());
        JsonHelper.WriteJson(QuestsFilePath, db.Quests.ToList());
        JsonHelper.WriteJson(SeasonsFilePath, db.Seasons.ToList());
        JsonHelper.WriteJson(EventsFilePath, db.Events.ToList());

        GitManager.Stage(UpdatesFilePath);
        GitManager.Stage(EventsFilePath);
        GitManager.Stage(QuestsFilePath);
        GitManager.Stage(SeasonsFilePath);

        GitManager.CommitAndPush($"Database update");
    }

    public void PullDataBase()
    {
        GitManager.Pull();

        using EOToolsDbContext db = new();

        db.RemoveRange(db.Quests);
        db.RemoveRange(db.Seasons);
        db.RemoveRange(db.Events);
        db.RemoveRange(db.Updates);

        // rebuild db
        db.Updates.AddRange(JsonHelper.ReadJson<List<UpdateModel>>(UpdatesFilePath));
        db.Events.AddRange(JsonHelper.ReadJson<List<EventModel>>(EventsFilePath));
        db.Seasons.AddRange(JsonHelper.ReadJson<List<SeasonModel>>(SeasonsFilePath));
        db.Quests.AddRange(JsonHelper.ReadJson<List<QuestModel>>(QuestsFilePath));

        db.SaveChanges();
    }
}
