using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOTools.Tools;

namespace EOTools.Config
{
    public partial class ConfigViewModel : ObservableObject
    {
        [ObservableProperty]
        private string electronicObserverDataFolderPath = "";

        [ObservableProperty]
        private string kancolleEOAPIFolder = "";

        [ObservableProperty]
        private string shipIconFolder = "";

        [ObservableProperty]
        private string eoDatabaseFile = "";

        [ObservableProperty]
        private bool disablePush = false;

        [ObservableProperty]
        private string electronicObserverApiUrl = "";

        [ObservableProperty]
        private string electronicObserverApiKey = "";

        public string DataFolderDisplay => string.IsNullOrEmpty(ElectronicObserverDataFolderPath) ? "Select a folder" : ElectronicObserverDataFolderPath;
        public string APIFolderDisplay => string.IsNullOrEmpty(KancolleEOAPIFolder) ? "Select a folder" : KancolleEOAPIFolder;
        public string IconFolderDisplay => string.IsNullOrEmpty(ShipIconFolder) ? "Select a folder" : ShipIconFolder;
        public string EoDatabaseDisplay => string.IsNullOrEmpty(EoDatabaseFile) ? "Select a file" : EoDatabaseFile;

        public ConfigViewModel()
        {
            ElectronicObserverDataFolderPath = AppSettings.ElectronicObserverDataFolderPath;
            KancolleEOAPIFolder = AppSettings.KancolleEOAPIFolder;
            ShipIconFolder = AppSettings.ShipIconFolder;
            DisablePush = AppSettings.DisablePush;
            EoDatabaseFile = AppSettings.EoDbPath;
            ElectronicObserverApiUrl = AppSettings.ElectronicObserverApiUrl;
            electronicObserverApiKey = AppSettings.ElectronicObserverApiKey;

            PropertyChanged += ConfigViewModel_PropertyChanged;
        }

        private void ConfigViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                // Save changes
                case nameof(ElectronicObserverDataFolderPath):
                    AppSettings.ElectronicObserverDataFolderPath = ElectronicObserverDataFolderPath;
                    OnPropertyChanged(nameof(DataFolderDisplay));
                    break;
                case nameof(KancolleEOAPIFolder):
                    AppSettings.KancolleEOAPIFolder = KancolleEOAPIFolder;
                    OnPropertyChanged(nameof(APIFolderDisplay));
                    break;
                case nameof(ShipIconFolder):
                    AppSettings.ShipIconFolder = ShipIconFolder;
                    OnPropertyChanged(nameof(IconFolderDisplay));
                    break;
                case nameof(DisablePush):
                    AppSettings.DisablePush = DisablePush;
                    break;
                case nameof(EoDatabaseFile):
                    AppSettings.EoDbPath = EoDatabaseFile;
                    OnPropertyChanged(nameof(EoDatabaseDisplay));
                    break;
                case nameof(ElectronicObserverApiUrl):
                    AppSettings.ElectronicObserverApiUrl = ElectronicObserverApiUrl;
                    break;
                case nameof(ElectronicObserverApiKey):
                    AppSettings.ElectronicObserverApiKey = ElectronicObserverApiKey;
                    break;
            }
        }

        [RelayCommand]
        private void OpenDataFolderDialog()
            => ElectronicObserverDataFolderPath = AppSettings.OpenFolderDialog("Select data repo path") ?? ElectronicObserverDataFolderPath;

        [RelayCommand]
        private void OpenAPIFolderDialog()
            => KancolleEOAPIFolder = AppSettings.OpenFolderDialog("Select API folder") ?? KancolleEOAPIFolder;

        [RelayCommand]
        private void OpenShipIconFolderDialog()
            => ShipIconFolder = AppSettings.OpenFolderDialog("Select ship icons folder") ?? ShipIconFolder;

        [RelayCommand]
        private void OpenEoDatabaseDialog()
            => EoDatabaseFile = AppSettings.OpenFileDialog("Select EO database", ".db") ?? EoDatabaseFile;

    }
}
