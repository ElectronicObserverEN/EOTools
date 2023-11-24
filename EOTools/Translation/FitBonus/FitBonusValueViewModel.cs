using EOTools.Models.FitBonus;
using System.Windows;

namespace EOTools.Translation.FitBonus
{
    public class FitBonusValueViewModel
    {
        public FitBonusValueModel? Model { get; set; }

        public int? Firepower { get; set; }

        public int? Torpedo { get; set; }

        public int? AntiAir { get; set; }

        public int? Armor { get; set; }

        public int? Evasion { get; set; }

        public int? Asw { get; set; }

        public int? Los { get; set; }

        public int? Accuracy { get; set; }

        public int? Range { get; set; }

        public FitBonusValueViewModel(FitBonusValueModel? model)
        {
            Model = model;

            LoadFromModel();
        }

        public Visibility DisplayFirepower => Model?.Firepower is null ? Visibility.Collapsed : Visibility.Visible;
        public Visibility DisplayTorpedo => Model?.Torpedo is null ? Visibility.Collapsed : Visibility.Visible;
        public Visibility DisplayAntiAir => Model?.AntiAir is null ? Visibility.Collapsed : Visibility.Visible;
        public Visibility DisplayArmor => Model?.Armor is null ? Visibility.Collapsed : Visibility.Visible;
        public Visibility DisplayEvasion => Model?.Evasion is null ? Visibility.Collapsed : Visibility.Visible;
        public Visibility DisplayASW => Model?.ASW is null ? Visibility.Collapsed : Visibility.Visible;
        public Visibility DisplayLOS => Model?.LOS is null ? Visibility.Collapsed : Visibility.Visible;
        public Visibility DisplayAccuracy => Model?.Accuracy is null ? Visibility.Collapsed : Visibility.Visible;
        public Visibility DisplayRange => Model?.Range is null ? Visibility.Collapsed : Visibility.Visible;

        public void LoadFromModel()
        {
            if (Model is null) return;

            Firepower = Model.Firepower;
            Torpedo = Model.Torpedo;
            AntiAir = Model.AntiAir;
            Armor = Model.Armor;

            Los = Model.LOS;
            Asw = Model.ASW;
            Evasion = Model.Evasion;
            Accuracy = Model.Accuracy;
        }

        public void SaveChanges()
        {
            if (Model is null) return;

            Model.Firepower = Firepower;
            Model.Torpedo = Torpedo;
            Model.AntiAir = AntiAir;
            Model.Armor = Armor;

            Model.LOS = Los;
            Model.ASW = Asw;
            Model.Evasion = Evasion;
            Model.Accuracy = Accuracy;
        }
    }
}
