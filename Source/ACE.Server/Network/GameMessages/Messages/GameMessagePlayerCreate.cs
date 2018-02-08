using ACE.Entity;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessagePlayerCreate : GameMessage
    {
        public GameMessagePlayerCreate(ObjectGuid guid) : base(GameMessageOpcode.PlayerCreate, GameMessageGroup.Group0A)
        {
            Writer.WriteGuid(guid);
        }
    }
}