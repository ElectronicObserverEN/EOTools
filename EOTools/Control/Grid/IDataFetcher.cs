using System.Threading.Tasks;

namespace EOTools.Control.Grid;

public interface IDataFetcher
{
    public Task<PaginatedResultModel<IGridRowFetched>?> LoadData(int skip, int take);
}
