using ACE.Entity.Enum;
using ACE.Server.Network.GameMessages;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventFellowshipFellowUpdateDone : GameMessage
    {
        public GameEventFellowshipFellowUpdateDone(Session session, WeenieError errorType = WeenieError.None)
            : base(GameMessageOpcode.GameEvent, GameMessageGroup.UIQueue)
        {
            //Writer.Write(session.Player.Guid.Full);
            //Writer.Write(session.GameEventSequence++);
            //Writer.Write((uint)GameEventType.FellowshipFellowUpdateDone);
            Writer.Write((uint)errorType);
        }
    }
}
