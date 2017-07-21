using ACE.Entity;

namespace ACE.Network.GameEvent.Events
{
    public class GameEventIdentifyObjectResponse : GameEventMessage
    {
        public GameEventIdentifyObjectResponse(Session session, WorldObject obj)
            : base(GameEventType.IdentifyObjectResponse, GameMessageGroup.Group09, session)
        {
            obj.SerializeIdentifyObjectResponse(Writer);
        }
    }
}
