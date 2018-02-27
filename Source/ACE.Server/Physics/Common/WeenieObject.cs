using ACE.Server.Physics.Combat;

namespace ACE.Server.Physics.Common
{
    public class WeenieObject
    {
        public double UpdateTime;

        public bool IsCorpse()
        {
            return false;
        }

        public bool IsStorage()
        {
            return false;
        }

        public void DoCollision(AtkCollisionProfile prof)
        {

        }

        public void DoCollisionEnd(int objectID)
        {

        }
    }
}
