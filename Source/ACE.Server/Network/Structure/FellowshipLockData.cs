using System.Collections.Generic;
using System.IO;

namespace ACE.Server.Network.Structure
{
    public class FellowshipLockData
    {
        public uint Unknown_1;
        public uint Unknown_2;
        public uint Unknown_3;
        public uint Timestamp;
        public uint Sequence;

        public FellowshipLockData() { }

        public FellowshipLockData(double timestamp)
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

    public static class FellowshipLockDataExtensions
    {
        public static ushort NumBuckets = 32;

        public static void Write(this BinaryWriter writer, FellowshipLockData fellowshipLockData)
        {
            writer.Write(fellowshipLockData.Unknown_1);
            writer.Write(fellowshipLockData.Unknown_2);
            writer.Write(fellowshipLockData.Unknown_3);
            writer.Write(fellowshipLockData.Timestamp);
            writer.Write(fellowshipLockData.Sequence);
        }

        public static void Write(this BinaryWriter writer, Dictionary<string, FellowshipLockData> fellowshipLocks)
        {
            writer.Write((ushort)fellowshipLocks.Count);
            writer.Write(NumBuckets);
            foreach (var kvp in fellowshipLocks)
            {
                writer.WriteString16L(kvp.Key);
                writer.Write(kvp.Value);
            }
        }
    }
}
