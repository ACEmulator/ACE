using System.Collections.Generic;
using System.IO;

using ACE.DatLoader.FileTypes;

namespace ACE.DatLoader
{
    public class PortalDatDatabase : DatDatabase
    {
        public PortalDatDatabase(string filename) : base(filename)
        {
        }


        public ContractTable ContractTable => ReadFromDat<ContractTable>(ContractTable.FILE_ID);

        public CharGen CharGen => ReadFromDat<CharGen>(CharGen.FILE_ID);

        public GeneratorTable GeneratorTable => ReadFromDat<GeneratorTable>(GeneratorTable.FILE_ID);

        public RegionDesc RegionDesc => ReadFromDat<RegionDesc>(RegionDesc.FILE_ID);

        public SpellComponentsTable SpellComponentsTable => ReadFromDat<SpellComponentsTable>(SpellComponentsTable.FILE_ID);

        public SpellTable SpellTable => ReadFromDat<SpellTable>(SpellTable.FILE_ID);

        public XpTable XpTable => ReadFromDat<XpTable>(XpTable.FILE_ID);


        public void ExtractCategorizedContents(string path)
        {
            foreach (KeyValuePair<uint, DatFile> entry in AllFiles)
            {
                string thisFolder;

                if (entry.Value.GetFileType(DatDatabaseType.Portal) != null)
                    thisFolder = Path.Combine(path, entry.Value.GetFileType(DatDatabaseType.Portal).ToString());
                else
                    thisFolder = Path.Combine(path, "UnknownType");

                if (!Directory.Exists(thisFolder))
                    Directory.CreateDirectory(thisFolder);

                string hex = entry.Value.ObjectId.ToString("X8");
                string thisFile = Path.Combine(thisFolder, hex + ".bin");

                // Use the DatReader to get the file data
                DatReader dr = GetReaderForFile(entry.Value.ObjectId);
                   
                File.WriteAllBytes(thisFile, dr.Buffer);
            }
        }
    }
}
