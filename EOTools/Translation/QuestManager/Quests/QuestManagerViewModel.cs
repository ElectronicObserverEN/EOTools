using CommunityToolkit.Mvvm.Input;
using EOTools.DataBase;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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
    public void AddQuest()
    {
        QuestModel model = new();
        QuestViewModel vm = new(model);
        QuestEditView view = new(vm);

        if (view.ShowDialog() == true)
        {
            vm.SaveChanges();
            using EOToolsDbContext db = new();

            db.Add(model);
            db.SaveChanges();
            QuestList.Add(vm);

            ReloadQuestList();
        }
    }


    [RelayCommand]
    public void EditQuest(QuestViewModel vm)
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

            vm.SaveChanges();

            using EOToolsDbContext db = new();
            db.Update(vm.Model);
            db.SaveChanges();

            ReloadQuestList();
        }
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
}
