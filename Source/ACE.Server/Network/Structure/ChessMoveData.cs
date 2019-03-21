using System.IO;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity.Chess;

namespace ACE.Server.Network.Structure
{
    /// <summary>
    /// Set of information related to a chess game move
    /// </summary>
    public class ChessMoveData
    {
        public ChessMoveType Type;      // type of move
        public ObjectGuid PlayerGuid;   // player making this move
        public ChessColor Color;         // team making this move

        // Select one section based on the value of type:
        // case 0x4:
        public ObjectGuid PieceGuid;    // guid of piece being moved
        public ChessPieceCoord From;    // position moved from

        // case 0x5:
        //public ObjectGuid PieceGuid;  // guid of piece being moved
        //public ChessPieceCoord From;  // position moved from
        public ChessPieceCoord To;      // position moved to

        // case 0x6:
        //public ObjectGuid PieceGuid;  // guid of piece being moved

        public ChessMoveData() { }

        public ChessMoveData(ObjectGuid playerGuid, ObjectGuid pieceGuid, GameMoveData data)
        {
            Type = data.MoveType;
            PlayerGuid = playerGuid;
            Color = data.Color;
            PieceGuid = pieceGuid;
            From = data.From;
            To = data.To;
        }
    }

    public static class ChestMoveDataExtensions
    {
        public static void Write(this BinaryWriter writer, ChessMoveData data)
        {
            writer.Write((int)data.Type);
            writer.Write(data.PlayerGuid.Full);
            //writer.Write((int)data.Color);

            // only ChessMoveType.FromTo used?
            switch (data.Type)
            {
                case ChessMoveType.Grid:
                    // xgrid / ygrid?
                    writer.Write(data.To);
                    break;

                case ChessMoveType.FromTo:
                    //writer.Write(data.PieceGuid.Full);
                    writer.Write(data.From);
                    writer.Write(data.To);
                    break;

                case ChessMoveType.SelectedPiece:
                    writer.Write(data.PieceGuid.Full);
                    break;
            }
        }
    }
}
