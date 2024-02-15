using CommunityToolkit.Mvvm.DependencyInjection;
using EOTools.Control.Grid;
using EOTools.DataBase;
using EOTools.ElectronicObserverApi;
using EOTools.Tools;
using System.Linq;
using System.Threading.Tasks;

namespace EOTools.Translation.FitBonus.FitBonusChecker;

public class FitBonusIssuesFetcher(EOToolsDbContext db) : IDataFetcher
{
    public async Task<PaginatedResultModel<IGridRowFetched>?> LoadData(int skip, int take)
    {
        if (string.IsNullOrEmpty(AppSettings.ElectronicObserverApiUrl)) return null;
        ElectronicObserverApiService api = Ioc.Default.GetRequiredService<ElectronicObserverApiService>();

        PaginatedResultModel<FitBonusIssueModel>? result = await api.GetJson<PaginatedResultModel<FitBonusIssueModel>>($"FitBonusIssues?issueState=1&skip={skip}&take={take}");

        if (result is null) return null;

        return new()
        {
            Results = result.Results.Select(model => new FitBonusIssueViewModel(model, db)),
            TotalCount = result.TotalCount,
        };
    }
}