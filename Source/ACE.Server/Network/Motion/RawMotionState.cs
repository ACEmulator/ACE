using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Network.Enum;

namespace ACE.Server.Network.Structure
{
    /// <summary>
    /// The raw movement commands sent by client
    /// </summary>
    public class RawMotionState
    {
        public static RawMotionState None = new RawMotionState();

        public MoveToState MoveToState;

        public uint PackedFlags;

        public RawMotionFlags Flags;        // stored as the first 11 bits of PackedFlags
        public ushort CommandListLength;    // starts at bit 12 of PackedFlags

        // choose valid sections by masking against Flags
        public HoldKey CurrentHoldKey;          // 0x1 - walk/run
        public MotionStance CurrentStyle;       // 0x2 - current stance
        public MotionCommand ForwardCommand;    // 0x4 - forward movement or motion command (default invalid or ready?)
        public HoldKey ForwardHoldKey;          // 0x8 - whether forward/back key is being held
        public float ForwardSpeed;              // 0x10 - forward/back movement speed
        public MotionCommand SidestepCommand;   // 0x20 - sidestep movement command
        public HoldKey SidestepHoldKey;         // 0x40 - indicates whether a sidestep key is being held
        public float SidestepSpeed;             // 0x80 - sidestep movement speed
        public MotionCommand TurnCommand;       // 0x100 - turn command - this is always sent as 1 direction in RawMotion,
                                                // with a negative speed for the opposite direction. A negative speed
                                                // is then in turn translated to the opposite Motion in InterpretedMotionState.
        public HoldKey TurnHoldKey;             // 0x200 - whether turn key is being held, or mouselook in progress
        public float TurnSpeed;                 // 0x400 - turn movement speed - somewhat static

        // commands: list of length commandListLength
        public List<MotionItem> Commands;

        public RawMotionState() { }

        public RawMotionState(MoveToState moveToState, BinaryReader reader)
        {
            MoveToState = moveToState;

            PackedFlags = reader.ReadUInt32();

            // security vulnerability here:
            // untrusted client input sending command list length
            Flags = (RawMotionFlags)(PackedFlags & 0x7FF);
            CommandListLength = (ushort)(PackedFlags >> 11);

            if ((Flags & RawMotionFlags.CurrentHoldKey) != 0)
                CurrentHoldKey = (HoldKey)reader.ReadUInt32();
            if ((Flags & RawMotionFlags.CurrentStyle) != 0)
                CurrentStyle = (MotionStance)reader.ReadUInt32();
            if ((Flags & RawMotionFlags.ForwardCommand) != 0)
                ForwardCommand = (MotionCommand)reader.ReadUInt32();
            if ((Flags & RawMotionFlags.ForwardHoldKey) != 0)
                ForwardHoldKey = (HoldKey)reader.ReadUInt32();
            if ((Flags & RawMotionFlags.ForwardSpeed) != 0)
                ForwardSpeed = reader.ReadSingle();
            if ((Flags & RawMotionFlags.SideStepCommand) != 0)
                SidestepCommand = (MotionCommand)reader.ReadUInt32();
            if ((Flags & RawMotionFlags.SideStepHoldKey) != 0)
                SidestepHoldKey = (HoldKey)reader.ReadUInt32();
            if ((Flags & RawMotionFlags.SideStepSpeed) != 0)
                SidestepSpeed = reader.ReadSingle();
            if ((Flags & RawMotionFlags.TurnCommand) != 0)
                TurnCommand = (MotionCommand)reader.ReadUInt32();
            if ((Flags & RawMotionFlags.TurnHoldKey) != 0)
                TurnHoldKey = (HoldKey)reader.ReadUInt32();
            if ((Flags & RawMotionFlags.TurnSpeed) != 0)
                TurnSpeed = reader.ReadSingle();

            if (CommandListLength > 0)
            {
                Commands = new List<MotionItem>();
                for (var i = 0; i < CommandListLength; i++)
                    Commands.Add(new MotionItem(moveToState.WorldObject, reader));
            }
        }

        public string ToString(bool showFlags)
        {
            var sb = new StringBuilder();

            if (showFlags)
                sb.AppendLine($"Flags: {Flags}");

            if ((Flags & RawMotionFlags.CurrentHoldKey) != 0)
                sb.AppendLine($"CurrentHoldKey: {CurrentHoldKey}");
            if ((Flags & RawMotionFlags.CurrentStyle) != 0)
                sb.AppendLine($"CurrentStyle: {CurrentStyle}");
            if ((Flags & RawMotionFlags.ForwardCommand) != 0)
                sb.AppendLine($"ForwardCommand: {ForwardCommand}");
            if ((Flags & RawMotionFlags.ForwardHoldKey) != 0)
                sb.AppendLine($"ForwardHoldKey: {ForwardHoldKey}");
            if ((Flags & RawMotionFlags.ForwardSpeed) != 0)
                sb.AppendLine($"ForwardSpeed: {ForwardSpeed}");
            if ((Flags & RawMotionFlags.SideStepCommand) != 0)
                sb.AppendLine($"SidestepCommand: {SidestepCommand}");
            if ((Flags & RawMotionFlags.SideStepHoldKey) != 0)
                sb.AppendLine($"SidestepHoldKey: {SidestepHoldKey}");
            if ((Flags & RawMotionFlags.SideStepSpeed) != 0)
                sb.AppendLine($"SidestepSpeed: {SidestepSpeed}");
            if ((Flags & RawMotionFlags.TurnCommand) != 0)
                sb.AppendLine($"TurnCommand: {TurnCommand}");
            if ((Flags & RawMotionFlags.TurnHoldKey) != 0)
                sb.AppendLine($"TurnHoldKey: {TurnHoldKey}");
            if ((Flags & RawMotionFlags.TurnSpeed) != 0)
                sb.AppendLine($"TurnSpeed: {TurnSpeed}");

            if (CommandListLength > 0)
            {
                sb.AppendLine($"CommandListLength: {CommandListLength}");
                foreach (var command in Commands)
                    sb.Append(command);
            }

            sb.AppendLine("---");

            return sb.ToString();
        }

        public override string ToString()
        {
            return ToString(true);
        }

        public bool HasMovement()
        {
            return (Flags & (RawMotionFlags.ForwardCommand | RawMotionFlags.TurnCommand | RawMotionFlags.SideStepCommand)) != 0;
        }

        public bool HasSoulEmote(bool checkForward = true)
        {
            if (checkForward && (Flags & RawMotionFlags.ForwardCommand) != 0 && SoulEmote.SoulEmotes.Contains(ForwardCommand))
                return true;

            if (CommandListLength > 0 && SoulEmote.SoulEmotes.Contains(Commands[0].MotionCommand))
                return true;

            return false;
        }
    }
}
