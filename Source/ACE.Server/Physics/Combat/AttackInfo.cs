using System.Collections.Generic;

namespace ACE.Server.Physics.Combat
{
    public class AttackInfo
    {
        public int AttackID;
        public int PartIndex;
        public float AttackRadius;
        public bool WaitingForCells;
        public int NumObjects;
        public List<AtkObjInfo> ObjectList;

        public void AddObject(uint objectID, int hitLocation)
        {

        }
    }
}
