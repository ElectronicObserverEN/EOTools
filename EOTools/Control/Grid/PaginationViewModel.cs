using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace EOTools.Control.Grid;

public partial class PaginationViewModel : ObservableObject
{
    [ObservableProperty]
    private IEnumerable<IGridRowFetched> _displayedData = new List<IGridRowFetched>();

    [ObservableProperty] 
    private IEnumerable<PaginationPageViewModel> _pages = [new()
    {
        PageIndex = 1,
        IsSelected = true
    }];

    [ObservableProperty]
    private int _selectedPage = 1;

    public int CountPerPage => 10;

    public required IDataFetcher Fetcher { get; set; }

    [RelayCommand]
    public async Task Reload()
    {
        PaginatedResultModel<IGridRowFetched>? result = await Fetcher.LoadData((SelectedPage - 1) * CountPerPage, SelectedPage * CountPerPage);

        if (result is null) return;

        DisplayedData = result.Results;

        int index = 0;

        Pages = Enumerable
            .Repeat<PaginationPageViewModel>(new()
            {
                PageIndex = ++index
            }, result.TotalCount / CountPerPage);
    }
}
