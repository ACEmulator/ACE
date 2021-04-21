using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Network.Structure;
using ACE.Server.WorldObjects;
using log4net;

namespace ACE.Server.Entity
{
    /// <summary>
    /// Convenience wrapper for transitioning the most common uses of Motion
    /// </summary>
    public class Motion
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // previously in MotionState base abstract class, only 1 reference
        // IsAutonomous TRUE would indicate a client-initiated movement
        public bool IsAutonomous;

        /// <summary>
        /// 5 different modes of operation: Invalid (General), MoveToObject, MoveToPosition, TurnToObject, TurnToHeading
        /// </summary>
        public MovementType MovementType;
        public MotionFlags MotionFlags;     // stick to object / standing long jump
        public MotionStance Stance;         // CurrentStyle

        // modes of operation:
        // General - InterpretedMotionState / StickyObject
        public InterpretedMotionState MotionState = new InterpretedMotionState();
        public ObjectGuid TargetGuid;       // this is dual-purposed for both Target and StickyObject (fixme)

        // MoveToObject - targetguid, position, movetoparams, runrate
        public Position Position;           // used for: MoveTo's
        public MoveToParameters MoveToParameters = new MoveToParameters();   // used for MoveTo and TurnTo
        public float RunRate;               // used for: MoveTo's

        // MoveToPosition - position, movetoparams, runrate

        // TurnToObject - target, desiredheading, turnto params
        public float DesiredHeading;        // overrides MovementParams version

        // TurnToHeading - turnto params

        /// <summary>
        /// Constructs a new Motion from a MotionStance
        /// This is used to switch combat stances
        /// </summary>
        public Motion(MotionStance stance)
        {
            Stance = stance;
        }

        /// <summary>
        /// Constructs a new MoveToObject / TurnToObject motion
        /// </summary>
        public Motion(WorldObject wo, WorldObject target, MovementType type)
        {
            if (wo.CurrentMotionState != null)
                Stance = wo.CurrentMotionState.Stance;
            else
            {
                Stance = new Motion(MotionStance.NonCombat).Stance;
                log.Warn($"{wo.Name} (0x{wo.Guid}) has a null CurrentMotionState, subbing in new Motion(MotionStance.NonCombat) for it.");
            }
            MovementType = type;
            Position = new Position(target.Location);
            TargetGuid = target.Guid;
        }

        /// <summary>
        /// Constructs a new MoveToPosition motion
        /// </summary>
        public Motion(WorldObject wo, Position position)
        {
            if (wo.CurrentMotionState != null)
                Stance = wo.CurrentMotionState.Stance;
            else
            {
                Stance = new Motion(MotionStance.NonCombat).Stance;
                log.Warn($"{wo.Name} (0x{wo.Guid}) has a null CurrentMotionState, subbing in new Motion(MotionStance.NonCombat) for it.");
            }
            MovementType = MovementType.MoveToPosition;
            Position = new Position(position);
        }

        /// <summary>
        /// Constructs a new TurnToHeading motion
        /// </summary>
        public Motion(WorldObject wo, Position position, float heading)
        {
            if (wo.CurrentMotionState != null)
                Stance = wo.CurrentMotionState.Stance;
            else
            {
                Stance = new Motion(MotionStance.NonCombat).Stance;
                log.Warn($"{wo.Name} (0x{wo.Guid}) has a null CurrentMotionState, subbing in new Motion(MotionStance.NonCombat) for it.");
            }
            MovementType = MovementType.TurnToHeading;
            Position = new Position(position);
            DesiredHeading = heading;
        }

        public Motion(MotionStance stance, MotionCommand motion, float speed = 1.0f)
        {
            Stance = stance;
            SetForwardCommand(motion, speed);
        }

        public Motion(WorldObject wo, MotionCommand motion, float speed = 1.0f)
        {
            if (wo.CurrentMotionState != null)
                Stance = wo.CurrentMotionState.Stance;
            else
            {
                Stance = new Motion(MotionStance.NonCombat).Stance;
                log.Warn($"{wo.Name} (0x{wo.Guid}) has a null CurrentMotionState, subbing in new Motion(MotionStance.NonCombat) for it.");
            }
            SetForwardCommand(motion, speed);
        }

        public Motion(Motion motion)
        {
            // copy constructor
            IsAutonomous = motion.IsAutonomous;
            MovementType = motion.MovementType;
            MotionFlags = motion.MotionFlags;
            Stance = motion.Stance;

            if (motion.MotionState != null)
                MotionState = new InterpretedMotionState(motion.MotionState);

            TargetGuid = motion.TargetGuid;

            if (motion.Position != null)
                Position = new Position(motion.Position);

            if (motion.MoveToParameters != null)
                MoveToParameters = new MoveToParameters(motion.MoveToParameters);

            RunRate = motion.RunRate;
            DesiredHeading = motion.DesiredHeading;
        }

        public void SetForwardCommand(MotionCommand motion, float speed = 1.0f)
        {
            MotionState.ForwardCommand = motion;
            MotionState.ForwardSpeed = speed;
        }

        public void SetSidestepCommand(MotionCommand motion, float speed = 1.0f)
        {
            MotionState.SidestepCommand = motion;
            MotionState.SidestepSpeed = speed;
        }

        public void SetTurnCommand(MotionCommand motion, float speed = 1.0f)
        {
            MotionState.TurnCommand = motion;
            MotionState.TurnSpeed = speed;
        }

        public void Persist(Motion motion)
        {
            SetSidestepCommand(motion.MotionState.SidestepCommand, motion.MotionState.SidestepSpeed);
            SetTurnCommand(motion.MotionState.TurnCommand, motion.MotionState.TurnSpeed);
        }
    }
}
