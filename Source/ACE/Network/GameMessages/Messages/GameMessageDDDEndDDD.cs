using ACE.Entity.Enum;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageDDDEndDDD : GameMessage
    {
        public GameMessageDDDEndDDD()
            : base(GameMessageOpcode.DDD_EndDDD, GameMessageGroup.DatabaseQueue)
        {
        }
    }
}