using ACE.Common;
using ACE.Server.Network;
using ACE.Server.Network.Enum;
using ACE.Server.Network.Handlers;
using ACE.Server.Network.Packets;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ACE.Server.Managers
{
    /// <summary>
    /// We use a single socket because the use of dual unidirectional sockets doesn't work for some client firewalls
    /// </summary>
    public static class NetworkManager
    {
        public const ushort ServerId = 0xB;
        public static ConcurrentDictionary<ushort, Session> Sessions { get; private set; } = new ConcurrentDictionary<ushort, Session>();
        private static ushort NextSessionId => (ushort)Enumerable.Range(1, ushort.MaxValue).Except(Sessions.Keys.Select(k => (int)k)).First();
        public static uint DefaultSessionTimeout = ConfigManager.Config.Server.Network.DefaultSessionTimeout;

        public static void ProcessPacket(ClientPacket packet, IPEndPoint endPoint, IPEndPoint listenerEndpoint)
        {
            if (listenerEndpoint.Port == ConfigManager.Config.Server.Network.Port + 1)
            {
                ServerPerformanceMonitor.RegisterEventStart(ServerPerformanceMonitor.MonitorType.ProcessPacket_1);
                if (packet.Header.Flags.HasFlag(PacketHeaderFlags.ConnectResponse))
                {
                    PacketInboundConnectResponse connectResponse = new PacketInboundConnectResponse(packet);

                    // This should be set on the second packet to the server from the client.
                    // This completes the three-way handshake.
                    Session session = null;

                    session =
                        (from k in Sessions
                         where
                             k.Value.State == SessionState.AuthConnectResponse &&
                             k.Value.ConnectionData.ConnectionCookie == connectResponse.Check &&
                             k.Value.EndPoint.Address.Equals(endPoint.Address)
                         select k).FirstOrDefault().Value;

                    if (session != null)
                    {
                        session.State = SessionState.AuthConnected;
                        session.sendResync = true;
                        AuthenticationHandler.HandleConnectResponse(session);
                    }

                }
                else if (packet.Header.Id == 0 && packet.Header.HasFlag(PacketHeaderFlags.CICMDCommand))
                {
                    // TODO: Not sure what to do with these packets yet
                }
                else
                {
                }
                ServerPerformanceMonitor.RegisterEventEnd(ServerPerformanceMonitor.MonitorType.ProcessPacket_1);
            }
            else // ConfigManager.Config.Server.Port + 0
            {
                ServerPerformanceMonitor.RegisterEventStart(ServerPerformanceMonitor.MonitorType.ProcessPacket_0);
                if (packet.Header.HasFlag(PacketHeaderFlags.LoginRequest))
                {
                    if (!Sessions.Any(k => k.Value.EndPoint.Equals(endPoint)) && Sessions.Count >= ConfigManager.Config.Server.Network.MaximumAllowedSessions)
                    {
                        SendLoginRequestReject(endPoint, CharacterError.LogonServerFull);
                    }
                    else if (ServerManager.ShutdownInitiated)
                    {
                        SendLoginRequestReject(endPoint, CharacterError.ServerCrash);
                    }
                    else
                    {
                        Session session = FindOrCreateSession(endPoint);
                        if (session != null)
                        {
                            if (session.State == SessionState.AuthConnectResponse)
                            {
                                // connect request packet sent to the client was corrupted in transit and session entered an unspecified state.
                                // ignore the request and remove the broken session and the client will start a new session.
                                RemoveSession(session);
                            }

                            session.ProcessSessionPacket(packet);
                        }
                        else
                        {
                            SendLoginRequestReject(endPoint, CharacterError.LogonServerFull);
                        }
                    }
                }
                else
                {
                    Session session = Sessions.FirstOrDefault(k => k.Value.ClientId == packet.Header.Id).Value;
                    if (session != null)
                    {
                        if (session.EndPoint.Equals(endPoint))
                        {
                            session.ProcessSessionPacket(packet);
                        }
                    }
                }
                ServerPerformanceMonitor.RegisterEventEnd(ServerPerformanceMonitor.MonitorType.ProcessPacket_0);
            }
        }

        /// <summary>
        /// Processes all inbound GameAction messages.<para />
        /// Dispatches all outgoing messages.<para />
        /// Removes dead sessions.
        /// </summary>
        public static int DoSessionWork()
        {
            int sessionCount = 0;
            // The session tick outbound processes pending actions and handles outgoing messages
            ServerPerformanceMonitor.RegisterEventStart(ServerPerformanceMonitor.MonitorType.DoSessionWork_TickOutbound);
            foreach (var s in Sessions)
                s.Value.TickOutbound();
            OutboundQueue.SendAll();
            ServerPerformanceMonitor.RegisterEventEnd(ServerPerformanceMonitor.MonitorType.DoSessionWork_TickOutbound);
            // Removes sessions in the NetworkTimeout state, including sessions that have reached a timeout limit.
            ServerPerformanceMonitor.RegisterEventStart(ServerPerformanceMonitor.MonitorType.DoSessionWork_RemoveSessions);
            foreach (var session in Sessions.Values)
            {
                if (session.PendingTermination != null && session.PendingTermination.TerminationStatus == SessionTerminationPhase.SessionWorkCompleted)
                {
                    session.DropSession();
                    session.PendingTermination.TerminationStatus = SessionTerminationPhase.WorldManagerWorkCompleted;
                }
                sessionCount++;
            }
            ServerPerformanceMonitor.RegisterEventEnd(ServerPerformanceMonitor.MonitorType.DoSessionWork_RemoveSessions);
            return sessionCount;
        }

        private static void SendLoginRequestReject(IPEndPoint endPoint, CharacterError error)
        {
            Session tempSession = new Session(endPoint, NextSessionId, ServerId);

            // First we must send the connect request response
            PacketOutboundConnectRequest connectRequest = new PacketOutboundConnectRequest(
                tempSession.ConnectionData.ServerTime,
                tempSession.ConnectionData.ConnectionCookie,
                tempSession.ClientId,
                tempSession.ConnectionData.ServerSeed,
                tempSession.ConnectionData.ClientSeed);
            tempSession.ConnectionData.DiscardSeeds();
            tempSession.EnqueueSend(connectRequest);

            // Then we send the error
            tempSession.SendCharacterError(error);

            tempSession.Update();
        }

        public static Session FindOrCreateSession(IPEndPoint endPoint)
        {
            Session session;
            session = Sessions.SingleOrDefault(s => endPoint.Equals(s.Value.EndPoint)).Value;
            if (session == null)
            {
                ushort cid = NextSessionId;
                session = Sessions.AddOrUpdate(cid, new Session(endPoint, cid, ServerId), (a, b) => b);
            }
            return session;
        }

        /// <summary>
        /// Removes a session, network client and network endpoint from the various tracker objects.
        /// </summary>
        public static void RemoveSession(Session session)
        {
            Sessions.Remove(session.ClientId, out Session xSession);
        }

        public static int GetSessionCount()
        {
            return Sessions.Count;
        }

        public static Session Find(uint accountId)
        {
            return Sessions.FirstOrDefault(k => k.Value.AccountId == accountId).Value;
        }

        public static Session Find(string account)
        {
            return Sessions.FirstOrDefault(k => k.Value.Account == account).Value;
        }


        private static readonly ConnectionListener[] listeners = new ConnectionListener[2];

        public static InboundPacketQueue InboundQueue { get; private set; }
        public static OutboundPacketQueue OutboundQueue { get; private set; }

        public static void Initialize()
        {
            IPAddress host;

            try
            {
                host = IPAddress.Parse(ConfigManager.Config.Server.Network.Host);
            }
            catch (Exception)
            {
                host = IPAddress.Any;
            }

            InboundQueue = new InboundPacketQueue();

            listeners[0] = new ConnectionListener(host, ConfigManager.Config.Server.Network.Port);

            listeners[1] = new ConnectionListener(host, ConfigManager.Config.Server.Network.Port + 1);

            listeners[0].Start();
            listeners[1].Start();

            OutboundQueue = new OutboundPacketQueue(listeners[0].Socket);
        }
        public static void Shutdown()
        {
            InboundQueue.Shutdown();
        }
    }
}
