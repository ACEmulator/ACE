using System;
using System.IO;

namespace ACE.DatLoader
{
    public class CellFile
    {
        /// <summary>
        /// private to force loading from the static method
        /// </summary>
        private CellFile()
        {

        }

        public uint BitFlags { get; private set; }

        public uint ObjectId { get; private set; }

        public uint FileOffset { get; private set; }

        public uint FileSize { get; private set; }

        public uint Date { get; private set; }

        public uint Iteration { get; private set; }

        /// <summary>
        /// populates a new CellFile from the specified buffer.
        /// </summary>
        public static CellFile FromBuffer(byte[] buffer, int offset)
        {
            CellFile cf = new CellFile()
            {
                BitFlags = BitConverter.ToUInt32(buffer, offset),
                ObjectId = BitConverter.ToUInt32(buffer, offset + 4),
                FileOffset = BitConverter.ToUInt32(buffer, offset + 8),
                FileSize = BitConverter.ToUInt32(buffer, offset + 12),
                Date = BitConverter.ToUInt32(buffer, offset + 16),
                Iteration = BitConverter.ToUInt32(buffer, offset + 20)
            };

            return cf;
        }

        /// <summary>
        /// uses the open stream to read content as a binary blob.
        /// </summary>
        public byte[] GetContent(FileStream stream)
        {
            stream.Seek(this.FileOffset, SeekOrigin.Begin);
            byte[] content = new byte[FileSize];
            stream.Read(content, 0, Convert.ToInt32(FileSize));
            return content;
        }
    }
}
