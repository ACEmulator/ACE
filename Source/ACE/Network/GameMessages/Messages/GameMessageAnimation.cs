using System;
using ACE.Network.Enum;
using ACE.Network.Sequence;
using System.Collections.Generic;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageAnimation : GameMessage
    {
        public GameMessageAnimation()
            : base(GameMessageOpcode.Animation, GameMessageGroup.Group0A)
        {

        }

        public GameMessageAnimation(Entity.WorldObject animationTarget, Session session, AnimationActivity activity,
            AnimationAction type, AnimationFlags flags, StanceMode stance, AnimationType animation, float speed)
            : this()
        {
            WriteBase(animationTarget, session, activity, type, flags, stance);
            List<AnimationItem> animations = new List<AnimationItem>() { new AnimationItem(animation, speed) };
            WriteAnimations(animationTarget, animations);
        }
        
        public GameMessageAnimation(Entity.WorldObject animationTarget, Session session, AnimationType animation, float speed = 1.0f)
            : this()
        {
            WriteBase(animationTarget, session, AnimationActivity.Idle, AnimationAction.General, AnimationFlags.None, StanceMode.Standing);
            List<AnimationItem> animations = new List<AnimationItem>() { new AnimationItem(animation, speed) };
            WriteAnimations(animationTarget, animations);
        }

        private void WriteBase(Entity.WorldObject animationTarget, Session session, AnimationActivity activity,
            AnimationAction type, AnimationFlags flags, StanceMode stance)
        {
            Writer.WriteGuid(animationTarget.Guid);
            Writer.Write((ushort)session.Player.TotalLogins);
            Writer.Write(animationTarget.Sequences.GetNextSequence(Sequence.SequenceType.AnimationMessage));
            Writer.Write((ushort)1); // Index, needs more research, it changes sometimes, but not every packet
            Writer.Write((ushort)activity);
            Writer.Write((byte)type);
            Writer.Write((byte)flags);
            Writer.Write((ushort)stance);
        }

        private void WriteAnimations(Entity.WorldObject animationTarget, List<AnimationItem> animations)
        {
            uint generalFlags = (uint)animations.Count << 7;
            Writer.Write((uint)generalFlags);
            foreach (var animation in animations)
            {
                Writer.Write((ushort)animation.Animation);
                Writer.Write(animationTarget.Sequences.GetNextSequence(Sequence.SequenceType.Animation));
                Writer.Write(animation.Speed);
            }
        }
    }
    public struct AnimationItem
    {
        public AnimationType Animation { get; set; }
        public float Speed { get; set; }

        public AnimationItem(AnimationType animation)
        {
            Animation = animation;
            Speed = 1.0f;
        }

        public AnimationItem(AnimationType animation, float speed)
        {
            Animation = animation;
            Speed = speed;
        }
    }

    public enum AnimationActivity
    {
        Idle = 0,
        Active = 1
    }

    public enum AnimationAction
    {
        General = 0,
        MoveToObject = 6,
        MoveToPosition = 7,
        TurnToObject = 8,
        TurnToPosition = 9
    }

    [Flags]
    public enum AnimationFlags
    {
        None = 0x0,
        HasTarget = 0x1,
        Jumping = 0x2 // Needs to be investigated
    }

    public enum StanceMode
    {
        UANoShieldAttack = 0x3C,
        Standing = 0x3D,
        MeleeNoShieldAttack = 0x3E,
        BowAttack = 0x3F,
        MeleeShieldAttack = 0x40,
        Spellcasting = 0x49
    }

    public enum AnimationType
    {
        Run = 7,
        FistJump = 76,
        ShakeFist = 121,
        Bow = 125,
        Logout1 = 286,
        Logout2 = 885
    }
}