using ACE.Entity;

namespace ACE.Server.Physics.Animation
{
    public class AnimSequenceNode
    {
        public Animation Anim;
        public float Framerate;
        public int LowFrame;
        public int HighFrame;

        public AnimSequenceNode() { }

        public AnimSequenceNode(AnimData animData)
        {

        }

        public Animation GetNext()
        {
            return null;
        }

        public Animation GetPev()
        {
            return null;
        }

        public double get_ending_frame()
        {
            return -1;
        }

        public int get_high_frame()
        {
            return -1;
        }

        public AnimFrame get_part_frame(int frameIdx)
        {
            return null;
        }


        public AFrame get_pos_frame(double frameNumber)
        {
            return null;
        }

        public AFrame get_pos_frame(int frameIdx)
        {
            return null;
        }

        public double get_starting_frame()
        {
            return -1;
        }

        public bool has_anim(int appraisalProfile)
        {
            return false;
        }

        public void multiply_framerate(float multiplier)
        {

        }

        public void set_animation_id(int animID)
        {

        }
    }
}
