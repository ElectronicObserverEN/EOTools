using System;

namespace EOTools.Translation.QuestManager.Updates;

public class UpdateModel
{
    public int Id { get; set; }

    public DateTime UpdateDate { get; set; } = DateTime.Now;

    public string Name { get; set; } = "";

    public string Description { get; set; } = "";

    public bool WasLiveUpdate { get; set; } = false;
}
