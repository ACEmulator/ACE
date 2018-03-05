using System.Collections.Generic;
using System.Numerics;
using ACE.Entity;

namespace ACE.Server.Physics.Animation
{
    public class Sequence
    {
        public int ID;
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

        public Sequence() { }

        public Sequence(bool id)
        {
            ID = id ? 1 : 0;
        }

        public Sequence(int id)
        {
            ID = id;
        }

        public Sequence(Frame frame)
        {

        }

        public Sequence(List<PhysicsPart> parts)
        {

        }

        public void Clear()
        {

        }

        public void CombinePhysics(Vector3 v, Vector3 o)
        {

        }

        public AnimFrame GetCurrAnimFrame()
        {
            return null;
        }

        public int GetCurrFrameNumber()
        {
            return -1;
        }

        public bool HasAnims()
        {
            return false;
        }

        public void SetObject(PhysicsObj physObj)
        {

        }

        public void SetOmega(Vector3 o)
        {

        }

        public void SetPlacementFrame(AnimFrame frame, int id)
        {

        }

        public void SetVelocity(Vector3 v)
        {

        }

        public void Update(double quantum, AFrame offsetFrame)
        {

        }

        public void advance_to_next_animation(double quantum, AnimSequenceNode currAnim, double frameNum, Frame retval)
        {

        }

        public void append_animation(AnimData animData)
        {

        }

        public void apply_physics(Frame frame, double quantum, double sign)
        {

        }

        public void apricot()
        {

        }

        public void clear_animations()
        {

        }

        public void clear_physics()
        {

        }

        public void execute_hooks(AnimFrame animFrame, int dir)
        {

        }

        public void multiply_cyclic_animation_framerate(float multipler)
        {

        }

        public void remove_all_link_animations()
        {

        }

        public void remove_cyclic_anims()
        {

        }

        public void remove_link_animations(int n)
        {

        }

        public void subtract_physics(Vector3 velocity, Vector3 omega)
        {
        }

        public void update_internal(double quantum, AnimSequenceNode currAnim, double frameNum, Frame retVal)
        {

        }
    }
}
