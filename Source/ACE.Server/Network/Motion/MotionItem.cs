using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using ACE.Entity.Enum;
using ACE.Server.Network.Sequence;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.Structure
{
    public class MotionItem
    {
        public WorldObject WorldObject;

        // technically this is just a Command,
        // which is MotionCommand & 0xFFFF (ushort)
        // to avoid redundancy, not creating a separate Command class here
        public MotionCommand MotionCommand;

        public ushort PackedSequence;       // write: (motionSequence & 0x7FFF) | (autonomous & 0x1 << 15)
                                            // sequence of the animation. Note the MSB appears to be used for autonomous flag,
                                            // so the max size of this member would be 1/2 a dword.
        public ushort ServerActionSequence; // read: packedSequence & 0x7FFF; Sequence of the animation.
        public bool IsAutonomous;           // read: packedSequence >> 15 == 1; True = client initiated, False = server initiated
        public float Speed;                 // the speed at which to perform the animation / movement.


        public MotionItem(WorldObject worldObject, MotionCommand motionCommand, float speed = 1.0f)
        {
            WorldObject = worldObject;

            MotionCommand = motionCommand;
            Speed = speed;
        }

        public MotionItem(WorldObject wo, BinaryReader reader)
        {
            WorldObject = wo;

            var rawCommand = reader.ReadUInt16();

            if (!PackedCommandExtensions.RawToInterpreted.TryGetValue(rawCommand, out MotionCommand))
            {
                Console.WriteLine($"MotionPack: couldn't find interpreted command for raw command {rawCommand}");
            }
            PackedSequence = reader.ReadUInt16();
            ServerActionSequence = (ushort)(PackedSequence & 0x7FFF);
            IsAutonomous = (PackedSequence >> 15) == 1;
            Speed = reader.ReadSingle();
        }

        public MotionItem(MotionItem motionItem)
        {
            // copy constructor
            WorldObject = motionItem.WorldObject;
            MotionCommand = motionItem.MotionCommand;
            PackedSequence = motionItem.PackedSequence;
            ServerActionSequence = motionItem.ServerActionSequence;
            IsAutonomous = motionItem.IsAutonomous;
            Speed = motionItem.Speed;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"MotionCommand: {MotionCommand}");
            sb.AppendLine($"IsAutonomous: {IsAutonomous}");
            sb.AppendLine($"Speed: {Speed}");

            return sb.ToString();
        }
    }

    public static class PackedCommandExtensions
    {
        public static Dictionary<ushort, MotionCommand> RawToInterpreted;

        static PackedCommandExtensions()
        {
            RawToInterpreted = new Dictionary<ushort, MotionCommand>();

            var interpretedCommands = System.Enum.GetValues(typeof(MotionCommand));
            foreach (var interpretedCommand in interpretedCommands)
            {
                RawToInterpreted.Add((ushort)(uint)interpretedCommand, (MotionCommand)interpretedCommand);
            }
        }

        public static void Write(this BinaryWriter writer, MotionItem mc)
        {
            var sequence = mc.WorldObject.Sequences;

            writer.Write((ushort)mc.MotionCommand);      // verified

            // should already be masked with 0x7FFF
            var nextSequence = sequence.GetNextSequence(SequenceType.Motion);

            if (mc.IsAutonomous)
                nextSequence[1] |= 0x80;    // if client-initiated motion, set upper bit

            writer.Write(nextSequence);

            writer.Write(mc.Speed);
        }
    }
}
