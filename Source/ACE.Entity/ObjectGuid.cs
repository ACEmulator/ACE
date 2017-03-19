using System.Diagnostics;

namespace ACE.Entity
{
    public enum GuidType
    {
        None   = 0,
        Player = 0x50,
    }

    public struct ObjectGuid
    {
        public uint Full { get; }
        public uint Low => Full & 0xFFFFFF;
        public GuidType High => (GuidType)(Full >> 24);

        public ObjectGuid(uint full) { Full = full; }

        public ObjectGuid(uint low, GuidType high)
        {
            //Debug.Assert(low <= 0xFFFFFFu);
            Full = low | ((uint)high << 24);
        }

        public bool IsPlayer() { return High == GuidType.Player; }

        public static bool operator ==(ObjectGuid g1, ObjectGuid g2)
        {
            return g1.Full == g2.Full;
        }

        public static bool operator !=(ObjectGuid g1, ObjectGuid g2)
        {
            return g1.Full != g2.Full;
        }
    }
}
