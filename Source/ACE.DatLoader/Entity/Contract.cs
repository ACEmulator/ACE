using System.IO;

namespace ACE.DatLoader.Entity
{
    public class Contract : IUnpackable
    {
        public uint Version { get; private set; }
        public uint ContractId { get; private set; }
        public string ContractName { get; private set; }

        public string Description { get; private set; }
        public string DescriptionProgress { get; private set; }

        public string NameNPCStart { get; private set; }
        public string NameNPCEnd { get; private set; }

        public string QuestflagStamped { get; private set; }
        public string QuestflagStarted { get; private set; }
        public string QuestflagFinished { get; private set; }
        public string QuestflagProgress { get; private set; }
        public string QuestflagTimer { get; private set; }
        public string QuestflagRepeatTime { get; private set; }

        public Position LocationNPCStart { get; } = new Position();
        public Position LocationNPCEnd { get; } = new Position();
        public Position LocationQuestArea { get; } = new Position();

        public void Unpack(BinaryReader reader)
        {
            Version = reader.ReadUInt32();
            ContractId = reader.ReadUInt32();
            ContractName = reader.ReadPString();
            reader.AlignBoundary();

            Description = reader.ReadPString();
            reader.AlignBoundary();
            DescriptionProgress = reader.ReadPString();
            reader.AlignBoundary();

            NameNPCStart = reader.ReadPString();
            reader.AlignBoundary();
            NameNPCEnd = reader.ReadPString();
            reader.AlignBoundary();

            QuestflagStamped = reader.ReadPString();
            reader.AlignBoundary();
            QuestflagStarted = reader.ReadPString();
            reader.AlignBoundary();
            QuestflagFinished = reader.ReadPString();
            reader.AlignBoundary();
            QuestflagProgress = reader.ReadPString();
            reader.AlignBoundary();
            QuestflagTimer = reader.ReadPString();
            reader.AlignBoundary();
            QuestflagRepeatTime = reader.ReadPString();
            reader.AlignBoundary();

            LocationNPCStart.Unpack(reader);
            LocationNPCEnd.Unpack(reader);
            LocationQuestArea.Unpack(reader);
        }
    }
}
