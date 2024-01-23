using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace EOTools.Translation.QuestManager.Updates;

public partial class UpdateViewModel : ObservableObject
{
    [ObservableProperty]
    private DateTime? _updateDate = DateTime.Now;

    [ObservableProperty]
    private TimeSpan? _updateStartTime = TimeSpan.Zero;

    [ObservableProperty]
    private TimeSpan? _updateEndTime = null;

    [ObservableProperty]
    private string _name = "";

    [ObservableProperty]
    private string _description = "";

    [ObservableProperty]
    private bool _wasLiveUpdate = false;

    [ObservableProperty]
    private string _startTweet = "";

    [ObservableProperty]
    private string _endTweet = "";

    public UpdateModel Model { get; private set; }

    public UpdateViewModel(UpdateModel update)
    {
        UpdateDate = update.UpdateDate;
        Name = update.Name;
        Description = update.Description;
        WasLiveUpdate = update.WasLiveUpdate;
        UpdateStartTime = update.UpdateStartTime;
        UpdateEndTime = update.UpdateEndTime;
        EndTweet = update.EndTweetLink;
        StartTweet = update.StartTweetLink;

        Model = update;
    }

    public void SaveChanges()
    {
        Model.UpdateDate = UpdateDate;
        Model.Name = Name;
        Model.Description = Description;
        Model.WasLiveUpdate = WasLiveUpdate;
        Model.UpdateStartTime = UpdateStartTime;
        Model.UpdateEndTime = UpdateEndTime;
        Model.EndTweetLink = EndTweet;
        Model.StartTweetLink = StartTweet;
    }
}
