using ACE.Entity.Enum;
using ACE.Network.Enum;
using ACE.Network.Motion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity.Events
{
    public class BroadcastEventArgs
    {
        private BroadcastEventArgs()
        {
        }

        public static BroadcastEventArgs CreateAction(BroadcastAction actionType, WorldObject sender)
        {
            return new BroadcastEventArgs()
            {
                ActionType = actionType,
                Sender = sender
            };
        }

        public BroadcastAction ActionType { get; private set; }

        public WorldObject Sender { get; private set; }
    }
}
