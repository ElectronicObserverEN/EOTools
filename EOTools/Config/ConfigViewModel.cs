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
        private bool disablePush = false;

        public string DataFolderDisplay => string.IsNullOrEmpty(ElectronicObserverDataFolderPath) ? "Select a folder" : ElectronicObserverDataFolderPath;
        public string APIFolderDisplay => string.IsNullOrEmpty(KancolleEOAPIFolder) ? "Select a folder" : KancolleEOAPIFolder;
        public string IconFolderDisplay => string.IsNullOrEmpty(ShipIconFolder) ? "Select a folder" : ShipIconFolder;

        public ConfigViewModel()
        {
            ElectronicObserverDataFolderPath = AppSettings.ElectronicObserverDataFolderPath;
            KancolleEOAPIFolder = AppSettings.KancolleEOAPIFolder;
            ShipIconFolder = AppSettings.ShipIconFolder;
            DisablePush = AppSettings.DisablePush;

            PropertyChanged += ConfigViewModel_PropertyChanged;
        }

        private void ConfigViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // Save changes
            if (e.PropertyName is nameof(ElectronicObserverDataFolderPath))
            {
                AppSettings.ElectronicObserverDataFolderPath = ElectronicObserverDataFolderPath;
                OnPropertyChanged(nameof(DataFolderDisplay));
            }
            if (e.PropertyName is nameof(KancolleEOAPIFolder))
            {
                AppSettings.KancolleEOAPIFolder = KancolleEOAPIFolder;
                OnPropertyChanged(nameof(APIFolderDisplay));
            }
            if (e.PropertyName is nameof(ShipIconFolder))
            {
                AppSettings.ShipIconFolder = ShipIconFolder;
                OnPropertyChanged(nameof(IconFolderDisplay));
            }
            if (e.PropertyName is nameof(DisablePush))
            {
                AppSettings.DisablePush = DisablePush;
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

    }
}
