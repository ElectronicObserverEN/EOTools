using CommunityToolkit.Mvvm.ComponentModel;
using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EOTools.Translation.QuestManager.Updates;

public partial class UpdateViewModel : ObservableObject
{
    [ObservableProperty]
    private DateTime updateDate = DateTime.Now;

    [ObservableProperty]
    private TimeSpan updateStartTime = TimeSpan.Zero;

    [ObservableProperty]
    private string name = "";

    [ObservableProperty]
    private string description = "";

    [ObservableProperty]
    private bool wasLiveUpdate = false;

    public UpdateModel Model { get; private set; }

    public UpdateViewModel(UpdateModel update)
    {
        UpdateDate = update.UpdateDate;
        Name = update.Name;
        Description = update.Description;
        WasLiveUpdate = update.WasLiveUpdate;
        UpdateStartTime = update.UpdateStartTime;

        Model = update;
    }

    public void SaveChanges()
    {
        Model.UpdateDate = UpdateDate;
        Model.Name = Name;
        Model.Description = Description;
        Model.WasLiveUpdate = WasLiveUpdate;
        Model.UpdateStartTime = UpdateStartTime;
    }
}
