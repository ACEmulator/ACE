using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network
{
    public class PackedDWORD
    {
        private uint rawValue = 0;

        private ushort maxPackedValue = 32767;

        public PackedDWORD(uint value)
        {
            rawValue = value;
        }

        public byte[] NetworkValue
        {
            get
            {
                if (rawValue < maxPackedValue)
                {
                    ushort networkValue = Convert.ToUInt16(rawValue);
                    return BitConverter.GetBytes(networkValue);
                }
                else
                {
                    uint crazyValue = (rawValue << 16) | (rawValue >> 16);
                    return BitConverter.GetBytes(crazyValue);
                }
            }
        }
    }
}
