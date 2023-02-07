using CommunityToolkit.Mvvm.Input;
using EOTools.DataBase;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EOTools.Translation.QuestManager.Updates;

public partial class UpdateManagerViewModel
{
    public ObservableCollection<UpdateViewModel> UpdateListSorted { get; set; } = new();
    public List<UpdateViewModel> UpdateList { get; set; }

    public UpdateManagerViewModel()
    {
        // Load updates
        using EOToolsDbContext db = new();
        UpdateList = new(db.Updates
            .Select(update => new UpdateViewModel(update))
            .ToList());

        ReloadUpdateList();
    }

    public void ReloadUpdateList()
    {
        UpdateListSorted.Clear();

        List<UpdateViewModel> updates = UpdateList.OrderBy(update => update.UpdateDate).ToList();

        foreach (UpdateViewModel update in updates)
        {
            UpdateListSorted.Add(update);
        }
    }

    [RelayCommand]
    public void AddUpdate()
    {
        UpdateModel model = new();
        UpdateViewModel vm = new(model);
        UpdateEditView view = new(vm);

        if (view.ShowDialog() == true)
        {
            vm.SaveChanges();
            using EOToolsDbContext db = new();

            db.Add(model);
            db.SaveChanges();
            UpdateList.Add(vm);

            ReloadUpdateList();
        }
    }


    [RelayCommand]
    public void EditUpdate(UpdateViewModel vm)
    {
        UpdateViewModel vmEdit = new(new()
        {
            Description = vm.Description,
            Name = vm.Name,
            UpdateDate = vm.UpdateDate,
            WasLiveUpdate = vm.WasLiveUpdate,
            UpdateStartTime = vm.UpdateStartTime,
        });

        UpdateEditView view = new(vmEdit);

        if (view.ShowDialog() == true)
        {
            vm.Description = vmEdit.Description;
            vm.Name = vmEdit.Name;
            vm.UpdateDate = vmEdit.UpdateDate;
            vm.WasLiveUpdate = vmEdit.WasLiveUpdate;
            vm.UpdateStartTime = vmEdit.UpdateStartTime;

            vm.SaveChanges();
            using EOToolsDbContext db = new();
            db.Update(vm.Model);
            db.SaveChanges();

            ReloadUpdateList();
        }
    }

    [RelayCommand]
    public void RemoveUpdate(UpdateViewModel vm)
    {
        using EOToolsDbContext db = new();
        db.Remove(vm.Model);
        db.SaveChanges();

        UpdateList.Remove(vm);

        ReloadUpdateList();
    }
}
