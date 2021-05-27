using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using log4net;

using ACE.Common;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Managers;
using ACE.Server.Network.Packets;
using ACE.Server.Network.Handlers;
using ACE.Server.Network.Enum;

namespace ACE.Server.Network.Managers
{
    public static class NetworkManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ILog packetLog = LogManager.GetLogger(System.Reflection.Assembly.GetEntryAssembly(), "Packets");

        // Hard coded server Id, this will need to change if we move to multi-process or multi-server model
        public const ushort ServerId = 0xB;

        /// <summary>
        /// Seconds until a session will timeout. 
        /// Raising this value allows connections to remain active for a longer period of time. 
        /// </summary>
        /// <remarks>
        /// If you're experiencing network dropouts or frequent disconnects, try increasing this value.
        /// </remarks>
        public static uint DefaultSessionTimeout = ConfigManager.Config.Server.Network.DefaultSessionTimeout;

        private static readonly ReaderWriterLockSlim sessionLock = new ReaderWriterLockSlim();
        private static readonly Session[] sessionMap = new Session[ConfigManager.Config.Server.Network.MaximumAllowedSessions];

        /// <summary>
        /// Handles ClientMessages in InboundMessageManager
        /// </summary>
        public static readonly ActionQueue InboundMessageQueue = new ActionQueue();

        public static void ProcessPacket(ConnectionListener connectionListener, ClientPacket packet, IPEndPoint endPoint)
        {
            if (connectionListener.ListenerEndpoint.Port == ConfigManager.Config.Server.Network.Port + 1)
            {
                ServerPerformanceMonitor.RestartEvent(ServerPerformanceMonitor.MonitorType.ProcessPacket_1);
                if (packet.Header.Flags.HasFlag(PacketHeaderFlags.ConnectResponse))
                {
                    packetLog.Debug($"{packet}, {endPoint}");
                    PacketInboundConnectResponse connectResponse = new PacketInboundConnectResponse(packet);

                    // This should be set on the second packet to the server from the client.
                    // This completes the three-way handshake.
                    sessionLock.EnterReadLock();
                    Session session = null;
                    try
                    {
                        session =
                            (from k in sessionMap
                             where
                                 k != null &&
                                 k.State == SessionState.AuthConnectResponse &&
                                 k.Network.ConnectionData.ConnectionCookie == connectResponse.Check &&
                                 k.EndPoint.Address.Equals(endPoint.Address)
                             select k).FirstOrDefault();
                    }
                    finally
                    {
                        sessionLock.ExitReadLock();
                    }
                    if (session != null)
                    {
                        session.State = SessionState.AuthConnected;
                        session.Network.sendResync = true;
                        AuthenticationHandler.HandleConnectResponse(session);
                    }

                }
                else if (packet.Header.Id == 0 && packet.Header.HasFlag(PacketHeaderFlags.CICMDCommand))
                {
                    // TODO: Not sure what to do with these packets yet
                }
                else
                {
                    log.ErrorFormat("Packet from {0} rejected. Packet sent to listener 1 and is not a ConnectResponse or CICMDCommand", endPoint);
                }
                ServerPerformanceMonitor.RegisterEventEnd(ServerPerformanceMonitor.MonitorType.ProcessPacket_1);
            }
            else // ConfigManager.Config.Server.Network.Port + 0
            {
                ServerPerformanceMonitor.RestartEvent(ServerPerformanceMonitor.MonitorType.ProcessPacket_0);
                if (packet.Header.HasFlag(PacketHeaderFlags.LoginRequest))
                {
                    packetLog.Debug($"{packet}, {endPoint}");
                    if (GetAuthenticatedSessionCount() >= ConfigManager.Config.Server.Network.MaximumAllowedSessions)
                    {
                        log.InfoFormat("Login Request from {0} rejected. Server full.", endPoint);
                        SendLoginRequestReject(connectionListener, endPoint, CharacterError.LogonServerFull);
                    }
                    else if (ServerManager.ShutdownInProgress)
                    {
                        log.InfoFormat("Login Request from {0} rejected. Server is shutting down.", endPoint);
                        SendLoginRequestReject(connectionListener, endPoint, CharacterError.ServerCrash1);
                    }
                    else if (ServerManager.ShutdownInitiated && (ServerManager.ShutdownTime - DateTime.UtcNow).TotalMinutes < 2)
                    {
                        log.InfoFormat("Login Request from {0} rejected. Server shutting down in less than 2 minutes.", endPoint);
                        SendLoginRequestReject(connectionListener, endPoint, CharacterError.ServerCrash1);
                    }
                    else
                    {
                        log.DebugFormat("Login Request from {0}", endPoint);

                        var ipAllowsUnlimited = ConfigManager.Config.Server.Network.AllowUnlimitedSessionsFromIPAddresses.Contains(endPoint.Address.ToString());
                        if (ipAllowsUnlimited || ConfigManager.Config.Server.Network.MaximumAllowedSessionsPerIPAddress == -1 || GetSessionEndpointTotalByAddressCount(endPoint.Address) < ConfigManager.Config.Server.Network.MaximumAllowedSessionsPerIPAddress)
                        {
                            var session = FindOrCreateSession(connectionListener, endPoint);
                            if (session != null)
                            {
                                if (session.State == SessionState.AuthConnectResponse)
                                {
                                    // connect request packet sent to the client was corrupted in transit and session entered an unspecified state.
                                    // ignore the request and remove the broken session and the client will start a new session.
                                    RemoveSession(session);
                                    log.Warn($"Bad handshake from {endPoint}, aborting session.");
                                }

                                session.ProcessPacket(packet);
                            }
                            else
                            {
                                log.InfoFormat("Login Request from {0} rejected. Failed to find or create session.", endPoint);
                                SendLoginRequestReject(connectionListener, endPoint, CharacterError.LogonServerFull);
                            }
                        }
                        else
                        {
                            log.InfoFormat("Login Request from {0} rejected. Session would exceed MaximumAllowedSessionsPerIPAddress limit.", endPoint);
                            SendLoginRequestReject(connectionListener, endPoint, CharacterError.LogonServerFull);
                        }
                    }
                }
                else if (sessionMap.Length > packet.Header.Id)
                {
                    var session = sessionMap[packet.Header.Id];
                    if (session != null)
                    {
                        if (session.EndPoint.Equals(endPoint))
                            session.ProcessPacket(packet);
                        else
                            log.DebugFormat("Session for Id {0} has IP {1} but packet has IP {2}", packet.Header.Id, session.EndPoint, endPoint);
                    }
                    else
                    {
                        log.DebugFormat("Unsolicited Packet from {0} with Id {1}", endPoint, packet.Header.Id);
                    }
                }
                else
                {
                    log.DebugFormat("Unsolicited Packet from {0} with Id {1}", endPoint, packet.Header.Id);
                }
                ServerPerformanceMonitor.RegisterEventEnd(ServerPerformanceMonitor.MonitorType.ProcessPacket_0);
            }
        }

