using ACE.Entity.Enum;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageDDDInterrogation: GameMessage
    {
        public GameMessageDDDInterrogation() 
            : base(GameMessageOpcode.DDD_Interrogation, GameMessageGroup.Group05)
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