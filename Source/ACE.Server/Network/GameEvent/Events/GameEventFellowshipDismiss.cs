using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventFellowshipDismiss : GameEventMessage
    {
        public GameEventFellowshipDismiss(Session session, Player dismissedPlayer)
            : base(GameEventType.FellowshipDismiss, GameMessageGroup.UIQueue, session)
        {
            // can be both S2C and C2S?
            Writer.Write(dismissedPlayer.Guid.Full);
        }
    }
}
