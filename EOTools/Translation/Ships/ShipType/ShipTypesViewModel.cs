using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace EOTools.Translation.Ships.ShipType;

public partial class ShipTypesViewModel : ObservableObject
{
    public IEnumerable<ShipTypes> ShipTypes => Enum.GetValues<ShipTypes>();

    [ObservableProperty] private ShipTypes _shipType;
}
