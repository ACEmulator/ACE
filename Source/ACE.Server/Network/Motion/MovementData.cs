using System;
using System.IO;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Network.Enum;
using ACE.Server.Network.Sequence;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.Structure
{
    /// <summary>
    /// The movement and animation for an object
    /// </summary>
    public class MovementData
    {
        public WorldObject WorldObject;

        public ushort MovementSequence;
        public ushort ServerControlSequence;
        public bool IsAutonomous;       // true = client initiated, false = server initiated
        public MovementType MovementType;
        public MotionFlags MotionFlags;
        public MotionStance CurrentStyle;

        // select one section based on MovementType:
        public MovementInvalid Invalid;
        public MoveToObject MoveToObject;
        public MoveToPosition MoveToPosition;
        public TurnToObject TurnToObject;
        public TurnToHeading TurnToHeading;

        public MovementData() { }

        public MovementData(WorldObject wo)
        {
            WorldObject = wo;
        }

        public MovementData(WorldObject wo, Motion motion)
        {
            WorldObject = wo;
            //var sequence = wo.Sequences;

            // do this here, or in network writer?
            IsAutonomous = motion.IsAutonomous;
            //MovementSequence = BitConverter.ToUInt16(sequence.GetNextSequence(SequenceType.ObjectMovement));

            //if (IsAutonomous)
                //ServerControlSequence = BitConverter.ToUInt16(sequence.GetCurrentSequence(SequenceType.ObjectServerControl));
            //else
                //ServerControlSequence = BitConverter.ToUInt16(sequence.GetNextSequence(SequenceType.ObjectServerControl));

            MovementType = motion.MovementType;
            MotionFlags = motion.MotionFlags;

            //if (motion.HasTarget)
                //MotionFlags |= MotionFlags.StickToObject;
            //if (motion.StandingLongJump)
                //MotionFlags |= MotionFlags.StandingLongJump;    // indicates if player started charging jump bar while standing still

            CurrentStyle = motion.Stance;

            switch (MovementType)
            {
                case MovementType.Invalid:
                    Invalid = new MovementInvalid(this, motion);
                    break;
                case MovementType.MoveToObject:
                    MoveToObject = new MoveToObject(motion);
                    break;
                case MovementType.MoveToPosition:
                    MoveToPosition = new MoveToPosition(motion);
                    break;
                case MovementType.TurnToObject:
                    TurnToObject = new TurnToObject(motion);
                    break;
                case MovementType.TurnToHeading:
                    TurnToHeading = new TurnToHeading(motion);
                    break;
            }
        }

        /// <summary>
        /// Converts a MoveToState packet from the client -> MovementData packet to send to other clients
        /// This is effectively a shortcut for converting RawMotionState -> InterpretedMotionState
        /// </summary>
        public MovementData(Creature creature, MoveToState state)
        {
            WorldObject = creature;

            var rawState = state.RawMotionState;

            // keeping most of this existing logic, ported from ConvertToClientAccepted
            if ((rawState.Flags & RawMotionFlags.CurrentStyle) != 0)
                CurrentStyle = rawState.CurrentStyle;

            // only using primary hold key?
            var holdKey = rawState.CurrentHoldKey;
            var speed = holdKey == HoldKey.Run ? creature.GetRunRate() : 1.0f;

            var interpState = new InterpretedMotionState(this);

            // move forwards / backwards / animation
            if ((rawState.Flags & RawMotionFlags.ForwardCommand) != 0 && !state.StandingLongJump)
            {
                if (rawState.ForwardCommand == MotionCommand.WalkForward || rawState.ForwardCommand == MotionCommand.WalkBackwards)
                {
                    interpState.ForwardCommand = MotionCommand.WalkForward;

                    if (rawState.ForwardCommand == MotionCommand.WalkForward && holdKey == HoldKey.Run)
                        interpState.ForwardCommand = MotionCommand.RunForward;

                    interpState.ForwardSpeed = speed;

                    if (rawState.ForwardCommand == MotionCommand.WalkBackwards)
                        interpState.ForwardSpeed *= -0.65f;
                }
                else
                    interpState.ForwardCommand = rawState.ForwardCommand;
            }

            // sidestep
            if ((rawState.Flags & RawMotionFlags.SideStepCommand) != 0 && !state.StandingLongJump)
            {
                interpState.SidestepCommand = MotionCommand.SideStepRight;
                interpState.SidestepSpeed = speed * 3.12f / 1.25f * 0.5f;

                if (rawState.SidestepCommand == MotionCommand.SideStepLeft)
                    interpState.SidestepSpeed *= -1;

                interpState.SidestepSpeed = Math.Clamp(interpState.SidestepSpeed, -3, 3);
            }

            // rotate
            if ((rawState.Flags & RawMotionFlags.TurnCommand) != 0)
            {
                interpState.TurnCommand = MotionCommand.TurnRight;
                interpState.TurnSpeed = holdKey == HoldKey.Run ? 1.5f : 1.0f;

                // mouselook
                if (rawState.TurnSpeed != 0 && rawState.TurnSpeed <= 1.5f)
                    interpState.TurnSpeed = rawState.TurnSpeed;

                if (rawState.TurnCommand == MotionCommand.TurnLeft)
                    interpState.TurnSpeed *= -1;
            }

            // contact/sticky?
            // this alone isn't enough for standing long jump,
            // and observing clients seems to show a buggy shallow arc jump
            // without the above exclusions of ForwardCommand / SidestepCommand
            if (state.StandingLongJump)
                MotionFlags |= MotionFlags.StandingLongJump;

            interpState.Commands = rawState.Commands;
            interpState.Flags = interpState.BuildMovementFlags();

            // this is a hack to make walking work correctly - investigate this
            // wouldn't all of these be autonomous?
            // walk backwards?
            //if (holdKey != HoldKey.Invalid || rawState.ForwardCommand == MotionCommand.WalkForward)
                IsAutonomous = true;

            Invalid = new MovementInvalid(this, interpState);
        }

        /// <summary>
        /// Serializes this movement data to a byte array
        /// Currently only used for CreateObject messages
        /// </summary>
        public byte[] Serialize()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(this, false);
                return stream.ToArray();
            }
        }
    }

    public static class MovementDataExtensions
    {
        public static void Write(this BinaryWriter writer, MovementData motion, bool header = true)
        {
            var wo = motion.WorldObject;
            var sequence = wo.Sequences;

            if (header)
            {
                writer.Write(sequence.GetNextSequence(SequenceType.ObjectMovement));

                if (motion.IsAutonomous)
                    writer.Write(sequence.GetCurrentSequence(SequenceType.ObjectServerControl));
                else
                    writer.Write(sequence.GetNextSequence(SequenceType.ObjectServerControl));

                writer.Write(Convert.ToByte(motion.IsAutonomous));
                writer.Align();
            }

            writer.Write((byte)motion.MovementType);
            writer.Write((byte)motion.MotionFlags);

            writer.Write((ushort)motion.CurrentStyle);    // send MotionStance as ushort

            switch (motion.MovementType)
            {
                case MovementType.Invalid:
                    writer.Write(motion.Invalid);
                    break;

                case MovementType.MoveToObject:
                    writer.Write(motion.MoveToObject);
                    break;

                case MovementType.MoveToPosition:
                    writer.Write(motion.MoveToPosition);
                    break;

                case MovementType.TurnToObject:
                    writer.Write(motion.TurnToObject);
                    break;

                case MovementType.TurnToHeading:
                    writer.Write(motion.TurnToHeading);
                    break;
            }
        }
    }
}
