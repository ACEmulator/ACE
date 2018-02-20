using System.Collections.Generic;
using System.Numerics;

namespace ACE.Server.Physics.Animation
{
    public class Sequence
    {
        public List<AnimSequenceNode> AnimList;
        public AnimSequenceNode FirstCyclic;
        public Vector3 Velocity;
        public Vector3 Omega;
        public PhysicsObj HookObj;
        public double FrameNumber;
        public AnimSequenceNode CurrAnim;
        public AnimFrame PlacementFrame;
        public int PlacementFrameId;
        public int IsTrivial;
    }
}
