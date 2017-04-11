using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    public class ObjDesc
    {
        public List<SubPalette> SubPalettes { get; set; } = new List<SubPalette>();
        public List<TextureMapChange> TextureChanges { get; set; } = new List<TextureMapChange>();
        public List<AnimationPartChange> AnimPartChanges { get; set; } = new List<AnimationPartChange>();

        public static ObjDesc ReadFromDat(ref DatReader datReader)
        {
            ObjDesc od = new ObjDesc();
            datReader.AlignBoundary(); // Align to the DWORD boundary before and after the ObjDesc
            datReader.ReadByte(); // ObjDesc always starts with 11.
            int numPalettes = datReader.ReadByte();
            int numTextureMapChanges = datReader.ReadByte();
            int numAnimPartChanges = datReader.ReadByte();
            for (int k = 0; k < numPalettes; k++)
            {
                // TODO - This isn't actually used anywhere in the CharGen system, so let's find a test care to make sure this is accurate!
                SubPalette subpalette = new SubPalette();
                subpalette.SubID = datReader.ReadUInt16();
                subpalette.SubID = datReader.ReadUInt16();
                subpalette.NumColors = Convert.ToUInt16(datReader.ReadByte());
                od.SubPalettes.Add(subpalette);
            }
            for (int k = 0; k < numTextureMapChanges; k++)
            {
                TextureMapChange texturechange = new TextureMapChange();
                texturechange.PartIndex = Convert.ToUInt16(datReader.ReadByte());
                texturechange.OldTexture = datReader.ReadUInt16();
                texturechange.NewTexture = datReader.ReadUInt16();
                od.TextureChanges.Add(texturechange);
            }
            for (int k = 0; k < numAnimPartChanges; k++)
            {
                AnimationPartChange apchange = new AnimationPartChange();
                apchange.PartIndex = Convert.ToUInt16(datReader.ReadByte());
                apchange.PartID = datReader.ReadUInt16();
                if(apchange.PartID == 0x8000) // To be honest, I'm not quite sure WHAT this is/means, but the math works out
                    apchange.PartID = datReader.ReadUInt16();
                od.AnimPartChanges.Add(apchange);
            }
            datReader.AlignBoundary(); // Align to the DWORD boundary before and after the ObjDesc

            return od;
        }
    }
}
