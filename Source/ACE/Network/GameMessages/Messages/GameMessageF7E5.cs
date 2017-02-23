
using ACE.Entity.Enum;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageF7E5: GameMessage
    {
        public GameMessageF7E5() : base(GameMessageOpcode.UnknownF7E5, 0x5)
        {
            Writer.Write(1ul);
            Writer.Write(1ul);
            Writer.Write(1ul);
            Writer.Write(2ul);
            Writer.Write(0ul);
            Writer.Write(1ul);
        }
    }
}