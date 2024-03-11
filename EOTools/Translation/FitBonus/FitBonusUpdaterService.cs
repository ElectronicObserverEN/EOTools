using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EOTools.Models.FitBonus;
using EOTools.Tools;
using EOTools.Translation.FitBonus.FitBonusSourceV1;
using EOTools.Translation.Ships.ShipNationality;
using EOTools.Translation.Ships.ShipType;

namespace EOTools.Translation.FitBonus;

public class FitBonusUpdaterService
{
    public string SourceUrl => AppSettings.FitBonusSourceUrl;
    public HttpClient HttpClient { get; } = new();

    public async Task<List<FitBonusPerEquipmentViewModel>?> GetFitBonuses()
    {
        List<FitBonusSourceV1.FitBonusSourceV1>? bonuses = await HttpClient.GetFromJsonAsync<List<FitBonusSourceV1.FitBonusSourceV1>>(SourceUrl);

        if (bonuses is null) return null;

        List<FitBonusPerEquipmentViewModel> eoBonuses = new();

        foreach (FitBonusSourceV1.FitBonusSourceV1 bonus in bonuses)
        {
            eoBonuses.Add(new(new()
            {
                EquipmentIds = bonus.Ids,
                EquipmentTypes = bonus.Types,
                Bonuses = bonus.Bonuses.Select(ConvertBonus).ToList(),
            }));
        }

        return eoBonuses;
    }

    private FitBonusDataModel ConvertBonus(FitBonusSourceV1_FitBonus bonus)
    {
        FitBonusDataModel model = new()
        {
            EquipmentLevel = bonus.Level,

            EquipmentRequired = bonus.RequiresId,
            EquipmentRequiresLevel = bonus.RequiresIdLevel,
            NumberOfEquipmentsRequiredAfterOtherFilters = bonus.RequiresIdNum switch
            {
                { } => bonus.RequiresIdNum,
                _ => bonus.Num,
            },

            EquipmentTypesRequired = bonus.RequiresType,
            NumberOfEquipmentTypesRequired = bonus.RequiresType switch
            {
                { } => 1,
                _ => null,
            },

            ShipClasses = bonus.ShipClass,
            ShipMasterIds = bonus.ShipId,
            ShipIds = bonus.ShipBase,
            ShipNationalities = bonus.ShipCountry?.Select(ConvertNationality).ToList(),
            ShipTypes = bonus.ShipType?.Select(st => (ShipTypes)st).ToList(),
        };

        if (bonus.RequiresAR > 0)
        {
            model.BonusesIfAirRadar = ConvertBonusValue(bonus.Bonus);
        }
        else if (bonus.RequiresSR > 0)
        {
            model.BonusesIfSurfaceRadar = ConvertBonusValue(bonus.Bonus);
        }
        else if (bonus.RequiresAccR > 0)
        {
            model.BonusesIfAccuracyRadar = ConvertBonusValue(bonus.Bonus);
        }
        else
        {
            model.Bonuses = ConvertBonusValue(bonus.Bonus);
        }

        return model;
    }

    private static FitBonusValueModel ConvertBonusValue(FitBonusSourceV1_BonusValue bonus)
    {
        return new()
        {
            Firepower = bonus.Houg,
            Torpedo = bonus.Raig,
            AntiAir = bonus.Tyku,
            Armor = bonus.Souk,
            Evasion = bonus.Kaih,
            ASW = bonus.Tais,
            LOS = bonus.Saku,
            Bombing = bonus.Baku,
            Accuracy = bonus.Houm,
            Range = bonus.Leng,
        };
    }

    private static ShipNationality ConvertNationality(string nationality)
    {
        return nationality switch
        {
            "JP" => ShipNationality.Japanese,
            "DE" => ShipNationality.German,
            "IT" => ShipNationality.Italian,
            "US" => ShipNationality.American,
            "GB" => ShipNationality.British,
            "FR" => ShipNationality.French,
            "AU" => ShipNationality.Australian,
            /*
               "" => ShipNationality.Russian,
               "" => ShipNationality.Swedish,
               "" => ShipNationality.Dutch,
            */
            _ => throw new NotImplementedException(),
        };
    }
}