        private static void SendLoginRequestReject(ConnectionListener connectionListener, IPEndPoint endPoint, CharacterError error)
        {
            var tempSession = new Session(connectionListener, endPoint, (ushort)(sessionMap.Length + 1), ServerId);

            SendLoginRequestReject(tempSession, error);
        }

        public static void SendLoginRequestReject(Session session, CharacterError error)
        {
            // First we must send the connect request response
            var connectRequest = new PacketOutboundConnectRequest(
                Timers.PortalYearTicks,
                session.Network.ConnectionData.ConnectionCookie,
                session.Network.ClientId,
                session.Network.ConnectionData.ServerSeed,
                session.Network.ConnectionData.ClientSeed);
            session.Network.ConnectionData.DiscardSeeds();
            session.Network.EnqueueSend(connectRequest);

            // Then we send the error
            session.SendCharacterError(error);

            session.Network.Update();
        }

        public static int GetSessionCount()
        {
            sessionLock.EnterReadLock();
            try
            {
                return sessionMap.Count(s => s != null);
            }
            finally
            {
                sessionLock.ExitReadLock();
            }
        }

        public static int GetAuthenticatedSessionCount()
        {
            sessionLock.EnterReadLock();
            try
            {
                return sessionMap.Count(s => s != null && s.AccountId != 0);
            }
            finally
            {
                sessionLock.ExitReadLock();
            }
        }

