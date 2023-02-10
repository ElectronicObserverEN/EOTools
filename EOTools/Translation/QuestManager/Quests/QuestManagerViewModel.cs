using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOTools.DataBase;
using EOTools.Models;
using EOTools.Tools;
using ModernWpf.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace EOTools.Translation.QuestManager.Quests;

public partial class QuestManagerViewModel : ObservableObject
{
    public ObservableCollection<QuestViewModel> QuestListFiltered { get; set; } = new();
    public List<QuestViewModel> QuestList { get; set; }

    [ObservableProperty]
    private string filter = "";

    public QuestManagerViewModel()
    {
        // Load Quests
        using EOToolsDbContext db = new();
        QuestList = new(db.Quests
            .Select(Quest => new QuestViewModel(Quest))
            .ToList());

        PropertyChanged += QuestManagerViewModel_PropertyChanged;

        ReloadQuestList();
    }

    private void QuestManagerViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Filter)) ReloadQuestList();
    }

    public void ReloadQuestList()
    {
        QuestListFiltered.Clear();

        List<QuestViewModel> quests = QuestList
            .Where(quest => string.IsNullOrEmpty(Filter) || quest.Code.ToLower().Contains(Filter.ToLower()) || quest.NameEN.ToLower().Contains(Filter.ToLower()) || quest.DescEN.ToLower().Contains(Filter.ToLower()))
            .ToList();

        foreach (QuestViewModel quest in quests)
        {
            QuestListFiltered.Add(quest);
        }
    }

    [RelayCommand]
    public async Task AddQuest()
    {
        QuestModel model = new();
        QuestViewModel vm = new(model);

        await ShowEditDialog(vm, true);
    }

    private async Task ShowEditDialog(QuestViewModel vm, bool newEntity)
    {
        QuestViewModel vmEdit = new(new()
        {
            ApiId = vm.ApiId,
            Code = vm.Code,

            NameEN = vm.NameEN,
            NameJP = vm.NameJP,
            DescEN = vm.DescEN,
            DescJP = vm.DescJP,

            Tracker = vm.Tracker,

            AddedOnUpdateId = vm.AddedOnUpdateId,
            RemovedOnUpdateId = vm.RemovedOnUpdateId,
            SeasonId = vm.SeasonId,
        });

        QuestEditView view = new(vmEdit);

        if (view.ShowDialog() == true)
        {
            vm.ApiId = vmEdit.ApiId;
            vm.Code = vmEdit.Code;
            vm.NameEN = vmEdit.NameEN;
            vm.NameJP = vmEdit.NameJP;
            vm.DescEN = vmEdit.DescEN;
            vm.DescJP = vmEdit.DescJP;
            vm.Tracker = vmEdit.Tracker;
            vm.SeasonId = vmEdit.SeasonId;
            vm.AddedOnUpdateId = vmEdit.AddedOnUpdateId;
            vm.RemovedOnUpdateId = vmEdit.RemovedOnUpdateId;

            try
            {
                vm.SaveChanges();

                if (newEntity)
                {
                    AddQuestToList(vm);
                }
                else
                {
                    using EOToolsDbContext db = new();
                    db.Update(vm.Model);
                    db.SaveChanges();

                    ReloadQuestList();
                }
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }

                ContentDialog errorDialog = new ContentDialog();
                errorDialog.Content = $"{ex.Message}\n\n\n\n{ex.StackTrace}";
                errorDialog.CloseButtonText = "Close";

                await errorDialog.ShowAsync();

                await ShowEditDialog(vm, newEntity);
            }
        }
    }

    public void AddQuestToList(QuestViewModel vm)
    {
        using EOToolsDbContext db = new();

        db.Add(vm.Model);
        db.SaveChanges();
        QuestList.Add(vm);

        ReloadQuestList();
    }

    [RelayCommand]
    public async Task EditQuest(QuestViewModel vm)
    {
        await ShowEditDialog(vm, false);
    }

    [RelayCommand]
    public void RemoveQuest(QuestViewModel vm)
    {
        using EOToolsDbContext db = new();
        db.Remove(vm.Model);
        db.SaveChanges();

        QuestList.Remove(vm);

        ReloadQuestList();
    }

    #region Data sync stuff

    [RelayCommand]
    public void UpdateFromTranslations()
    {
        // read files
        List<QuestData> questsTranslations = GetDataFromQuestsTranslations();
        List<QuestTrackerData> questsTrackers = GetTrackersData();

        // Create data models from tl and trackers
        List<QuestModel> questDataModels = new();

        foreach (QuestData questTranslation in questsTranslations)
        {
            if (questTranslation.QuestID > 9000) continue;

            QuestTrackerData? tracker = questsTrackers
                    .Where(tracker => tracker.QuestID == questTranslation.QuestID)
                    .FirstOrDefault();

            questDataModels.Add(new()
            {
                ApiId = questTranslation.QuestID,

                Code = questTranslation.Code,

                NameEN = questTranslation.NameEN,
                NameJP = questTranslation.NameJP,
                DescEN = questTranslation.DescEN,
                DescJP = questTranslation.DescJP,

                Tracker = tracker switch
                {
                    QuestTrackerData trackerNotNull => trackerNotNull.QuestData
                    .ToString()
                    .Replace("\r\n", "")
                    .Replace(" ", ""),

                    _ => ""
                }
            });
        }

        List<string> codes = QuestList.Select(quest => quest.Code).ToList();

        foreach (QuestModel model in questDataModels)
        {
            if (!codes.Contains(model.Code))
                AddQuestToList(new(model));
            /*else
            {
                // update tacker
                QuestViewModel vmToChange = QuestList.Find(vm => vm.Code == model.Code);
                vmToChange.Tracker = model.Tracker;

                vmToChange.SaveChanges();
                using EOToolsDbContext db = new();
                db.Update(vmToChange.Model);
                db.SaveChanges();
            }*/
        }
    }

    private List<QuestData> GetDataFromQuestsTranslations()
    {
        JObject json = JsonHelper.ReadJsonObject(AppSettings.QuestTLFilePath);

        List<QuestData> listOfQuests = new();

        foreach (JProperty questKey in json.Properties())
        {
            QuestData? newQuest = ParseJsonQuest(questKey, json);

            if (newQuest != null)
                listOfQuests.Add(newQuest);
        }

        return listOfQuests;
    }

    private QuestData? ParseJsonQuest(JProperty _questKey, JObject _jsonObject)
    {
        if (_questKey.Name == "version")
        {
            return null;
        }

        JObject _questData = (JObject)_jsonObject[_questKey.Name];

        return new QuestData(int.Parse(_questKey.Name), _questData);
    }

    private List<QuestTrackerData> GetTrackersData()
    {
        JArray json = JsonHelper.ReadJsonArray(AppSettings.QuestTrackerFilePath);

        List<QuestTrackerData> trackers = new List<QuestTrackerData>();

        foreach (JArray trackerData in json)
        {
            QuestTrackerData newTracker = new QuestTrackerData(trackerData);
            trackers.Add(newTracker);
        }

        return trackers;
    }

    [RelayCommand]
    public void UpdateTranslations()
    {
        UpdateQuestDataService service = new();
        service.UpdateQuestTranslations();
    }

    [RelayCommand]
    public void UpdateTrackers()
    {
        UpdateQuestDataService service = new();
        service.UpdateQuestTrackers();
    }

    [RelayCommand]
    public void AddQuestFromClipboard()
    {
        string questJson = Clipboard.GetText();

        JObject quests = (JObject)JsonHelper.ReadJsonFromString(questJson);

        foreach (JProperty questKey in quests.Properties())
        {
            JObject questData = (JObject)quests[questKey.Name]!;

            QuestModel newModel = new()
            {
                ApiId = int.Parse(questKey.Name),

                Code = questKey.Name,

                DescJP = questData.Value<string>("name_jp") ?? "",
                NameJP = questData.Value<string>("desc_jp") ?? "",

                NameEN = questData.Value<string>("name_jp") ?? "",
                DescEN = questData.Value<string>("desc_jp") ?? "",
            };

            AddQuestToList(new(newModel));
        }
    }
    #endregion

}
