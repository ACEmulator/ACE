using System.Diagnostics;

namespace ACE.Entity
{
    public enum GuidType
    {
        None   = 0,
        Player = 1,
    }

    public class ObjectGuid
    {
        public uint Full { get; }

        public ObjectGuid(uint low, GuidType high)
        {
            Debug.Assert(low <= 0xFFFFFFu);
            Full = low | ((uint)high << 24);
        }

        public uint GetLow() { return Full & 0xFFFFFF; }
        public GuidType GetHigh() { return (GuidType)(Full >> 24); }

        public bool IsPlayer() { return GetHigh() == GuidType.Player; }
    }
}
