using EOTools.RPCTools;
using EOTools.Tools;
using EOTools.Translation;
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

    }
}
