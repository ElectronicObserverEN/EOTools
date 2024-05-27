using System;
using EOTools.DataBase;
using EOTools.Models.Ships;
using EOTools.Models.ShipTranslation;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using EOTools.Models;

namespace EOTools.Tools;

public class UpdateShipDataService : TranslationUpdateService
{
    private GitManager GitManager => new(AppSettings.ElectronicObserverDataFolderPath);

    private string UpdateFilePath => Path.Combine(AppSettings.ElectronicObserverDataFolderPath, "Translations", "en-US", "update.json");
    public static string ShipTranslationsFilePath => Path.Combine(AppSettings.ElectronicObserverDataFolderPath, "Translations", "en-US", "ship.json");

    private Dictionary<string, string> Suffixes { get; set; } = [];

    public void UpdateShipTranslations()
    {
        // --- Stage & push
        GitManager.Pull();

        // Get version
        JObject updateJson = JsonHelper.ReadJsonObject(UpdateFilePath);
        string version = (int.Parse(updateJson.Value<string>("ship")) + 1).ToString();

        ShipTranslationModel? toSerialize = JsonSerializer.Deserialize<ShipTranslationModel>(File.ReadAllText(ShipTranslationsFilePath));

        if (toSerialize is null) return;

        Suffixes = toSerialize.Suffixes;

        UpdateOtherLanguage("en-US");
        OtherLanguages.ForEach(UpdateOtherLanguage);

        new DatabaseSyncService().StageDatabaseChangesToGit();
        
        GitManager.CommitAndPush($"Ships - {version}");
    }

    public void UpdateOtherLanguage(string language)
    {
        string updatePath = UpdateFilePath.Replace("en-US", language);
        string shipPath = ShipTranslationsFilePath.Replace("en-US", language);

        // Get version
        JObject updateJson = JsonHelper.ReadJsonObject(updatePath);
        string version = (int.Parse(updateJson.Value<string>("ship")) + 1).ToString();

        updateJson["ship"] = version;

        using EOToolsDbContext db = new();

        List<ShipModel> ships = db.Ships
            .AsEnumerable()
            .OrderBy(ship => ship.ApiId)
            .ToList();

        List<ShipClassModel> classes = db.ShipClass
            .AsEnumerable()
            .OrderBy(shipClass => shipClass.ApiId)
            .ToList();

        ShipTranslationModel? toSerialize = JsonSerializer.Deserialize<ShipTranslationModel>(File.ReadAllText(shipPath));

        if (toSerialize is null) return;

        toSerialize.Version = version;
        Dictionary<string, string> translationsShips = toSerialize.Ships;
        Dictionary<string, string> translationsClasses = toSerialize.Classes;
        
        foreach (ShipModel model in ships)
        {
            if (!translationsShips.ContainsKey(model.NameJP) && ShouldBeTranslated(model))
            {
                translationsShips.Add(model.NameJP, model.NameEN);
            }
        }

        foreach (ShipClassModel model in classes)
        {
            if (!translationsClasses.ContainsKey(model.NameJapanese))
            {
                translationsClasses.Add(model.NameJapanese, model.NameEnglish);
            }
        }

        JsonSerializerOptions options = new()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true,
        };

        File.WriteAllText(shipPath, JsonSerializer.Serialize(toSerialize, options), Encoding.UTF8);

        JsonHelper.WriteJson(updatePath, updateJson);

        GitManager.Stage(shipPath);
        GitManager.Stage(updatePath);
    }

    private bool ShouldBeTranslated(ShipModel model)
    {
        if (model.NameJP == model.NameEN) return false;

        foreach (KeyValuePair<string, string> suffix in Suffixes)
        {
            if (model.NameJP.Contains(suffix.Key) && model.NameEN.Contains(suffix.Value))
            {
                return false;
            }
        }

        return true;
    }
}
