using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            foreach (KeyValuePair<uint, DatFile> entry in this.AllFiles)
            {
                if (entry.Value.GetFileType() != null)
                    thisFolder = Path.Combine(path, entry.Value.GetFileType().ToString());
                else
                    thisFolder = Path.Combine(path, "UnknownType");

                if (!Directory.Exists(thisFolder))
                {
                    Directory.CreateDirectory(thisFolder);
                }

                string hex = entry.Value.ObjectId.ToString("X8");
                string thisFile = Path.Combine(thisFolder, hex + ".bin");

                // Use the DatReader to get the file data
                DatReader dr = this.GetReaderForFile(entry.Value.ObjectId);
                   
                File.WriteAllBytes(thisFile, dr.Buffer);
            }
        }
    }
}
