using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader
{
    public class CellDatDatabase : DatDatabase
    {
        public CellDatDatabase(string filename) : base(filename, DatDatabaseType.Cell)
        {
        }

        public void ExtractLandblockContents(string path)
        {
            foreach (KeyValuePair<uint, DatFile> entry in this.AllFiles)
            {
                string thisFolder = Path.Combine(path, (entry.Value.ObjectId >> 16).ToString("X4"));

                if (!Directory.Exists(thisFolder))
                {
                    Directory.CreateDirectory(thisFolder);
                }

                // Use the DatReader to get the file data - file blocks can extend over block size.
                DatReader dr = this.GetReaderForFile(entry.Value.ObjectId);

                string hex = entry.Value.ObjectId.ToString("X8");
                string thisFile = Path.Combine(thisFolder, hex + ".bin");
                File.WriteAllBytes(thisFile, dr.Buffer);
            }
        }
    }
}
