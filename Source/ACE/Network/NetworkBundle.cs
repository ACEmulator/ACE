using System.Collections.Generic;

using ACE.Network.GameMessages;

namespace ACE.Network
{
    public class NetworkBundle
    {
        private bool propChanged = false;

        public bool NeedsSending
        {
            get
            {
                return propChanged || Messages.Count > 0;
            }
        }

        public Queue<GameMessage> Messages;

        private float clientTime;
        public float ClientTime
        {
            get { return clientTime; }
            set { clientTime = value; propChanged = true; }
        }

        private bool timeSync;
        public bool TimeSync
        {
            get { return timeSync; }
            set { timeSync = value; propChanged = true; }
        }

        private bool ackSeq;
        public bool SendAck
        {
            get { return ackSeq; }
            set { ackSeq = value; propChanged = true; }
        }

        public bool encryptedChecksum { get; set; }

        public NetworkBundle()
        {
            Messages = new Queue<GameMessage>();
            clientTime = -1f;
            timeSync = false;
            ackSeq = false;
            encryptedChecksum = false;
        }
    }
}
