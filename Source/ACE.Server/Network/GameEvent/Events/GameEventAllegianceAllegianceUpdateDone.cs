using ACE.Entity.Enum;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventAllegianceAllegianceUpdateDone : GameEventMessage
    {
        public GameEventAllegianceAllegianceUpdateDone(Session session, WeenieError errorType = WeenieError.None)
            : base(GameEventType.AllegianceAllegianceUpdateDone, GameMessageGroup.UIQueue, session, 8)
        {
            Writer.Write((uint)errorType);
        }
    }
}
