﻿using ACE.Entity;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageUpdateObject : GameMessage
    {
        public GameMessageUpdateObject(WorldObject worldObject)
            : base(GameMessageOpcode.UpdateObject, GameMessageGroup.Group0A)
        {
            worldObject.SerializeUpdateObject(this.Writer);
        }
    }
}
