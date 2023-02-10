using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOTools.DataBase;
using EOTools.Translation.QuestManager.Seasons;
using EOTools.Translation.QuestManager.Updates;
using System.Collections.Generic;

namespace EOTools.Translation.QuestManager.Quests;

public partial class QuestViewModel : ObservableObject
{
    private EOToolsDbContext DbContext { get; } = new();

    [ObservableProperty]
    private string code = "";

    [ObservableProperty]
    private string nameJP = "";

    [ObservableProperty]
    private string nameEN = "";

    [ObservableProperty]
    private string descJP = "";

    [ObservableProperty]
    private string descEN = "";

    [ObservableProperty]
    private string tracker = "";

    [ObservableProperty]
    private int apiId;

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


    [ObservableProperty]
    private int? seasonId;

    public string SeasonDisplay => Season?.Name ?? "Select a season";
    public SeasonModel? Season => SeasonId switch
    {
        int id => DbContext.Seasons.Find(id),
        _ => null,
    };

    public QuestModel Model { get; private set; }

    public QuestModel? SelectedTemplate { get; set; } = null;
    public static QuestModel[] QuestTemplates => new[]
    {
        new QuestModel
        {
            NameEN = "Battle model",
            DescEN = "Organize a fleet with XX in your fleet and score an S rank at the boss nodes on XX, and XX (Part X)"
        },
        new QuestModel
        {
            NameEN = "Battle model with 1-6",
            DescEN = "Organize a fleet with XX in your fleet and score an S rank at the boss nodes on XX, and reach the anchor node N on 1-6."
        },
        new QuestModel
        {
            NameEN = "PVP Model",
            DescEN = "Organize a fleet with XX in your fleet and score S rank XX times in PvP"
        },
        new QuestModel
        {
            NameEN = "Arsenal Model 1",
            DescEN = "Have your secretary flagship XX equip XX in the first slot and then proceed to scrap XX and have XX (All resources and Materials will be consumed upon completion)"
        },
        new QuestModel
        {
            NameEN = "Arsenal Model 2",
            DescEN = "Have XX (All resources and Materials will be consumed upon completion)"
        },
        new QuestModel
        {
            NameEN = "Expedition model",
            DescEN = "Complete expeditions XX and XX XXX times"
        },
    };

    public QuestViewModel(QuestModel Quest)
    {
        ApiId = Quest.ApiId;

        Code = Quest.Code;

        NameJP = Quest.NameJP;
        NameEN = Quest.NameEN;
        DescJP = Quest.DescJP;
        DescEN = Quest.DescEN;

        Tracker = Quest.Tracker;

        RemovedOnUpdateId = Quest.RemovedOnUpdateId;
        AddedOnUpdateId = Quest.AddedOnUpdateId;

        SeasonId = Quest.SeasonId;

        Model = Quest;

        PropertyChanged += QuestViewModel_PropertyChanged;
    }

    private void QuestViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName is nameof(AddedOnUpdateId)) OnPropertyChanged(nameof(AddedOnUpdateDisplay));
        if (e.PropertyName is nameof(RemovedOnUpdateId)) OnPropertyChanged(nameof(RemovedOnUpdateDisplay));
        if (e.PropertyName is nameof(SeasonId)) OnPropertyChanged(nameof(SeasonDisplay));
    }

    public void SaveChanges()
    {
        Model.ApiId = ApiId;
        Model.Code = Code;

        Model.NameJP = NameJP;
        Model.NameEN = NameEN;
        Model.DescJP = DescJP;
        Model.DescEN = DescEN;

        Model.Tracker = Tracker;

        Model.RemovedOnUpdateId = RemovedOnUpdateId;
        Model.AddedOnUpdateId = AddedOnUpdateId;
        Model.SeasonId = SeasonId;
    }

    [RelayCommand]
    public void OpenAddedOnUpdateList()
    {
        UpdateListViewModel vm = new();
        UpdateListView list = new(vm);

        if (list.ShowDialog() is true)
        {
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

    [RelayCommand]
    public void OpenSeasonList()
    {
        SeasonListViewModel vm = new();
        SeasonListView list = new(vm);

        if (list.ShowDialog() is true)
        {
            SeasonId = vm.PickedSeason?.Id;

            if (vm.PickedSeason is SeasonModel season)
            {
                AddedOnUpdateId = season.AddedOnUpdateId;
                RemovedOnUpdateId = season.RemovedOnUpdateId;
            }
        }
    }


    [RelayCommand]
    public void ClearSeason()
    {
        SeasonId = null;
    }

    [RelayCommand]
    public void ApplyTemplate()
    {
        if (SelectedTemplate is null) return;
        DescEN = SelectedTemplate.DescEN;
    }
}
