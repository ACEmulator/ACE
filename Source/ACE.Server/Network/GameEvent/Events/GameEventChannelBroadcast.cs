using ACE.Entity.Enum;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventChannelBroadcast : GameEventMessage
    {
        public GameEventChannelBroadcast(Session session, Channel chatChannel, string senderName, string messageText) : base(GameEventType.ChannelBroadcast, GameMessageGroup.UIQueue, session)
        {
            Writer.Write((uint)chatChannel);
            Writer.WriteString16L(senderName); // This should be "" when sending back to initiator which makes client go "You Say..." instead of "PlayerName says..."
            Writer.WriteString16L(messageText);
        }
    }
}
