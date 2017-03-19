using System;
using ACE.Network.Enum;
using ACE.Network.Sequence;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageAnimation : GameMessage
    {
        public GameMessageAnimation(Entity.WorldObject animationTarget, Session session, AnimationActivity activity, 
            AnimationType type, AnimationFlags flags, StanceMode stance, Animations animation, float speed)
            : base(GameMessageOpcode.Animation, GameMessageGroup.Group0A)
        {
            Writer.WriteGuid(animationTarget.Guid);
            Writer.Write((ushort)session.Player.TotalLogins);
            Writer.Write(animationTarget.Sequences.GetNextSequence(Sequence.SequenceType.AnimationMessage));
            Writer.Write((ushort)1); // Index, needs more research, it changes sometimes, but not every packet
            Writer.Write((ushort)activity);
            Writer.Write((byte)type);
            Writer.Write((byte)flags);
            Writer.Write((ushort)stance);

            if(type == AnimationType.General)
            {
                uint generalFlags = 1 << 7;
                Writer.Write((uint)generalFlags);
                Writer.Write((ushort)animation);
                Writer.Write(animationTarget.Sequences.GetNextSequence(Sequence.SequenceType.Animation));
                Writer.Write(speed);
            }
        }
    }

    public enum AnimationActivity
    {
        Idle = 0,
        Active = 1
    }

    public enum AnimationType
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
        None        = 0x0,
        HasTarget   = 0x1, 
        Jumping     = 0x2 // Needs to be investigated
    }

    public enum StanceMode
    {
        UANoShieldAttack        = 0x3C,
        Standing                = 0x3D,
        MeleeNoShieldAttack     = 0x3E,
        BowAttack               = 0x3F,
        MeleeShieldAttack       = 0x40,
        Spellcasting            = 0x49
    }

    public enum Animations
    {
        Run = 7,
        FistJump = 76,
        ShakeFist = 121,
        Bow = 125,
        Logout1 = 286,
        Logout2 = 885
    }
}