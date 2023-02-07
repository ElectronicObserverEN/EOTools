using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOTools.DataBase;
using EOTools.Translation.QuestManager.Updates;
using System.Runtime.Serialization;

namespace EOTools.Translation.QuestManager.Seasons;

public partial class SeasonViewModel : ObservableObject
{
    private EOToolsDbContext DbContext { get; } = new();

    [ObservableProperty]
    private string name = "";

    [ObservableProperty]
    private int? addedOnUpdateId;

    public string AddedOnUpdateDisplay => AddedOnUpdate?.Name ?? "Select an update";
    public UpdateModel? AddedOnUpdate => AddedOnUpdateId switch
    {
        int id => DbContext.Updates.Find(id),
        _ => null,
    };

    [ObservableProperty]
    private int? removedOnUpdateId;
    public UpdateModel? RemovedOnUpdate => RemovedOnUpdateId switch
    {
        int id => DbContext.Updates.Find(id),
        _ => null,
    };
    public string RemovedOnUpdateDisplay => RemovedOnUpdate?.Name ?? "Select an update";

    public SeasonModel Model { get; private set; }

    public SeasonViewModel(SeasonModel season)
    {
        Name = season.Name;
        RemovedOnUpdateId = season.RemovedOnUpdateId;
        AddedOnUpdateId = season.AddedOnUpdateId;

        Model = season;

        PropertyChanged += SeasonViewModel_PropertyChanged;
    }

    private void SeasonViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName is nameof(AddedOnUpdateId)) OnPropertyChanged(nameof(AddedOnUpdateDisplay));
        if (e.PropertyName is nameof(RemovedOnUpdateId)) OnPropertyChanged(nameof(RemovedOnUpdateDisplay));
    }

    public void SaveChanges()
    {
        Model.Name = Name;

        Model.RemovedOnUpdateId = RemovedOnUpdateId;

        Model.AddedOnUpdateId = AddedOnUpdateId;
    }

    [RelayCommand]
    public void OpenAddedOnUpdateList()
    {
        UpdateListViewModel vm = new();
        UpdateListView list = new(vm);

        if (list.ShowDialog() is true)
        {
            using EOToolsDbContext db = new();
            AddedOnUpdateId = vm.PickedUpdate?.Id;
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
            RemovedOnUpdateId = vm.PickedUpdate?.Id;
        }
    }

    [RelayCommand]
    public void ClearAddedOnUpdate()
    {
        AddedOnUpdateId = null;
    }

    [RelayCommand]
    public void ClearRemovedOnUpdate()
    {
        RemovedOnUpdateId = null;
    }
}
