using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity
{
    public class Model
    {
        public byte Index { get; } //index of model

        public uint Guid { get; }  //- 0x01000000

        public Model(byte index, uint guid)
        {
            Index = index;
            Guid = guid;
        }
    }
}
