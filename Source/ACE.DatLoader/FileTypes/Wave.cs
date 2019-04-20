using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// These are client_portal.dat files starting with 0x0A. All are stored in .WAV data format, though the header slightly different than a .WAV file header.
    /// I'm not sure of an instance where the server would ever need this data, but it's fun nonetheless and included for completion sake.
    /// </summary>
    [DatFileType(DatFileType.Wave)]
    public class Wave : FileType
    {
        public byte[] Header { get; private set; }
        public byte[] Data { get; private set; }

        public override void Unpack(BinaryReader reader)
        {
            int objectId   = reader.ReadInt32();
            int headerSize = reader.ReadInt32();
            int dataSize   = reader.ReadInt32();

            Header = reader.ReadBytes(headerSize);
            Data   = reader.ReadBytes(dataSize);
        }

        /// <summary>
        /// Exports Wave to a playable .wav file
        /// </summary>
        public void ExportWave(string directory)
        {
            string ext = ".wav";
            if (Header[0] == 0x55) ext = ".mp3";

            string filename = Path.Combine(directory, Id.ToString("X8") + ext);

            // Good summary of the header for a WAV file and what all this means
            // http://www.topherlee.com/software/pcm-tut-wavformat.html

            FileStream f = new FileStream(filename, FileMode.Create);
            ReadData(f);
            f.Close();
        }

        public void ReadData(Stream stream)
        {
            var binaryWriter = new BinaryWriter(stream);

            binaryWriter.Write(System.Text.Encoding.ASCII.GetBytes("RIFF"));

            uint filesize = (uint)(Data.Length + 36); // 36 is added for all the extra we're adding for the WAV header format
            binaryWriter.Write(filesize);

            binaryWriter.Write(System.Text.Encoding.ASCII.GetBytes("WAVE"));

            binaryWriter.Write(System.Text.Encoding.ASCII.GetBytes("fmt"));
            binaryWriter.Write((byte)0x20); // Null ending to the fmt

            binaryWriter.Write((int)0x10); // 16 ... length of all the above

            // AC audio headers start at Format Type,
            // and are usually 18 bytes, with some exceptions
            // notably objectID A000393 which is 30 bytes

            // WAV headers are always 16 bytes from Format Type to end of header,
            // so this extra data is truncated here.
            binaryWriter.Write(Header.Take(16).ToArray());

            binaryWriter.Write(System.Text.Encoding.ASCII.GetBytes("data"));
            binaryWriter.Write((uint)Data.Length);
            binaryWriter.Write(Data);
        }
    }
}
