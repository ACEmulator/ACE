using ACE.Entity.Enum;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageDDDInterrogation : GameMessage
    {
        public GameMessageDDDInterrogation()
            : base(GameMessageOpcode.DDD_Interrogation, GameMessageGroup.DatabaseQueue)
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