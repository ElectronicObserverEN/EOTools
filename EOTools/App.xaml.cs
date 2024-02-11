using CommunityToolkit.Mvvm.DependencyInjection;
using EOTools.DataBase;
using EOTools.ElectronicObserverApi;
using EOTools.Tools.CurrentDeck;
using EOTools.Tools.Translations;
using EOTools.Translation.FitBonus;
using Microsoft.Extensions.DependencyInjection;
using ModernWpf.Controls;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using EOTools.Tools;
using EOTools.Tools.AssetParser;

namespace EOTools
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
  {
        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // Process unhandled exception
            //MessageBox.Show($"{e.Exception.Message}\n\n\n\n{e.Exception.StackTrace}");

            ShowErrorMessage(e.Exception);

            // Prevent default unhandled exception processing
            e.Handled = true;
        }

        public static async Task ShowErrorMessage(string message)
        {
            ContentDialog errorDialog = new();
            errorDialog.Content = message;
            errorDialog.CloseButtonText = "Close";

            await errorDialog.ShowAsync();
        }

        public static async Task ShowErrorMessage(Exception ex)
        {
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }

            await ShowErrorMessage($"{ex.Message}\n\n\n\n{ex.StackTrace}");
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            ServiceProvider services = new ServiceCollection()
                .AddSingleton<ShipTranslationService>()
                .AddSingleton<CurrentDeckService>()
                .AddSingleton<ElectronicObserverApiService>()
                .AddSingleton<FitBonusManager>()
                .AddSingleton<AssetStructureReader>()
                .AddSingleton<AssetReader>()
                .AddSingleton<ToolManager>()
                .AddDbContext<EOToolsDbContext>()
                .BuildServiceProvider();

            Ioc.Default.ConfigureServices(services);

            base.OnStartup(e);
        }
    }
}
