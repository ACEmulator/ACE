using System.IO;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// These are client_portal.dat files starting with 0x0A. All are stored in .WAV data format, though the header slightly different than a .WAV file header.
    /// I'm not sure of an instance where the server would ever need this data, but it's fun nonetheless and included for completion sake.
    /// </summary>
    public class Wave : IUnpackable
    {
        public uint Id { get; private set; }
        public byte[] Header { get; private set; }
        public byte[] Data { get; private set; }

        public void Unpack(BinaryReader reader)
        {
            Id = reader.ReadUInt32();

            int headerSize  = reader.ReadInt32() - 2; // not sure why this is required, it just is.
            int dataSize    = reader.ReadInt32();

            Header  = reader.ReadBytes(headerSize);
            Data    = reader.ReadBytes(dataSize);
        }

        public static Wave ReadFromDat(uint fileId)
        {
            // Check the FileCache so we don't need to hit the FileSystem repeatedly
            if (DatManager.PortalDat.FileCache.ContainsKey(fileId))
                return (Wave)DatManager.PortalDat.FileCache[fileId];

            DatReader datReader = DatManager.PortalDat.GetReaderForFile(fileId);

            Wave wave = new Wave();

            using (var memoryStream = new MemoryStream(datReader.Buffer))
            using (var reader = new BinaryReader(memoryStream))
                wave.Unpack(reader);

            // Store this object in the FileCache
            DatManager.PortalDat.FileCache[fileId] = wave;

            return wave;
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
