using System.IO;

namespace ACE.Network
{
    public static class Extensions
    {
        public static string ReadString16L(this BinaryReader reader)
        {
            ushort length = reader.ReadUInt16();
            return length != 0 ? new string(reader.ReadChars(length)) : string.Empty;
        }

        public static void WriteString16L(this BinaryWriter writer, string data)
        {
            writer.Write((ushort)data.Length);
            writer.Write(data.ToCharArray());
        }

        public static string ReadString32L(this BinaryReader reader)
        {
            uint length = reader.ReadUInt32();
            return length != 0 ? new string(reader.ReadChars((int)length)) : string.Empty;
        }

        public static void Skip(this BinaryReader reader, uint length) { reader.BaseStream.Position += length; }
    }
}
