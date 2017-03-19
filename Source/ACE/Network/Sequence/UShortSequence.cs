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

        public byte[] NextValue
        {
            get
            {
                return BitConverter.GetBytes(++value);
            }
        }
    }
}
