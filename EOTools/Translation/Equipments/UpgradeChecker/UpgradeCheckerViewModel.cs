using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using EOTools.DataBase;
using EOTools.ElectronicObserverApi.Models;
using EOTools.Models.EquipmentUpgrade;
using EOTools.Models.KancolleApi.EquipmentRemodel;
using EOTools.Models.Ships;
using EOTools.Tools;
using EOTools.Tools.CurrentDeck;
using EOTools.Translation.Ships.ShipList;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;
using EOTools.ElectronicObserverApi;

namespace EOTools.Translation.Equipments.UpgradeChecker;

public partial class UpgradeCheckerViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<UpgradeIssueViewModel> tooManyUpgradePerShipList = new();

    [ObservableProperty]
    private UpgradeIssueViewModel? selectedIssue;

    [ObservableProperty]
    private ShipModel? selectedShip;

    public string SelectedShipString => SelectedShip?.NameEN ?? "Select a ship";

    private List<UpgradeDataPerShipViewModel> UpgradesPerShip { get; set; }

    public List<UpgradeDataPerDayAndShipViewModel> UpgradesForSelectedShip => SelectedShip switch
    {
        ShipModel ship => GetUpgradesPerShip(ship),
        _ => new()
    };

    private DispatcherTimer UpgradeCheckerTimer { get; }

    [ObservableProperty]
    public bool upgradeCheckerIsRunning = false;

    [ObservableProperty]
    public string upgradeCheckerStatus = ""; 

    public UpgradeCheckerViewModel()
    {
        using EOToolsDbContext db = new();
        UpgradesPerShip = db.Ships.Select(ship => new UpgradeDataPerShipViewModel(ship)).ToList();

        LoadUpgradeIssuesView();

        PropertyChanged += UpgradeCheckerViewModel_PropertyChanged;
        PropertyChanged += UpgradeCheckerViewModel_PropertyChanged2;

        UpgradeCheckerTimer = new(TimeSpan.FromSeconds(5), DispatcherPriority.Normal, CheckUpgradeFromApi, Dispatcher.CurrentDispatcher);
    }

    private void UpgradeCheckerViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName is not nameof(SelectedIssue)) return;

        SelectedShip = SelectedIssue?.Ship;
    }

    private void UpgradeCheckerViewModel_PropertyChanged2(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName is not nameof(SelectedShip)) return;

        OnPropertyChanged(nameof(UpgradesForSelectedShip));
        OnPropertyChanged(nameof(SelectedShipString));
    }

    private void LoadUpgradeIssuesView()
    {
        TooManyUpgradePerShipList = new(UpgradesPerShip
            .Where(upg => upg.Days.Any(day => day.Improvments.Count > 3))
            .Select(upg => new TooManyUpgradePerShipViewModel(upg.ShipModel)));

        // Load issues from API in the background
        App.Current?.Dispatcher?.InvokeAsync(LoadIssuesFromApi);
    }

    private async Task LoadIssuesFromApi()
    {
        try
        {
            if (string.IsNullOrEmpty(AppSettings.ElectronicObserverApiUrl)) return;
            ElectronicObserverApiService api = Ioc.Default.GetRequiredService<ElectronicObserverApiService>();

            List<UserReportedEquipmentUpgradeIssueModel>? issues = new();

                issues = await api.GetJson<List<UserReportedEquipmentUpgradeIssueModel>?>("EquipmentUpgradeIssues?issueState=1");

            if (issues is null) return;
            
            List<UpgradeIssueViewModel> issuesViewModels = new();

            await using EOToolsDbContext db = new();

            foreach (UserReportedEquipmentUpgradeIssueModel issue in issues)
            {
                // Parse issues into "proper types"
                ConvertUserReportedIssueToTypedIssue(issue, db, issuesViewModels);
            }
            
            App.Current?.Dispatcher?.Invoke(() =>
            {
                issuesViewModels = MergesIssues(issuesViewModels);
                issuesViewModels.ForEach(issue => TooManyUpgradePerShipList.Add(issue));
            });
        }
        catch (Exception ex)
        {
            await App.ShowErrorMessage(ex);
        }
    }

    private void ConvertUserReportedIssueToTypedIssue(UserReportedEquipmentUpgradeIssueModel issue, EOToolsDbContext db, List<UpgradeIssueViewModel> issuesViewModels)
    {
        foreach (int actualEquipmentId in issue.ActualUpgrades)
        {
            if (!issue.ExpectedUpgrades.Contains(actualEquipmentId) && !IsBaseUpgradeEquipment(actualEquipmentId))
            {
                ShipModel shipModel = db.Ships.First(ship => ship.ApiId == issue.HelperId);
                issuesViewModels.Add(new MissingEquipmentUpgradeViewModel(shipModel)
                {
                    Day = issue.Day,
                    EquipmentId = actualEquipmentId,
                    UserReportedIssues = new() { issue }
                });
            }
        }

        foreach (int expectedEquipmentId in issue.ExpectedUpgrades)
        {
            if (!issue.ActualUpgrades.Contains(expectedEquipmentId))
            {
                ShipModel shipModel = db.Ships.First(ship => ship.ApiId == issue.HelperId);
                issuesViewModels.Add(new EquipmentCantBeUpgradedViewModel(shipModel)
                {
                    Day = issue.Day,
                    EquipmentId = expectedEquipmentId,
                    UserReportedIssues = new() { issue }
                });
            }
        }
    }

    private List<UpgradeDataPerDayAndShipViewModel> GetUpgradesPerShip(ShipModel ship)
    {
        UpgradeDataPerShipViewModel? shipUpgrades = UpgradesPerShip
           .FirstOrDefault(upg => upg.ShipModel.ApiId == ship.ApiId);

        if (shipUpgrades is null) return new();

        return shipUpgrades.Days.Select(day => new UpgradeDataPerDayAndShipViewModel(day.Day, ship, day.Improvments)).ToList();
    }

    [RelayCommand]
    private void OpenShipSelection()
    {
        ShipListViewModel vm = new();
        ShipListView view = new(vm);

        if (view.ShowDialog() is true)
        {
            SelectedShip = vm.PickedShip;
        }
    }
    
    [RelayCommand]
    private async Task SetIssuesAsFixed(UpgradeIssueViewModel issue)
    {
        if (string.IsNullOrEmpty(AppSettings.ElectronicObserverApiUrl)) return;
        ElectronicObserverApiService api = Ioc.Default.GetRequiredService<ElectronicObserverApiService>();

        List<UserReportedEquipmentUpgradeIssueModel> list = issue.UserReportedIssues.ToList();

        foreach (UserReportedEquipmentUpgradeIssueModel userReportedIssue in list)
        {
            // is the issue actually solved ?
            if (!TooManyUpgradePerShipList.Any(issueVm => issueVm.Equals(issue) is false && issueVm.UserReportedIssues.Contains(userReportedIssue)))
            {
                if (!await api.Put($"EquipmentUpgradeIssues/{userReportedIssue.Id}/closeIssue"))
                {
                    return;
                }
            }

            issue.UserReportedIssues.Remove(userReportedIssue);
        }

        TooManyUpgradePerShipList.Remove(issue);
    }

    private void CheckUpgradeFromApi(object? sender, EventArgs e)
    {
        if (!UpgradeCheckerIsRunning) return;

        List<UpgradeIssueViewModel> issues = GetIssuesFromApiFile();
        issues = MergesIssues(issues);

        foreach (UpgradeIssueViewModel issue in issues)
        {
            AddIssue(issue);
        }
    }

    private void AddIssue(UpgradeIssueViewModel issue)
    {
        TooManyUpgradePerShipList.Add(issue);
    }

    private List<UpgradeIssueViewModel> MergesIssues(List<UpgradeIssueViewModel> issues)
    {
        List<UpgradeIssueViewModel> newIssues = new List<UpgradeIssueViewModel>();

        // only return missing issues
        foreach (UpgradeIssueViewModel issue in issues)
        {
            UpgradeIssueViewModel? similarIssue = TooManyUpgradePerShipList.FirstOrDefault(issue.Equals);

            if (similarIssue is null)
            {
                newIssues.Add(issue);
            }
            else
            {
                similarIssue.UserReportedIssues.AddRange(issue.UserReportedIssues);
            }
        }

        return newIssues;
    }

    private List<UpgradeIssueViewModel> GetIssuesFromApiFile()
    {
        string path = Path.Combine(AppSettings.KancolleEOAPIFolder, "kcsapi", "api_req_kousyou", "remodel_slotlist");
        List<RemodelSlotListElement>? upgradesApi = SystemJsonHelper.ReadKCJson<List<RemodelSlotListElement>>(path);

        if (upgradesApi is null) return new();

        // Dont read it twice
        File.Delete(path);

        UpgradeCheckerStatus = $"Reading data ...";

        // Get helper : 
        CurrentDeckService deck = Ioc.Default.GetRequiredService<CurrentDeckService>();
        deck.UpdateDeck();
        ShipModel dbShip = deck.Fleets[0].Ships[1].GetMasterShip();

        DayOfWeek day = (DateTime.UtcNow + new TimeSpan(9, 0, 0)).DayOfWeek;

        // get saved upgrades : 
        UpgradeDataPerDayAndShipViewModel upgradesPerShip = GetUpgradesPerShip(dbShip).First(upgs => upgs.Day == day);

        List<UpgradeIssueViewModel> issues = new();

        foreach (RemodelSlotListElement upgApi in upgradesApi)
        {
            if (!upgradesPerShip.Improvments.Any(upgSaved => upgSaved.EquipmentId == upgApi.ApiSlotId) && !IsBaseUpgradeEquipment(upgApi.ApiSlotId))
            {
                issues.Add(new MissingEquipmentUpgradeViewModel(dbShip)
                {
                    Day = day,
                    EquipmentId = upgApi.ApiSlotId,
                });
            }
        }

        foreach (EquipmentUpgradeDataModel upgSaved in upgradesPerShip.Improvments)
        {
            if (!upgradesApi.Any(upgApi => upgSaved.EquipmentId == upgApi.ApiSlotId))
            {
                issues.Add(new EquipmentCantBeUpgradedViewModel(dbShip)
                {
                    Day = day,
                    EquipmentId = upgSaved.EquipmentId,
                });
            }
        }

        UpgradeCheckerStatus = $"{dbShip.NameEN}'s data was read. {issues.Count} issues found !";

        return issues;
    }

    private bool IsBaseUpgradeEquipment(int equipmentId) => equipmentId switch
    {
        2 // 12.7cm Twin Gun
        or 4 // 14cm Single Gun
        or 14 // 61cm Quadruple Torpedo
        or 44 // Type 94 Depth Charge Projector
            => true,
        _ => false
    };
}
