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
    private PaginationPageViewModel _currentPage;

    public int ItemsPerPage => 10;

    [ObservableProperty]
    private PaginationPageViewModel _lastPage;

    public required IDataFetcher Fetcher { get; set; }

    public PaginationViewModel()
    {
        _currentPage = Pages.First();
        _lastPage = Pages.Last();
    }

    [RelayCommand]
    public async Task Reload()
    {
        PaginatedResultModel<IGridRowFetched>? result = await Fetcher.LoadData((CurrentPage.PageIndex - 1) * ItemsPerPage, ItemsPerPage);

        if (result is null) return;

        DisplayedData = result.Results;
        List<PaginationPageViewModel> paginationPageViewModels = new();

        for (int index = 0; index <= result.TotalCount / ItemsPerPage; index++)
        {
            paginationPageViewModels.Add(new()
            {
                PageIndex = index + 1,
            });
        }

        Pages = paginationPageViewModels;

        CurrentPage = paginationPageViewModels.Find(page => page.PageIndex == CurrentPage.PageIndex) ?? paginationPageViewModels[0];

        LastPage = paginationPageViewModels[^1];
    }

    [RelayCommand]
    private async Task PreviousPage()
    {
        List<PaginationPageViewModel> paginationPageViewModels = Pages.ToList();
        CurrentPage = paginationPageViewModels.Find(page => page.PageIndex == CurrentPage.PageIndex - 1) ?? paginationPageViewModels[0];

        await Reload();
    }

    [RelayCommand]
    private async Task NextPage()
    {
        List<PaginationPageViewModel> paginationPageViewModels = Pages.ToList();
        CurrentPage = paginationPageViewModels.Find(page => page.PageIndex == CurrentPage.PageIndex + 1) ?? paginationPageViewModels[^1];

        await Reload();
    }
}
