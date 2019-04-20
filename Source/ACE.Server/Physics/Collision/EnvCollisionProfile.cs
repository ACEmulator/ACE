using System.Numerics;

namespace ACE.Server.Physics.Collision
{
    public enum EnvCollisionProfileFlags
    {
        Undefined = 0x0,
        MyContact = 0x1,
    };

    public class EnvCollisionProfile
    {
        public Vector3 Velocity;
        public EnvCollisionProfileFlags Flags;

        public void SetMeInContact(bool hasContact)
        {
            if (hasContact)
                Flags = EnvCollisionProfileFlags.MyContact;
            else
                Flags = EnvCollisionProfileFlags.Undefined;
        }
    }
}
