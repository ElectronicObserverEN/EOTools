using CommunityToolkit.Mvvm.DependencyInjection;
using EOTools.DataBase;
using EOTools.Tools;
using EOTools.Tools.CurrentDeck;
using EOTools.Tools.Translations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

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

            Exception ex = e.Exception;

            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }

            ContentDialog errorDialog = new ContentDialog();
            errorDialog.Content = $"{ex.Message}\n\n\n\n{ex.StackTrace}";
            errorDialog.CloseButtonText = "Close";

            errorDialog.ShowAsync();

            // Prevent default unhandled exception processing
            e.Handled = true;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            ServiceProvider services = new ServiceCollection()
                .AddSingleton<ShipTranslationService>()
                .AddSingleton<CurrentDeckService>()
                .BuildServiceProvider();

            Ioc.Default.ConfigureServices(services);

            base.OnStartup(e);
        }
    }
}
