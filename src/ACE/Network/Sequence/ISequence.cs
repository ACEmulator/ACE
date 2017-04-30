using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network.Sequence
{
    public interface ISequence
    {
        byte[] NextValue { get; }
        byte[] CurrentValue { get; }
    }
}
