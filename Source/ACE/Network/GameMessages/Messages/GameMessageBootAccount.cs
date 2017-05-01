using ACE.Entity;
using ACE.Entity.Enum;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageBootAccount : GameMessage
    {
        public GameMessageBootAccount(Session session)
            : base(GameMessageOpcode.AccountBoot, GameMessageGroup.Group09)
        {
        }
    }
}
