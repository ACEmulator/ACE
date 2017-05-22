using ACE.Entity;
using ACE.Entity.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.LandManagers
{
    public abstract class LandMediator
    {
        public abstract void Register(WorldObject wo);
        public abstract void Broadcast(BroadcastEventArgs args, Quadrant quadrant);
        public abstract void Update(WorldObject wo);
        public abstract void UnRegister(WorldObject wo);
        public abstract WorldObject ReadOnlyClone(ObjectGuid objectguid);
    }
}
