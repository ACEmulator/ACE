using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network.Sequence
{
    public class UShortSequence : ISequence
    {
        private ushort value;

        public UShortSequence(ushort startingValue = 0)
        {
            value = startingValue;
        }

        public byte[] CurrentValue
        {
            get
            {
                if (value == 0)
                    return BitConverter.GetBytes(UInt16.MaxValue);
                return BitConverter.GetBytes(value - 1);
            }
        }

        public byte[] NextValue
        {
            get
            {
                if (value == UInt16.MaxValue)
                {
                    value = 0;
                    return BitConverter.GetBytes(UInt16.MaxValue);
                }
                return BitConverter.GetBytes(value++);
            }
        }
    }
}
