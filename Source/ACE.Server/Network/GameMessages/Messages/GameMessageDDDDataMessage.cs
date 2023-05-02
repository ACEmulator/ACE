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
            var datFileType = 0;
            var datFileID = 0;

            switch (datDatabaseType)
            {
                case DatDatabaseType.Portal:

                    datFileType = 0;
                    datFileID = 1;

                    break;

                case DatDatabaseType.Cell:

                    datFileType = 1;
                    datFileID = 2;

                    break;

                case DatDatabaseType.Language:

                    datFileType = 1;
                    datFileID = 3;

                    break;
            }

            var datFileContents = DDDManager.TryGetDatFileContentsForTransmission(datFileId, datDatabaseType, out var datFile, out var isCompressed);

            if (datFileContents == null)
            {
                // do something here?
                return;
            }

            var fileId = datFile.ObjectId;
            var fileType = datFile.GetFileType(datDatabaseType);

            Writer.Write((uint)datFileType); // DatFileType
            Writer.Write((uint)datFileID); // DatFileID

            Writer.Write((uint)fileType);
            Writer.Write(fileId);

            Writer.Write(datFile.Iteration);

            Writer.Write(isCompressed);

            Writer.Write(3); // version
            Writer.Write(datFileContents.Length + 4); // length + size of this message
            Writer.Write(datFileContents);
        }
    }
}
