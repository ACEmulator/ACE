using System.Collections.Generic;
using System.IO;

namespace ACE.DatLoader
{
    public class PortalDatDatabase : DatDatabase
    {
        public PortalDatDatabase(string filename) : base(filename)
        {
        }

        public void ExtractCategorizedContents(string path)
        {
            foreach (KeyValuePair<uint, DatFile> entry in AllFiles)
            {
                string thisFolder;

                if (entry.Value.GetFileType(DatDatabaseType.Portal) != null)
                    thisFolder = Path.Combine(path, entry.Value.GetFileType(DatDatabaseType.Portal).ToString());
                else
                    thisFolder = Path.Combine(path, "UnknownType");

                if (!Directory.Exists(thisFolder))
                    Directory.CreateDirectory(thisFolder);

                string hex = entry.Value.ObjectId.ToString("X8");
                string thisFile = Path.Combine(thisFolder, hex + ".bin");

                // Use the DatReader to get the file data
                DatReader dr = GetReaderForFile(entry.Value.ObjectId);
                   
                File.WriteAllBytes(thisFile, dr.Buffer);
            }
        }
    }
}
