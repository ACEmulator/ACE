using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

using ACE.Network.GameMessages;
using ACE.Network.Handlers;
using ACE.Network.Managers;

namespace ACE.Network
{
    public class NetworkSession
    {
        private readonly Object lockObject = new Object();

        private const int minimumTimeBetweenBundles = 5; // 5ms
        private const int timeBetweenTimeSync = 20000; // 20s
        private const int timeBetweenAck = 2000; // 2s

        private readonly Session session;

        private NetworkBundle currentBundle = new NetworkBundle();

        private DateTime nextResync = DateTime.UtcNow;
        private DateTime nextAck = DateTime.UtcNow;
        private DateTime nextSend = DateTime.UtcNow;
        private bool sendAck = false;
        private bool sendResync = false;
        private uint lastReceivedSequence = 0;

        private readonly Dictionary<uint /*seq*/, ServerPacket> cachedPackets = new Dictionary<uint /*seq*/, ServerPacket>();

        private readonly Queue<ServerPacket> packetQueue = new Queue<ServerPacket>();

        public readonly SessionConnectionData ConnectionData = new SessionConnectionData();

        public NetworkSession(Session session)
        {
            this.session = session;
        }

        public void EnqueueSend(params GameMessage[] messages)
        {
            lock (lockObject)
            {
                currentBundle.EncryptedChecksum = true;

                foreach (var message in messages)
                    currentBundle.Enqueue(message);
            }
        }

        public void EnqueueSend(params ServerPacket[] packets)
        {
            lock (lockObject)
            {
                foreach (var packet in packets)
                    packetQueue.Enqueue(packet);
            }
        }

        public void Update(double lastTick)
        {
            lock (lockObject)
            {
                ConnectionData.ServerTime += lastTick;

                if (sendResync && !currentBundle.TimeSync && DateTime.UtcNow > nextResync)
                {
                    currentBundle.TimeSync = true;
                    currentBundle.EncryptedChecksum = true;
                    nextResync = DateTime.UtcNow.AddMilliseconds(timeBetweenTimeSync);
                }

                if (sendAck && !currentBundle.SendAck && DateTime.UtcNow > nextAck)
                {
                    currentBundle.SendAck = true;
                    nextAck = DateTime.UtcNow.AddMilliseconds(timeBetweenAck);
                }

                if (currentBundle.NeedsSending && DateTime.UtcNow > nextSend)
                {
                    FlushBundle();
                }

                FlushPackets();
            }
        }

        public void HandlePacket(ClientPacket packet)
        {
            lock (lockObject)
            {
                lastReceivedSequence = packet.Header.Sequence;

                if (packet.Header.HasFlag(PacketHeaderFlags.EchoRequest))
                    FlagEcho(packet.HeaderOptional.ClientTime);

                if (packet.Header.HasFlag(PacketHeaderFlags.AckSequence))
                    AcknowledgeSequence(packet.HeaderOptional.Sequence);

                if (packet.Header.HasFlag(PacketHeaderFlags.TimeSynch))
                {
                    //Do something with this...
                    //Based on network traces these are not 1:1.  Server seems to send them every 20 seconds per port.
                    //Client seems to send them alternatingly every 2 or 4 seconds per port.
                    //We will send this at a 20 second time interval.  I don't know what to do with these when we receive them at this point.
                }

                if (packet.Header.HasFlag(PacketHeaderFlags.RequestRetransmit))
                {
                    foreach (uint sequence in packet.HeaderOptional.RetransmitData)
                        Retransmit(sequence);
                }

                if (packet.Header.HasFlag(PacketHeaderFlags.LoginRequest))
                {
                    AuthenticationHandler.HandleLoginRequest(packet, session);
                    return;
                }

                if (packet.Header.HasFlag(PacketHeaderFlags.ConnectResponse))
                {
                    sendResync = true;
                    AuthenticationHandler.HandleConnectResponse(packet, session);
                    return;
                }

                foreach (ClientPacketFragment fragment in packet.Fragments)
                    InboundMessageManager.HandleClientFragment(fragment, session);
            }
        }

        private void FlagEcho(float clientTime)
        {
            //Debug.Assert(clientTime == -1f, "Multiple EchoRequests before Flush, potential issue with network logic!");
            currentBundle.ClientTime = clientTime;
            currentBundle.EncryptedChecksum = true;
        }

