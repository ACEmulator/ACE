using ACE.Database.Models.World;

namespace ACE.Database.Extensions
{
    public static class WorldDbExtensions
    {
        public static TreasureMaterialBase Clone(this TreasureMaterialBase materialBase)
        {
            var result = new TreasureMaterialBase();
            result.Id = materialBase.Id;
            result.MaterialCode = materialBase.MaterialCode;
            result.MaterialId = materialBase.MaterialId;
            result.Probability = materialBase.Probability;
            result.Tier = materialBase.Tier;
            return result;
        }

        public static TreasureMaterialGroups Clone(this TreasureMaterialGroups materialGroup)
        {
            var result = new TreasureMaterialGroups();
            result.Id = materialGroup.Id;
            result.MaterialGroup = materialGroup.MaterialGroup;
            result.MaterialId = materialGroup.MaterialId;
            result.Probability = materialGroup.Probability;
            result.Tier = materialGroup.Tier;
            return result;
        }

        public static TreasureMaterialColor Clone(this TreasureMaterialColor materialColor)
        {
            var result = new TreasureMaterialColor();
            result.ColorCode = materialColor.ColorCode;
            result.Id = materialColor.Id;
            result.MaterialId = materialColor.MaterialId;
            result.PaletteTemplate = materialColor.PaletteTemplate;
            result.Probability = materialColor.Probability;
            return result;
        }
    }
}
