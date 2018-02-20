using ACE.Entity.Enum;
using ACE.Server.Physics.Animation;
using ACE.Server.Physics.Sound;

namespace ACE.Server.Physics
{
    public class PhysObjProfile
    {
        public Setup Setup;
        public MotionTable MotionTable;
        public SoundTable SoundTable;
        public int PhysicsEffectTable;
        public int PhysicsBitmask;
        public int Placement;
        public double Scale;
        public double Friction;
        public double Elasticity;
        public double Translucency;
        public PlayScript DefaultScript;
        public double DefaultScriptIntensity;
    }
}
