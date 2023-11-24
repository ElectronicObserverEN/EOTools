using CommunityToolkit.Mvvm.ComponentModel;
using EOTools.Models.Ships;

namespace EOTools.Translation.Ships.ShipClass;

public partial class ShipClassViewModel : ObservableObject
{
    public ShipClassModel Model { get; private set; }

    [ObservableProperty]
    private string _nameEnglish = "";

    [ObservableProperty]
    private string _nameJapanese = "";

    [ObservableProperty]
    private int _apiId;

    public ShipClassViewModel(ShipClassModel model)
    {
        Model = model;
        LoadModel();
    }

    public void LoadModel()
    {
        NameEnglish = Model.NameEnglish;
        NameJapanese = Model.NameJapanese;
        ApiId = Model.ApiId;
    }

    public void SaveChanges()
    {
        Model.NameEnglish = NameEnglish;
        Model.NameJapanese = NameJapanese;
        Model.ApiId = ApiId;
    }
}
