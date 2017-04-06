using ACE.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageAutonomousPosition : GameMessage
    {
        public GameMessageAutonomousPosition(WorldObject worldObject)
            : base(GameMessageOpcode.AutonomousPosition, GameMessageGroup.Group07)
        {
            if (worldObject is Player)
            {
                Player p = worldObject as Player;
                Writer.WriteGuid(p.Guid);
                p.Location.Serialize(Writer, true, false);
                Writer.Write((ushort)1); // instance_timestamp - always 1 in my pcaps
                Writer.Write((ushort)0); // server_control_timestamp - always 0 in my pcaps
                Writer.Write((ushort)0); // teleport_timestamp - always 0 in my pcaps
                Writer.Write((ushort)0); // force_position_timestamp - always 0 in my pcaps
                Writer.Write(1u); // contact - always "true" / 1 in my pcaps
            }
        }
    }
}
