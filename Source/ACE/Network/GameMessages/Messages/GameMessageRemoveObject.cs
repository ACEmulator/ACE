using ACE.Entity;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageRemoveObject : GameMessage
    {
        public GameMessageRemoveObject(ObjectGuid guid) : base(GameMessageOpcode.ObjectDelete, GameMessageGroup.Group0A)
        {
            // TODO: Verify.  this was done without referencing the protocol spec
            Writer.WriteGuid(guid);
        }
    }
}
