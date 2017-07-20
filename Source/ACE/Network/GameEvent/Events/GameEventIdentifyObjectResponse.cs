using ACE.Entity;
using ACE.Network.Enum;
using System.Linq;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Network.GameMessages.Messages;
using System.IO;
using System.Collections.Generic;
using System;

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
