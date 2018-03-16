using ACE.Server.Network.GameMessages;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventAllegianceAllegianceUpdateDone : GameEventMessage
    {
        public GameEventAllegianceAllegianceUpdateDone(Session session)
            : base(GameEventType.AllegianceAllegianceUpdateDone, GameMessageGroup.UIQueue, session)
        {
            Writer.Write(0x00);
        }
    }
}
