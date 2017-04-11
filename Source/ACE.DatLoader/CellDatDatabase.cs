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
            using (FileStream stream = new FileStream(this.FilePath, FileMode.Open))
            {
                foreach (var file in this.AllFiles)
                {
                    // TODO - Make this function with the Dictionary and DatReader
                    /*
                    string thisFolder = Path.Combine(path, (file.ObjectId >> 16).ToString("X4"));

                    if (!Directory.Exists(thisFolder))
                    {
                        Directory.CreateDirectory(thisFolder);
                    }

                    byte[] buffer = new byte[file.FileSize];
                    stream.Seek(file.FileOffset, SeekOrigin.Begin);
                    stream.Read(buffer, 0, buffer.Length);

                    string hex = file.ObjectId.ToString("X8");
                    string thisFile = Path.Combine(thisFolder, hex + ".bin");
                    File.WriteAllBytes(thisFile, buffer);
                    */
                }
            }
        }
    }
}
