using System.IO;

namespace ACE.DatLoader
{
    public class PortalDatDatabase : DatDatabase
    {
        public PortalDatDatabase(string filename) : base(filename, DatDatabaseType.Portal)
        {
        }

        public void ExtractCategorizedContents(string path)
        {
            string thisFolder = null;

            foreach (var entry in AllFiles)
            {
                if (entry.Value.GetFileType() != null)
                    thisFolder = Path.Combine(path, entry.Value.GetFileType().ToString());
                else
                    thisFolder = Path.Combine(path, "UnknownType");

                if (!Directory.Exists(thisFolder))
                {
                    Directory.CreateDirectory(thisFolder);
                }

                var hex = entry.Value.ObjectId.ToString("X8");
                var thisFile = Path.Combine(thisFolder, hex + ".bin");

                // Use the DatReader to get the file data
                var dr = GetReaderForFile(entry.Value.ObjectId);

                File.WriteAllBytes(thisFile, dr.Buffer);
            }
        }
    }
}
