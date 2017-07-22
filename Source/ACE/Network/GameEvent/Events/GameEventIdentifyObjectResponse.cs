using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using System;
using System.Linq;

namespace ACE.Network.GameEvent.Events
{
    public class GameEventIdentifyObjectResponse : GameEventMessage
    {
        public GameEventIdentifyObjectResponse(Session session, WorldObject obj, bool success)
            : base(GameEventType.IdentifyObjectResponse, GameMessageGroup.Group09, session)
        {
            IdentifyResponseFlags flags = IdentifyResponseFlags.None;

            Writer.Write(obj.Guid.Full);

            obj.SerializeIdentifyObjectResponse(Writer, success, flags);
        }
    }
}
