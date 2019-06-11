using ACE.Entity.Enum;

namespace ACE.Server.Entity
{
    public class EmoteSetCriteria
    {
        public EmoteCategory Category;
        public uint? WCID;
        public MotionStance? Style;         // only used for Heartbeat
        public MotionCommand? Substyle;     // only used for Heartbeat
        public string Quest;
        public VendorType? VendorType;
        public float? MinHealth;            // only used for WoundedTaunt
        public float? MaxHealth;            // only used for WoundedTaunt
    }
}
