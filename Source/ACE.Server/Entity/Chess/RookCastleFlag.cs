using System.Numerics;
using ACE.Entity.Enum;

namespace ACE.Server.Entity.Chess
{
    public class RookCastleFlag
    {
        public Vector2 Vector;
        public ChessMoveFlag Flag;

        public RookCastleFlag(Vector2 vector, ChessMoveFlag flag)
        {
            Vector = vector;
            Flag = flag;
        }
    }
}
