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
            var appraiseInfo = new AppraiseInfo(obj, session.Player, success);

            Writer.Write(obj.Guid.Full);
            Writer.Write(appraiseInfo);
        }

        // Empty Appraisal response, for when you only have a guid and nothing else.
        public GameEventIdentifyObjectResponse(Session session, uint objectGuid)
            : base(GameEventType.IdentifyObjectResponse, GameMessageGroup.UIQueue, session)
        {
            var appraiseInfo = new AppraiseInfo();

            Writer.Write(objectGuid);
            Writer.Write(appraiseInfo);
        }
    }
}
