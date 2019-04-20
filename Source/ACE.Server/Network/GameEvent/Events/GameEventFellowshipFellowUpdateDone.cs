using ACE.Entity.Enum;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventFellowshipFellowUpdateDone : GameEventMessage
    {
        // outdated message, not found in end of retail?
        public GameEventFellowshipFellowUpdateDone(Session session, WeenieError errorType = WeenieError.None)
            : base(GameEventType.FellowshipFellowUpdateDone, GameMessageGroup.UIQueue, session)
        {
            //Writer.Write((uint)errorType);  // should this be here?
        }
    }
}
