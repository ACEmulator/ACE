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

        public GameMessageMotion(WorldObject animationTarget, Session session, MotionActivity activity,
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
            WriteBase(animationTarget, session, MotionActivity.Idle, MovementTypes.Invalid, MotionFlags.None, MotionStance.Standing);
            List<MotionItem> animations = new List<MotionItem>() { new MotionItem(command, speed) };
            WriteAnimations(animationTarget, animations);
        }

        private void WriteBase(WorldObject animationTarget, Session session, MotionActivity activity,
            MovementTypes type, MotionFlags flags, MotionStance stance)
        {
            Writer.WriteGuid(animationTarget.Guid); // Object_Id (uint)
            Writer.Write((ushort)session.Player.TotalLogins); // Instance_Timestamp
            Writer.Write(animationTarget.Sequences.GetNextSequence(SequenceType.MotionMessage)); // Movement_Timestamp
            Writer.Write(animationTarget.Sequences.GetNextSequence(SequenceType.ServerControl)); // Server_Control_Timestamp
            // The order of these is in question between the movement and server control.   Looks like in the client this is the right order
            // in AC Log View it is reversed.

            // TODO: research the correct order.   We can probably get away with either for now.
            Writer.Write((ushort)type); // movement_type
            Writer.Write((byte)activity); // autonomous flag - 1 or 0.   I think this is set if you have are holding the run key or some other autonomous movement
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

