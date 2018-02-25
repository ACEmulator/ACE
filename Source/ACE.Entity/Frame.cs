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
    }
}
