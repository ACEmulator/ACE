using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using ACE.Network.GameMessages;
using ACE.Network.Managers;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Sockets;

namespace ACE.Network
{
    public class NetworkBuffer
    {
        private const int minimumTimeBetweenPackets = 5; // 5ms
        private const int timeBetweenTimeSync = 20000; // 20s
        private const int timeBetweenAck = 2000; // 2s

        private ConnectionType connectionType;
        private Session session;
        private NetworkBundle currentBundle;
        private DateTime nextResync;
        private DateTime nextAck;
        private DateTime nextSend;
        private bool sendAck = false;
        private bool sendResync = false;

        public uint LastReceivedSequence { get; set; } = 0;
        private ConcurrentDictionary<uint /*seq*/, ServerPacket2> CachedPackets { get; } = new ConcurrentDictionary<uint, ServerPacket2>();

        public NetworkBuffer(Session session, ConnectionType connType)
        {
            this.session = session;
            this.connectionType = connType;
            currentBundle = new NetworkBundle();
            nextSend = DateTime.Now.AddSeconds(-2);
            nextResync = DateTime.Now;
            nextAck = DateTime.Now;
        }

        public void Enqueue(GameMessage message)
        {
            currentBundle.encryptedChecksum = true;
            currentBundle.Messages.Enqueue(message);
        }

        public void FlagEcho(float clientTime)
        {
            //Debug.Assert(clientTime == -1f, "Multiple EchoRequests before Flush, potential issue with network logic!");
            currentBundle.ClientTime = clientTime;
            currentBundle.encryptedChecksum = true;
        }

        public void AcknowledgeSequence(uint sequence)
        {
            if (!sendAck)
                sendAck = true;
            var removalList = CachedPackets.Where(x => x.Key < sequence);
            foreach(var item in removalList)
            {
                ServerPacket2 removedPacket;
                CachedPackets.TryRemove(item.Key, out removedPacket);
            }
        }

        public void SetTimers()
        {
            nextResync = DateTime.Now;
            nextAck = DateTime.Now.AddMilliseconds(timeBetweenAck);
        }

        public void StartResync()
        {
            sendResync = true;
        }

        public void Update()
        {
            if (sendResync && !currentBundle.TimeSync && DateTime.Now > nextResync)
            {
                currentBundle.TimeSync = true;
                currentBundle.encryptedChecksum = true;
                nextResync = DateTime.Now.AddMilliseconds(timeBetweenTimeSync);
            }
            if (sendAck && !currentBundle.SendAck && DateTime.Now > nextAck)
            {
                currentBundle.SendAck = true;
                nextAck = DateTime.Now.AddMilliseconds(timeBetweenAck);
            }
            if (currentBundle.NeedsSending && DateTime.Now > nextSend)
            {
                var bundleToSend = currentBundle;
                currentBundle = new NetworkBundle();
                SendBundle(bundleToSend);
                nextSend = DateTime.Now.AddMilliseconds(minimumTimeBetweenPackets);
            }
        }

        public void Retransmit(uint sequence)
        {
            ServerPacket2 cachedPacket;
            if (CachedPackets.TryGetValue(sequence, out cachedPacket))
            {
                Console.WriteLine("Retransmit " + sequence);
                if (!cachedPacket.Header.HasFlag(PacketHeaderFlags.Retransmission))
                {
                    cachedPacket.Header.Flags |= PacketHeaderFlags.Retransmission;
                }

                SendPacket(cachedPacket);
            }
        }

        private void SendPacket(ServerPacket2 packet) { NetworkManager.GetSocket(this.connectionType).SendTo(packet.GetPayload(), session.EndPoint); }

        //Create Packet
        //If first packet for this bundle, fill any optional headers
        //Append fragments that will fit
        //--Check each fragment to see if it is a partial fit
        //If we have a carryOverFragment or more messages, create additional packets.

