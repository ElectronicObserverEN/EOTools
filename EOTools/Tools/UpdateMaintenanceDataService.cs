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

        // get last update : 
        (UpdateModel? update, int updState) = GetMaintenanceState();

        if (update is not null)
        {
            updateData["kancolle_mt"] = $"{update.UpdateDate.Add(update.UpdateStartTime):yyyy/MM/dd hh:mm:ss}";

            updateData["MaintInfoLink"] = string.IsNullOrEmpty(update.EndTweetLink) switch
            {
                true => string.IsNullOrEmpty(update.StartTweetLink) switch
                {
                    true => "https://twitter.com/KanColle_STAFF",
                    false => update.StartTweetLink
                },
                false => update.EndTweetLink,
            };
        }

        updateData["event_state"] = updState;

        new DatabaseSyncService().StageDatabaseChangesToGit();

        JsonHelper.WriteJson(UpdateFilePath, updateData);

        GitManager.Stage(UpdateFilePath);

        string commitMessage = update switch
        {
            UpdateModel => $"Maintenance information - {update.Name}",
            _ => "Clear maintenance information"
        };

        GitManager.CommitAndPush(commitMessage);
    }

    private (UpdateModel?, int) GetMaintenanceState()
    {
        // get last update : 
        using EOToolsDbContext db = new();

        UpdateModel? update = db.Updates
            .AsEnumerable()
            .Where(upd => UpdateInProgress(upd) || UpdateIsComing(upd))
            .OrderBy(upd => upd.UpdateDate)
            .FirstOrDefault();

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

    /// <summary>
    /// Returns true if update is coming
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    private bool UpdateIsComing(UpdateModel model)
    {
        DateTime dateNowJST = DateTime.UtcNow + new TimeSpan(9, 0, 0);
        DateTime start = model.UpdateDate.Date.Add(model.UpdateStartTime);

        // Update has started ?
        return start > dateNowJST;
    }

    /// <summary>
    /// Returns true if update is in progress
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    private bool UpdateInProgress(UpdateModel model)
    {
        DateTime dateNowJST = DateTime.UtcNow + new TimeSpan(9, 0, 0);
        DateTime start = model.UpdateDate.Date.Add(model.UpdateStartTime);

        // Update has started and no end time => update in progress
        if (start < dateNowJST && model.UpdateEndTime is null) return true;

        // Update has started and end time => update could be in progress
        if (start < dateNowJST && model.UpdateEndTime is TimeSpan endTime)
        {
            DateTime end = model.UpdateDate.Date.Add(endTime);

            // End didn't happen yet => Update is in progress
            if (end > dateNowJST) return true;
        }
        return false;
    }
}
