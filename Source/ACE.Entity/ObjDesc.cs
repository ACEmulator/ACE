using System.Collections.Generic;

namespace ACE.Entity
{
    public class ObjDesc
    {
        public uint PaletteID { get; set; }
        public List<SubPalette> SubPalettes { get; set; } = new List<SubPalette>();
        public List<TextureMapChange> TextureChanges { get; set; } = new List<TextureMapChange>();
        public List<AnimationPartChange> AnimPartChanges { get; set; } = new List<AnimationPartChange>();
    }
}
