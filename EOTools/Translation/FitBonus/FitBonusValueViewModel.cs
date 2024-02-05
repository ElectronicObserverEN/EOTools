using EOTools.Models.FitBonus;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;

namespace EOTools.Translation.FitBonus
{
    public partial class FitBonusValueViewModel : ObservableObject
    {
        public FitBonusValueModel Model { get; set; }

        [ObservableProperty] private int _firepower;
        [ObservableProperty] private int _torpedo;
        [ObservableProperty] private int _antiAir;
        [ObservableProperty] private int _armor;
        [ObservableProperty] private int _evasion;
        [ObservableProperty] private int _asw;
        [ObservableProperty] private int _los;
        [ObservableProperty] private int _accuracy;
        [ObservableProperty] private int _range;

        public FitBonusValueViewModel(FitBonusValueModel model)
        {
            Model = model;

            LoadFromModel();
        }

        public Visibility DisplayFirepower => Model.Firepower is null ? Visibility.Collapsed : Visibility.Visible;
        public Visibility DisplayTorpedo => Model.Torpedo is null ? Visibility.Collapsed : Visibility.Visible;
        public Visibility DisplayAntiAir => Model.AntiAir is null ? Visibility.Collapsed : Visibility.Visible;
        public Visibility DisplayArmor => Model.Armor is null ? Visibility.Collapsed : Visibility.Visible;
        public Visibility DisplayEvasion => Model.Evasion is null ? Visibility.Collapsed : Visibility.Visible;
        public Visibility DisplayASW => Model.ASW is null ? Visibility.Collapsed : Visibility.Visible;
        public Visibility DisplayLOS => Model.LOS is null ? Visibility.Collapsed : Visibility.Visible;
        public Visibility DisplayAccuracy => Model.Accuracy is null ? Visibility.Collapsed : Visibility.Visible;
        public Visibility DisplayRange => Model.Range is null ? Visibility.Collapsed : Visibility.Visible;

        public void LoadFromModel()
        {
            Firepower = Model.Firepower ?? 0;
            Torpedo = Model.Torpedo ?? 0;
            AntiAir = Model.AntiAir ?? 0;
            Armor = Model.Armor ?? 0;

            Los = Model.LOS ?? 0;
            Asw = Model.ASW ?? 0;
            Evasion = Model.Evasion ?? 0;
            Accuracy = Model.Accuracy ?? 0;
        }

        public void SaveChanges()
        {
            Model.Firepower = Firepower != 0 ? Firepower : null;
            Model.Torpedo = Torpedo != 0 ? Torpedo : null;
            Model.AntiAir = AntiAir != 0 ? AntiAir : null;
            Model.Armor = Armor != 0 ? Armor : null;

            Model.LOS = Los != 0 ? Los : null;
            Model.ASW = Asw != 0 ? Asw : null;
            Model.Evasion = Evasion != 0 ? Evasion : null;
            Model.Accuracy = Accuracy != 0 ? Accuracy : null;
        }

        [RelayCommand]
        private void PasteBonus()
        {
            string text = Clipboard.GetText();

            FitBonusValueModel? bonus = JsonConvert.DeserializeObject<FitBonusValueModel>(text);

            if (bonus is null) return;

            Model = bonus;
            LoadFromModel();
        }
    }
}
