using System;
using System.Collections.Generic;
using System.Reflection;

using ACE.Managers;
using ACE.Network.Enum;
using ACE.Network.Fragments;
using ACE.Network.GameAction;
using ACE.Network.Handlers;

namespace ACE.Network.Managers
{
    public static class PacketManager
    {
        private class FragmentHandlerInfo
        {
            public FragmentHandler Handler { get; set; }
            public FragmentAttribute Attribute { get; set; }
        }

        public delegate void FragmentHandler(ClientPacketFragment fragement, Session session);
        private static Dictionary<FragmentOpcode, FragmentHandlerInfo> fragmentHandlers;

        private static Dictionary<GameActionOpcode, Type> actionHandlers;

        public static void Initialise()
        {
            DefineFragmentHandlers();
            DefineActionHandlers();
        }

        private static void DefineFragmentHandlers()
        {
            fragmentHandlers = new Dictionary<FragmentOpcode, FragmentHandlerInfo>();
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                foreach (var methodInfo in type.GetMethods())
                {
                    foreach (var fragmentHandlerAttribute in methodInfo.GetCustomAttributes<FragmentAttribute>())
                    {
                        var fragmentHandler = new FragmentHandlerInfo()
                        {
                            Handler   = (FragmentHandler)Delegate.CreateDelegate(typeof(FragmentHandler), methodInfo),
                            Attribute = fragmentHandlerAttribute
                        };

                        fragmentHandlers[fragmentHandlerAttribute.Opcode] = fragmentHandler;
                    }
                }
            }
        }

        private static void DefineActionHandlers()
        {
            actionHandlers = new Dictionary<GameActionOpcode, Type>();
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
                foreach (var actionHandlerAttribute in type.GetCustomAttributes<GameActionAttribute>())
                    actionHandlers[actionHandlerAttribute.Opcode] = type;
        }

        private static bool CheckPacketHeader(ClientPacket packet, Session session)
        {
            if (packet.Header.HasFlag(PacketHeaderFlags.LoginRequest) && session.State != SessionState.AuthLoginRequest)
                return false;

            if (packet.Header.HasFlag(PacketHeaderFlags.ConnectResponse) && session.State != SessionState.AuthConnectResponse && session.State != SessionState.WorldConnectResponse)
                return false;

            if (packet.Header.HasFlag(PacketHeaderFlags.AckSequence | PacketHeaderFlags.TimeSynch | PacketHeaderFlags.EchoRequest | PacketHeaderFlags.Flow) && session.State == SessionState.AuthLoginRequest)
                return false;

            if (packet.Header.HasFlag(PacketHeaderFlags.WorldLoginRequest) && session.State != SessionState.WorldLoginRequest)
                return false;

            return true;
        }

