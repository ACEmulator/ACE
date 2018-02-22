using System.Collections.Generic;
using System.Numerics;
using ACE.Entity;

namespace ACE.Server.Physics.Animation
{
    public class MotionData
    {
        public char NumAnims;
        public List<AnimData> Anims;
        public Vector3 Velocity;
        public Vector3 Omega;
        public char Bitfield;
    }
}
