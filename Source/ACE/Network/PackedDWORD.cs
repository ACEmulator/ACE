using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network
{
    public struct PackedDWORD
    {
        private uint rawValue;
        
        public PackedDWORD(uint value)
        {
            rawValue = value;
        }
        
        public static implicit operator PackedDWORD(uint value)
        {
            return new PackedDWORD(value);
        }

        public byte[] NetworkValue
        {
            get
            {
                if (rawValue <= 32767)
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
