﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader
{
    public class PortalDatDatabase : DatDatabase
    {
        public PortalDatDatabase(string filename) : base(filename, DatDatabaseType.Cell)
        {

        }

        public void ExtractCategorizedContents(string path)
        {
            using (FileStream stream = new FileStream(this.FilePath, FileMode.Open))
            {
                foreach (var file in this.AllFiles)
                {
                    if (file.GetFileType() != null)
                    {
                        string thisFolder = Path.Combine(path, file.GetFileType().ToString());

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
                }
            }
        }
    }
}
