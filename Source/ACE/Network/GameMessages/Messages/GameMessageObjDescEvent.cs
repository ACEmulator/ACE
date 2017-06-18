using ACE.Entity;

namespace ACE.Network.GameMessages.Messages
{
    /// <summary>
    /// via Skunkwors Protocol Docs (F625: Change Model)
    /// Sent whenever a character changes their clothes. It contains the entire description of what they're wearing (and possibly their facial features as well). 
    /// This message is only sent for changes, when the character is first created, the body of this message is included inside the creation message.
    /// </summary>
    public class GameMessageObjDescEvent : GameMessage
    {
        public GameMessageObjDescEvent(WorldObject worldObject)
            : base(GameMessageOpcode.ObjDescEvent, GameMessageGroup.Group0A)
        {
            worldObject.SerializeUpdateModelData(this.Writer);
        }
    }
}
