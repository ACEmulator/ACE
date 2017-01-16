using System;
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
	}
}
