using ACE.Entity.Enum;
using ACE.Server.Network.GameMessages;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventAllegianceAllegianceUpdateDone : GameEventMessage
    {
        public GameEventAllegianceAllegianceUpdateDone(Session session, WERROR errorType = WERROR.WERROR_NONE)
            : base(GameEventType.AllegianceAllegianceUpdateDone, GameMessageGroup.UIQueue, session)
        {
            Writer.Write((uint)errorType);
        }
    }
}
