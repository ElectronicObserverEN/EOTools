using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOTools.DataBase;
using EOTools.Translation.QuestManager.Updates;

namespace EOTools.Translation.QuestManager.Events;

public partial class EventViewModel : ObservableObject
{
    private EOToolsDbContext DbContext { get; } = new();

    [ObservableProperty]
    private string name = "";

    [ObservableProperty]
    private int apiId;

    [ObservableProperty]
    private int? startOnUpdateId;

    public string StartOnUpdateDisplay => StartOnUpdate?.Name ?? "Select an update";
    public UpdateModel? StartOnUpdate => StartOnUpdateId switch
    {
        int id => DbContext.Updates.Find(id),
        _ => null,
    };

    [ObservableProperty]
    private int? endOnUpdateId;
    public UpdateModel? EndOnUpdate => EndOnUpdateId switch
    {
        int id => DbContext.Updates.Find(id),
        _ => null,
    };
    public string EndOnUpdateDisplay => EndOnUpdate?.Name ?? "Select an update";

    public EventModel Model { get; private set; }

    public EventViewModel(EventModel Event)
    {
        Name = Event.Name;
        EndOnUpdateId = Event.EndOnUpdateId;
        StartOnUpdateId = Event.StartOnUpdateId;
        ApiId = Event.ApiId;

        Model = Event;

        PropertyChanged += EventViewModel_PropertyChanged;
    }

    private void EventViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName is nameof(EndOnUpdateId)) OnPropertyChanged(nameof(EndOnUpdateDisplay));
        if (e.PropertyName is nameof(StartOnUpdateId)) OnPropertyChanged(nameof(StartOnUpdateDisplay));
    }

    public void SaveChanges()
    {
        Model.Name = Name;
        Model.StartOnUpdateId = StartOnUpdateId;
        Model.EndOnUpdateId = EndOnUpdateId;
        Model.ApiId = ApiId;
    }

    [RelayCommand]
    public void OpenAddedOnUpdateList()
    {
        UpdateListViewModel vm = new();
        UpdateListView list = new(vm);

        if (list.ShowDialog() is true)
        {
            using EOToolsDbContext db = new();
            StartOnUpdateId = vm.PickedUpdate?.Id;
        }
    }

    [RelayCommand]
    public void OpenRemovedOnUpdateList()
    {
        UpdateListViewModel vm = new();
        UpdateListView list = new(vm);

        if (list.ShowDialog() is true)
        {
            using EOToolsDbContext db = new();
            EndOnUpdateId = vm.PickedUpdate?.Id;
        }
    }

    [RelayCommand]
    public void ClearAddedOnUpdate()
    {
        StartOnUpdateId = null;
    }

    [RelayCommand]
    public void ClearRemovedOnUpdate()
    {
        EndOnUpdateId = null;
    }
}
