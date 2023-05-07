using EOTools.Models.ShipTranslation;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Nodes;

namespace EOTools.Tools.Translations;

/// <summary>
/// Copy paste of EO's ShipTranslationData
/// </summary>
public class ShipTranslationService : TranslationBase
{
    private string FilePath = Path.Combine(AppSettings.ElectronicObserverDataFolderPath, "Translations", "en-US", "ship.json");

    private Dictionary<string, string> ShipList;
    private Dictionary<string, string> TypeList;
    private Dictionary<string, string> SuffixList;
    private Dictionary<string, string> ClassList;

    private bool isShipLoaded => ShipList != null && SuffixList != null;
    private bool isTypeLoaded => TypeList != null;
    private bool isClassLoaded => ClassList != null;

    private Dictionary<int, string> NameCache { get; } = new();

    public string Name(string rawData, int shipId)
    {
        if (isShipLoaded == false) return rawData;

        if (!NameCache.ContainsKey(shipId))
        {
            NameCache.Add(shipId, TranslateName(rawData));
        }

        return NameCache[shipId];
    }

    private string TranslateName(string rawData)
    {
        // save current ship name to prevent suffix replacements that can show up in names
        // tre suffix can be found in Intrepid which gets you In Trepid
        string currentShipName = "";

        foreach (var s in ShipList.OrderByDescending(s => s.Key.Length))
        {
            if (rawData.Equals(s.Key)) return s.Value;

            if (rawData.StartsWith(s.Key))
            {
                var pos = rawData.IndexOf(s.Key);
                rawData = rawData.Remove(pos, s.Key.Length).Insert(pos, s.Value);
                currentShipName = s.Key;
            }
        }

        var name = rawData; // prevent suffix from being replaced twice.

        foreach (var sf in SuffixList.OrderByDescending(sf => sf.Key.Length))
        {
            if (rawData.Contains(sf.Key))
            {
                var pos = rawData.IndexOf(sf.Key);

                if (pos < currentShipName.Length) continue;

                rawData = rawData.Remove(pos, sf.Key.Length).Insert(pos, new string('0', sf.Value.Length));
                name = name.Remove(pos, sf.Key.Length).Insert(pos, sf.Value);

                if (rawData.Substring(pos - 1, 1).Contains(" ") == false)
                {
                    rawData = rawData.Insert(pos, " ");
                    name = name.Insert(pos, " ");
                }
            }
        }

        return name;
    }

    /// <summary>
    /// Translation of the class of a ship
    /// </summary>
    /// <param name="rawData"></param>
    /// <returns></returns>
    public string Class(string rawData) => isClassLoaded && ClassList.ContainsKey(rawData) ? ClassList[rawData] : rawData;

    public string TypeName(string rawData) => isTypeLoaded && TypeList.ContainsKey(rawData) ? TypeList[rawData] : rawData;

    public ShipTranslationService()
    {
        Initialize();
    }

    public override void Initialize()
    {
        NameCache.Clear();
        ShipList = new Dictionary<string, string>();
        TypeList = new Dictionary<string, string>();
        SuffixList = new Dictionary<string, string>();
        ClassList = new Dictionary<string, string>();
        LoadDictionary(FilePath);
    }

    public void LoadDictionary(string path)
    {
        ShipTranslationModel? json = Load<ShipTranslationModel>(path);
        if (json is null) return;

        ShipList = json.Ships;
        TypeList = json.Types;
        SuffixList = json.Suffixes;
        ClassList = json.Classes;
    }
}
