using CommunityToolkit.Mvvm.DependencyInjection;
using EOTools.Tools.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EOTools.Models.Ships;

public class ShipModel
{
    public int Id { get; set; }

    public string NameEN { get; set; } = "";

    public string NameJP { get; set; } = "";

    public int ApiId { get; set; }

    public string GetNameEN() => Ioc.Default.GetRequiredService<ShipTranslationService>().Name(NameJP, ApiId);

    [NotMapped]
    public bool IsFriendly => ApiId < 1500;
}
