using System.IO;

namespace ACE.Common.Extensions
{
    public static class BinaryReaderExtensions
    {
        private static uint CalculatePadMultiple(uint length, uint multiple) { return multiple * ((length + multiple - 1u) / multiple) - length; }

        public static string ReadString32L(this BinaryReader reader)
        {
            uint length = reader.ReadUInt32();
            return length != 0u ? new string(reader.ReadChars((int)length)) : string.Empty;
        }

        public static string ReadString16L(this BinaryReader reader)
        {
            ushort length = reader.ReadUInt16();
            string rdrStr = (length != 0 ? new string(reader.ReadChars(length)) : string.Empty);

            // client pads string length to be a multiple of 4 including the 2 bytes for length
            reader.Skip(CalculatePadMultiple(sizeof(ushort) + (uint)length, 4u));
            return rdrStr;
        }

        public static void Skip(this BinaryReader reader, uint length) { reader.BaseStream.Position += length; }
    }
}
