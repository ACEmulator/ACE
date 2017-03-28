using ACE.Network.Enum;
using ACE.Network.Sequence;
using System.Collections.Generic;

namespace ACE.Network.GameMessages.Messages
{
    using global::ACE.Entity;

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
            List<MotionItem> animations = new List<MotionItem>() { new MotionItem(command, speed) };
            if (command != MotionCommand.MotionInvalid)
            {
                animations.Add(new MotionItem(command, speed));
            }
            WriteAnimations(animationTarget, animations);
        }

        public GameMessageMotion(Entity.WorldObject animationTarget, Session session, MotionActivity activity,
           MotionType type, MotionFlags flags, MotionStance stance, MotionCommand command, MotionData motionData, float speed)
           : this()
        {
            this.WriteBase(animationTarget, session, activity, type, flags, stance);
            this.Writer.Write((uint)motionData.MotionStateFlag);
            motionData.Serialize(this.Writer);
            List<MotionItem> animations = new List<MotionItem>() { new MotionItem(command, speed) };
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
            List<MotionItem> animations = new List<MotionItem>() { new MotionItem(command, speed) };
            if (command != MotionCommand.MotionInvalid)
            {
                animations.Add(new MotionItem(command, speed));
            }
            WriteAnimations(animationTarget, animations);
        }


        private void WriteBase(Entity.WorldObject animationTarget, Session session, MotionActivity activity,
            MotionType type, MotionFlags flags, MotionStance stance)

        {
            Writer.WriteGuid(animationTarget.Guid);
            Writer.Write((ushort)session.Player.TotalLogins);
            Writer.Write(animationTarget.Sequences.GetNextSequence(Sequence.SequenceType.MotionMessage));
            Writer.Write((ushort)1); // Index, needs more research, it changes sometimes, but not every packet
            Writer.Write(animationTarget.Sequences.GetNextSequence(SequenceType.MotionMessage));
            Writer.Write(animationTarget.Sequences.GetNextSequence(SequenceType.ServerControl));
            Writer.Write((ushort)activity);
            Writer.Write((byte)type);
            Writer.Write((byte)flags);
            Writer.Write((ushort)stance);
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