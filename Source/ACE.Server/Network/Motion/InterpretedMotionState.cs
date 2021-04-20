using System.Collections.Generic;
using System.IO;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.Structure
{
    /// <summary>
    /// Information related to movement and animation
    /// </summary>
    public class InterpretedMotionState
    {
        public MovementStateFlag Flags;
        public MovementData MovementData;

        public MotionStance CurrentStyle;

        public MotionCommand ForwardCommand;
        public MotionCommand SidestepCommand;
        public MotionCommand TurnCommand;

        public float ForwardSpeed = 1.0f;
        public float SidestepSpeed = 1.0f;
        public float TurnSpeed = 1.0f;

        // commands: list of length commandListLength
        public List<MotionItem> Commands;

        public InterpretedMotionState()
        {
            ForwardCommand = MotionCommand.Ready;
        }

        public InterpretedMotionState(InterpretedMotionState state)
        {
            // copy constructor
            Flags = state.Flags;
            MovementData = state.MovementData;
            CurrentStyle = state.CurrentStyle;
            ForwardCommand = state.ForwardCommand;
            SidestepCommand = state.SidestepCommand;
            TurnCommand = state.TurnCommand;
            ForwardSpeed = state.ForwardSpeed;
            SidestepSpeed = state.SidestepSpeed;
            TurnSpeed = state.TurnSpeed;

            if (state.Commands != null)
            {
                Commands = new List<MotionItem>();
                foreach (var command in state.Commands)
                    Commands.Add(new MotionItem(command));
            }
        }

        public InterpretedMotionState(MovementData data)
        {
            MovementData = data;
            CurrentStyle = data.CurrentStyle;

            Flags = BuildMovementFlags();
        }

        public InterpretedMotionState(MovementData data, Motion motion)
        {
            MovementData = data;
            CurrentStyle = data.CurrentStyle;

            var state = motion.MotionState;

            ForwardCommand = state.ForwardCommand;
            SidestepCommand = state.SidestepCommand;
            TurnCommand = state.TurnCommand;

            ForwardSpeed = state.ForwardSpeed;
            SidestepSpeed = state.SidestepSpeed;
            TurnSpeed = state.TurnSpeed;

            Commands = state.Commands;

            Flags = BuildMovementFlags();
        }

        /// <summary>
        /// Builds the MovementFlags based on the current state
        /// </summary>
        public MovementStateFlag BuildMovementFlags()
        {
            var flags = MovementStateFlag.Invalid;

            if (CurrentStyle != 0 && CurrentStyle != MotionStance.Invalid)
                flags |= MovementStateFlag.CurrentStyle;

            if (ForwardCommand != MotionCommand.Invalid)
                flags |= MovementStateFlag.ForwardCommand;
            if (SidestepCommand != MotionCommand.Invalid)
                flags |= MovementStateFlag.SideStepCommand;
            if (TurnCommand != MotionCommand.Invalid)
                flags |= MovementStateFlag.TurnCommand;

            if (ForwardSpeed != 1.0f)
                flags |= MovementStateFlag.ForwardSpeed;
            if (SidestepSpeed != 1.0f)
                flags |= MovementStateFlag.SideStepSpeed;
            if (TurnSpeed != 1.0f)
                flags |= MovementStateFlag.TurnSpeed;

            return flags;
        }

        public void AddCommand(WorldObject worldObject, MotionCommand motionCommand, float speed = 1.0f)
        {
            if (Commands == null)
                Commands = new List<MotionItem>();

            Commands.Add(new MotionItem(worldObject, motionCommand, speed));
        }

        public bool HasMovement()
        {
            return (ForwardCommand != MotionCommand.Invalid && ForwardCommand != MotionCommand.Ready) || TurnCommand != MotionCommand.Invalid || SidestepCommand != MotionCommand.Invalid;
        }
    }

    public static class InterpretedMotionStateExtensions
    {
        public static void Write(this BinaryWriter writer, InterpretedMotionState state)
        {
            var numCommands = state.Commands != null ? state.Commands.Count : 0;

            writer.Write((uint)state.Flags | (uint)numCommands << 7);

            // for MotionStance / MotionCommand, write as ushort
            if ((state.Flags & MovementStateFlag.CurrentStyle) != 0)
                writer.Write((ushort)state.CurrentStyle);

            if ((state.Flags & MovementStateFlag.ForwardCommand) != 0)
                writer.Write((ushort)state.ForwardCommand);

            if ((state.Flags & MovementStateFlag.SideStepCommand) != 0)
                writer.Write((ushort)state.SidestepCommand);

            if ((state.Flags & MovementStateFlag.TurnCommand) != 0)
                writer.Write((ushort)state.TurnCommand);

            if ((state.Flags & MovementStateFlag.ForwardSpeed) != 0)
                writer.Write(state.ForwardSpeed);

            if ((state.Flags & MovementStateFlag.SideStepSpeed) != 0)
                writer.Write(state.SidestepSpeed);

            if ((state.Flags & MovementStateFlag.TurnSpeed) != 0)
                writer.Write(state.TurnSpeed);

            if (numCommands > 0)
            {
                foreach (var motion in state.Commands)
                    writer.Write(motion);
            }

            // align to DWORD boundary
            writer.Align();
        }
    }
}
