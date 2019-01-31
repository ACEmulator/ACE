using ACE.Server.Network.Structure;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventAllegianceInfoResponse : GameEventMessage
    {
        public GameEventAllegianceInfoResponse(Session session, uint playerGuid, AllegianceProfile profile)
            : base(GameEventType.AllegianceInfoResponse, GameMessageGroup.UIQueue, session)
        {
            Writer.Write(playerGuid);
            Writer.Write(profile);
        }
    }
}
