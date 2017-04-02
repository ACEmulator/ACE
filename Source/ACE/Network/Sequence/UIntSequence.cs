using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network.Sequence
{
    public class UIntSequence : ISequence
    {
        private uint value;

        public UIntSequence(uint startingValue = 0)
        {
            value = startingValue;
        }

        public byte[] CurrentValue
        {
            get
            {
                if (value == 0)
                    return BitConverter.GetBytes(UInt32.MaxValue);
                return BitConverter.GetBytes(value - 1);
            }
        }

        public byte[] NextValue
        {
            get
            {
                if (value == UInt32.MaxValue)
                {
                    value = 0;
                    return BitConverter.GetBytes(UInt32.MaxValue);
                }
                return BitConverter.GetBytes(value++);
            }
        }
    }
}
