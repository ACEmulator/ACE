using System;
using System.Numerics;

namespace ACE.Server.Physics.Collision
{
    [Flags]
    public enum ObjCollisionProfileFlags
    {
        Undefined = 0x0,
        Creature = 0x1,
        Player = 0x2,
        Attackable = 0x4,
        Missile = 0x8,
        Contact = 0x10,
        MyContact = 0x20,
        Door = 0x40,
        Cloaked = 0x80,
    };

    public class ObjCollisionProfile
    {
        public int ID;
        public Vector3 Velocity;
        public int wcid;
        public int ItemType;
        public ObjCollisionProfileFlags Flags;
    }
}
