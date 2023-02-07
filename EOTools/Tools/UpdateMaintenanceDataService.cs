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
    private string UpdatesFilePath => Path.Combine(AppSettings.ElectronicObserverDataFolderPath, "Data", "Updates.json");
    private string EventsFilePath => Path.Combine(AppSettings.ElectronicObserverDataFolderPath, "Data", "Events.json");

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
            .OrderByDescending(upd => upd.UpdateDate)
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

        JsonHelper.WriteJson(UpdateFilePath, updateData);

        JsonHelper.WriteJson(UpdatesFilePath, db.Updates.ToList());
        JsonHelper.WriteJson(EventsFilePath, db.Events.ToList());

        GitManager.Stage(UpdateFilePath);
        GitManager.Stage(UpdatesFilePath);
        GitManager.Stage(EventsFilePath);

        GitManager.CommitAndPush($"Maintenance information - {update.Name}");
    }
}
