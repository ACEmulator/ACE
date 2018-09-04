using ACE.Entity;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventRegisterTrade : GameEventMessage
    {
        public GameEventRegisterTrade(Session session, ObjectGuid initiator, ObjectGuid partner)
            : base(GameEventType.RegisterTrade, GameMessageGroup.UIQueue, session)
        {
            Writer.WriteGuid(initiator);
            Writer.WriteGuid(partner);
            Writer.Write(0L);
        }
    }
}
