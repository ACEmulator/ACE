using System;
using System.Collections.Generic;
using ACE.Server.Physics.Common;

namespace ACE.Server.Physics
{
    [Flags]
    public enum PhysicsState
    {
        Static = 0x1,
        Unused1 = 0x2,
        Ethereal = 0x4,
        ReportCollisions = 0x8,
        IgnoreCollisions = 0x10,
        NoDraw = 0x20,
        Missile = 0x40,
        Pushable = 0x80,
        AlignPath = 0x100,
        PathClipped = 0x200,
        Gravity = 0x400,
        LightingOn = 0x800,
        ParticleEmitter = 0x1000,
        Unused2 = 0x2000,
        Hidden = 0x4000,
        ScriptedCollision = 0x8000,
        HasPhysicsBSP = 0x10000,
        Inelastic = 0x20000,
        HasDefaultAnim = 0x40000,
        HasDefaultScript = 0x80000,
        Cloaked = 0x100000,
        ReportCollisionsAsEnvironment = 0x200000,
        EdgeSlide = 0x400000,
        Sledding = 0x800000,
        Frozen = 0x1000000
    };

    [Flags]
    public enum TransientStateFlags
    {
        Contact = 0x1,
        OnWalkable = 0x2,
        Sliding = 0x4,
        WaterContact = 0x8,
        StationaryFall = 0x10,
        StationaryStop = 0x20,
        StationaryStuck = 0x40,
        Active = 0x80,
        CheckEthereal = 0x100
    };

    public enum PhysicsTimeStamp
    {
        Position = 0x0,
        Movement = 0x1,
        State = 0x2,
        Vector = 0x3,
        Teleport = 0x4,
        ServerControlledMove = 0x5,
        ForcePosition = 0x6,
        ObjDesc = 0x7,
        Instance = 0x8,
        NumPhysics = 0x9
    };

    public class Physics
    {
        public ObjectMaint ObjMaint;
        public SmartBox SmartBox;
        public PhysicsObj Player;
        public List<PhysicsObj> Iter;

        public static void AddStaticAnimatingObject(PhysicsObj obj)
        {

        }

        public static void RemoveStaticAnimatingObject(PhysicsObj obj)
        {

        }
    }
}
