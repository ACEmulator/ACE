using ACE.Server.Entity;
using ACE.Server.WorldObjects;
using ACE.Server.Network.Sequence;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageAutonomousPosition : GameMessage
    {
        public GameMessageAutonomousPosition(WorldObject worldObject)
            : base(GameMessageOpcode.AutonomousPosition, GameMessageGroup.SecureWeenieQueue)
        {
            if (worldObject is Player p)
            {
                Writer.WriteGuid(p.Guid);
                p.Location.Serialize(Writer, true, false);
                Writer.Write(worldObject.Sequences.GetCurrentSequence(SequenceType.ObjectInstance)); // instance_timestamp - always 1 in my pcaps
                Writer.Write(worldObject.Sequences.GetCurrentSequence(SequenceType.ObjectServerControl)); // server_control_timestamp - always 0 in my pcaps
                Writer.Write(worldObject.Sequences.GetCurrentSequence(SequenceType.ObjectTeleport)); // teleport_timestamp - always 0 in my pcaps
                Writer.Write(worldObject.Sequences.GetCurrentSequence(SequenceType.ObjectForcePosition)); // force_position_timestamp - always 0 in my pcaps
                Writer.Write(1u); // contact - always "true" / 1 in my pcaps
            }
        }
    }
}
