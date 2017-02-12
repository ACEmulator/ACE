using System;
using System.IO;
using ACE.Entity;

namespace ACE.Network
{
    public static class Extensions
    {
        private static uint CalculatePadMultiple(uint length, uint multiple) { return multiple * ((length + multiple - 1u) / multiple) - length; }

        public static string ReadString16L(this BinaryReader reader)
        {
            ushort length = reader.ReadUInt16();
            string rdrStr = (length != 0 ? new string(reader.ReadChars(length)) : string.Empty);

            // client pads string length to be a multiple of 4 including the 2 bytes for length
            reader.Skip(CalculatePadMultiple(sizeof(ushort) + (uint)length, 4u));
            return rdrStr;
        }

        public static void WriteString16L(this BinaryWriter writer, string data)
        {
            writer.Write((ushort)data.Length);
            writer.Write(data.ToCharArray());

            // client expects string length to be a multiple of 4 including the 2 bytes for length
            writer.Pad(CalculatePadMultiple(sizeof(ushort) + (uint)data.Length, 4u));
        }

        public static string ReadString32L(this BinaryReader reader)
        {
            uint length = reader.ReadUInt32();
            return length != 0u ? new string(reader.ReadChars((int)length)) : string.Empty;
        }

        public static void WriteUInt16BE(this BinaryWriter writer, ushort value)
        {
            ushort beValue = (ushort)((ushort)((value & 0xFF) << 8) | ((value >> 8) & 0xFF));
            writer.Write(beValue);
        }

        public static void Skip(this BinaryReader reader, uint length) { reader.BaseStream.Position += length; }

        public static void Pad(this BinaryWriter writer, uint pad) { writer.Write(new byte[pad]); }

        public static void Align(this BinaryWriter writer)
        {
            writer.Pad(CalculatePadMultiple((uint)writer.BaseStream.Length, 4u));
        }

        /// <summary>
        /// This will output bytesToOutput bytes of fragment.Data (starting from startPosition) to the console.<para />
        /// The original Data.Position will be restored after the data is output. 
        /// </summary>
        public static void OutputDataToConsole(this PacketFragment fragment, bool outputIndex = true, bool outputHex = true, bool outputASCII = true, int startPosition = 0, int bytesToOutput = 9999)
        {
            var originalPosition = fragment.Data.Position;
            fragment.Data.Position = startPosition;

            byte[] buffer = new byte[Math.Min(fragment.Data.Length, bytesToOutput)];
            fragment.Data.Read(buffer, 0, buffer.Length);

            string indexOutput = null;
            string binaryOutput = null;
            string asciiOutput = null;

            for (int i = 0; i < buffer.Length; i++)
            {
                indexOutput += (i % 100).ToString("D2");

                binaryOutput += buffer[i].ToString("X2");

                asciiOutput += " "; // This right justifies the ASCII with the index or hex
                if (Char.IsControl((char)buffer[i]))
                    asciiOutput += " ";
                else
                    asciiOutput += (char)buffer[i];
            }

            if (outputIndex) Console.WriteLine(indexOutput);
            if (outputHex) Console.WriteLine(binaryOutput);
            if (outputASCII) Console.WriteLine(asciiOutput);

            fragment.Data.Position = originalPosition;
        }

        public static void WritePosition(this BinaryWriter writer, uint value, long position)
        {
            long originalPosition = writer.BaseStream.Position;
            writer.BaseStream.Position = position;
            writer.Write(value);
            writer.BaseStream.Position = originalPosition;
        }

        public static ObjectGuid ReadGuid(this BinaryReader reader) { return new ObjectGuid(reader.ReadUInt32()); }

        public static void WriteGuid(this BinaryWriter writer, ObjectGuid guid) { writer.Write(guid?.Full ?? 0u); }
    }
}
