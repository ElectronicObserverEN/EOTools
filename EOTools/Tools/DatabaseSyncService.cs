using EOTools.DataBase;
using EOTools.Models;
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
    private string EquipmentFilePath => Path.Combine(AppSettings.ElectronicObserverDataFolderPath, "Data", "Equipments.json");

    public void StageDatabaseChangesToGit()
    {
        using EOToolsDbContext db = new();

        JsonHelper.WriteJson(UpdatesFilePath, db.Updates.ToList());
        JsonHelper.WriteJson(QuestsFilePath, db.Quests.ToList());
        JsonHelper.WriteJson(SeasonsFilePath, db.Seasons.ToList());
        JsonHelper.WriteJson(EventsFilePath, db.Events.ToList());
        JsonHelper.WriteJson(EquipmentFilePath, db.Equipments.ToList());

        GitManager.Stage(UpdatesFilePath);
        GitManager.Stage(EventsFilePath);
        GitManager.Stage(QuestsFilePath);
        GitManager.Stage(SeasonsFilePath);
        GitManager.Stage(EquipmentFilePath);
    }

    public void PushDatabaseChangesToGit()
        => GitManager.CommitAndPush($"Database update");

    public void PullDataBase()
    {
        GitManager.Pull();

        using EOToolsDbContext db = new();

        db.RemoveRange(db.Quests);
        db.RemoveRange(db.Seasons);
        db.RemoveRange(db.Events);
        db.RemoveRange(db.Updates);
        db.RemoveRange(db.Equipments);

        // rebuild db
        db.Updates.AddRange(JsonHelper.ReadJson<List<UpdateModel>>(UpdatesFilePath));
        db.Events.AddRange(JsonHelper.ReadJson<List<EventModel>>(EventsFilePath));
        db.Seasons.AddRange(JsonHelper.ReadJson<List<SeasonModel>>(SeasonsFilePath));
        db.Quests.AddRange(JsonHelper.ReadJson<List<QuestModel>>(QuestsFilePath));
        db.Equipments.AddRange(JsonHelper.ReadJson<List<EquipmentModel>>(EquipmentFilePath));

        db.SaveChanges();
    }
}
