using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.DatLoader.Entity;

namespace ACE.DatLoader.FileTypes
{
    public class SpellComponentsTable
    {
        uint FileId { get; set; }
        List<CSpellBase> SpellComponents { get; set; } = new List<CSpellBase>();

        public static CharGen ReadFromDat()
        {
            // Check the FileCache so we don't need to hit the FileSystem repeatedly
            if (DatManager.PortalDat.FileCache.ContainsKey(0x0E00000F))
            {
                return (CharGen)DatManager.PortalDat.FileCache[0x0E00000F];
            }
            else
            {                
                // Create the datReader for the proper file
                DatReader datReader = DatManager.PortalDat.GetReaderForFile(0x0E00000F);
                SpellComponentsTable comps = new SpellComponentsTable();

                comps.FileId = datReader.ReadInt32();


            }
        }
