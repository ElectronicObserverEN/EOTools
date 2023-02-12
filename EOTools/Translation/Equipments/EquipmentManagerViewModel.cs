using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOTools.DataBase;
using EOTools.Models;
using EOTools.Tools;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EOTools.Translation.Equipments;

public partial class EquipmentManagerViewModel : ObservableObject
{
    public ObservableCollection<EquipmentViewModel> EquipmentModels { get; set; }

    public EquipmentManagerViewModel()
    {
        using EOToolsDbContext db = new();
        EquipmentModels = new(db.Equipments.Select(model => new EquipmentViewModel(model)).ToList());
    }


    private void AddNewEquipment(EquipmentModel model)
    {
        EquipmentViewModel vm = new(model);

        using EOToolsDbContext db = new();
        db.Equipments.Add(model);
        db.SaveChanges();

        EquipmentModels.Add(vm);
    }

    #region Data import and export stuff
    [RelayCommand]
    public void ImportFromTranslations()
    {
        // CSV file exported from EO converted to json
        JArray equipments = JsonHelper.ReadJsonArray("EquipmentData.json");

        // Translations to get JP name : 
        List<EquipData> translations = LoadEquipmentTranslations();

        EquipmentModels.Clear();
        using EOToolsDbContext db = new();
        db.Equipments.RemoveRange(db.Equipments);
        db.SaveChanges();

        foreach (JObject equipmentJson in equipments)
        {
            EquipmentModel model = new()
            {
                ApiId = equipmentJson.Value<int>("装備ID"),
                NameEN = equipmentJson.Value<string>("装備名"),
            };

            model.NameJP = translations.FirstOrDefault(eq => eq.NameEN == model.NameEN)?.NameJP ?? model.NameEN;

            AddNewEquipment(model);
        }
    }

    private List<EquipData> LoadEquipmentTranslations()
    {
        JObject translationsJson = JsonHelper.ReadJsonObject(AppSettings.EquipmentTLFilePath);
        JObject jsonEquipList = translationsJson["equipment"].Value<JObject>();
        List<EquipData> results = new();

        // --- Equips
        foreach (JProperty equip in jsonEquipList.Properties())
        {
            string _nameEN = equip.Name;
            string _nameJP = jsonEquipList[_nameEN].ToString();

            results.Add(new(_nameEN, _nameJP));
        }

        return results;
    }


    [RelayCommand]
    public void UpdateTranslations()
    {
        UpdateEquipmentDataService service = new();
        service.UpdateEquipmentTranslations();
    }
    #endregion
}
