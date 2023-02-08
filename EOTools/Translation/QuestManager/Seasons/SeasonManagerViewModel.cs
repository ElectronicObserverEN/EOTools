using CommunityToolkit.Mvvm.Input;
using EOTools.DataBase;
using EOTools.Translation.QuestManager.Quests;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EOTools.Translation.QuestManager.Seasons;

public partial class SeasonManagerViewModel
{
    public ObservableCollection<SeasonViewModel> SeasonListSorted { get; set; } = new();
    public List<SeasonViewModel> SeasonList { get; set; }

    public SeasonManagerViewModel()
    {
        // Load seasons
        using EOToolsDbContext db = new();
        SeasonList = new(db.Seasons
            .Select(season => new SeasonViewModel(season))
            .ToList());

        ReloadSeasonList();
    }

    public void ReloadSeasonList()
    {
        SeasonListSorted.Clear();

        List<SeasonViewModel> updates = SeasonList.OrderBy(update => update?.AddedOnUpdate?.UpdateDate).ToList();

        foreach (SeasonViewModel update in updates)
        {
            SeasonListSorted.Add(update);
        }
    }

    [RelayCommand]
    public void AddSeason()
    {
        SeasonModel model = new();
        SeasonViewModel vm = new(model);
        SeasonEditView view = new(vm);

        if (view.ShowDialog() == true)
        {
            vm.SaveChanges();
            using EOToolsDbContext db = new();

            db.Add(model);
            db.SaveChanges();
            SeasonList.Add(vm);

            ReloadSeasonList();
        }
    }


    [RelayCommand]
    public void EditSeason(SeasonViewModel vm)
    {
        SeasonViewModel vmEdit = new(new()
        {
            Name = vm.Name,
            AddedOnUpdateId = vm.AddedOnUpdateId,
            RemovedOnUpdateId = vm.RemovedOnUpdateId,
        });

        SeasonEditView view = new(vmEdit);

        if (view.ShowDialog() == true)
        {
            vm.Name = vmEdit.Name;
            vm.AddedOnUpdateId = vmEdit.AddedOnUpdateId;
            vm.RemovedOnUpdateId = vmEdit.RemovedOnUpdateId;

            vm.SaveChanges();

            using EOToolsDbContext db = new();
            db.Update(vm.Model);
            db.SaveChanges();

            ReloadSeasonList();
        }
    }

    [RelayCommand]
    public void RemoveSeason(SeasonViewModel vm)
    {
        using EOToolsDbContext db = new();
        db.Remove(vm.Model);
        db.SaveChanges();

        SeasonList.Remove(vm);

        ReloadSeasonList();
    }

    [RelayCommand]
    public void EndQuests(SeasonViewModel vm)
    {
        if (vm.RemovedOnUpdate is null) return;

        using EOToolsDbContext db = new();
        List<QuestModel> quests = db.Quests.Where(q => q.SeasonId == vm.Model.Id).ToList();

        foreach (QuestModel quest in quests)
        {
            quest.RemovedOnUpdateId = vm.RemovedOnUpdate.Id;
            db.Update(quest);
        }

        db.SaveChanges();
    }
}
