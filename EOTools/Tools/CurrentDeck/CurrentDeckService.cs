using EOTools.DataBase;
using EOTools.Models;
using EOTools.Models.KancolleApi;
using EOTools.Models.KancolleApi.Port;
using EOTools.Models.Ships;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace EOTools.Tools.CurrentDeck;

/// <summary>
/// TODO : on ship change, if ship already in a fleet, replace it with new ship
/// TODO : ship removed from fleet
/// </summary>
public class CurrentDeckService
{
    public List<FleetModel> Fleets { get; set; } = new();

    public Port LastPort { get; set; } = new();

    public void UpdateDeck()
    {
        using ElectronicObserverContext eoDb = new();

        ApiFile lastPort = eoDb.ApiFiles
            .OrderBy(api => api.TimeStamp)
            .Last(api => api.Name == "api_port/port");

        List<ApiFile> apiToParse = eoDb.ApiFiles.Where(api => api.Id >= lastPort.Id).ToList();

        Fleets = new();

        foreach (ApiFile api in apiToParse)
        {
            ParseApi(api);
        }
    }

    private void ParseApi(ApiFile api)
    {
        if (api.Name is "api_port/port" && api.ApiFileType is ApiFileType.Response) ParsePortAPI();
        if (api.Name is "api_req_hensei/change" && api.ApiFileType is ApiFileType.Request) ParseChangeAPI(JsonSerializer.Deserialize<ApiReqHenseiChangeRequest>(api.Content)!);
    }

    private void ParsePortAPI()
    {
        // EO don't actually store all the data in its db to avoid having heavy db
        string portPath = Path.Combine(AppSettings.KancolleEOAPIFolder, "kcsapi", "api_port", "port");
        Port? api = SystemJsonHelper.ReadKCJson<Port>(portPath);

        if (api is null) return;

        LastPort = api;

        Fleets = api.ApiDeckPort
            .Select(fleet => new FleetModel()
            {
                Ships = fleet.ApiShip
                .Where(s => s > 0)
                .Select(s => new ShipDataModel()
                {
                    ApiId = s,
                    MasterShipId = LastPort.ApiShip.FirstOrDefault(ship => ship.ApiId == s)?.ApiShipId ?? 0
                }).ToList()
            }).ToList();
    }

    private void ParseChangeAPI(ApiReqHenseiChangeRequest api)
    {
        int apiId = int.Parse(api.ApiId);
        int apiShipId = int.Parse(api.ApiShipId);
        int slotId = int.Parse(api.ApiShipIdx);

        FleetModel fleet = Fleets[apiId - 1];
        ShipDataModel ship = fleet.Ships[slotId];

        ship.ApiId = apiShipId;
        ship.MasterShipId = LastPort.ApiShip.FirstOrDefault(ship => ship.ApiId == apiShipId)?.ApiShipId ?? 0;
    }
}
