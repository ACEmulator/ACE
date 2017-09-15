using ACE.Entity;

namespace ACE.DatLoader.Entity
{
    public class Contract
    {
        public uint Version { get; set; }
        public uint ContractId { get; set; }
        public string ContractName { get; set; }
        public string Description { get; set; }
        public string DescriptionProgress { get; set; }
        public string NameNPCStart { get; set; }
        public string NameNPCEnd { get; set; }
        public string QuestflagStamped { get; set; }
        public string QuestflagStarted { get; set; }
        public string QuestflagFinished { get; set; }
        public string QuestflagProgress { get; set; }
        public string QuestflagTimer { get; set; }
        public string QuestflagRepeatTime { get; set; }
        public Position LocationNPCStart { get; set; }
        public Position LocationNPCEnd { get; set; }
        public Position LocationQuestArea { get; set; }

        public static Contract Read(DatReader datReader)
        {
            Contract obj = new Contract();

            obj.Version = datReader.ReadUInt32();
            obj.ContractId = datReader.ReadUInt32();
            obj.ContractName = datReader.ReadPString();
            datReader.AlignBoundary();

            obj.Description = datReader.ReadPString();
            datReader.AlignBoundary();
            obj.DescriptionProgress = datReader.ReadPString();
            datReader.AlignBoundary();

            obj.NameNPCStart = datReader.ReadPString();
            datReader.AlignBoundary();
            obj.NameNPCEnd = datReader.ReadPString();
            datReader.AlignBoundary();

            obj.QuestflagStamped = datReader.ReadPString();
            datReader.AlignBoundary();
            obj.QuestflagStarted = datReader.ReadPString();
            datReader.AlignBoundary();
            obj.QuestflagFinished = datReader.ReadPString();
            datReader.AlignBoundary();
            obj.QuestflagProgress = datReader.ReadPString();
            datReader.AlignBoundary();
            obj.QuestflagTimer = datReader.ReadPString();
            datReader.AlignBoundary();
            obj.QuestflagRepeatTime = datReader.ReadPString();
            datReader.AlignBoundary();

            obj.LocationNPCStart = PositionExtensions.ReadLandblockPosition(datReader);
            obj.LocationNPCEnd = PositionExtensions.ReadLandblockPosition(datReader);
            obj.LocationQuestArea = PositionExtensions.ReadLandblockPosition(datReader);

            return obj;
        }
    }
}
