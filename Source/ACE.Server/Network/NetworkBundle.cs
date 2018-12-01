using ACE.Server.Network.GameMessages;
using System.Collections.Generic;

namespace ACE.Server.Network
{

    internal class NetworkBundle
    {
        private bool propChanged;

        public bool NeedsSending => propChanged || messages.Count > 0;

        public bool HasMoreMessages => messages.Count > 0;

        private Queue<GameMessage> messages = new Queue<GameMessage>();

        private float clientTime = -1f;
        public float ClientTime
        {
            get => clientTime;
            set
            {
                clientTime = value;
                propChanged = true;
            }
        }

        private bool timeSync;
        public bool TimeSync
        {
            get => timeSync;
            set
            {
                timeSync = value;
                propChanged = true;
            }
        }

        private bool ackSeq;
        public bool SendAck
        {
            get => ackSeq;
            set
            {
                ackSeq = value;
                propChanged = true;
            }
        }

        public bool EncryptedChecksum { get; set; }

        public int CurrentSize { get; private set; }

        public void Enqueue(GameMessage message)
        {
            CurrentSize += (int)message.Data.Length;
            messages.Enqueue(message);
        }

        public GameMessage Dequeue()
        {
            return messages.Dequeue();
        }
    }
}
