using System;
using System.IO;
using ACE.Entity;
using ACE.Server.Network.Enum;

namespace ACE.Server.Network
{
    public static class Extensions
    {
        public static string GetDescription(this SessionTerminationReason reason)
        {
            if ((int)reason > BootReasonDescriptions.Length - 1)
            {
                return "<reason>";
            }
            return BootReasonDescriptions[(int)reason];
        }

        private static readonly string[] BootReasonDescriptions =
        {
            "",
            "PacketHeader Disconnect",
            "AccountSelectCallback threw an exception.",
            "Network Timeout",
            "client sent network error disconnect",
            "Account Booted",
            "Bad handshake",
            "Pong sent, closing connection.",
            "Not Authorized: No password or GlsTicket included in login request",
            "Not Authorized: Account Not Found",
            "Account In Use: Found another session already logged in for this account.",
            "Not Authorized: Password does not match.",
            "Not Authorized: GlsTicket is not implemented to process login request",
            "Client connection failure",
            "MainSocket.SendTo exception occured"
        };

        private static uint CalculatePadMultiple(uint length, uint multiple) { return multiple * ((length + multiple - 1u) / multiple) - length; }

        public static void WriteString16L(this BinaryWriter writer, string data)
        {
            if (data == null) data = "";

            writer.Write((ushort)data.Length);
            writer.Write(System.Text.Encoding.GetEncoding(1252).GetBytes(data));

            // client expects string length to be a multiple of 4 including the 2 bytes for length
            writer.Pad(CalculatePadMultiple(sizeof(ushort) + (uint)data.Length, 4u));
        }

        public static void WritePackedDword(this BinaryWriter writer, uint value)
        {
            if (value <= 32767)
            {
                ushort networkValue = Convert.ToUInt16(value);
                writer.Write(BitConverter.GetBytes(networkValue));
            }
            else
            {
                uint packedValue = (value << 16) | ((value >> 16) | 0x8000);
                writer.Write(BitConverter.GetBytes(packedValue));
            }
        }

        public static void WritePackedDwordOfKnownType(this BinaryWriter writer, uint value, uint type)
        {
            if ((value & type) > 0)
                value -= type;

            writer.WritePackedDword(value);
        }

        public static void WriteUInt16BE(this BinaryWriter writer, ushort value)
        {
            ushort beValue = (ushort)((ushort)((value & 0xFF) << 8) | ((value >> 8) & 0xFF));
            writer.Write(beValue);
        }

        public static void Pad(this BinaryWriter writer, uint pad) { writer.Write(new byte[pad]); }

        public static void Pad(this BinaryReader reader, uint pad) { reader.ReadBytes((int)pad); }

        public static void Align(this BinaryWriter writer)
        {
            writer.Pad(CalculatePadMultiple((uint)writer.BaseStream.Length, 4u));
        }

        public static void Align(this BinaryReader reader)
        {
            reader.Pad(CalculatePadMultiple((uint)reader.BaseStream.Length, 4u));
        }

        public static string BuildPacketString(this byte[] bytes, int startPosition = 0, int bytesToOutput = 9999)
        {
            TextWriter tw = new StringWriter();
            byte[] buffer = bytes;

            int column = 0;
            int row = 0;
            int columns = 16;
            tw.Write("   x  ");
            for (int i = 0; i < columns; i++)
            {
                tw.Write(i.ToString().PadLeft(3));
            }
            tw.WriteLine("  |Text");
            tw.Write("   0  ");

            string asciiLine = "";
            for (int i = 0; i < buffer.Length; i++)
            {
                if (column >= columns)
                {
                    row++;
                    column = 0;
                    tw.WriteLine("  |" + asciiLine);
                    asciiLine = "";
                    tw.Write((row * columns).ToString().PadLeft(4));
                    tw.Write("  ");
                }

                tw.Write(buffer[i].ToString("X2").PadLeft(3));

                if (Char.IsControl((char)buffer[i]))
                    asciiLine += " ";
                else
                    asciiLine += (char)buffer[i];
                column++;
            }

            tw.Write("".PadLeft((columns - column) * 3));
            tw.WriteLine("  |" + asciiLine);
            return tw.ToString();
        }

        public static void WritePosition(this BinaryWriter writer, uint value, long position)
        {
            long originalPosition = writer.BaseStream.Position;
            writer.BaseStream.Position = position;
            writer.Write(value);
            writer.BaseStream.Position = originalPosition;
        }

        public static ObjectGuid ReadGuid(this BinaryReader reader) { return new ObjectGuid(reader.ReadUInt32()); }

        public static void WriteGuid(this BinaryWriter writer, ObjectGuid guid) { writer.Write(guid.Full); }
    }
}
