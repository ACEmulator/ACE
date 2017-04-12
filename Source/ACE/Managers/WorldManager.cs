using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

using ACE.Common;
using ACE.Entity;
using ACE.Network;

using log4net;
using System.Net.Sockets;
using ACE.Network.Packets;
using ACE.Database;
using ACE.Common.Cryptography;
using ACE.Network.Enum;

namespace ACE.Managers
{
    public static class WorldManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // Hard coded server Id, this will need to change if we move to multi-process or multi-server model
        public const ushort ServerId = 0xB;
        private static readonly List<ConnectionListener> Listeners = new List<ConnectionListener>();
        private static readonly NetworkSession[] sessionMap = new NetworkSession[128]; // TODO Placeholder, should be config MaxSessions
        private static readonly List<Session> sessions = new List<Session>();
        private static readonly ReaderWriterLockSlim sessionLock = new ReaderWriterLockSlim();

        private static bool pendingWorldStop;

        public static DateTime WorldStartTime { get; } = DateTime.UtcNow;

        public static DerethDateTime WorldStartFromTime { get; } = new DerethDateTime().UTCNowToLoreTime;

        public static double PortalYearTicks { get; private set; } = WorldStartFromTime.Ticks;

        public static void Initialise()
        {
            IPAddress host;

            try
            {
                host = IPAddress.Parse(ConfigManager.Config.Server.Network.Host);
            }
            catch (Exception ex)
            {
                log.Error($"Unable to use {ConfigManager.Config.Server.Network.Host} as host due to: {ex.ToString()}");
                log.Error("Using IPAddress.Any as host instead.");
                host = IPAddress.Any;
            }

            for (uint i = 0; i < 2; i++)
            {
                log.Info($"Binding ConnectionListener to {host}:{ConfigManager.Config.Server.Network.Port + i}");
                var listener = new ConnectionListener(host, ConfigManager.Config.Server.Network.Port + i);
                listener.PacketReceived += Listener_PacketReceived;
                listener.Start();
                Listeners.Add(listener);
            }

            var thread = new Thread(UpdateWorld);
            thread.Start();
            log.DebugFormat("ServerTime initialized to {0}", WorldStartFromTime.ToString());
        }

        private static void Listener_PacketReceived(object sender, ClientPacket packet, IPEndPoint endpoint)
        {
            ProcessPacket(packet, endpoint);
        }

        public static Socket GetSocket(int id = 1)
        {
            return Listeners[id].Socket;
        }

