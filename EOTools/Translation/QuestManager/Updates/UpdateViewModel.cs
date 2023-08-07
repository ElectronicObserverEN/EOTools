using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Text;
using System.Windows;
using CommunityToolkit.Mvvm.Input;

namespace EOTools.Translation.QuestManager.Updates;

public partial class UpdateViewModel : ObservableObject
{
    [ObservableProperty]
    private DateTime updateDate = DateTime.Now;

    [ObservableProperty]
    private TimeSpan updateStartTime = TimeSpan.Zero;

    [ObservableProperty]
    private TimeSpan? updateEndTime = null;

    [ObservableProperty]
    private string name = "";

    [ObservableProperty]
    private string description = "";

    [ObservableProperty]
    private bool wasLiveUpdate = false;

    [ObservableProperty]
    private string startTweet = "";

    [ObservableProperty]
    private string endTweet = "";

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

    [RelayCommand]
    private void CopyDiscordTimeStampToClipBoard()
    {
        StringBuilder timestamp = new StringBuilder();

        // Times are in JST, need to convert back to UTC
        DateTime start = UpdateDate.Add(UpdateStartTime);
        TimeZoneInfo japaneseTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
        DateTimeOffset startUtcTime = TimeZoneInfo.ConvertTimeToUtc(start, japaneseTimeZone);

        timestamp.AppendLine($"Maintenance starts on <t:{startUtcTime.ToUnixTimeSeconds()}:F> (<t:{startUtcTime.ToUnixTimeSeconds()}:R>)");

        if (UpdateEndTime is { } updateEnd)
        {
            DateTime end = UpdateDate.Add(updateEnd);
            DateTimeOffset endUtcTime = TimeZoneInfo.ConvertTimeToUtc(end, japaneseTimeZone);

            timestamp.AppendLine($"Maintenance should end on <t:{endUtcTime.ToUnixTimeSeconds()}:F> (<t:{endUtcTime.ToUnixTimeSeconds()}:R>)");
        }

        Clipboard.SetText(timestamp.ToString());
    }
}
