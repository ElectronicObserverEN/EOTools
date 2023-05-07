using CommunityToolkit.Mvvm.DependencyInjection;
using EOTools.Tools.Translations;

namespace EOTools.Models.Ships;

public class ShipModel
{
    public int Id { get; set; }

    public string NameEN { get; set; } = "";

    public string NameJP { get; set; } = "";

    public int ApiId { get; set; }

    public string GetNameEN() => Ioc.Default.GetRequiredService<ShipTranslationService>().Name(NameJP, ApiId);

    public bool IsFriendly => ApiId < 1500;
}
