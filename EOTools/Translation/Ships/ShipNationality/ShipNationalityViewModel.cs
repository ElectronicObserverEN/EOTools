using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace EOTools.Translation.Ships.ShipNationality;

public partial class ShipNationalityViewModel : ObservableObject
{
    public IEnumerable<ShipNationality> Nationalities => Enum.GetValues<ShipNationality>();

    [ObservableProperty] private ShipNationality _nationality;
}
