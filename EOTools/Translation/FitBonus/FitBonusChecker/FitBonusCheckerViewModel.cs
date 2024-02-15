using CommunityToolkit.Mvvm.ComponentModel;
using EOTools.DataBase;
using System.Threading.Tasks;
using EOTools.Control.Grid;

namespace EOTools.Translation.FitBonus.FitBonusChecker;

public partial class FitBonusCheckerViewModel : ObservableObject
{
    private EOToolsDbContext DataBase { get; }

    public FitBonusIssuesFetcher Fetcher { get; }

    public PaginationViewModel Pagination { get; }

    public FitBonusCheckerViewModel(EOToolsDbContext dbContext)
    {
        DataBase = dbContext;

        Fetcher = new(DataBase);

        Pagination = new PaginationViewModel()
        {
            Fetcher = Fetcher
        };
    }

    public async Task Initialize()
    {
        await Pagination.Reload();
    }
}
