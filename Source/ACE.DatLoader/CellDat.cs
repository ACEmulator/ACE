using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader
{
    public class CellDat
    {
        public CellDirectory RootDirectory { get; private set; }

        public List<CellFile> AllFiles { get; private set; }

        public CellDat(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(filePath);
            }

            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                stream.Seek(0x160u, SeekOrigin.Begin);
                byte[] firstDirBuffer = new byte[4];
                stream.Read(firstDirBuffer, 0, sizeof(uint));
                uint firstDirectoryOffset = BitConverter.ToUInt32(firstDirBuffer, 0);

                RootDirectory = new CellDirectory(firstDirectoryOffset, stream);                
            }

            AllFiles = new List<CellFile>();
            RootDirectory.AddFilesToList(AllFiles);
        }
    }
}
