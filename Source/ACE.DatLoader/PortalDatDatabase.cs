using System.Collections.Generic;
using System.IO;

using ACE.DatLoader.FileTypes;

namespace ACE.DatLoader
{
    public class PortalDatDatabase : DatDatabase
    {
        public PortalDatDatabase(string filename, bool keepOpen = false) : base(filename, keepOpen)
        {
            BadData = ReadFromDat<BadData>(BadData.FILE_ID);
            ChatPoseTable = ReadFromDat<ChatPoseTable>(ChatPoseTable.FILE_ID);
            CharGen = ReadFromDat<CharGen>(CharGen.FILE_ID);
            ContractTable = ReadFromDat<ContractTable>(ContractTable.FILE_ID);
            GeneratorTable = ReadFromDat<GeneratorTable>(GeneratorTable.FILE_ID);
            NameFilterTable = ReadFromDat<NameFilterTable>(NameFilterTable.FILE_ID);
            RegionDesc = ReadFromDat<RegionDesc>(RegionDesc.FILE_ID);
            SecondaryAttributeTable = ReadFromDat<SecondaryAttributeTable>(SecondaryAttributeTable.FILE_ID);
            SkillTable = ReadFromDat<SkillTable>(SkillTable.FILE_ID);
            SpellComponentsTable = ReadFromDat<SpellComponentsTable>(SpellComponentsTable.FILE_ID);
            SpellTable = ReadFromDat<SpellTable>(SpellTable.FILE_ID);
            XpTable = ReadFromDat<XpTable>(XpTable.FILE_ID);
        }

        public BadData BadData { get; }
        public ChatPoseTable ChatPoseTable { get; }
        public CharGen CharGen { get; }
        public ContractTable ContractTable { get; }
        public GeneratorTable GeneratorTable { get; }
        public NameFilterTable NameFilterTable { get; }
        public RegionDesc RegionDesc { get; }
        public SecondaryAttributeTable SecondaryAttributeTable { get; }
        public SkillTable SkillTable { get; }
        public SpellComponentsTable SpellComponentsTable { get; }
        public SpellTable SpellTable { get; }
        public XpTable XpTable { get; }

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
