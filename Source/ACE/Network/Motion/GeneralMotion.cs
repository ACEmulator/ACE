using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.Network.Enum;
using ACE.Entity;

namespace ACE.Network.Motion
{
    public class GeneralMotion : MotionState
    {
        public bool HasTarget { get; set; } = false;
        public bool Jumping { get; set; } = false;

        public MovementData MovementData { get; }

        public MotionStance Stance { get; }

        public List<MotionItem> Commands { get; } = new List<MotionItem>();

        public GeneralMotion(MotionStance stance)
        {
            Stance = stance;
            MovementData = new MovementData();
        }

        public GeneralMotion(MotionStance stance, MotionItem motionItem)
        {
            Stance = stance;
            MovementData = new MovementData();
            Commands.Add(motionItem);
        }

        public override byte[] GetPayload(WorldObject animationTarget)
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write((byte)MovementTypes.General); // movement_type
            MotionFlags flags = MotionFlags.None;
            if (HasTarget)
                flags |= MotionFlags.HasTarget;
            if (Jumping)
                flags |= MotionFlags.Jumping;

            writer.Write((byte)flags); // these can be or and has sticky object | is long jump mode |
            writer.Write((ushort)Stance); // called command in the client
            MovementStateFlag generalFlags = MovementData.MovementStateFlag;

            generalFlags += (uint)Commands.Count << 7;
            writer.Write((uint)generalFlags);

            MovementData.Serialize(writer);

            foreach (var item in Commands)
            {
                writer.Write((ushort)item.Motion);
                writer.Write(animationTarget.Sequences.GetNextSequence(Sequence.SequenceType.Motion));
                writer.Write(item.Speed);
            }
            return stream.ToArray();
        }
    }
}
