using System;
using ACE.Network.Enum;
using ACE.Network.Sequence;
using System.Collections.Generic;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageMotion : GameMessage
    {
        public GameMessageMotion()
            : base(GameMessageOpcode.Motion, GameMessageGroup.Group0A)
        {

        }

        public GameMessageMotion(Entity.WorldObject animationTarget, Session session, MotionActivity activity,
            MovementTypes type, MotionFlags flags, MotionStance stance, MotionCommand command, float speed)
            : this()
        {
            WriteBase(animationTarget, session, activity, type, flags, stance);
            List<MotionItem> animations = new List<MotionItem>() { new MotionItem(command, speed) };
            WriteAnimations(animationTarget, animations);
        }

        public GameMessageMotion(Entity.WorldObject animationTarget, Session session, MotionCommand command, float speed = 1.0f)
            : this()
        {
            WriteBase(animationTarget, session, MotionActivity.Idle, MovementTypes.Invalid, MotionFlags.None, MotionStance.Standing);
            List<MotionItem> animations = new List<MotionItem>() { new MotionItem(command, speed) };
            WriteAnimations(animationTarget, animations);
        }

        private void WriteBase(Entity.WorldObject animationTarget, Session session, MotionActivity activity,
            MovementTypes type, MotionFlags flags, MotionStance stance, ushort autonomous = 0)
        {
            Writer.WriteGuid(animationTarget.Guid); // Object_Id
            Writer.Write((ushort)session.Player.TotalLogins); // Instance_Timestamp
            Writer.Write(animationTarget.Sequences.GetNextSequence(Sequence.SequenceType.MotionMessage)); // Movement_Timestamp
            Writer.Write(animationTarget.Sequences.GetNextSequence(SequenceType.ServerControl)); // Server_Control_Timestamp
            // The order of these is in question between the movement and server control.   Looks like in the client this is the right order
            // in AC Log View it is reversed.
            // TODO: reseach the correct order.   We can probably get away with either for now.
            Writer.Write(autonomous); // autonomous flag - 1 or 0.   I think this is set if you have are holding the run key or some other autonomous movement
            Writer.Write((ushort)type); // movement_type
            Writer.Write((ushort)flags); // these can be or and has sticky object | is long jump mode |
            Writer.Write((ushort)stance); // called command in the client

        }

        private void WriteAnimations(Entity.WorldObject animationTarget, List<MotionItem> items)
        {
            uint generalFlags = (uint)items.Count << 7;
            Writer.Write((uint)generalFlags);
            foreach (var item in items)
            {
                Writer.Write((ushort)item.Motion);
                Writer.Write(animationTarget.Sequences.GetNextSequence(Sequence.SequenceType.Motion));
                Writer.Write(item.Speed);
            }
        }
    }
}
/*
GameMessageMotion (Aka Animation)
uint ObjectId - ID of object moving
ushort logins - Number of user logins (instance_timestamp)
ushort sequence - Number of animations this login for this object (movement_timestamp)
ushort index - Changes sometimes, but not every message (server_control_timestamp)
ushort activity - Idle/Active
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
