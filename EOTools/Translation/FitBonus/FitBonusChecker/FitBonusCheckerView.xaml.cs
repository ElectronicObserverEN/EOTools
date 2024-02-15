using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using EOTools.DataBase;

namespace EOTools.Translation.FitBonus.FitBonusChecker;

public partial class FitBonusCheckerView
{
    private FitBonusCheckerViewModel ViewModel { get; }

    public FitBonusCheckerView()
    {
        EOToolsDbContext db = Ioc.Default.GetRequiredService<EOToolsDbContext>();

        ViewModel = new FitBonusCheckerViewModel(db);

        DataContext = ViewModel;

        InitializeComponent();
    }

    private async void FitBonusCheckerView_OnLoaded(object sender, RoutedEventArgs e)
    {
        await ViewModel.Initialize();
    }
}
