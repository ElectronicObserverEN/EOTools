using System.ComponentModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using EOTools.DataBase;
using EOTools.Models.Ships;
using EOTools.Translation.Ships.ShipClass;

namespace EOTools.Translation.Ships;

public partial class ShipViewModel : ObservableObject
{
    public ShipModel Model { get; }

    [ObservableProperty] private string _nameEN = "";

    [ObservableProperty] private string _nameJP = "";

    [ObservableProperty] private int _apiId;

    [ObservableProperty] private int? _classId;

    public string ClassName => ClassId switch
    {
        { } id => Database.ShipClass.FirstOrDefault(sc => sc.ApiId == id)?.NameEnglish ?? "Select a class",
        _ => "Select a class"
    };

    private EOToolsDbContext Database { get; } = Ioc.Default.GetRequiredService<EOToolsDbContext>();

    public ShipViewModel(ShipModel model)
    {
        Model = model;
        LoadModel();

        PropertyChanged += OnClassIdChanged;
    }

    private void OnClassIdChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is not nameof(ClassId)) return;

        OnPropertyChanged(nameof(ClassName));
    }

    public void LoadModel()
    {
        NameEN = Model.NameEN;
        NameJP = Model.NameJP;
        ApiId = Model.ApiId;
        ClassId = Model.ShipClassId;
    }

    public void SaveChanges()
    {
        Model.NameEN = NameEN;
        Model.NameJP = NameJP;
        Model.ApiId = ApiId;
        Model.ShipClassId = ClassId;
    }

    public bool IsFriendly => Model.IsFriendly;

    [RelayCommand]
    private void OpenClassPicker()
    {
        ShipClassListViewModel vm = new();
        ShipClassListView view = new(vm);

        if (view.ShowDialog() is not true) return;
        if (vm.SelectedClass is null) return;

        ClassId = vm.SelectedClass.ApiId;
    }
}
