using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity
{
    public interface ITickable
    {
        void Tick(double tickTime, LazyBroadcastList broadcaster);
    }
}
