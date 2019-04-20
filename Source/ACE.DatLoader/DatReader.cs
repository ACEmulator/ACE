using System;
using System.IO;

namespace ACE.DatLoader
{
    public class DatReader
    {
        public byte[] Buffer { get; }

        public DatReader(string datFilePath, uint offset, uint size, uint blockSize)
        {
            using (var stream = new FileStream(datFilePath, FileMode.Open, FileAccess.Read))
            {
                Buffer = ReadDat(stream, offset, size, blockSize);

                stream.Close();
            }
        }

        public DatReader(FileStream stream, uint offset, uint size, uint blockSize)
        {
            Buffer = ReadDat(stream, offset, size, blockSize);
        }

        private static byte[] ReadDat(FileStream stream, uint offset, uint size, uint blockSize)
        {
            var buffer = new byte[size];

            stream.Seek(offset, SeekOrigin.Begin);

            // Dat "file" is broken up into sectors that are not neccessarily congruous. Next address is stored in first four bytes of each sector.
            uint nextAddress = GetNextAddress(stream, 0);

            int bufferOffset = 0;

            while (size > 0)
            {
                if (size < blockSize)
                {
                    stream.Read(buffer, bufferOffset, Convert.ToInt32(size));
                    size = 0; // We know we've read the only/last sector, so just set this to zero to proceed.
                }
                else
                {
                    stream.Read(buffer, bufferOffset, Convert.ToInt32(blockSize) - 4); // Read in our sector into the buffer[]
                    bufferOffset += Convert.ToInt32(blockSize) - 4; // Adjust this so we know where in our buffer[] the next sector gets appended to
                    stream.Seek(nextAddress, SeekOrigin.Begin); // Move the file pointer to the start of the next sector we read above.
                    nextAddress = GetNextAddress(stream, 0); // Get the start location of the next sector.
                    size -= (blockSize - 4); // Decrease this by the amount of data we just read into buffer[] so we know how much more to go
                }
            }

            return buffer;
        }

        private static uint GetNextAddress(FileStream stream, int relOffset)
        {
            // The location of the start of the next sector is the first four bytes of the current sector. This should be 0x00000000 if no next sector.
            byte[] nextAddressBytes = new byte[4];

            if (relOffset != 0)
                stream.Seek(relOffset, SeekOrigin.Current); // To be used to back up 4 bytes from the origin at the start

            stream.Read(nextAddressBytes, 0, 4);

            return BitConverter.ToUInt32(nextAddressBytes, 0);
        }
    }
}
