using System.Collections.Generic;
using ACE.Server.Physics.Animation;

namespace ACE.Server.Physics.Combat
{
    public class AttackInfo
    {
        public int AttackID;
        public int PartIndex;
        public float AttackRadius;
        public int WaitingForCells;
        public int NumObjects;
        public List<ObjectInfo> ObjectList;
    }
}
