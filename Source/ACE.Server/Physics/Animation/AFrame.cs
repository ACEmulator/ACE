using System.Numerics;

namespace ACE.Server.Physics.Animation
{
    public class AFrame
    {
        public Vector3 Origin;
        public Quaternion Orientation;

        public static AFrame Combine(AFrame a, AFrame b)
        {
            return null;
        }

        public AFrame Combine(AFrame a, AFrame b, Vector3 scale)
        {
            return null;
        }

        public bool IsValid()
        {
            return true;
        }

        public bool IsValidExceptForHeading()
        {
            return true;
        }

        public Vector3 LocalToGlobal(Vector3 v)
        {
            return v;
        }

        public Vector3 LocalToGlobalVec(Vector3 v)
        {
            return v;
        }

        public void set_heading(float degrees)
        {

        }

        public void set_vector_heading(Vector3 heading)
        {

        }

        public double get_heading()
        {
            return -1;
        }

        public void Rotate(Vector3 w)
        {

        }
    }
}
