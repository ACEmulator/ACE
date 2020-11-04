using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World
{
    public partial class WeeniePropertiesGenerator
    {
        public uint Id { get; set; }
        public uint ObjectId { get; set; }
        public float Probability { get; set; }
        public uint WeenieClassId { get; set; }
        public float? Delay { get; set; }
        public int InitCreate { get; set; }
        public int MaxCreate { get; set; }
        public uint WhenCreate { get; set; }
        public uint WhereCreate { get; set; }
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

        public virtual Weenie Object { get; set; }
    }
}
