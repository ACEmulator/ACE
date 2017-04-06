using ACE.Entity;
using ACE.Network.Sequence;
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
                p.Position.Serialize(Writer, true, false);
                Writer.Write(worldObject.Sequences.GetCurrentSequence(SequenceType.ObjectInstance)); // instance_timestamp - always 1 in my pcaps
                Writer.Write(worldObject.Sequences.GetCurrentSequence(SequenceType.ObjectServerControl)); // server_control_timestamp - always 0 in my pcaps
                Writer.Write(worldObject.Sequences.GetCurrentSequence(SequenceType.ObjectTeleport)); // teleport_timestamp - always 0 in my pcaps
                Writer.Write(worldObject.Sequences.GetCurrentSequence(SequenceType.ObjectForcePosition)); // force_position_timestamp - always 0 in my pcaps
                Writer.Write(1u); // contact - always "true" / 1 in my pcaps
            }
        }
    }
}
