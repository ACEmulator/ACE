using System.Numerics;

namespace ACE.Server.Physics.Animation
{
    public enum FrameInitializationEnum
    {
        FRAME_NO_INITIALIZATION = 0x0
    };

    public class Frame
    {
        public Quaternion Orientation;
        public Vector3 Origin;
    }
}
