using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using ACE.Network.GameMessages;
using ACE.Network.Managers;

namespace ACE.Network
{
    public class NetworkBuffer
    {
        public ConnectionType connType;
        public Session session;
        private NetworkBundle currentBundle;
        private DateTime lastResync;
        private DateTime lastAck;

        public NetworkBuffer(Session session, ConnectionType connType)
        {
            this.session = session;
            this.connType = connType;
            currentBundle = new NetworkBundle(session, connType);
            lastResync = DateTime.Now;
            lastAck = DateTime.Now;
        }

        public void Queue(GameMessage message)
        {
            currentBundle.messages.Enqueue(message);
        }

        public void Flush()
        {
            var bundleToSend = currentBundle;
            currentBundle = new NetworkBundle(session, connType);
            if (DateTime.Now > lastResync.AddSeconds(20))
            {
                bundleToSend.timeSync = true;
                lastResync = DateTime.Now;
            }
            if (DateTime.Now > lastResync.AddSeconds(2))
            {
                bundleToSend.ackSeq = true;
                lastAck = DateTime.Now;
            }
            
            //NetworkManager.SendWorldMessages(session, messagesToSend);
        }

        public void SetEcho(float clientTime)
        {
            Debug.Assert(clientTime == -1f, "Multiple EchoRequests before Flush, potential issue with network logic!");
            currentBundle.clientTime = clientTime;
        }
    }
}
