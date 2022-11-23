using Newtonsoft.Json;

namespace EOTools.Models.EquipmentUpgrade.Source;

public class EquipmentSourceData
{
    [JsonProperty("improvable")]
    public bool Improvable { get; set; }

}