using CommunityToolkit.Mvvm.ComponentModel;
using EOTools.Models.EquipmentUpgrade;

namespace EOTools.Translation.EquipmentUpgrade;

public partial class EquipmentUpgradeImprovmentCostDetailViewModel : ObservableObject
{
    [ObservableProperty]
    private int devmatCost;
    [ObservableProperty]
    private int sliderDevmatCost;
    [ObservableProperty]
    private int improveMatCost;
    [ObservableProperty]
    private int sliderImproveMatCost;

    public EquipmentUpgradeImprovmentCostDetail Model { get; set; }

    public EquipmentUpgradeImprovmentCostDetailViewModel(EquipmentUpgradeImprovmentCostDetail model)
    {
        Model = model;

        LoadFromModel();
    }

    public void LoadFromModel()
    {
        DevmatCost = Model.DevmatCost;
        SliderDevmatCost = Model.SliderDevmatCost;
        ImproveMatCost = Model.ImproveMatCost;
        SliderImproveMatCost = Model.SliderImproveMatCost;
    }

    public void SaveChanges()
    {
        Model.DevmatCost = DevmatCost;
        Model.SliderDevmatCost = SliderDevmatCost;
        Model.ImproveMatCost = ImproveMatCost;
        Model.SliderImproveMatCost = SliderImproveMatCost;
    }
}
