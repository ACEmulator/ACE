using ACE.Entity;
using ACE.Network.GameEvent;
using ACE.Network.Sequence;

namespace ACE.Network.GameMessages.Messages
{
    /// <summary>
    /// stack - this is far more generic, used for many things but for now, i am keeping it basic.. money
    /// </summary>
    public class GameMessageUpdateQualityEvent : GameMessage
    {
        public GameMessageUpdateQualityEvent(Session session)
            : base(GameMessageOpcode.PUpdateQualityEvent, GameMessageGroup.Group09)
        {
            Writer.Write((byte)1); // wts .. maybe a sequance based off of the object .. ie vendor or player ?
            Writer.Write(0x14); // coin value -- see STypeInt in aclog for more info on this. many options!
            Writer.Write(5000);
        }
    }
}