        private void AcknowledgeSequence(uint sequence)
        {
            // TODO Sending Acks seems to cause some issues.  Needs further research.
            //if (!sendAck)
            //    sendAck = true;

            foreach (var key in cachedPackets.Keys.ToList())
            {
                if (key < sequence)
                    cachedPackets.Remove(key);
            }
        }

        private void Retransmit(uint sequence)
        {
            ServerPacket cachedPacket;

            if (cachedPackets.TryGetValue(sequence, out cachedPacket))
            {
                Console.WriteLine("Retransmit " + sequence);

                if (!cachedPacket.Header.HasFlag(PacketHeaderFlags.Retransmission))
                    cachedPacket.Header.Flags |= PacketHeaderFlags.Retransmission;

                SendPacketRaw(cachedPacket);
            }
        }

        private void FlushBundle()
        {
            if (currentBundle.NeedsSending)
            {
                var bundleToSend = currentBundle;
                currentBundle = new NetworkBundle();
                SendBundle(bundleToSend);
                nextSend = DateTime.UtcNow.AddMilliseconds(minimumTimeBetweenBundles);
            }
        }

        private void FlushPackets()
        {
            while (packetQueue.Count > 0)
            {
                ServerPacket packet = packetQueue.Dequeue();

                if (packet.Header.HasFlag(PacketHeaderFlags.EncryptedChecksum) && ConnectionData.PacketSequence < 2)
                    ConnectionData.PacketSequence = 2;

                packet.Header.Sequence = ConnectionData.PacketSequence++;
                packet.Header.Id = 0x0B; // This value is currently the hard coded Server ID. It can be something different...
                packet.Header.Table = 0x14;
                packet.Header.Time = (ushort)ConnectionData.ServerTime;

                if (packet.Header.Sequence >= 2u)
                    cachedPackets.Add(packet.Header.Sequence, packet);

                SendPacket(packet);
            }
        }

        private void SendPacket(ServerPacket packet)
        {
            if (packet.Header.HasFlag(PacketHeaderFlags.EncryptedChecksum))
            {
                uint issacXor = session.GetIssacValue(PacketDirection.Server);
                packet.IssacXor = issacXor;
            }

            SendPacketRaw(packet);
        }

        private void SendPacketRaw(ServerPacket packet)
        {
            Socket socket = SocketManager.GetSocket();
            byte[] payload = packet.GetPayload();
#if NETWORKDEBUG
            System.Net.IPEndPoint listenerEndpoint = (System.Net.IPEndPoint)socket.LocalEndPoint;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(String.Format("Sending Packet (Len: {0}) [{1}:{2}=>{3}:{4}]", payload.Length, listenerEndpoint.Address, listenerEndpoint.Port, session.EndPoint.Address, session.EndPoint.Port));
            sb.AppendLine(payload.BuildPacketString());
            Console.WriteLine(sb.ToString());
#endif
            socket.SendTo(payload, session.EndPoint);
        }

