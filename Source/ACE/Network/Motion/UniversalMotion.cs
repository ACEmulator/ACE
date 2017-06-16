using System.Collections.Generic;
using System.IO;
using ACE.Network.Enum;
using ACE.Entity;
using System;
using ACE.Network.Sequence;

namespace ACE.Network.Motion
{
    public class UniversalMotion : MotionState
    {
        public uint Flag { get; set; } = 0x0041EE0F;

        public float MinimumDistance { get; set; } = 0.00f;

        public ObjectGuid TargetGuid { get; set; } = new ObjectGuid(0);

        public float FailDistance { get; set; } = float.MaxValue;

        public float Speed { get; set; } = 1.0f;

        public float Heading { get; set; } = 0.0f;

        public float DesiredHeading { get; set; } = 0.0f;

        public float DistanceFrom { get; set; } = 0.60f;

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

        public UniversalMotion(MotionStance stance, MovementData movementData)
        {
            Stance = stance;
            MovementData = movementData;
        }

        public UniversalMotion(MotionStance stance, Position moveToObjectPosition, ObjectGuid targetGuid)
        {
            Stance = stance;
            Position = moveToObjectPosition;
            TargetGuid = targetGuid;
            MovementTypes = MovementTypes.MoveToObject;
        }
        public UniversalMotion(MotionStance stance, MotionItem motionItem)
        {
            Stance = stance;
            MovementData = new MovementData();
            Commands.Add(motionItem);
        }

        public override byte[] GetPayload(ObjectGuid animationTargetGuid, SequenceManager sequence)
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
                            writer.Write(sequence.GetNextSequence(SequenceType.Motion));
                            writer.Write(item.Speed);
                        }
                        break;
                    }
                case MovementTypes.MoveToPosition:
                case MovementTypes.MoveToObject:
                    {
                        try
                        {
                            if (MovementTypes == MovementTypes.MoveToObject)
                                writer.Write(TargetGuid.Full);

                            Position.Serialize(writer, false);
                            writer.Write(Flag);
                            writer.Write(DistanceFrom);
                            writer.Write(MinimumDistance);
                            writer.Write(FailDistance);
                            writer.Write(Speed);
                            writer.Write(WalkRunThreshold);
                            writer.Write(Heading);
                            writer.Write(RunRate);
                            // TODO: This needs to be calculated and the flag needs to be deciphered Og II
                        }
                        catch (Exception)
                        {
                            // Do nothing
                            // TODO: This prevents a crash in Kryst and possibly other locations, Please investigate and fix if possible.
                        }
                        break;
                    }
                case MovementTypes.TurnToObject:
                    {
                        writer.Write(animationTargetGuid.Full);
                        writer.Write(Heading);
                        writer.Write(Flag);
                        writer.Write(Speed);
                        writer.Write(DesiredHeading); // always 0.0 in every pcap of this type.
                        break;
                    }
                case MovementTypes.TurnToHeading:
                    {
                        writer.Write(Flag);
                        writer.Write(Speed);
                        writer.Write(Heading);
                        break;
                    }
            }

            return stream.ToArray();
        }

        public UniversalMotion(byte[] currentMotionState)
        {
            MemoryStream stream = new MemoryStream(currentMotionState);
            BinaryReader reader = new BinaryReader(stream);
            MovementTypes = (MovementTypes)reader.ReadByte(); // movement_type

            MotionFlags flags = (MotionFlags)reader.ReadByte(); // these can be or and has sticky object | is long jump mode |

            if ((flags & MotionFlags.HasTarget) != 0)
                HasTarget = true;
            if ((flags & MotionFlags.Jumping) != 0)
                Jumping = true;

            Stance = (MotionStance)reader.ReadUInt16(); // called command in the client

            MovementStateFlag generalFlags = (MovementStateFlag)reader.ReadUInt32();

            MovementData = new MovementData();

            if ((generalFlags & MovementStateFlag.CurrentStyle) != 0)
            {
                try
                {
                    MovementData.CurrentStyle = (ushort)reader.ReadUInt32();
                }
                catch (Exception)
                {
                    MovementData.CurrentStyle = 0;
                }
            }

            if ((generalFlags & MovementStateFlag.ForwardCommand) != 0)
            {
                try
                {
                    MovementData.ForwardCommand = (ushort)reader.ReadUInt32();
                }
                catch (Exception)
                {
                    MovementData.ForwardCommand = 0;
                }
            }

            if ((generalFlags & MovementStateFlag.ForwardSpeed) != 0)
            {
                try
                {
                    MovementData.ForwardSpeed = (ushort)reader.ReadSingle();
                }
                catch (Exception)
                {
                    MovementData.ForwardSpeed = 0;
                }
            }

            if ((generalFlags & MovementStateFlag.SideStepCommand) != 0)
            {
                try
                {
                    MovementData.SideStepCommand = (ushort)reader.ReadUInt32();
                }
                catch (Exception)
                {
                    MovementData.SideStepCommand = 0;
                }
            }

            if ((generalFlags & MovementStateFlag.SideStepSpeed) != 0)
            {
                try
                {
                    MovementData.SideStepSpeed = (ushort)reader.ReadSingle();
                }
                catch (Exception)
                {
                    MovementData.SideStepSpeed = 0;
                }
            }

            if ((generalFlags & MovementStateFlag.TurnCommand) != 0)
            {
                try
                {
                    MovementData.TurnCommand = (ushort)reader.ReadUInt32();
                }
                catch (Exception)
                {
                    MovementData.TurnCommand = 0;
                }
            }

            if ((generalFlags & MovementStateFlag.TurnSpeed) != 0)
            {
                try
                {
                    MovementData.TurnSpeed = (ushort)reader.ReadSingle();
                }
                catch (Exception)
                {
                    MovementData.TurnSpeed = 0;
                }
            }
        }
    }
}
