using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network
{
    public enum PacketDirection
    {
        None,
        Client, // C->S
        Server  // S->C
    }
}
