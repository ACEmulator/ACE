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

        public GeneralMotion(byte[] currentMotionState)
        {
            MemoryStream stream = new MemoryStream(currentMotionState);
            BinaryReader reader = new BinaryReader(stream);
            MovementTypes movementType = (MovementTypes)reader.ReadByte(); // movement_type

            // MotionFlags flags = MotionFlags.None;
            // if (HasTarget)
            //    flags |= MotionFlags.HasTarget;
            // if (Jumping)
            //    flags |= MotionFlags.Jumping;

            MotionFlags flags = (MotionFlags)reader.ReadByte(); // these can be or and has sticky object | is long jump mode |

            if ((flags & MotionFlags.HasTarget) != 0)
                HasTarget = true;
            if ((flags & MotionFlags.Jumping) != 0)
                Jumping = true;

            Stance = (MotionStance)reader.ReadUInt16(); // called command in the client
            // MovementStateFlag generalFlags = MovementData.MovementStateFlag;

            // generalFlags += (uint)Commands.Count << 7;
            MovementStateFlag generalFlags = (MovementStateFlag)reader.ReadUInt32();

            // MovementData.Serialize(writer);

            MovementData = new MovementData();

            if ((generalFlags & MovementStateFlag.CurrentStyle) != 0)
                MovementData.CurrentStyle = (ushort)reader.ReadUInt32();

            if ((generalFlags & MovementStateFlag.ForwardCommand) != 0)
                MovementData.ForwardCommand = (ushort)reader.ReadUInt32();
                
            if ((generalFlags & MovementStateFlag.ForwardSpeed) != 0)
                MovementData.ForwardSpeed = (float)reader.ReadSingle();

            if ((generalFlags & MovementStateFlag.SideStepCommand) != 0)
                MovementData.SideStepCommand = (ushort)reader.ReadUInt32();

            if ((generalFlags & MovementStateFlag.SideStepSpeed) != 0)
                MovementData.SideStepSpeed = (float)reader.ReadSingle();

            if ((generalFlags & MovementStateFlag.TurnCommand) != 0)
                MovementData.TurnCommand = (ushort)reader.ReadUInt32();

            if ((generalFlags & MovementStateFlag.TurnSpeed) != 0)
                MovementData.TurnSpeed = (float)reader.ReadSingle();

            // foreach (var item in Commands)
            // {
            //    writer.Write((ushort)item.Motion);
            //    writer.Write(animationTarget.Sequences.GetNextSequence(Sequence.SequenceType.Motion));
            //    writer.Write(item.Speed);
            // }

            // if ((generalFlags & MovementStateFlag.ForwardCommand) != 0)
            // {
            //    MotionItem item = new MotionItem();
            //    item.Motion = (MotionCommand)reader.ReadUInt16();
            //    item.Speed = reader.ReadSingle();
            //    Commands.Add(item);
            // }
            // if ((generalFlags & MovementStateFlag.SideStepCommand) != 0)
            // {
            //    MotionItem item = new MotionItem();
            //    item.Motion = (MotionCommand)reader.ReadUInt16();
            //    item.Speed = reader.ReadSingle();
            //    Commands.Add(item);
            // }
            // if ((generalFlags & MovementStateFlag.TurnCommand) != 0)
            // {
            //    MotionItem item = new MotionItem();
            //    item.Motion = (MotionCommand)reader.ReadUInt16();
            //    item.Speed = reader.ReadSingle();
            //    Commands.Add(item);
            // }

            // return stream.ToArray();
        }
    }
}
