using System;
using System.IO;
using System.Numerics;

namespace ACE.DatLoader
{
    static class BinaryReaderExtensions
    {
        /// <summary>
        /// Aligns the stream to the next DWORD boundary.
        /// </summary>
        public static void AlignBoundary(this BinaryReader reader)
        {
            // Aligns the DatReader to the next DWORD boundary.
            long alignDelta = reader.BaseStream.Position % 4;

            if (alignDelta != 0)
                reader.BaseStream.Position += (int)(4 - alignDelta);
        }


        /// <summary>
        /// A Compressed UInt32 can be 1, 2, or 4 bytes.<para />
        /// If the first MSB (0x80) is 0, it is one byte.<para />
        /// If the first MSB (0x80) is set and the second MSB (0x40) is 0, it's 2 bytes.<para />
        /// If both (0x80) and (0x40) are set, it's 4 bytes.
        /// </summary>
        public static uint ReadCompressedUInt32(this BinaryReader reader)
        {
            var b0 = reader.ReadByte();
            if ((b0 & 0x80) == 0)
                return b0;

            var b1 = reader.ReadByte();
            if ((b0 & 0x40) == 0)
                return (uint)(((b0 & 0x7F) << 8) | b1);

            var s = reader.ReadUInt16();
            return (uint)(((((b0 & 0x3F) << 8) | b1) << 16) | s);
        }

        /// <summary>
        /// First reads a UInt16. If the MSB is set, it will be masked with 0x3FFF, shifted left 2 bytes, and then OR'd with the next UInt16. The sum is then added to knownType.
        /// </summary>
        public static uint ReadAsDataIDOfKnownType(this BinaryReader reader, uint knownType)
        {
            var value = reader.ReadUInt16();

            if ((value & 0x8000) != 0)
            {
                var lower = reader.ReadUInt16();
                var higher = (value & 0x3FFF) << 16;

                return (uint)(knownType + (higher | lower));
            }

            return (knownType + value);
        }

        /// <summary>
        /// Returns a string as defined by the first sizeOfLength-byte's length
        /// </summary>
        public static string ReadPString(this BinaryReader reader, uint sizeOfLength = 2)
        {
            int stringlength;
            switch (sizeOfLength)
            {
                case 1:
                    stringlength = reader.ReadByte();
                    break;
                case 2:
                default:
                    stringlength = reader.ReadUInt16();
                    break;
            }

            byte[] thestring = reader.ReadBytes(stringlength);

            return System.Text.Encoding.Default.GetString(thestring);
        }

        /// <summary>
        /// Returns a string as defined by the first byte's length and removes the obfuscation (upper/lower nibbles swapped)
        /// </summary>
        public static string ReadObfuscatedString(this BinaryReader reader)
        {
            int stringlength = reader.ReadUInt16();

            byte[] thestring = reader.ReadBytes(stringlength);

            for (var i = 0; i < stringlength; i++)
                // flip the bytes in the string to undo the obfuscation: i.e. 0xAB => 0xBA
                thestring[i] = (byte)((thestring[i] >> 4) | (thestring[i] << 4));

            return System.Text.Encoding.GetEncoding(1252).GetString(thestring);
        }

        public static string ReadUnicodeString(this BinaryReader reader)
        {
            uint stringLength = reader.ReadCompressedUInt32();
            string thestring = "";
            for (int i = 0; i < stringLength; i++)
            {
                ushort myChar = reader.ReadUInt16();
                thestring += Convert.ToChar(myChar);
            }
            return thestring;
        }

        public static string Reverse(this string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
 
        /// <summary>
        /// Returns a Vector3 object read out as 3 floats, x y z
        /// </summary>
        public static Vector3 ReadVector3(this BinaryReader reader)
        {
            var x = reader.ReadSingle();
            var y = reader.ReadSingle();
            var z = reader.ReadSingle();

            return new Vector3(x, y, z);
        }
    }
}
