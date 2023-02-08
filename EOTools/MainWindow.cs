using EOTools.RPCTools;
using EOTools.Tools;
using EOTools.Translation;
using EOTools.Translation.EquipmentUpgrade;
using EOTools.Translation.FitBonus;
using EOTools.Translation.QuestManager.Events;
using EOTools.Translation.QuestManager.Quests;
using EOTools.Translation.QuestManager.Seasons;
using EOTools.Translation.QuestManager.Updates;
using System.Windows;

namespace EOTools
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            AppSettings.LoadSettings();
            WindowState = WindowState.Maximized;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            // --- Display RPC check form
            MainContentFrame.Content = new RPCManager();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            // --- Open quest translation
            MainContentFrame.Content = new TranslationQuestForm();
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

        private void ManageTrackers(object sender, RoutedEventArgs e)
        {
            // --- Open trackers
            MainContentFrame.Content = new QuestTrackerForm();
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

        private void UpdateMaintData(object sender, RoutedEventArgs e)
        {
            UpdateMaintenanceDataService service = new();
            service.UpdateMaintenanceData();
        }

        private void PushDatabase(object sender, RoutedEventArgs e)
        {
            DatabaseSyncService service = new();
            service.PushDatabaseChangesToGit();
        }


        private void PullDatabase(object sender, RoutedEventArgs e)
        {
            DatabaseSyncService service = new();
            service.PullDataBase();
        }

    }
}
