using ACE.Entity.Enum;

namespace ACE.Server.Network.Motion
{
    public struct MotionItem
    {
        public MotionCommand Motion { get; set; }
        public float Speed { get; set; }

        public MotionItem(MotionCommand animation)
        {
            Motion = animation;
            Speed = 1.0f;
        }

        public MotionItem(MotionCommand animation, float speed)
        {
            Motion = animation;
            Speed = speed;
        }
    }
}
