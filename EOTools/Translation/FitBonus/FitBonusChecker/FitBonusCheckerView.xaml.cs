using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using EOTools.DataBase;
using EOTools.ElectronicObserverApi;

namespace EOTools.Translation.FitBonus.FitBonusChecker;

public partial class FitBonusCheckerView
{
    private FitBonusCheckerViewModel ViewModel { get; }

    public FitBonusCheckerView()
    {
        EOToolsDbContext db = Ioc.Default.GetRequiredService<EOToolsDbContext>();
        ElectronicObserverApiService api = Ioc.Default.GetRequiredService<ElectronicObserverApiService>();

        ViewModel = new FitBonusCheckerViewModel(db, api);

        DataContext = ViewModel;

        InitializeComponent();
    }

    private async void FitBonusCheckerView_OnLoaded(object sender, RoutedEventArgs e)
    {
        await ViewModel.Initialize();
    }
}
