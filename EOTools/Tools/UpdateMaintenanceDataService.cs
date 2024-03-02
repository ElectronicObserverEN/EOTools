using EOTools.DataBase;
using EOTools.Translation.QuestManager.Events;
using EOTools.Translation.QuestManager.Updates;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace EOTools.Tools;

public class UpdateMaintenanceDataService
{
    private GitManager GitManager => new GitManager(AppSettings.ElectronicObserverDataFolderPath);

    private string UpdateFilePath => Path.Combine(AppSettings.ElectronicObserverDataFolderPath, "update.json");

    public void UpdateMaintenanceData()
    {
        if (string.IsNullOrEmpty(AppSettings.ElectronicObserverDataFolderPath))
        {
            using var dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                AppSettings.ElectronicObserverDataFolderPath = dialog.SelectedPath;
            }

            if (string.IsNullOrEmpty(AppSettings.ElectronicObserverDataFolderPath)) return;
        }

        JObject updateData = JsonHelper.ReadJsonObject(UpdateFilePath);

        SetOldUpdateTime(updateData);
        UpdateModel? update = SetUpdateTime(updateData);

        new DatabaseSyncService().StageDatabaseChangesToGit();

        JsonHelper.WriteJson(UpdateFilePath, updateData);

        GitManager.Stage(UpdateFilePath);

        string commitMessage = update switch
        {
            not null => $"Maintenance information - {update.Name}",
            _ => "Clear maintenance information",
        };

        GitManager.CommitAndPush(commitMessage);
    }

    private UpdateModel? SetUpdateTime(JObject updateData)
    {
        // get last update : 
        (UpdateModel? update, int updState) = GetMaintenanceState();

        if (update is { UpdateDate: { } updateDate, UpdateStartTime: { } updateStartTime })
        {
            updateData["MaintStart"] = $"{updateDate.Date.Add(updateStartTime):yyyy/MM/dd HH:mm:ss}";
            updateData["MaintEnd"] = update.UpdateEndTime switch
            {
                {} endTime => $"{updateDate.Date.Add(endTime):yyyy/MM/dd HH:mm:ss}",
                _ => null,
            };

            updateData["MaintInfoLink"] = string.IsNullOrEmpty(update.EndTweetLink) switch
            {
                true => string.IsNullOrEmpty(update.StartTweetLink) switch
                {
                    true => "https://twitter.com/KanColle_STAFF",
                    false => update.StartTweetLink,
                },
                false => update.EndTweetLink,
            };
        }

        updateData["MaintEventState"] = updState;

        return update;
    }

    private void SetOldUpdateTime(JObject updateData)
    {
        // get last update : 
        (UpdateModel update, int updState) = GetOldMaintenanceState();

        if (update is { UpdateDate: { } updateDate, UpdateStartTime: { } updateTime })
        {
            updateData["kancolle_mt"] = $"{updateDate.Date.Add(updateTime):yyyy/MM/dd HH:mm:ss}"; // For backward compatibility
        }

        updateData["event_state"] = updState;
    }

    private (UpdateModel?, int) GetMaintenanceState()
    {
        // get last update : 
        using EOToolsDbContext db = new();

        UpdateModel? update = db.Updates
            .AsEnumerable()
            .Where(upd => !upd.WasLiveUpdate)
            .Where(upd => upd.UpdateDate is not null)
            .Where(upd => upd.UpdateStartTime is not null)
            .Where(upd => upd.UpdateInProgress() || upd.UpdateIsComing())
            .MinBy(upd => upd.UpdateDate);

        if (update is null)
        {
            return (null, (int)MaintenanceState.None);
        }

        EventModel? eventEnd = db.Events
            .AsEnumerable()
            .FirstOrDefault(ev => ev.EndOnUpdateId == update.Id);

        if (eventEnd != null)
        {
            return new(update, (int)MaintenanceState.EventEnd);
        }

        EventModel? eventStart = db.Events
            .AsEnumerable()
            .FirstOrDefault(ev => ev.StartOnUpdateId == update.Id);

        if (eventStart != null)
        {
            return new(update, (int)MaintenanceState.EventStart);
        }

        return new(update, (int)MaintenanceState.Regular);
    }

    private (UpdateModel?, int) GetOldMaintenanceState()
    {
        // get last update : 
        using EOToolsDbContext db = new();

        UpdateModel? update = db.Updates
            .AsEnumerable()
            .Where(upd => !upd.WasLiveUpdate)
            .Where(upd => upd.UpdateDate is not null)
            .Where(upd => upd.UpdateStartTime is not null)
            .Where(upd => upd.UpdateInProgress() || upd.UpdateIsComing())
            .MinBy(upd => upd.UpdateDate);

        if (update is null)
        {
            return (null, (int)MaintenanceState.None);
        }

        EventModel? eventEnd = db.Events
            .AsEnumerable()
            .FirstOrDefault(ev => ev.EndOnUpdateId == update.Id);

        if (eventEnd != null)
        {
            return new(update, (int)MaintenanceState.EventEnd);
        }

        EventModel? eventStart = db.Events
            .AsEnumerable()
            .FirstOrDefault(ev => ev.StartOnUpdateId == update.Id);

        if (eventStart != null && update.UpdateInProgress() && update.UpdateEndTime is TimeSpan end)
        {
            update.UpdateStartTime = end;
            return new(update, (int)MaintenanceState.EventStart);
        }

        return new(update, (int)MaintenanceState.Regular);
    }
}
