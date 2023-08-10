using System.Collections.Generic;

using ACE.DatLoader;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageDDDBeginDDD : GameMessage
    {
        public GameMessageDDDBeginDDD(uint totalMissingIterations, uint totalFileSize, Dictionary<DatDatabaseType, Dictionary<uint, List<uint>>> missingIterations)
            : base(GameMessageOpcode.DDD_BeginDDD, GameMessageGroup.DatabaseQueue)
        {
            Writer.Write(totalFileSize); // Total Size of Patches to Download
            Writer.Write(totalMissingIterations); // Number of MissingIterations

            // PCAPs show iterations list order was Portal, Language, Cell
            if (missingIterations.ContainsKey(DatDatabaseType.Portal))
                WriteIterations(DatDatabaseType.Portal, missingIterations[DatDatabaseType.Portal]);

            if (missingIterations.ContainsKey(DatDatabaseType.Language))
                WriteIterations(DatDatabaseType.Language, missingIterations[DatDatabaseType.Language]);

            if (missingIterations.ContainsKey(DatDatabaseType.Cell))
                WriteIterations(DatDatabaseType.Cell, missingIterations[DatDatabaseType.Cell]);
        }

        private void WriteIterations(DatDatabaseType datDatabaseType, Dictionary<uint, List<uint>> iterations)
        {
            foreach (var iteration in iterations)
            {
                switch (datDatabaseType)
                {
                    case DatDatabaseType.Portal:
                        Writer.Write(0);
                        Writer.Write(1);
                        break;

                    case DatDatabaseType.Cell:
                        Writer.Write(1);
                        Writer.Write(2);
                        break;

                    case DatDatabaseType.Language:
                        Writer.Write(1);
                        Writer.Write(3);
                        break;
                }

                Writer.Write((int)iteration.Key); // iteration
                if (datDatabaseType == DatDatabaseType.Cell)
                {
                    // PCAPs show Cell updates were purges only, while the other two were downloads

                    Writer.Write(0); // IDsToDownload Count
                    Writer.Write(iteration.Value.Count); // IDsToPurge Count
                    foreach (var file in iteration.Value)
                    {
                        Writer.Write(file); // IDsToPurge
                    }
                }
                else
                {
                    Writer.Write(iteration.Value.Count); // IDsToDownload Count
                    foreach (var file in iteration.Value)
                    {
                        Writer.Write(file); // IDsToDownload
                    }
                    Writer.Write(0); // IDsToPurge Count
                }
            }
        }
    }
}
