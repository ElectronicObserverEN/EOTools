using CommunityToolkit.Mvvm.DependencyInjection;
using EOTools.DataBase;
using EOTools.Models.Ships;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;

namespace EOTools.Tools;

public class DatabaseSyncService
{
    private GitManager GitManager => new(AppSettings.ElectronicObserverDataFolderPath);

    private string QuestsFilePath => Path.Combine(AppSettings.ElectronicObserverDataFolderPath, "Data", "Quests.json");
    private string SeasonsFilePath => Path.Combine(AppSettings.ElectronicObserverDataFolderPath, "Data", "Seasons.json");
    private string EquipmentFilePath => Path.Combine(AppSettings.ElectronicObserverDataFolderPath, "Data", "Equipments.json");
    private string ShipFilePath => Path.Combine(AppSettings.ElectronicObserverDataFolderPath, "Data", "Ships.json");
    private string ShipClassFilePath => Path.Combine(AppSettings.ElectronicObserverDataFolderPath, "Data", "ShipClass.json");
    private string DataBaseRepoPath => Path.Combine(AppSettings.ElectronicObserverDataFolderPath, "Data", "Data.old.db");

    public static string DataBaseLocalPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "EOTools", "EOTools.db");

    public void StageDatabaseChangesToGit()
    {
        PullDataBase();

        using EOToolsDbContext db = new();

        JsonHelper.WriteJson(QuestsFilePath, db.Quests.ToList());
        JsonHelper.WriteJson(SeasonsFilePath, db.Seasons.ToList());
        JsonHelper.WriteJson(EquipmentFilePath, db.Equipments.ToList());
        JsonHelper.WriteJson(ShipFilePath, db.Ships.Include(nameof(ShipModel.ShipClass)).ToList());
        JsonHelper.WriteJson(ShipClassFilePath, db.ShipClass.ToList());

        File.Copy(DataBaseLocalPath, DataBaseRepoPath, true);

        GitManager.Stage(QuestsFilePath);
        GitManager.Stage(SeasonsFilePath);
        GitManager.Stage(EquipmentFilePath);
        GitManager.Stage(ShipFilePath);
        GitManager.Stage(ShipClassFilePath);
        GitManager.Stage(DataBaseRepoPath);
    }

    public void PushDatabaseChangesToGit()
        => GitManager.CommitAndPush($"Database update");

    public void PullDataBase()
    {
        GitManager.Pull();
    }

    public void PullAndRestoreDataBase()
    {
        GitManager.Pull();

        EOToolsDbContext db = Ioc.Default.GetRequiredService<EOToolsDbContext>();

        db.Database.CloseConnection();

        File.Copy(DataBaseRepoPath, DataBaseLocalPath, true);

        db.Database.OpenConnection();
    }
}
