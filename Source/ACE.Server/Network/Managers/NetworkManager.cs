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
using System.Threading;

namespace ACE.Server.Managers
{
    public static class NetworkManager
    {
        public const ushort ServerId = 0xB;
        public static InboundPacketQueue InboundQueue { get; private set; }
        public static OutboundPacketQueue OutboundQueue { get; set; }
        public static ConcurrentDictionary<ushort, Session> Sessions { get; private set; } = new ConcurrentDictionary<ushort, Session>();
        private static ushort NextSessionId => (ushort)Enumerable.Range(1, ushort.MaxValue).Except(Sessions.Keys.Select(k => (int)k)).First();
        private static readonly ConnectionListener[] listeners = new ConnectionListener[2];

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

            new Thread(new ThreadStart(() => { listeners[0].Start(); })) { Name = $"{listeners[0].Socket.LocalEndPoint} Listener" }.Start();
            new Thread(new ThreadStart(() => { listeners[1].Start(); })) { Name = $"{listeners[1].Socket.LocalEndPoint} Listener" }.Start();

            OutboundQueue = new OutboundPacketQueue(listeners[0].Socket);
        }
        public static void Shutdown()
        {
            InboundQueue.Shutdown();
        }
        public static void ProcessPacket(ClientPacket packet, IPEndPoint endPoint, IPEndPoint listenerEndpoint, Session VerifiedSession)
        {
            if (listenerEndpoint.Port == ConfigManager.Config.Server.Network.Port + 1)
            {
                ServerPerformanceMonitor.RegisterEventStart(ServerPerformanceMonitor.MonitorType.ProcessPacket_1);
                if (packet.Header.Flags.HasFlag(PacketHeaderFlags.ConnectResponse))
                {
                    PacketInboundConnectResponse connectResponse = new PacketInboundConnectResponse(packet);
                    Session session = null;
                    session =
                        (from k in Sessions.Values
                         where
                             k.State == SessionState.AuthConnectResponse &&
                             k.ConnectionCookie == connectResponse.Check &&
                             k.EndPoint.Address.Equals(endPoint.Address)
                         select k).FirstOrDefault();
                    if (session != null)
                    {
                        session.State = SessionState.AuthConnected;
                        session.sendResync = true;
                        AuthenticationHandler.HandleConnectResponse(session);
                    }
                }
                ServerPerformanceMonitor.RegisterEventEnd(ServerPerformanceMonitor.MonitorType.ProcessPacket_1);
            }
            else
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
                        SendLoginRequestReject(endPoint, CharacterError.ServerCrash1);
                    }
                    else
                    {
                        Session session = FindOrCreateSession(endPoint);
                        if (session != null)
                        {
                            if (session.State == SessionState.AuthConnectResponse)
                            {
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
                    if (VerifiedSession == null)
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
                    else
                    {
                        VerifiedSession.ProcessSessionPacket(packet);
                    }
                }
                ServerPerformanceMonitor.RegisterEventEnd(ServerPerformanceMonitor.MonitorType.ProcessPacket_0);
            }
        }
        public static int DoSessionWork()
        {
            int sessionCount = 0;
            ServerPerformanceMonitor.RegisterEventStart(ServerPerformanceMonitor.MonitorType.DoSessionWork_TickOutbound);
            foreach (KeyValuePair<ushort, Session> s in Sessions)
            {
                s.Value.TickOutbound();
            }
            OutboundQueue.SendAll();
            ServerPerformanceMonitor.RegisterEventEnd(ServerPerformanceMonitor.MonitorType.DoSessionWork_TickOutbound);
            ServerPerformanceMonitor.RegisterEventStart(ServerPerformanceMonitor.MonitorType.DoSessionWork_RemoveSessions);
            foreach (Session session in Sessions.Values)
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
        public static void RemoveSession(Session session)
        {
            Sessions.Remove(session.ClientId, out Session xSession);
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
        public static Session Find(uint accountId)
        {
            return Sessions.FirstOrDefault(k => k.Value.AccountId == accountId).Value;
        }
        public static Session Find(string account)
        {
            return Sessions.FirstOrDefault(k => k.Value.Account == account).Value;
        }

        private static void SendLoginRequestReject(IPEndPoint endPoint, CharacterError error)
        {
            Session tempSession = new Session(endPoint, NextSessionId, ServerId);
            PacketOutboundConnectRequest connectRequest = new PacketOutboundConnectRequest(
                tempSession.ServerTime,
                tempSession.ConnectionCookie,
                tempSession.ClientId,
                tempSession.ServerSeed,
                tempSession.ClientSeed);
            tempSession.DiscardSeeds();
            tempSession.EnqueueSend(connectRequest);
            tempSession.SendCharacterError(error);
            tempSession.Update();
        }
    }
}
