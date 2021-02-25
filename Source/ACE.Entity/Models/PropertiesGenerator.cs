using System;

using ACE.Entity.Enum;

namespace ACE.Entity.Models
{
    public class PropertiesGenerator
    {
        /// <summary>
        /// This is only used to tie this property back to a specific database row
        /// </summary>
        public uint DatabaseRecordId { get; set; }

        public float Probability { get; set; }
        public uint WeenieClassId { get; set; }
        public float? Delay { get; set; }
        public int InitCreate { get; set; }
        public int MaxCreate { get; set; }
        public RegenerationType WhenCreate { get; set; }
        public RegenLocationType WhereCreate { get; set; }
        public int? StackSize { get; set; }
        public uint? PaletteId { get; set; }
        public float? Shade { get; set; }
        public uint? ObjCellId { get; set; }
        public float? OriginX { get; set; }
        public float? OriginY { get; set; }
        public float? OriginZ { get; set; }
        public float? AnglesW { get; set; }
        public float? AnglesX { get; set; }
        public float? AnglesY { get; set; }
        public float? AnglesZ { get; set; }

        public PropertiesGenerator Clone()
        {
            var result = new PropertiesGenerator
            {
                Probability = Probability,
                WeenieClassId = WeenieClassId,
                Delay = Delay,
                InitCreate = InitCreate,
                MaxCreate = MaxCreate,
                WhenCreate = WhenCreate,
                WhereCreate = WhereCreate,
                StackSize = StackSize,
                PaletteId = PaletteId,
                Shade = Shade,
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
