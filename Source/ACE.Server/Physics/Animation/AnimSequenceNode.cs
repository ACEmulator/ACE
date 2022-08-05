using System;

using ACE.DatLoader.Entity;
using ACE.Server.Physics.Common;

namespace ACE.Server.Physics.Animation
{
    public class AnimSequenceNode
    {
        public Animation Anim;
        public float Framerate;
        public int LowFrame;
        public int HighFrame;

        public AnimSequenceNode()
        {
            Framerate = 30.0f;
            LowFrame = 0;
            HighFrame = -1;
        }

        public AnimSequenceNode(AnimData animData)
        {
            Framerate = animData.Framerate;
            LowFrame = animData.LowFrame;
            HighFrame = animData.HighFrame;

            set_animation_id(animData.AnimID);
        }

        public float get_ending_frame()
        {
            if (Framerate >= 0.0f)
                return HighFrame + 1 - PhysicsGlobals.EPSILON;
            else
                return LowFrame;
        }

        public int get_high_frame()
        {
            return HighFrame;
        }

        public int get_low_frame()
        {
            return LowFrame;
        }

        public AnimationFrame get_part_frame(int frameIdx)
        {
            if (Anim == null) return null;
            if (frameIdx < 0 || frameIdx >= Anim.NumFrames)
                return null;

            return Anim.PartFrames[frameIdx];
        }

        public AFrame get_pos_frame(int frameIdx)
        {
            if (Anim == null) return new AFrame();
            if (frameIdx < 0 || frameIdx >= Anim.PosFrames.Count)
                return new AFrame();

            return Anim.PosFrames[frameIdx];
        }

        public AFrame get_pos_frame(float frameIdx)
        {
            return get_pos_frame((int)Math.Floor(frameIdx));
        }

        public float get_starting_frame()
        {
            if (Framerate >= 0.0f)
                return LowFrame;
            else
                return HighFrame + 1 - PhysicsGlobals.EPSILON;
        }

        public bool has_anim(int appraisalProfile = 0)
        {
            return Anim != null;
        }

        public void multiply_framerate(float multiplier)
        {
            if (multiplier < 0.0f)
            {
                var swap = LowFrame;
                LowFrame = HighFrame;
                HighFrame = swap;
            }
            Framerate *= multiplier;
        }

        public void set_animation_id(uint animID)
        {
            var anim = DBObj.GetAnimation(animID);
            Anim = new Animation(anim);
            if (Anim == null) return;

            if (HighFrame < 0)
                HighFrame = (int)(Anim.NumFrames - 1);

            if (LowFrame >= Anim.NumFrames)
                LowFrame = (int)(Anim.NumFrames - 1);

            if (HighFrame >= Anim.NumFrames)
                HighFrame = (int)(Anim.NumFrames - 1);

            if (LowFrame > HighFrame)
                HighFrame = LowFrame;
        }
    }
}
