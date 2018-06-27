using ACE.Entity.Enum;
using ACE.Server.Network.Structure;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventIdentifyObjectResponse : GameEventMessage
    {
        public GameEventIdentifyObjectResponse(Session session, WorldObject obj, bool success)
            : base(GameEventType.IdentifyObjectResponse, GameMessageGroup.UIQueue, session)
        {
            var appraiseInfo = new AppraiseInfo(obj, success);

            Writer.Write(obj.Guid.Full);
            Writer.Write(appraiseInfo);
        }
    }
}
