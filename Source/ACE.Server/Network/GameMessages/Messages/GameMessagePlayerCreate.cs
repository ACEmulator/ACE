using ACE.Entity;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessagePlayerCreate : GameMessage
    {
        public GameMessagePlayerCreate(ObjectGuid guid) : base(GameMessageOpcode.PlayerCreate, GameMessageGroup.SmartboxQueue, 8)
        {
            Writer.WriteGuid(guid);
        }
    }
}
