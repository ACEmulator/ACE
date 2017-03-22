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
            MotionType type, MotionFlags flags, MotionStance stance, MotionCommand command, float speed)
            : this()
        {
            WriteBase(animationTarget, session, activity, type, flags, stance);
            List<MotionItem> animations = new List<MotionItem>();
            if (command != MotionCommand.MotionInvalid)
            {
                animations.Add(new MotionItem(command, speed));
            }
            WriteAnimations(animationTarget, animations);
        }

        public GameMessageMotion(Entity.WorldObject animationTarget, Session session, MotionCommand command, float speed = 1.0f)
            : this()
        {
            WriteBase(animationTarget, session, MotionActivity.Idle, MotionType.General, MotionFlags.None, MotionStance.Standing);
            List<MotionItem> animations = new List<MotionItem>();
            if (command != MotionCommand.MotionInvalid)
            {
                animations.Add(new MotionItem(command, speed));
            }
            WriteAnimations(animationTarget, animations);
        }

        public enum MotionState
        {
          bf_current_style = 01,
          bf_forward_command = 02,
          bf_forward_speed = 04,
          bf_sidestep_command = 08,
          bf_sidestep_speed = 10,
          bf_turn_command = 20,
          bf_turn_speed = 40
        }
        private void WriteBase(Entity.WorldObject animationTarget, Session session, MotionActivity activity,
            MotionType type, MotionFlags flags, MotionStance stance)

        {
            Writer.WriteGuid(animationTarget.Guid);
            Writer.Write((ushort)session.Player.TotalLogins);
            Writer.Write(animationTarget.Sequences.GetNextSequence(Sequence.SequenceType.MotionMessage));
            Writer.Write((ushort)1); // Index, needs more research, it changes sometimes, but not every packet
            Writer.Write((ushort)activity);
            Writer.Write((byte)type);
            Writer.Write((byte)flags);
            Writer.Write((ushort)stance);

            // TODO: Almost there CFS

            Writer.Write((uint)MotionState.bf_forward_command);
            Writer.Write((uint)MotionState.bf_turn_command);
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
