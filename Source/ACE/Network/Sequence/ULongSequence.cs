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

        public byte[] NextValue
        {
            get
            {
                return BitConverter.GetBytes(value++);
            }
        }
    }
}
