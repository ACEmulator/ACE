using System.IO;

namespace ACE.Common.Extensions
{
    public static class BinaryReaderExtensions
    {
        private static uint CalculatePadMultiple(uint length, uint multiple)
        {
            return multiple * ((length + multiple - 1u) / multiple) - length;
        }

        public static string ReadString32L(this BinaryReader reader)
        {
            uint length = reader.ReadUInt32();

            // 32L strings are crazy.  the only place this is known as of time of writing this is in the
            // Login header packet.  it's a DWORD of the data length, followed by a packed word of the 
            // string length.  for most cases, this means the string comes out with a 1 or 2 character
            // prefix that just needs to get tossed.

            if (length == 0)
                return "";

            reader.Skip(1);
            length--;

            if (length > 255)
            {
                reader.Skip(1);
                length--;
            }

            string rdrStr = (length != 0 ? new string(reader.ReadChars((int)length)) : string.Empty);

            // in the login header, this is completely unnecessary as it's the end of the packet.  if
            // we find this is ever used somewhere else, we would need to validate it.
            reader.Skip(CalculatePadMultiple(sizeof(uint) + length, 4u));
            return rdrStr;
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
