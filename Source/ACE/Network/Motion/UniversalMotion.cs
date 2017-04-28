using System.Collections.Generic;
using System.IO;
using ACE.Network.Enum;
using ACE.Entity;
using System;

namespace ACE.Network.Motion
{
    public class UniversalMotion : MotionState
    {
        public uint Flag { get; set; } = 0x0041EE0F;

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

        public override byte[] GetPayload(WorldObject animationTarget, float distanceFromObject = 0.6f)
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
                    {
                        // 4320783 = EE0F
                        // 4320847 = EE4F
                        // 4321264 = EFF0
                        // 4321136 = EF70
                        // 4319823 = EA4F
                        // EE9F -? fail distance 100
                        writer.Write(animationTarget.Guid.Full);
                        Position.Serialize(writer, false);
                        // TODO: Og Fix to real numbers
                        writer.Write(Flag);
                        writer.Write(distanceFromObject);
                        writer.Write((float)0);
                        writer.Write(float.MaxValue);
                        writer.Write((float)1);
                        writer.Write((float)15);
                        writer.Write((float)0);
                        writer.Write(1.0f);
                        break;
                    }
                case MovementTypes.MoveToPosition:
                    {
                        try
                        {
                            Position.Serialize(writer, false);
                            // TODO: Og Fix to real numbers
                            writer.Write(Flag);
                            writer.Write(distanceFromObject);
                            writer.Write((float)0);
                            writer.Write(float.MaxValue);
                            writer.Write((float)1);
                            writer.Write((float)15);
                            writer.Write((float)0);
                            writer.Write(1.0f);
                        }
                        catch (Exception)
                        {
                            // Do nothing

                            // TODO: This prevents a crash in Kryst and possibly other locations, Please investigate and fix if possible.
                        }
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
