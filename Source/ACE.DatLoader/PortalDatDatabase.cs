
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
            TabooTable = ReadFromDat<TabooTable>(TabooTable.FILE_ID);
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
        public TabooTable TabooTable { get; }
        public XpTable XpTable { get; }
    }
}
