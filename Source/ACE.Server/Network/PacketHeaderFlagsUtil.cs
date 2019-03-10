using System.Collections.Generic;
using System.Linq;

namespace ACE.Server.Network
{
    public static class PacketHeaderFlagsUtil
    {
        public static string UnfoldFlags(PacketHeaderFlags flags)
        {
            List<string> result = new List<string>();
            foreach (PacketHeaderFlags r in System.Enum.GetValues(typeof(PacketHeaderFlags)))
            {
                if ((flags & r) != 0)
                {
                    result.Add(r.ToString());
                }
            }
            return result.DefaultIfEmpty().Aggregate((a, b) => a + " | " + b);
        }
    }
}
