using System.Numerics;

namespace ACE.Server.Physics.Animation
{
    public class AFrame
    {
        public Vector3 Origin;
        public Quaternion Orientation;

        public void Combine(AFrame a, AFrame b, Vector3 scale)
        {

        }

        public bool IsValid()
        {
            return true;
        }

        public bool IsValidExceptForHeading()
        {
            return true;
        }

        public Vector3 LocalToGlobalVec(Vector3 v)
        {
            return v;
        }

        public void set_heading(float degrees)
        {

        }
    }
}
