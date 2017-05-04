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

        // TODO: Figure out of these require the DatReader or not
        public void ExtractLandblockContents(string path)
        {
            using (FileStream stream = new FileStream(this.FilePath, FileMode.Open))
            {
                foreach (KeyValuePair<uint, DatFile> entry in this.AllFiles)
                {
                    string thisFolder = Path.Combine(path, (entry.Value.ObjectId >> 16).ToString("X4"));

                    if (!Directory.Exists(thisFolder))
                    {
                        Directory.CreateDirectory(thisFolder);
                    }

                    byte[] buffer = new byte[entry.Value.FileSize];
                    stream.Seek(entry.Value.FileOffset, SeekOrigin.Begin);
                    stream.Read(buffer, 0, buffer.Length);

                    string hex = entry.Value.ObjectId.ToString("X8");
                    string thisFile = Path.Combine(thisFolder, hex + ".bin");
                    File.WriteAllBytes(thisFile, buffer);
                }
            }
        }
    }
}
