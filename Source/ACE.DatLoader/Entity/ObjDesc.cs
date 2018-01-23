using System.Collections.Generic;
using System.IO;

namespace ACE.DatLoader.Entity
{
    public class ObjDesc : IUnpackable
    {
        public uint PaletteID { get; private set; }
        public List<SubPalette> SubPalettes { get; } = new List<SubPalette>();
        public List<TextureMapChange> TextureChanges { get; } = new List<TextureMapChange>();
        public List<AnimationPartChange> AnimPartChanges { get; } = new List<AnimationPartChange>();

        public void Unpack(BinaryReader reader)
        {
            reader.AlignBoundary();

            reader.ReadByte(); // ObjDesc always starts with 11.

            var numPalettes             = reader.ReadByte();
            var numTextureMapChanges    = reader.ReadByte();
            var numAnimPartChanges      = reader.ReadByte();

            if (numPalettes > 0)
                PaletteID = reader.ReadAsDataIDOfKnownType(0x04000000);

            SubPalettes.Unpack(reader, numPalettes);
            TextureChanges.Unpack(reader, numTextureMapChanges);
            AnimPartChanges.Unpack(reader, numAnimPartChanges);

            reader.AlignBoundary();
        }
    }
}
