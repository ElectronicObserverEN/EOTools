using CommunityToolkit.Mvvm.Input;
using EOTools.DataBase;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EOTools.Translation.QuestManager.Events;

public partial class EventManagerViewModel
{
    public ObservableCollection<EventViewModel> EventListSorted { get; set; } = new();
    public List<EventViewModel> EventList { get; set; }

    public EventManagerViewModel()
    {
        // Load Events
        using EOToolsDbContext db = new();
        EventList = new(db.Events
            .Select(Event => new EventViewModel(Event))
            .ToList());

        ReloadEventList();
    }

    public void ReloadEventList()
    {
        EventListSorted.Clear();

        List<EventViewModel> updates = EventList.OrderBy(update => update?.StartOnUpdate?.UpdateDate).ToList();

        foreach (EventViewModel update in updates)
        {
            EventListSorted.Add(update);
        }
    }

    [RelayCommand]
    public void AddEvent()
    {
        EventModel model = new();
        EventViewModel vm = new(model);
        EventEditView view = new(vm);

        if (view.ShowDialog() == true)
        {
            vm.SaveChanges();
            using EOToolsDbContext db = new();

            db.Add(model);
            db.SaveChanges();
            EventList.Add(vm);

            ReloadEventList();
        }
    }


    [RelayCommand]
    public void EditEvent(EventViewModel vm)
    {
        EventViewModel vmEdit = new(new()
        {
            Name = vm.Name,
            StartOnUpdateId = vm.StartOnUpdateId,
            EndOnUpdateId = vm.EndOnUpdateId,
        });

        EventEditView view = new(vmEdit);

        if (view.ShowDialog() == true)
        {
            vm.Name = vmEdit.Name;
            vm.StartOnUpdateId = vmEdit.StartOnUpdateId;
            vm.EndOnUpdateId = vmEdit.EndOnUpdateId;

            vm.SaveChanges();

            using EOToolsDbContext db = new();
            db.Update(vm.Model);
            db.SaveChanges();

            ReloadEventList();
        }
    }

    [RelayCommand]
    public void RemoveEvent(EventViewModel vm)
    {
        using EOToolsDbContext db = new();
        db.Remove(vm.Model);
        db.SaveChanges();

        EventList.Remove(vm);

        ReloadEventList();
    }
}
