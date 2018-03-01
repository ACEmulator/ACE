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

        public void InterpolateRotation(AFrame from, AFrame to, float t)
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

        public Vector3 LocalToGlobal(Vector3 v)
        {
            return v;
        }

        public Vector3 LocalToGlobalVec(Vector3 v)
        {
            return v;
        }

        public void Rotate(Vector3 rotation)
        {

        }

        public void Rotate(Quaternion rotation)
        {

        }

        public Vector3 get_vector_heading()
        {
            return Vector3.Zero;
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
    }
}
