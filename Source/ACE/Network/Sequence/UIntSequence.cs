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

        public byte[] NextValue
        {
            get
            {
                return BitConverter.GetBytes(value++);
            }
        }
    }
}
