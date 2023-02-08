using EOTools.DataBase;
using System.IO;
using System.Linq;

namespace EOTools.Tools;

public class DatabaseSyncService
{
    private GitManager GitManager => new GitManager(AppSettings.ElectronicObserverDataFolderPath);

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

        return;

        GitManager.Stage(UpdatesFilePath);
        GitManager.Stage(EventsFilePath);
        GitManager.Stage(QuestsFilePath);
        GitManager.Stage(SeasonsFilePath);

        GitManager.CommitAndPush($"Database update");
    }
}
