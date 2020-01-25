using System;

namespace ACE.Entity.Models
{
    public class PropertiesEmoteAction
    {
        public uint Order { get; set; } // TODO investigate the need for this? maybe make sure the order is correct in the list when these objects are created
        public uint Type { get; set; }
        public float Delay { get; set; }
        public float Extent { get; set; }
        public int? Motion { get; set; }
        public string Message { get; set; }
        public string TestString { get; set; }
        public int? Min { get; set; }
        public int? Max { get; set; }
        public long? Min64 { get; set; }
        public long? Max64 { get; set; }
        public double? MinDbl { get; set; }
        public double? MaxDbl { get; set; }
        public int? Stat { get; set; }
        public bool? Display { get; set; }
        public int? Amount { get; set; }
        public long? Amount64 { get; set; }
        public long? HeroXP64 { get; set; }
        public double? Percent { get; set; }
        public int? SpellId { get; set; }
        public int? WealthRating { get; set; }
        public int? TreasureClass { get; set; }
        public int? TreasureType { get; set; }
        public int? PScript { get; set; }
        public int? Sound { get; set; }
        public sbyte? DestinationType { get; set; }
        public uint? WeenieClassId { get; set; }
        public int? StackSize { get; set; }
        public int? Palette { get; set; }
        public float? Shade { get; set; }
        public bool? TryToBond { get; set; }
        public uint? ObjCellId { get; set; }
        public float? OriginX { get; set; }
        public float? OriginY { get; set; }
        public float? OriginZ { get; set; }
        public float? AnglesW { get; set; }
        public float? AnglesX { get; set; }
        public float? AnglesY { get; set; }
        public float? AnglesZ { get; set; }

        public PropertiesEmoteAction Clone()
        {
            var result = new PropertiesEmoteAction
            {
                Order = Order,
                Type = Type,
                Delay = Delay,
                Extent = Extent,
                Motion = Motion,
                Message = Message,
                TestString = TestString,
                Min = Min,
                Max = Max,
                Min64 = Min64,
                Max64 = Max64,
                MinDbl = MinDbl,
                MaxDbl = MaxDbl,
                Stat = Stat,
                Display = Display,
                Amount = Amount,
                Amount64 = Amount64,
                HeroXP64 = HeroXP64,
                Percent = Percent,
                SpellId = SpellId,
                WealthRating = WealthRating,
                TreasureClass = TreasureClass,
                TreasureType = TreasureType,
                PScript = PScript,
                Sound = Sound,
                DestinationType = DestinationType,
                WeenieClassId = WeenieClassId,
                StackSize = StackSize,
                Palette = Palette,
                Shade = Shade,
                TryToBond = TryToBond,
                ObjCellId = ObjCellId,
                OriginX = OriginX,
                OriginY = OriginY,
                OriginZ = OriginZ,
                AnglesW = AnglesW,
                AnglesX = AnglesX,
                AnglesY = AnglesY,
                AnglesZ = AnglesZ,
            };

            return result;
        }
    }
}
