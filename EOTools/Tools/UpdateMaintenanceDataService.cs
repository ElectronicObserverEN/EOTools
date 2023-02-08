using EOTools.DataBase;
using EOTools.Translation.QuestManager.Events;
using EOTools.Translation.QuestManager.Updates;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        using EOToolsDbContext db = new();

        UpdateModel? update = db.Updates
            .AsEnumerable()
            .Where(upd => UpdateInProgress(upd) || UpdateIsComing(upd))
            .OrderBy(upd => upd.UpdateDate)
            .FirstOrDefault();

        if (update is null) return;

        updateData["kancolle_mt"] = $"{update.UpdateDate.Add(update.UpdateStartTime):yyyy/MM/dd hh:mm:ss}";

        EventModel? eventStart = db.Events
            .AsEnumerable()
            .FirstOrDefault(ev => ev.StartOnUpdateId == update.Id);

        EventModel? eventEnd = db.Events
            .AsEnumerable()
            .FirstOrDefault(ev => ev.EndOnUpdateId == update.Id);

        updateData["event_state"] = (int)MaintenanceState.Regular;

        if (eventEnd != null)
        {
            updateData["event_state"] = (int)MaintenanceState.EventEnd;
        }
        else if (eventStart != null)
        {
            updateData["event_state"] = (int)MaintenanceState.EventStart;
        }

        new DatabaseSyncService().PushDatabaseChangesToGit();

        JsonHelper.WriteJson(UpdateFilePath, updateData);

        return;

        GitManager.Stage(UpdateFilePath);

        GitManager.CommitAndPush($"Maintenance information - {update.Name}");

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
