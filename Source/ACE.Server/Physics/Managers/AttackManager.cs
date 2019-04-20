using System.Collections.Generic;

namespace ACE.Server.Physics.Combat
{
    public class AttackManager
    {
        public float AttackRadius;
        public int CurrentAttack;
        public HashSet<AttackInfo> PendingAttacks;

        public AttackInfo NewAttack(int partIdx)
        {
            return null;
        }

        public void AttackDone(AttackInfo attackInfo)
        {

        }
    }
}
