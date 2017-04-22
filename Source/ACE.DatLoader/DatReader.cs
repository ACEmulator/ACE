using System;
using System.IO;

namespace ACE.DatLoader
{
    public class DatReader
    {
        public int Offset { get; set; }
        public byte[] Buffer { get; private set; }

        public DatReader(string datFilePath, uint offset, uint size, uint sectorSize)
        {
            uint nextAddress = 0;
            FileStream stream = new FileStream(datFilePath, FileMode.Open, FileAccess.Read);
            using (stream)
            {
                this.Buffer = new byte[size];
                stream.Seek(offset, SeekOrigin.Begin);
                // Dat "file" is broken up into sectors that are not neccessarily congruous. Next address is stored in first four bytes of each sector.
                if (size > sectorSize)
                    nextAddress = this.GetNextAddress(stream, -4);

                int bufferOffset = 0;
                while (size > 0)
                {
                    if (size < sectorSize)
                    {
                        stream.Read(Buffer, bufferOffset, Convert.ToInt32(size));
                        size = 0; // We know we've read the only/last sector, so just set this to zero to proceed.
                    }
                    else
                    {
                        stream.Read(Buffer, bufferOffset, Convert.ToInt32(sectorSize) - 4); // Read in our sector into the buffer[]
                        bufferOffset += Convert.ToInt32(sectorSize) - 4; // Adjust this so we know where in our buffer[] the next sector gets appended to
                        stream.Seek(nextAddress, SeekOrigin.Begin); // Move the file pointer to the start of the next sector we read above.
                        nextAddress = GetNextAddress(stream, 0); // Get the start location of the next sector.
                        size -= (sectorSize - 4); // Decrease this by the amount of data we just read into buffer[] so we know how much more to go
                    }
                }
                stream.Close();
            }
        }

        private uint GetNextAddress(FileStream stream, int relOffset)
        {
            // The location of the start of the next sector is the first four bytes of the current sector. This should be 0x00000000 if no next sector.
            byte[] nextAddressBytes = new byte[4];
            if (relOffset != 0)
                stream.Seek(relOffset, SeekOrigin.Current); // To be used to back up 4 bytes from the origin at the start
            stream.Read(nextAddressBytes, 0, 4);
            return BitConverter.ToUInt32(nextAddressBytes, 0);
        }

        public uint ReadUInt32()
        {
            uint data = BitConverter.ToUInt32(Buffer, Offset);
            Offset += 4;
            return data;
        }

        public int ReadInt32()
        {
            int data = BitConverter.ToInt32(Buffer, Offset);
            Offset += 4;
            return data;
        }

        public ushort ReadUInt16()
        {
            ushort data = BitConverter.ToUInt16(Buffer, Offset);
            Offset += 2;
            return data;
        }

        public short ReadInt16()
        {
            short data = BitConverter.ToInt16(Buffer, Offset);
            Offset += 2;
            return data;
        }

        public byte ReadByte()
        {
            byte data = Buffer[Offset];
            Offset += 1;
            return data;
        }

        public float ReadSingle()
        {
            float data = BitConverter.ToSingle(Buffer, Offset);
            Offset += 4;
            return data;
        }

        /// <summary>
        /// Returns a string as defined by the first byte's length
        /// </summary>
        public string ReadPString()
        {
            int stringlength = this.ReadByte();
            byte[] thestring = new byte[stringlength];
            Array.Copy(Buffer, Offset, thestring, 0, stringlength);
            Offset += stringlength;
            return System.Text.Encoding.ASCII.GetString(thestring);
        }

        /// <summary>
        /// Returns a string as defined by the first byte's length and removes the obfuscation
        /// </summary>
        public string ReadOString()
        {
            int stringlength = this.ReadByte();
            Offset += 1; // unknown, seems to be mostly 00
            byte[] thestring = new byte[stringlength];
            Array.Copy(Buffer, Offset, thestring, 0, stringlength);
            for (var i = 0; i < stringlength; i++)
            {
                // flip the bytes in the string to undo the obfuscation: i.e. 0xAB => 0xBA
                thestring[i] = (byte)((thestring[i] >> 4) | ((thestring[i] << 4) & 0x00FF));
            }
            
            Offset += stringlength;
            return System.Text.Encoding.ASCII.GetString(thestring);
        }

        public string ReadString(int stringlength)
        {
            byte[] thestring = new byte[stringlength];
            Array.Copy(Buffer, Offset, thestring, 0, stringlength);
            Offset += stringlength;
            return System.Text.Encoding.ASCII.GetString(thestring);
        }

        public void AlignBoundary()
        {
            // Aligns the DatReader to the next DWORD boundary.
            long alignDelta = Offset % 4;
            if (alignDelta != 0)
            {
                Offset += ((int)(4 - alignDelta));
            }
        }
    }
}
