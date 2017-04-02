using log4net;
using ACE.Network.Enum;
using ACE.Network.Sequence;
using System.Collections.Generic;

namespace ACE.Network.GameMessages.Messages
{
    using System.Diagnostics.Eventing.Reader;
    using System.Runtime.Remoting.Messaging;

    using global::ACE.Entity;

    public class GameMessageMotion : GameMessage
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private void Log(string message)
        {
            log.Debug($"Error: {message}");
        }

        public GameMessageMotion()
            : base(GameMessageOpcode.Motion, GameMessageGroup.Group0A)
        {

        }

        public GameMessageMotion(WorldObject animationTarget, Session session, MotionAutonomous activity,
            MovementTypes type, MotionFlags flags, MotionStance stance, MovementData movement, List<MotionItem> animations = null)
            : this()
        {
            WriteBase(animationTarget, session, activity, type, flags, stance);
            movement?.Serialize(this.Writer);
            if (animations == null) this.Writer.Write((ushort)0);
            else this.WriteAnimations(animationTarget, animations);
        }

        public GameMessageMotion(Entity.WorldObject animationTarget, Session session, MotionCommand command, float speed = 1.0f)
            : this()
        {
            WriteBase(animationTarget, session, MotionAutonomous.False, MovementTypes.Invalid, MotionFlags.None, MotionStance.Standing);
            List<MotionItem> animations = new List<MotionItem>() { new MotionItem(command, speed) };
            WriteAnimations(animationTarget, animations);
        }

        private void WriteBase(WorldObject animationTarget, Session session, MotionAutonomous autonomous,
            MovementTypes type, MotionFlags flags, MotionStance stance)
        {
            Writer.WriteGuid(animationTarget.Guid); // Object_Id (uint)
            Writer.Write((ushort)session.Player.TotalLogins); // Instance_Timestamp
            Writer.Write(animationTarget.Sequences.GetNextSequence(SequenceType.MotionMessage)); // Movement_Timestamp
            if (autonomous == MotionAutonomous.False)
                Writer.Write(animationTarget.Sequences.GetNextSequence(Sequence.SequenceType.MotionMessageAutonomous)); // Server_Control_Timestamp
            else
                Writer.Write(animationTarget.Sequences.GetCurrentSequence(Sequence.SequenceType.MotionMessageAutonomous)); // Server_Control_Timestamp
            Writer.Write((ushort)type); // movement_type
            Writer.Write((byte)autonomous); // autonomous flag - 1 or 0.   I think this is set if you have are holding the run key or some other autonomous movement
            Writer.Write((byte)flags); // these can be or and has sticky object | is long jump mode |
            Writer.Write((ushort)stance); // called command in the client
        }

        private void WriteAnimations(WorldObject animationTarget, List<MotionItem> items)
        {
            if (animationTarget == null)
            {
                Log("We have a null for animationTarget - that is wrong, wrong wrong.");
                return;
            }
            var generalFlags = (uint)items.Count << 7;
            Writer.Write(generalFlags);
            foreach (var item in items)
            {
                Writer.Write((ushort)item.Motion);
                Writer.Write(animationTarget.Sequences.GetNextSequence(Sequence.SequenceType.Motion));
                Writer.Write(item.Speed);
            }
        }
    }
}
<<<<<<< HEAD

=======
/*
GameMessageMotion (Aka Animation)
uint ObjectId - ID of object moving
ushort logins - Number of user logins (instance_timestamp)
ushort sequence - Number of animations this login for this object (movement_timestamp)
ushort idleSequence - Number of idle animations this login for this object (server_control_timestamp)
ushort activity - Idle/Active, Idle will be sent by the server for periodic idle animations
byte motionType - Kind of motion (move to position, turn, or general)
byte typeFlags - flags for some additional information on packet (ex. target for sticky melee)
ushort stance - current(?) stance
if(motionType == 0) //General
    uint generalFlags
    if(generalFlags & 01)
        ushort stance2
    if(generalFlags & 02)
        ushort forwardCommand
    if(generalFlags & 08)
        ushort sidestepCommand
    if(generalFlags & 20)
        ushort turnCommand
    if(generalFlags & 04)
        float forwardSpeed
    if(generalFlags & 10)
        float sidestepSpeed
    if(generalFlags & 40)
        float turnSpeed
    list(length = generalFlags << 7)
        ushort animationId
        ushort sequence
        float animationSpeed
if(motionType == 6)
    //Todo MoveToObject
if(motionType == 7)
    //Todo MoveToPosition
if(motionType == 8)
    //Todo TurnToObject
if(motionType == 9)
    //Todo TurnToPosition
*/
>>>>>>> 868f082200d7858037eef5cf2c64b6ef68c376c7