        private static void ProcessPacket(ClientPacket packet, IPEndPoint endPoint)
        {
            if (packet.Header.HasFlag(PacketHeaderFlags.LoginRequest))
            {
                log.DebugFormat("Login Request from {0}", endPoint);
                HandleLoginRequest(packet, endPoint);
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
                    log.DebugFormat("Null Session for Id {0}", packet.Header.Id);
                }
            }
        }

        public static async void HandleLoginRequest(ClientPacket packet, IPEndPoint endPoint)
        {
            var session = FindOrCreateSession(endPoint);
            PacketInboundLoginRequest loginRequest = new PacketInboundLoginRequest(packet);
            try
            {
                var result = await DatabaseManager.Authentication.GetAccountByName(loginRequest.Account);
                AccountSelectCallback(result, session);
            }
            catch (IndexOutOfRangeException)
            {
                AccountSelectCallback(null, session);
            }
        }

        private static void AccountSelectCallback(Account account, Session session)
        {
            if (session != null)
            {
                var connectRequest = new PacketOutboundConnectRequest(session.Network.ConnectionData.ServerTime, 0, session.Network.ClientId, ISAAC.ServerSeed, ISAAC.ClientSeed);
                session.Network.EnqueueSend(connectRequest);

                if (account == null)
                {
                    session.Terminate(CharacterError.AccountDoesntExist);
                    return;
                }

                if (WorldManager.Find(account.Name) != null)
                {
                    session.Terminate(CharacterError.AccountInUse);
                    return;
                }

                /*if (glsTicket != digest)
                {
                }*/

                // TODO: check for account bans

                session.SetAccount(account.AccountId, account.Name, account.AccessLevel);
            }
        }

        private static Session FindOrCreateSession(IPEndPoint endPoint)
        {
            Session session = null;
            sessionLock.EnterWriteLock();
            try
            {
                session = sessions.SingleOrDefault(s => endPoint.Equals(s.Network.EndPoint));
                if (session == null)
                {
                    for (ushort i = 0; i < sessionMap.Length; i++)
                    {
                        if (sessionMap[i] == null)
                        {
                            log.InfoFormat("Creating new session for {0} with id {1}", endPoint, i);
                            NetworkSession networkSession = new NetworkSession(endPoint, i, ServerId);
                            sessionMap[i] = networkSession;
                            session = new Session(networkSession);
                            session.StateChanged += Session_StateChanged;
                            sessions.Add(session);
                            break;
                        }
                    }
                }
            }
            finally
            {
                sessionLock.ExitWriteLock();
            }
            // If session is still null we either have no room or had some kind of failure, we'll create a temporary session just to send an error back.
            if (session == null)
            {
                log.WarnFormat("Failed to create a new session for {0}", endPoint);
                var errorNetworkSession = new NetworkSession(endPoint, (ushort)(sessionMap.Length + 1), ServerId);
                var errorSession = new Session(errorNetworkSession);
                errorSession.Terminate(Network.Enum.CharacterError.LogonServerFull);
            }
            return session;
        }

        private static void Session_StateChanged(object sender, Session.SessionStateChangedEventArgs e)
        {
            if (e.NewState == SessionState.Terminated)
            {
                Session session = (Session)sender;
                RemoveSession(session);
            }
        }

        private static void RemoveSession(Session session)
        {
            sessionLock.EnterWriteLock();
            try
            {
                log.InfoFormat("Removing session for {0} with id {1}", session.Network.EndPoint, session.Network.ClientId);
                if (sessions.Contains(session))
                    sessions.Remove(session);
                if (sessionMap[session.Network.ClientId] == session.Network)
                    sessionMap[session.Network.ClientId] = null;
            }
            finally
            {
                sessionLock.ExitWriteLock();
            }
        }

        public static Session Find(string account)
        {
            sessionLock.EnterReadLock();
            try
            {
                return sessions.SingleOrDefault(s => s.AccountName == account);
            }
            finally
            {
                sessionLock.ExitReadLock();
            }
        }

        public static Session Find(ObjectGuid characterGuid)
        {
            sessionLock.EnterReadLock();
            try
            {
                return sessions.SingleOrDefault(s => s.Player?.Guid.Low == characterGuid.Low);
            }
            finally
            {
                sessionLock.ExitReadLock();
            }
        }

        /// <summary>
        /// This will loop through the active sessions and find one with a player that is named <paramref name="name"/>, ignoring case.
        /// </summary>
        public static Session FindByPlayerName(string name, bool isOnlineRequired = true)
        {
            sessionLock.EnterReadLock();
            try
            {
                if (isOnlineRequired)
                    return sessions.SingleOrDefault(s => s.Player != null && s.Player.IsOnline && String.Compare(s.Player.Name, name, StringComparison.OrdinalIgnoreCase) == 0);

                return sessions.SingleOrDefault(s => s.Player != null && String.Compare(s.Player.Name, name, StringComparison.OrdinalIgnoreCase) == 0);
            }
            finally
            {
                sessionLock.ExitReadLock();
            }
        }

        public static List<Session> FindInverseFriends(ObjectGuid characterGuid)
        {
            sessionLock.EnterReadLock();
            try
            {
                return sessions.Where(s => s.Player?.Friends.FirstOrDefault(f => f.Id.Low == characterGuid.Low) != null).ToList();
            }
            finally
            {
                sessionLock.ExitReadLock();
            }
        }

        public static List<Session> GetAll(bool isOnlineRequired = true)
        {
            sessionLock.EnterReadLock();
            try
            {
                if (isOnlineRequired)
                    return sessions.Where(s => s.Player != null && s.Player.IsOnline).ToList();

                return sessions.Where(s => s.Player != null).ToList();
            }
            finally
            {
                sessionLock.ExitReadLock();
            }
        }

        public static void StopWorld() { pendingWorldStop = true; }

        private static void UpdateWorld()
        {
            log.DebugFormat("Starting UpdateWorld thread");
            double lastTick = 0d;

            var worldTickTimer = new Stopwatch();
            while (!pendingWorldStop)
            {
                worldTickTimer.Restart();
                List<Session> updatingSessions;
                sessionLock.EnterReadLock();
                try
                {
                    updatingSessions = new List<Session>(sessions);
                }
                finally
                {
                    sessionLock.ExitReadLock();
                }

                Parallel.ForEach(updatingSessions, s => s.Update(lastTick));

                Thread.Sleep(1);

                lastTick = (double)worldTickTimer.ElapsedTicks / Stopwatch.Frequency;
                PortalYearTicks += lastTick;
            }
        }
    }
}