        public static int GetUniqueSessionEndpointCount()
        {
            sessionLock.EnterReadLock();
            try
            {
                var ipAddresses = new HashSet<IPAddress>();

                foreach (var s in sessionMap)
                {
                    if (s != null)
                        ipAddresses.Add(s.EndPoint.Address);
                }

                return ipAddresses.Count;
            }
            finally
            {
                sessionLock.ExitReadLock();
            }
        }

        public static int GetSessionEndpointTotalByAddressCount(IPAddress address)
        {
            sessionLock.EnterReadLock();
            try
            {
                int result = 0;

                foreach (var s in sessionMap)
                {
                    if (s != null && s.EndPoint.Address.Equals(address))
                        result++;
                }

                return result;
            }
            finally
            {
                sessionLock.ExitReadLock();
            }
        }

        public static Session FindOrCreateSession(ConnectionListener connectionListener, IPEndPoint endPoint)
        {
            Session session;

            sessionLock.EnterUpgradeableReadLock();
            try
            {
                session = sessionMap.SingleOrDefault(s => s != null && endPoint.Equals(s.EndPoint));
                if (session == null)
                {
                    sessionLock.EnterWriteLock();
                    try
                    {
                        for (ushort i = 0; i < sessionMap.Length; i++)
                        {
                            if (sessionMap[i] == null)
                            {
                                log.DebugFormat("Creating new session for {0} with id {1}", endPoint, i);
                                session = new Session(connectionListener, endPoint, i, ServerId);
                                sessionMap[i] = session;
                                break;
                            }
                        }
                    }
                    finally
                    {
                        sessionLock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                sessionLock.ExitUpgradeableReadLock();
            }

            return session;
        }

        public static Session Find(uint accountId)
        {
            sessionLock.EnterReadLock();
            try
            {
                return sessionMap.SingleOrDefault(s => s != null && s.AccountId == accountId);
            }
            finally
            {
                sessionLock.ExitReadLock();
            }
        }

        public static Session Find(string account)
        {
            sessionLock.EnterReadLock();
            try
            {
                return sessionMap.SingleOrDefault(s => s != null && s.Account == account);
            }
            finally
            {
                sessionLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Removes a session, network client and network endpoint from the various tracker objects.
        /// </summary>
        public static void RemoveSession(Session session)
        {
            sessionLock.EnterWriteLock();
            try
            {
                log.DebugFormat("Removing session for {0} with id {1}", session.EndPoint, session.Network.ClientId);
                if (sessionMap[session.Network.ClientId] == session)
                    sessionMap[session.Network.ClientId] = null;
            }
            finally
            {
                sessionLock.ExitWriteLock();
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

            sessionLock.EnterUpgradeableReadLock();
            try
            {
                // The session tick outbound processes pending actions and handles outgoing messages
                ServerPerformanceMonitor.RestartEvent(ServerPerformanceMonitor.MonitorType.DoSessionWork_TickOutbound);
                Parallel.ForEach(sessionMap, ConfigManager.Config.Server.Threading.NetworkManagerParallelOptions, s => s?.TickOutbound());
                ServerPerformanceMonitor.RegisterEventEnd(ServerPerformanceMonitor.MonitorType.DoSessionWork_TickOutbound);

                // Removes sessions in the NetworkTimeout state, including sessions that have reached a timeout limit.
                ServerPerformanceMonitor.RestartEvent(ServerPerformanceMonitor.MonitorType.DoSessionWork_RemoveSessions);
                foreach (var session in sessionMap.Where(k => !Equals(null, k)))
                {
                    if (session.PendingTermination != null && session.PendingTermination.TerminationStatus == SessionTerminationPhase.SessionWorkCompleted)
                    {
                        session.DropSession();
                        session.PendingTermination.TerminationStatus = SessionTerminationPhase.WorldManagerWorkCompleted;
                    }

                    sessionCount++;
                }
                ServerPerformanceMonitor.RegisterEventEnd(ServerPerformanceMonitor.MonitorType.DoSessionWork_RemoveSessions);
            }
            finally
            {
                sessionLock.ExitUpgradeableReadLock();
            }
            return sessionCount;
        }

        public static void DisconnectAllSessionsForShutdown()
        {
            foreach (var session in sessionMap)
            {
                session?.Terminate(SessionTerminationReason.ServerShuttingDown, new GameMessages.Messages.GameMessageCharacterError(CharacterError.ServerCrash1));
            }
        }
    }
}
