using ACE.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
