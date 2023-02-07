using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOTools.DataBase;
using System.Collections.Generic;
using System.Linq;

namespace EOTools.Translation.QuestManager.Updates;

public partial class UpdateListViewModel : ObservableObject
{
    public List<UpdateViewModel> UpdateList { get; set; }

    public UpdateViewModel? SelectedUpdate { get; set; }
    public UpdateModel? PickedUpdate { get; set; }

    public UpdateListViewModel()
    {
        // Load updates
        using EOToolsDbContext db = new();

        UpdateList = new(db.Updates
            .Select(update => new UpdateViewModel(update))
            .ToList());
    }

    [RelayCommand]
    public void SelectUpdate()
    {
        PickedUpdate = SelectedUpdate?.Model;
    }
}
