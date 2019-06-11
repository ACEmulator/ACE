using System;
using System.Numerics;
using ACE.Entity.Enum;

namespace ACE.Server.Physics.Collision
{
    [Flags]
    public enum ObjCollisionProfileFlags
    {
        Undefined  = 0x0,
        Creature   = 0x1,
        Player     = 0x2,
        Attackable = 0x4,
        Missile    = 0x8,
        Contact    = 0x10,
        MyContact  = 0x20,
        Door       = 0x40,
        Cloaked    = 0x80,
    };

    public class ObjCollisionProfile
    {
        public uint ID;
        public Vector3 Velocity;
        public uint WCID;
        public ItemType ItemType;
        public ObjCollisionProfileFlags Flags;

        public ObjCollisionProfile() { }

        public ObjCollisionProfile(uint id, Vector3 velocity, bool missile, bool contact, bool myContact)
        {
            ID = id;
            Velocity = velocity;

            if (missile)
                Flags |= ObjCollisionProfileFlags.Missile;
            if (contact)
                Flags |= ObjCollisionProfileFlags.Contact;
            if (myContact)
                Flags |= ObjCollisionProfileFlags.MyContact;
        }
    }
}
