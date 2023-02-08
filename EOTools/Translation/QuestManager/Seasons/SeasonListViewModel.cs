using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOTools.DataBase;
using System.Collections.Generic;
using System.Linq;

namespace EOTools.Translation.QuestManager.Seasons;

public partial class SeasonListViewModel : ObservableObject
{
    public List<SeasonViewModel> SeasonList { get; set; }

    public SeasonViewModel? SelectedSeason { get; set; }
    public SeasonModel? PickedSeason { get; set; }

    public SeasonListViewModel()
    {
        // Load Seasons
        using EOToolsDbContext db = new();

        SeasonList = new(db.Seasons
            .Select(Season => new SeasonViewModel(Season))
            .ToList());
    }

    [RelayCommand]
    public void SelectSeason()
    {
        PickedSeason = SelectedSeason?.Model;
    }
}
