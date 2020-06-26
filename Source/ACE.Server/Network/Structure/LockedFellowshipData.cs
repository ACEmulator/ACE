using System.Collections.Generic;
using System.IO;

namespace ACE.Server.Network.Structure
{
    public class LockedFellowshipData
    {
        public uint Unknown_1;
        public uint Unknown_2;
        public uint Unknown_3;
        public uint Timestamp;
        public uint Sequence;

        public LockedFellowshipData() { }

        public LockedFellowshipData(double timestamp)
        {
            Unknown_1 = 0x0; // always 0 in pcaps
            Unknown_2 = 0x0; // unknown?
            Unknown_3 = 0x0; // unknown?
            Timestamp = (uint)timestamp;
            Sequence = 1;
        }

        public void UpdateTimestamp(double timestamp)
        {
            Timestamp = (uint)timestamp;
            Sequence++;
        }
    }

    public static class LockedFellowshipDataExtensions
    {
        public static void Write(this BinaryWriter writer, LockedFellowshipData lockedFellowshipData)
        {
            writer.Write(lockedFellowshipData.Unknown_1);
            writer.Write(lockedFellowshipData.Unknown_2);
            writer.Write(lockedFellowshipData.Unknown_3);
            writer.Write(lockedFellowshipData.Timestamp);
            writer.Write(lockedFellowshipData.Sequence);
        }

        public static void Write(this BinaryWriter writer, Dictionary<string, LockedFellowshipData> lockedFellowshipDataList)
        {
            PackableHashTable.WriteHeader(writer, lockedFellowshipDataList.Count);
            foreach (var kvp in lockedFellowshipDataList)
            {
                writer.WriteString16L(kvp.Key);
                writer.Write(kvp.Value);
            }
        }
    }
}
