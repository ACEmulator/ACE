using System.Collections.Generic;
using System.IO;
using ACE.Network.Enum;
using ACE.Entity;

namespace ACE.Network.Motion
{
    public class UniversalMotion : MotionState
    {
        public uint Flag { get; set; } = 0x0041EE0F;

        public float MinimumDistance { get; set; } = 0.00f;

        public float FailDistance { get; set; } = float.MaxValue;

        public float Speed { get; set; } = 1.0f;

        public float WalkRunThreshold { get; set; } = 15.0f;
        /// <summary>
        /// I believe this to be calculated based on a factor of your quickness, run, enchanments, less burden factor Og II
        /// </summary>
        public float RunRate { get; set; } = 1.0f;

        public MovementTypes MovementTypes { get; set; } = MovementTypes.General;

        public bool HasTarget { get; set; } = false;

        public bool Jumping { get; set; } = false;

        /// <summary>
        /// Called Command in the client
        /// </summary>
        public MotionStance Stance { get; }

        public MovementData MovementData { get; }

        public List<MotionItem> Commands { get; } = new List<MotionItem>();

        public Position Position { get; }

        public UniversalMotion(MotionStance stance)
        {
            Stance = stance;
            MovementData = new MovementData();
        }

        public UniversalMotion(MotionStance stance, WorldObject moveToObject)
        {
            Stance = stance;
            Position = moveToObject.PhysicsData.Position;
            MovementTypes = MovementTypes.MoveToObject;
        }
        public UniversalMotion(MotionStance stance, MotionItem motionItem)
        {
            Stance = stance;
            MovementData = new MovementData();
            Commands.Add(motionItem);
        }

        public override byte[] GetPayload(WorldObject animationTarget, float distanceFromObject = 0.6f, float heading = 0.00f)
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write((byte)MovementTypes);
            MotionFlags flags = MotionFlags.None;
            if (HasTarget)
                flags |= MotionFlags.HasTarget;
            if (Jumping)
                flags |= MotionFlags.Jumping;

            writer.Write((byte)flags); // these can be or and has sticky object | is long jump mode |
            writer.Write((ushort)Stance); // called command in the client
            switch (MovementTypes)
            {
                case MovementTypes.General:
                    {
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
                        break;
                    }
                case MovementTypes.MoveToObject:
                case MovementTypes.MoveToPosition:
                    {
                        if (MovementTypes == MovementTypes.MoveToObject)
                            writer.Write(animationTarget.Guid.Full);
                        Position.Serialize(writer, false);
                        writer.Write(Flag);
                        writer.Write(distanceFromObject);
                        writer.Write(MinimumDistance);
                        writer.Write(FailDistance);
                        writer.Write(Speed);
                        writer.Write(WalkRunThreshold);
                        writer.Write(heading);
                        writer.Write(RunRate);
                        // TODO: This needs to be calculated and the flag needs to be deciphered Og II
                        break;
                    }
                case MovementTypes.TurnToObject:
                    {
                        writer.Write(animationTarget.Guid.Full);
                        writer.Write(heading);
                        writer.Write(Flag);
                        writer.Write(Speed);
                        writer.Write(heading);
                        break;
                    }
                case MovementTypes.TurnToHeading:
                    {
                        writer.Write(Flag);
                        writer.Write(Speed);
                        writer.Write(heading);
                        break;
                    }
            }

            return stream.ToArray();
        }
        public UniversalMotion(byte[] currentMotionState)
        {
            MemoryStream stream = new MemoryStream(currentMotionState);
            BinaryReader reader = new BinaryReader(stream);
            MovementTypes movementType = (MovementTypes)reader.ReadByte(); // movement_type

            MotionFlags flags = (MotionFlags)reader.ReadByte(); // these can be or and has sticky object | is long jump mode |

            if ((flags & MotionFlags.HasTarget) != 0)
                HasTarget = true;
            if ((flags & MotionFlags.Jumping) != 0)
                Jumping = true;

            Stance = (MotionStance)reader.ReadUInt16(); // called command in the client

            MovementStateFlag generalFlags = (MovementStateFlag)reader.ReadUInt32();
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
        }
    }
}
