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

            FileStream f = new FileStream("a.wav", FileMode.Create);
            BinaryWriter wr = new BinaryWriter(f);

            // TODO - Handle file save logic here

        }

    }
}
