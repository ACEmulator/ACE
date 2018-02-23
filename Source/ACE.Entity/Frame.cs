using System.Numerics;

namespace ACE.Entity
{
    public class Frame
    {
        public Vector3 Origin { get; set; }
        public Quaternion Orientation { get; set; }

        public Frame()
        {
            Origin = Vector3.Zero;
            Orientation = Quaternion.Identity;
        }

        public void Combine(Frame a, Frame b, Vector3 scale)
        {

        }

        public Vector3 LocalToGlobalVec(Vector3 v)
        {
            return v;
        }
    }
}
