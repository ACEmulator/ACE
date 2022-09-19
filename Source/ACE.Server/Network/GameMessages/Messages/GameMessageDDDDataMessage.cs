using System;
using System.Linq;

using ACE.DatLoader;
using ACE.Server.Managers;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageDDDDataMessage : GameMessage
    {
        public GameMessageDDDDataMessage(uint datFileId, DatDatabaseType datDatabaseType)
            : base(GameMessageOpcode.DDD_DataMessage, GameMessageGroup.DatabaseQueue)
        {
            //var datFileFound = false;

            var datFileType = 0;
            var datFileID = 0;

            //var datFile = new DatFile();
            //var datFileContents = Array.Empty<byte>();

            switch (datDatabaseType)
            {
                case DatDatabaseType.Portal:

                    datFileType = 0;
                    datFileID = 1;

                    //datFileFound = DatManager.PortalDat.AllFiles.TryGetValue(datFileId, out datFile);

                    //datFileContents = DatManager.PortalDat.GetReaderForFile(datFileId).Buffer;

                    break;

                case DatDatabaseType.Cell:

                    datFileType = 1;
                    datFileID = 2;

                    //datFileFound = DatManager.CellDat.AllFiles.TryGetValue(datFileId, out datFile);

                    //datFileContents = DatManager.CellDat.GetReaderForFile(datFileId).Buffer;

                    break;

                case DatDatabaseType.Language:

                    datFileType = 1;
                    datFileID = 3;

                    //datFileFound = DatManager.LanguageDat.AllFiles.TryGetValue(datFileId, out datFile);

                    //datFileContents = DatManager.LanguageDat.GetReaderForFile(datFileId).Buffer;

                    break;
            }

            var datFileContents = DDDManager.TryGetDatFileContentsForTransmission(datFileId, datDatabaseType, out var datFile, out var isCompressed);

            //if (!datFileFound)
            //{

            //    return;
            //}

            if (datFileContents == null)
            {

                return;
            }

            var fileId = datFile.ObjectId;
            var fileType = datFile.GetFileType(datDatabaseType);

            Writer.Write((uint)datFileType); // DatFileType
            Writer.Write((uint)datFileID); // DatFileID

            Writer.Write((uint)fileType);
            Writer.Write(fileId);

            Writer.Write(datFile.Iteration);

            //var compressDatFile = DDDManager.DatFileSizes[datDatabaseType][datFileId].CompressedFileSize > 0;

            //if (compressDatFile)
            //{
            //    //var compressedDatFile = Ionic.Zlib.ZlibStream.CompressBuffer(datFileContents);
            //    var compressedDatFile = DDDManager.Compress(datFileContents);

            //    Writer.Write(true);

            //    Writer.Write(3); // version
            //    var a = BitConverter.GetBytes(datFile.FileSize);
            //    var b = a.Concat(compressedDatFile).ToArray();
            //    Writer.Write(b.Length + 4); // length + size of this message
            //    Writer.Write(b);
            //}
            //else
            //{
            //    Writer.Write(false);

            //    Writer.Write(3); // version
            //    Writer.Write(datFileContents.Length + 4); // length + size of this message
            //    Writer.Write(datFileContents);
            //}

            Writer.Write(isCompressed);

            Writer.Write(3); // version
            Writer.Write(datFileContents.Length + 4); // length + size of this message
            Writer.Write(datFileContents);
        }
    }
}
