using CommunityToolkit.Mvvm.ComponentModel;
using EOTools.Models.Ships;

namespace EOTools.Translation.Ships;

public partial class ShipViewModel : ObservableObject
{
    public ShipModel Model { get; private set; }

    [ObservableProperty]
    private string nameEN = "";

    [ObservableProperty]
    private string nameJP = "";

    [ObservableProperty]
    private int apiId;

    public ShipViewModel(ShipModel model)
    {
        Model = model;
        LoadModel();
    }

    public void LoadModel()
    {
        NameEN = Model.NameEN;
        NameJP = Model.NameJP;
        ApiId = Model.ApiId;
    }

    public void SaveChanges()
    {
        Model.NameEN = NameEN;
        Model.NameJP = NameJP;
        Model.ApiId = ApiId;
    }

    public bool IsFriendly => Model.IsFriendly;
}
