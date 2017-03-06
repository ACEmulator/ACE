using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity
{
    public class ModelPallete
    {
        public uint Guid { get; }
        public byte Offset { get; }
        public byte Length { get; }

        public ModelPallete(uint guid, byte offset, byte length)
        {
            Guid = guid;
            Offset = offset;
            Length = length;
        }
    }
}
