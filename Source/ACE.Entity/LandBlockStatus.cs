using ACE.Entity.Enum;

namespace ACE.Entity
{
    public class LandBlockStatus
    {
        public int Playercount;
        public LandBlockStatusFlag LandBlockStatusFlag = new LandBlockStatusFlag();
        public LandblockId LandBlockId = new LandblockId();
    }
}
