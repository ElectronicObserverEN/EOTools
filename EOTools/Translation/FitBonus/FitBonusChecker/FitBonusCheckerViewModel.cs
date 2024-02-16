using CommunityToolkit.Mvvm.ComponentModel;
using EOTools.DataBase;
using System.Threading.Tasks;
using EOTools.Control.Grid;
using CommunityToolkit.Mvvm.Input;
using EOTools.ElectronicObserverApi;

namespace EOTools.Translation.FitBonus.FitBonusChecker;

public partial class FitBonusCheckerViewModel : ObservableObject
{
    private EOToolsDbContext DataBase { get; }
    private ElectronicObserverApiService ElectronicObserverApiService { get; }

    public FitBonusIssuesFetcher Fetcher { get; }

    public PaginationViewModel Pagination { get; }

    public FitBonusCheckerViewModel(EOToolsDbContext dbContext, ElectronicObserverApiService api)
    {
        DataBase = dbContext;
        ElectronicObserverApiService = api;

        Fetcher = new(DataBase, api);

        Pagination = new PaginationViewModel()
        {
            Fetcher = Fetcher
        };
    }

    public async Task Initialize()
    {
        await Pagination.Reload();
    }
    
    [RelayCommand]
    private async Task SetAsFixed(int id)
    {
        await ElectronicObserverApiService.Put($"FitBonusIssues/{id}/closeIssue");
        await Pagination.Reload();
    }
}
