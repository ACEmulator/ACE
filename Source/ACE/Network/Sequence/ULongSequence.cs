using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network.Sequence
{
    public class ULongSequence : ISequence
    {
        private ulong value;

        public ULongSequence(ulong startingValue = 0)
        {
            value = startingValue;
        }

        public byte[] CurrentValue
        {
            get
            {
                if (value == 0)
                    return BitConverter.GetBytes(UInt64.MaxValue);
                return BitConverter.GetBytes(value - 1);
            }
        }

        public byte[] NextValue
        {
            get
            {
                if (value == UInt64.MaxValue)
                {
                    value = 0;
                    return BitConverter.GetBytes(UInt64.MaxValue);
                }
                return BitConverter.GetBytes(value++);
            }
        }
    }
}
