using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.DatLoader.Entity;
using System.Globalization.TextInfo;

namespace ACE.DatLoader.FileTypes
{
    public class SpellComponentsTable
    {
        private uint FileId { get; set; }
        public Dictionary<uint, SpellComponentBase> SpellComponents { get; set; } = new Dictionary<uint, SpellComponentBase>();

        public static SpellComponentsTable ReadFromDat()
        {
            // Check the FileCache so we don't need to hit the FileSystem repeatedly
            if (DatManager.PortalDat.FileCache.ContainsKey(0x0E00000F))
            {
                return (SpellComponentsTable)DatManager.PortalDat.FileCache[0x0E00000F];
            }
            else
            {
                // Create the datReader for the proper file
                DatReader datReader = DatManager.PortalDat.GetReaderForFile(0x0E00000F);
                SpellComponentsTable comps = new SpellComponentsTable();

                comps.FileId = datReader.ReadUInt32();
                uint unknown = datReader.ReadUInt32();
                // loop through the entire file...
                while (datReader.Offset < datReader.Buffer.Length)
                {
                    SpellComponentBase newComp = new SpellComponentBase();
                    uint compId = datReader.ReadUInt32();
                    newComp.Name = datReader.ReadObfuscatedString();
                    datReader.AlignBoundary();
                    newComp.Category = datReader.ReadUInt32();
                    newComp.Icon = datReader.ReadUInt32();
                    newComp.Gesture = datReader.ReadUInt32();
                    newComp.Time = datReader.ReadSingle();
                    newComp.Text = datReader.ReadObfuscatedString();
                    datReader.AlignBoundary();
                    newComp.CDM = datReader.ReadSingle();
                    comps.SpellComponents.Add(compId, newComp);
                }

                DatManager.PortalDat.FileCache[0x0E00000F] = comps;
                return comps;
            }
        }

        // TODO - Complete this function.
        public static string GetSpellWords(List<uint> comps)
        {
            string result = "";
            return result;
        }
    }
}
