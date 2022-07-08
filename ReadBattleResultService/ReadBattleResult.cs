using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadBattleResultService
{
    public static class ReadBattleResult
    {
        public static ILogger<Worker>? Logger { get; set; }

        public static void ReadAndParseFile(string filepath, string destinationPath)
        {
            JObject kcJson = ReadJsonBattleResult(filepath);
            if (kcJson is null) return;

            string? _fleetName = kcJson["api_data"]?["api_enemy_info"]?.Value<string>("api_deck_name");

            if (_fleetName is null) return;

            JArray parsedFleetJson = JsonHelper.ReadJsonArray(destinationPath) ?? new JArray();
            List<string> fleetNames = parsedFleetJson.Select(_token => _token.Value<string>()).ToList();
            if (!fleetNames.Contains(_fleetName))
            {
                if (Logger != null) Logger.LogInformation("{time}: Added new fleet \"{fleet}\"", DateTimeOffset.Now, _fleetName);
                parsedFleetJson.Add(_fleetName);
                JsonHelper.WriteJson(destinationPath, parsedFleetJson);
            }
        }

        private static JObject ReadJsonBattleResult(string filepath)
        {
            return JsonHelper.ReadKCJson(filepath);
        }
    }
}

//{ "api_ship_id":[1571,1530,1530,1530],"api_win_rank":"S","api_get_exp":140,"api_mvp":2,"api_member_lv":120,"api_member_exp":90622594,"api_get_base_exp":150,"api_get_ship_exp":[-1,270,360,180,180,180,-1],"api_get_exp_lvup":[[748409,761500],[6683755,6910000],[6687812,6910000],[7276127,7320000],[1121566,1125000]],"api_dests":4,"api_destsf":1,"api_quest_name":"ブルネイ泊地沖","api_quest_level":5,"api_enemy_info":{ "api_level":"","api_rank":"","api_deck_name":"深海潜水艦隊 II群"},"api_first_clear":0,"api_mapcell_incentive":0,"api_get_flag":[0,0,0],"api_get_eventflag":0,"api_get_exmap_rate":0,"api_get_exmap_useitem_id":0,"api_escape_flag":0,"api_escape":null}