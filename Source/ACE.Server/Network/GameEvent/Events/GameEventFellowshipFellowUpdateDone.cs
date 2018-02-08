using ACE.Server.Network.GameMessages;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventFellowshipFellowUpdateDone : GameMessage
    {
        public GameEventFellowshipFellowUpdateDone(Session session)
            : base(GameMessageOpcode.GameEvent, GameMessageGroup.Group09)
        {
            Writer.Write(session.Player.Guid.Full);
            Writer.Write(session.GameEventSequence++);
            Writer.Write((uint)GameEvent.GameEventType.FellowshipFellowUpdateDone);
        }
    }
}
