using Newtonsoft.Json;
using System.Collections.Generic;

namespace EOTools.Models.EquipmentUpgrade.Source;


public class EquipmentUpgradeSourceDataImprovement
{
    [JsonProperty("upgrade")]
    public object Upgrade { get; set; }

    [JsonProperty("req")]
    public List<List<object>> Req { get; set; }

    [JsonProperty("resource")]
    public List<List<object>> Resource { get; set; }
}
