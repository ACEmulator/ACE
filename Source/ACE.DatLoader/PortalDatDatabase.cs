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
            // string thisFolder = null;

            using (FileStream stream = new FileStream(this.FilePath, FileMode.Open))
            {
                // TODO - Make this function with the Dictionary and DatReader
                /*
                foreach (var file in this.AllFiles)
                {
                    if (file.GetFileType() != null)
                        thisFolder = Path.Combine(path, file.GetFileType().ToString());
                    else
                        thisFolder = Path.Combine(path, "UnknownType");

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
                }
                */
            }
        }
    }
}
