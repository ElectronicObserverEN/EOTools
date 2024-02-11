using System.IO;
using EOTools.Config;
using EOTools.DataBase;
using EOTools.RPCTools;
using EOTools.Tools;
using EOTools.Translation;
using EOTools.Translation.Equipments;
using EOTools.Translation.EquipmentUpgrade;
using EOTools.Translation.FitBonus;
using EOTools.Translation.QuestManager.Events;
using EOTools.Translation.QuestManager.Quests;
using EOTools.Translation.QuestManager.Seasons;
using EOTools.Translation.QuestManager.Updates;
using EOTools.Translation.Ships;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using EOTools.Translation.Ships.ShipClass;

namespace EOTools
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ToolManager Tools { get; }

        public MainWindow()
        {
            Tools = Ioc.Default.GetRequiredService<ToolManager>();

            InitializeComponent();

            AppSettings.LoadSettings();

            if (string.IsNullOrEmpty(AppSettings.ElectronicObserverDataFolderPath))
            {
                AppSettings.ElectronicObserverDataFolderPath = AppSettings.OpenFolderDialog("Choose Data repo path");
            }

            if (string.IsNullOrEmpty(AppSettings.ElectronicObserverDataFolderPath))
            {
                App.Current.Shutdown();
            }

            if (!File.Exists(DatabaseSyncService.DataBaseLocalPath))
            {
                new DatabaseSyncService().PullAndRestoreDataBase();
            }

            using EOToolsDbContext db = new();
            db.Database.Migrate();

            WindowState = WindowState.Maximized;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            // --- Display RPC check form
            MainContentFrame.Content = new RPCManager();
        }

        private void MenuItemShipTranslation_Click(object sender, RoutedEventArgs e)
        {
            // --- Open quest translation
            MainContentFrame.Content = new TranslationShipForm();
        }

        private void MenuItemEquipTranslation_Click(object sender, RoutedEventArgs e)
        {
            // --- Open equipment translation
            MainContentFrame.Content = new TranslationEquipForm();
        }

        private void MenuItemDestinationUpdate_Click(object sender, RoutedEventArgs e)
        {
            // --- Open trackers
            MainContentFrame.Content = new DestinationUpdateForm();
        }

        private void MenuItemTagUpdate_Click(object sender, RoutedEventArgs e)
        {
            // --- Open trackers
            MainContentFrame.Content = new TagUpdaterForm();
        }

        private void MenuItemTagTranslationUpdate_Click(object sender, RoutedEventArgs e)
        {
            // --- Open trackers
            MainContentFrame.Content = new TagTranslationForm();
        }

        private void MenuItemMapTranslationUpdate_Click(object sender, RoutedEventArgs e)
        {
            // --- Open trackers
            MainContentFrame.Content = new MapNameTranslation();
        }

        private void MenuItemFitBonusUpdate_Click(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Content = new FitBonusView();
        }

        private void MenuItemEqUpgradeUpdate_Click(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Content = new EquipmentUpgradeView();
        }

        private void ManageUpdateClick(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Content = new UpdateManagerView();
        }

        private void ManageSeasonClick(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Content = new SeasonManagerView();
        }

        private void ManageEventClick(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Content = new EventManagerView();
        }

        private void ManageQuestsClick(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Content = new QuestManagerView();
        }

        private void ManageEquipmentsClick(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Content = new EquipmentManagerView();
        }

        private void ManageShipsClick(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Content = new ShipManagerView();
        }

        private void ManageShipClassClick(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Content = new ShipClassManagerView();
        }

        private void ConfigClick(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Content = new ConfigView();
        }

        private void UpdateMaintData(object sender, RoutedEventArgs e)
        {
            UpdateMaintenanceDataService service = new();
            service.UpdateMaintenanceData();
        }

        private void PushDatabase(object sender, RoutedEventArgs e)
        {
            DatabaseSyncService service = new();
            service.PullDataBase();
            service.StageDatabaseChangesToGit();
            service.PushDatabaseChangesToGit();
        }


        private void PullDatabase(object sender, RoutedEventArgs e)
        {
            DatabaseSyncService service = new();
            service.PullAndRestoreDataBase();
        }

        private async void OpenAssetViewer(object sender, RoutedEventArgs e)
        {
            await Tools.OpenAssetViewer();
        }
    }
}
