﻿using CommunityToolkit.Mvvm.Input;
using EOTools.DataBase;
using EOTools.Models;
using EOTools.Tools;
using ModernWpf.Controls;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EOTools.Translation.QuestManager.Quests;

public partial class QuestManagerViewModel
{
    public ObservableCollection<QuestViewModel> QuestListFiltered { get; set; } = new();
    public List<QuestViewModel> QuestList { get; set; }

    public QuestManagerViewModel()
    {
        // Load Quests
        using EOToolsDbContext db = new();
        QuestList = new(db.Quests
            .Select(Quest => new QuestViewModel(Quest))
            .ToList());

        ReloadQuestList();
    }

    public void ReloadQuestList()
    {
        QuestListFiltered.Clear();

        List<QuestViewModel> quests = QuestList;

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

            questDataModels.Add(new()
            {
                ApiId = questTranslation.QuestID,

                Code = questTranslation.Code,

                NameEN = questTranslation.NameEN,
                NameJP = questTranslation.NameJP,
                DescEN = questTranslation.DescEN,
                DescJP = questTranslation.DescJP,

                Tracker = questsTrackers
                    .Where(tracker => tracker.QuestID == questTranslation.QuestID)
                    .Select(tracker => tracker.QuestData.ToString())
                    .FirstOrDefault() ?? ""
            });
        }

        List<string> codes = QuestList.Select(quest => quest.Code).ToList();

        foreach (QuestModel model in questDataModels)
        {
            if (!codes.Contains(model.Code))
                AddQuestToList(new(model));
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
    #endregion

}