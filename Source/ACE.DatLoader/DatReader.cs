using System;
using System.IO;

namespace ACE.DatLoader
{
    public class DatReader
    {
        public int Offset { get; set; }
        private byte[] buffer;

        public DatReader(string datFilePath, uint offset, uint size, uint sectorSize)
        {
            uint nextAddress = 0;
            FileStream stream = new FileStream(datFilePath, FileMode.Open, FileAccess.Read);
            using (stream)
            {
                this.buffer = new byte[size];
                stream.Seek(offset, SeekOrigin.Begin);

                if (size > sectorSize)
                    nextAddress = this.GetNextAddress(stream, -4);

                int bufferOffset = 0;
                while (size > 0)
                {
                    if (size < sectorSize)
                    {
                        stream.Read(buffer, bufferOffset, Convert.ToInt32(size));
                        size = 0;
                    }
                    else
                    {
                        stream.Read(buffer, bufferOffset, Convert.ToInt32(sectorSize) - 4);
                        bufferOffset += Convert.ToInt32(sectorSize) - 4;
                        offset += sectorSize - 4;
                        stream.Seek(nextAddress, SeekOrigin.Begin);
                        nextAddress = GetNextAddress(stream, 0);
                        size -= (sectorSize + 4);
                    }
                }
                stream.Close();
            }
        }

        private uint GetNextAddress(FileStream stream, int relOffset)
        {
            byte[] nextAddressBytes = new byte[4];
            if (relOffset != 0)
                stream.Seek(relOffset, SeekOrigin.Current); // To be used to back up 4 bytes from the origin at the start
            stream.Read(nextAddressBytes, 0, 4);
            return BitConverter.ToUInt32(nextAddressBytes, 0);
        }

        public uint ReadUInt32()
        {
            uint data = BitConverter.ToUInt32(buffer, Offset);
            Offset += 4;
            return data;
        }

        public int ReadInt32()
        {
            int data = BitConverter.ToInt32(buffer, Offset);
            Offset += 4;
            return data;
        }

        public uint ReadUInt16()
        {
            uint data = BitConverter.ToUInt16(buffer, Offset);
            Offset += 2;
            return data;
        }

        public int ReadInt16()
        {
            int data = BitConverter.ToInt16(buffer, Offset);
            Offset += 2;
            return data;
        }

        public int ReadByte()
        {
            byte data = buffer[Offset];
            Offset += 1;
            return data;
        }

        public float ReadSingle()
        {
            float data = BitConverter.ToSingle(buffer, Offset);
            Offset += 4;
            return data;
        }

        public string ReadPString()
        {
            int stringlength = this.ReadByte();
            byte[] thestring = new byte[stringlength];
            Array.Copy(buffer, Offset, thestring, 0, stringlength);
            Offset += stringlength;
            return System.Text.Encoding.ASCII.GetString(thestring);
        }

        public string ReadString(int stringlength)
        {
            byte[] thestring = new byte[stringlength];
            Array.Copy(buffer, Offset, thestring, 0, stringlength);
            Offset += stringlength;
            return System.Text.Encoding.ASCII.GetString(thestring);
        }

        public void AlignBoundary()
        {
            long alignDelta = Offset % 4;
            if (alignDelta != 0)
            {
                Offset += ((int)(4 - alignDelta));
            }
        }
    }
}