        private void SendBundle(NetworkBundle bundle)
        {
            Socket socket = NetworkManager.GetSocket(this.connectionType);
            var connectionData = (connectionType == ConnectionType.Login ? session.LoginConnection : session.WorldConnection);
            bool firstPacket = true;

            MessageFragment carryOverMessage = null;

            while (firstPacket || carryOverMessage != null || bundle.Messages.Count > 0)
            {
                uint issacXor = (bundle.encryptedChecksum) ? session.GetIssacValue(PacketDirection.Server, connectionType) : 0u;
                ServerPacket2 packet = new ServerPacket2(issacXor);
                PacketHeader packetHeader = packet.Header;
                if (bundle.encryptedChecksum)
                    packetHeader.Flags |= PacketHeaderFlags.EncryptedChecksum;

                uint availableSpace = Packet.MaxPacketDataSize;

                if (firstPacket)
                {
                    firstPacket = false;
                    using (var bodyStream = new MemoryStream())
                    {
                        using (var bodyWriter = new BinaryWriter(bodyStream))
                        {

                            if (bundle.SendAck) //0x4000
                            {
                                packetHeader.Flags |= PacketHeaderFlags.AckSequence;
                                bodyWriter.Write(LastReceivedSequence);
                            }
                            if (bundle.TimeSync) //0x1000000
                            {
                                packetHeader.Flags |= PacketHeaderFlags.TimeSynch;
                                bodyWriter.Write(connectionData.ServerTime);
                            }
                            if (bundle.ClientTime != -1f) //0x4000000
                            {
                                packetHeader.Flags |= PacketHeaderFlags.EchoResponse;
                                bodyWriter.Write(bundle.ClientTime);
                                bodyWriter.Write((float)connectionData.ServerTime - bundle.ClientTime);
                            }
                            //TODO body content and checksum.
                            bodyWriter.Flush();
                            packet.SetBody(bodyStream.ToArray());
                        }
                    }
                }

                if (carryOverMessage != null || bundle.Messages.Count > 0)
                {
                    packetHeader.Flags |= PacketHeaderFlags.BlobFragments;

                    while (carryOverMessage != null || bundle.Messages.Count > 0)
                    {
                        if (availableSpace <= PacketFragmentHeader.HeaderSize)
                            break;

                        MessageFragment currentMessageFragment;

                        if (carryOverMessage != null)
                        {
                            currentMessageFragment = carryOverMessage;
                            carryOverMessage = null;
                            currentMessageFragment.index++;
                        }
                        else
                        {
                            currentMessageFragment = new MessageFragment(bundle.Messages.Dequeue());
                        }

                        var currentGameMessage = currentMessageFragment.message;

                        ServerPacketFragment2 fragment = new ServerPacketFragment2();
                        PacketFragmentHeader fragmentHeader = fragment.Header;
                        availableSpace -= PacketFragmentHeader.HeaderSize;

                        currentMessageFragment.sequence = currentMessageFragment.sequence == 0 ? connectionData.FragmentSequence++ : currentMessageFragment.sequence;

                        uint dataToSend = (uint)currentGameMessage.Data.Length - currentMessageFragment.position;
                        if (dataToSend > availableSpace)
                        {
                            dataToSend = availableSpace;
                            carryOverMessage = currentMessageFragment;
                        }

                        if (currentMessageFragment.count == 0)
                        {
                            uint remainingData = (uint)currentGameMessage.Data.Length - dataToSend;
                            currentMessageFragment.count = (ushort)(Math.Ceiling((double)remainingData / PacketFragment.MaxFragmentDataSize) + 1);
                        }

                        fragmentHeader.Sequence = currentMessageFragment.sequence;
                        fragmentHeader.Id = 0x80000000;
                        fragmentHeader.Count = currentMessageFragment.count;
                        fragmentHeader.Index = currentMessageFragment.index;
                        fragmentHeader.Group = 9;

                        fragment.Body = currentGameMessage.Data.ToArray();

                        currentMessageFragment.position = currentMessageFragment.position + dataToSend;

                        packet.AddFragment(fragment);
                    }
                }

                if (packetHeader.HasFlag(PacketHeaderFlags.EncryptedChecksum) && connectionData.PacketSequence < 2)
                    connectionData.PacketSequence = 2;

                packetHeader.Sequence = connectionData.PacketSequence++;
                packetHeader.Id = (ushort)(connectionType == ConnectionType.Login ? 0x0B : 0x18);
                packetHeader.Table = 0x14;
                packetHeader.Time = (ushort)connectionData.ServerTime;

                if (connectionType == ConnectionType.World && packetHeader.Sequence >= 2u)
                    CachedPackets.TryAdd(packet.Header.Sequence, packet);
                byte[] payload = packet.GetPayload();
                Console.WriteLine("SendBundle");
                payload.OutputDataToConsole();
                Console.WriteLine();
                socket.SendTo(payload, session.EndPoint);
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