        public static void HandlePacket(ConnectionType type, ClientPacket packet, Session session)
        {
            if (!CheckPacketHeader(packet, session))
            {
                // server treats all packets sent during the first 30 seconds as invalid packets due to server crash, this will move clients to the disconnect screen
                if (DateTime.Now < WorldManager.WorldStartTime.AddSeconds(30d))
                    session.SendCharacterError(CharacterError.ServerCrash);
                return;
            }

            // CLinkStatusAverages::OnPingResponse
            if (packet.Header.HasFlag(PacketHeaderFlags.EchoRequest))
            {
                var connectionData = (type == ConnectionType.Login ? session.LoginConnection : session.WorldConnection);

                // used to calculate round trip time (ping)
                // client calculates: currentTime - requestTime - serverDrift
                var echoResponse = new ServerPacket((ushort)(type == ConnectionType.Login ? 0x0B : 0x18), PacketHeaderFlags.EncryptedChecksum | PacketHeaderFlags.EchoResponse);
                echoResponse.Payload.Write(packet.HeaderOptional.ClientTime);
                echoResponse.Payload.Write((float)connectionData.ServerTime - packet.HeaderOptional.ClientTime);

                NetworkManager.SendPacket(type, echoResponse, session);
            }

            // ClientNet::HandleTimeSynch
            if (packet.Header.HasFlag(PacketHeaderFlags.TimeSynch))
            {
                var connectionData = (type == ConnectionType.Login ? session.LoginConnection : session.WorldConnection);

                // used to update time at client and check for overspeed (60s desync and client will disconenct with speed hack warning)
                var timeSynchResponse = new ServerPacket((ushort)(type == ConnectionType.Login ? 0x0B : 0x18), PacketHeaderFlags.EncryptedChecksum | PacketHeaderFlags.TimeSynch);
                timeSynchResponse.Payload.Write(connectionData.ServerTime);

                NetworkManager.SendPacket(type, timeSynchResponse, session);
            }

            if (packet.Header.HasFlag(PacketHeaderFlags.RequestRetransmit))
            {
                foreach (uint sequence in packet.HeaderOptional.RetransmitData)
                {
                    CachedPacket cachedPacket;
                    if (session.CachedPackets.TryGetValue(sequence, out cachedPacket))
                    {
                        if (!cachedPacket.Packet.Header.HasFlag(PacketHeaderFlags.Retransmission))
                        {
                            cachedPacket.Packet.Header.Flags |= PacketHeaderFlags.Retransmission;

                            uint issacXor;
                            cachedPacket.Packet.Header.Checksum = packet.CalculateChecksum(session, ConnectionType.World, cachedPacket.IssacXor, out issacXor);
                        }

                        NetworkManager.SendPacketDirect(ConnectionType.World, cachedPacket.Packet, session);
                    }
                }
            }

            if (packet.Header.HasFlag(PacketHeaderFlags.LoginRequest))
            {
                AuthenticationHandler.HandleLoginRequest(packet, session);
                return;
            }

            if (packet.Header.HasFlag(PacketHeaderFlags.WorldLoginRequest))
            {
                AuthenticationHandler.HandleWorldLoginRequest(packet, session);
                return;
            }

            if (packet.Header.HasFlag(PacketHeaderFlags.ConnectResponse))
            {
                if (type == ConnectionType.Login)
                {
                    AuthenticationHandler.HandleConnectResponse(packet, session);
                    return;
                }

                AuthenticationHandler.HandleWorldConnectResponse(packet, session);
                return;
            }

            if (packet.Header.HasFlag(PacketHeaderFlags.Disconnect))
                HandleDisconnectResponse(packet, session);

            foreach (ClientPacketFragment fragment in packet.Fragments)
            {
                var opcode = (FragmentOpcode)fragment.Payload.ReadUInt32();
                if (!fragmentHandlers.ContainsKey(opcode))
                    Console.WriteLine($"Received unhandled fragment opcode: 0x{(uint)opcode:X4}");
                else
                {
                    FragmentHandlerInfo fragmentHandlerInfo;
                    if (fragmentHandlers.TryGetValue(opcode, out fragmentHandlerInfo))
                        if (fragmentHandlerInfo.Attribute.State == session.State)
                            fragmentHandlerInfo.Handler.Invoke(fragment, session);
                }
            }
        }

        public static void HandleGameAction(GameActionOpcode opcode, ClientPacketFragment fragment, Session session)
        {
            if (!actionHandlers.ContainsKey(opcode))
                Console.WriteLine($"Received unhandled action opcode: 0x{(uint)opcode:X4}");
            else
            {
                Type actionType;
                if (actionHandlers.TryGetValue(opcode, out actionType))
                {
                    var gameAction = (GameActionPacket)Activator.CreateInstance(actionType, session, fragment);
                    gameAction.Read();
                    gameAction.Handle();
                }
            }
        }

        private static void HandleDisconnectResponse(ClientPacket packet, Session session)
        {
            WorldManager.Remove(session);
        }
    }
}
