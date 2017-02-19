using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.Network.GameMessages;
using ACE.Network.Managers;

namespace ACE.Network
{
    public class NetworkBundle
    {
        public Session sender { get; }
        public ConnectionType connectionType { get; }
        public Queue<GameMessage> messages { get; }
        public float clientTime { get; set; }
        public bool timeSync { get; set; }
        public bool ackSeq { get; set; }

        public NetworkBundle(Session session, ConnectionType connType)
        {
            sender = session;
            connectionType = connType;
            messages = new Queue<GameMessage>();
            clientTime = -1f;
            timeSync = false;
            ackSeq = false;
        }
    }
}
