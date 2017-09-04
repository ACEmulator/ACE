using System.IO;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// These are client_portal.dat files starting with 0x0A. All are stored in .WAV data format, though the header slightly different than a .WAV file header.
    /// I'm not sure of an instance where the server would ever need this data, but it's fun nonetheless and included for completion sake.
    /// </summary>
    public class Wave
    {
        public uint Id { get; set; }
        public byte[] Header { get; set; }
        public byte[] Data { get; set; }

        public static Wave ReadFromDat(uint fileId)
        {
            // Check the FileCache so we don't need to hit the FileSystem repeatedly
            if (DatManager.PortalDat.FileCache.ContainsKey(fileId))
            {
                return (Wave)DatManager.PortalDat.FileCache[fileId];
            }
            else
            {
                DatReader datReader = DatManager.PortalDat.GetReaderForFile(fileId);
                Wave obj = new Wave();

                obj.Id = datReader.ReadUInt32();

                int headerSize = datReader.ReadInt32() - 2; // not sure why this is required, it just is.
                int dataSize = datReader.ReadInt32();

                obj.Header = datReader.ReadBytes(headerSize);
                obj.Data = datReader.ReadBytes(dataSize);

                // Store this object in the FileCache
                DatManager.PortalDat.FileCache[fileId] = obj;

                return obj;
            }
        }

        /// <summary>
        /// Exports Wave to a playable .wav file
        /// </summary>
        public static void ExportWave(uint fileId, string directory)
        {
            Wave wav = ReadFromDat(fileId);

            string filename = Path.Combine(directory, fileId.ToString("X8") + ".wav");

            // Good summary of the header for a WAV file and what all this means
            // http://www.topherlee.com/software/pcm-tut-wavformat.html

            FileStream f = new FileStream(filename, FileMode.Create);
            BinaryWriter binaryWriter = new BinaryWriter(f);

            binaryWriter.Write(System.Text.Encoding.ASCII.GetBytes("RIFF"));

            uint filesize = (uint)(wav.Header.Length + wav.Data.Length + 20); // 20 is added for all the extra we're adding for the WAV header format
            binaryWriter.Write(filesize);

            binaryWriter.Write(System.Text.Encoding.ASCII.GetBytes("WAVE"));

            binaryWriter.Write(System.Text.Encoding.ASCII.GetBytes("fmt"));
            binaryWriter.Write((byte)0x20); // Null ending to the fmt

            binaryWriter.Write((int)0x10); // 16 ... length of all the above

            binaryWriter.Write(wav.Header);

            binaryWriter.Write(System.Text.Encoding.ASCII.GetBytes("data"));
            binaryWriter.Write((uint)wav.Data.Length);
            binaryWriter.Write(wav.Data);
    
            f.Close();
        }
    }
}
