using EOTools.Models;
using System.Windows;

namespace EOTools.Translation.FitBonus
{
    public class FitBonusValueViewModel
    {
        public FitBonusValueModel? Model { get; set; }

        public FitBonusValueViewModel(FitBonusValueModel? model)
        {
            Model = model;
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
    }
}
