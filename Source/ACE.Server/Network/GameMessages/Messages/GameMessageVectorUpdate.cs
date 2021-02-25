using System;
using System.Numerics;
using ACE.Server.Network.Sequence;
using ACE.Server.Network.Structure;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageVectorUpdate : GameMessage
    {
        public GameMessageVectorUpdate(WorldObject worldObject)
            : base(GameMessageOpcode.VectorUpdate, GameMessageGroup.SmartboxQueue)
        {
            // object guid
            // velocity - Vector3
            // omega - Vector3
            // instance sequence - ushort
            // vector sequence - ushort

            var velocity = worldObject.PhysicsObj?.Velocity ?? Vector3.Zero;
            var omega = worldObject.PhysicsObj?.Omega ?? Vector3.Zero;

            Writer.WriteGuid(worldObject.Guid);
            Writer.Write(velocity);
            Writer.Write(omega);

            Writer.Write(worldObject.Sequences.GetCurrentSequence(SequenceType.ObjectInstance));
            Writer.Write(worldObject.Sequences.GetNextSequence(SequenceType.ObjectVector));
        }
    }
}