        /// <summary>
        /// This function handles turning a bundle of messages (representing all messages accrued in a timeslice),
        /// into 1 or more packets, combining multiple messages into one packet or spliting large message across
        /// several packets as needed.
        /// 
        /// 1 Create Packet
        /// 2 If first packet for this bundle, fill any optional headers
        /// 3 Append messages that will fit
        /// 4 If we have more messages, create additional packets.
        /// 5 If any packet is greater than the max packet size, split it across two fragments
        /// </summary>
        /// <param name="bundle"></param>
        private void SendBundle(NetworkBundle bundle)
        {
            bool firstPacket = true;

            MessageFragment carryOverMessage = null;

            while (firstPacket || carryOverMessage != null || bundle.Messages.Count > 0)
            {
                ServerPacket packet = new ServerPacket();
                PacketHeader packetHeader = packet.Header;

                if (bundle.EncryptedChecksum)
                    packetHeader.Flags |= PacketHeaderFlags.EncryptedChecksum;

                uint availableSpace = Packet.MaxPacketDataSize;

                if (firstPacket)
                {
                    firstPacket = false;

                    if (bundle.SendAck) //0x4000
                    {
                        packetHeader.Flags |= PacketHeaderFlags.AckSequence;
                        packet.BodyWriter.Write(lastReceivedSequence);
                    }

                    if (bundle.TimeSync) //0x1000000
                    {
                        packetHeader.Flags |= PacketHeaderFlags.TimeSynch;
                        packet.BodyWriter.Write(ConnectionData.ServerTime);
                    }

                    if (bundle.ClientTime != -1f) //0x4000000
                    {
                        packetHeader.Flags |= PacketHeaderFlags.EchoResponse;
                        packet.BodyWriter.Write(bundle.ClientTime);
                        packet.BodyWriter.Write((float)ConnectionData.ServerTime - bundle.ClientTime);
                    }

                    availableSpace -= (uint)packet.Data.Length;
                }

                if (carryOverMessage != null || bundle.Messages.Count > 0)
                {
                    packetHeader.Flags |= PacketHeaderFlags.BlobFragments;
                    int fragmentCount = 0;
                    while (carryOverMessage != null || bundle.Messages.Count > 0)
                    {
                        MessageFragment currentMessageFragment;

                        if (carryOverMessage != null) // If we have a carryOverMessage, use that
                        {
                            currentMessageFragment = carryOverMessage;
                            carryOverMessage = null;
                        }
                        else // If we don't have a carryOverMessage, go ahead and dequeue next message from the bundle
                        {
                            currentMessageFragment = new MessageFragment(bundle.Messages.Dequeue());
                        }

                        var currentGameMessage = currentMessageFragment.message;

                        availableSpace -= PacketFragmentHeader.HeaderSize; // Account for fragment header

                        // Compute amount of data to send based on the total length and current position
                        uint dataToSend = (uint)currentGameMessage.Data.Length - currentMessageFragment.position;

                        if (dataToSend > availableSpace) // Message is too large to fit in packet
                        {
                            carryOverMessage = currentMessageFragment; 
                            if (fragmentCount == 0) // If this is first message in packet, this is just a really large message, so proceed with splitting it
                                dataToSend = availableSpace;
                            else // Otherwise there are other messages already, so we'll break and come back and see if the message will fit
                                break;
                        }

                        if (currentMessageFragment.count == 0) // Compute number of fragments if we have not already
                        {
                            uint remainingData = (uint)currentGameMessage.Data.Length - dataToSend;
                            currentMessageFragment.count = (ushort)(Math.Ceiling((double)remainingData / PacketFragment.MaxFragmentDataSize) + 1);
                        }

                        // Set sequence, if new, pull next sequence from ConnectionData, if it is a carryOver, reuse that sequence
                        currentMessageFragment.sequence = currentMessageFragment.sequence == 0 ? ConnectionData.FragmentSequence++ : currentMessageFragment.sequence;

                        // Build ServerPacketFragment structure
                        ServerPacketFragment fragment = new ServerPacketFragment();
                        PacketFragmentHeader fragmentHeader = fragment.Header;
                        fragmentHeader.Sequence = currentMessageFragment.sequence;
                        fragmentHeader.Id = 0x80000000;
                        fragmentHeader.Count = currentMessageFragment.count;
                        fragmentHeader.Index = currentMessageFragment.index;
                        fragmentHeader.Group = (ushort)currentMessageFragment.message.Group;

                        // Read data starting at current position reading dataToSend bytes
                        currentGameMessage.Data.Seek(currentMessageFragment.position, SeekOrigin.Begin);
                        fragment.Content = new byte[dataToSend];
                        currentGameMessage.Data.Read(fragment.Content, 0, (int)dataToSend);

                        // Increment position and index
                        currentMessageFragment.position = currentMessageFragment.position + dataToSend;
                        currentMessageFragment.index++;

                        // Add fragment to packet
                        packet.AddFragment(fragment);
                        fragmentCount++;

                        // Deduct consumed space
                        availableSpace -= dataToSend;

                        // Smallest message I am aware of requires HeaderSize + 4 bytes, so if we have less space then that, go ahead and break
                        if (availableSpace <= PacketFragmentHeader.HeaderSize + 4)
                            break;
                    }
                }

                packetQueue.Enqueue(packet);
            }
        }

        private class MessageFragment
        {
            public GameMessage message { get; private set; }
            public uint position { get; set; }
            public uint sequence { get; set; }
            public ushort index { get; set; }
            public ushort count { get; set; }

            public MessageFragment(GameMessage message)
            {
                this.message = message;
                index = 0;
                count = 0;
                position = 0;
                sequence = 0;
            }
        }
    }
}
