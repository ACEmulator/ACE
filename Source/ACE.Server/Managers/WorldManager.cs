using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using log4net;

using ACE.Common;
using ACE.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.WorldObjects;
using ACE.Server.Network;
using ACE.Server.Network.GameMessages;
using ACE.Server.Physics;
using ACE.Server.Physics.Common;
using ACE.Database;

using Landblock = ACE.Server.Entity.Landblock;
using Position = ACE.Entity.Position;

namespace ACE.Server.Managers
{
    public static class WorldManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // Hard coded server Id, this will need to change if we move to multi-process or multi-server model
        public const ushort ServerId = 0xB;
        private static Session[] sessionMap = new Session[ConfigManager.Config.Server.Network.MaximumAllowedSessions];
        private static readonly List<Session> sessions = new List<Session>();
        private static readonly ReaderWriterLockSlim sessionLock = new ReaderWriterLockSlim();
        private static List<IPEndPoint> loggedInClients = new List<IPEndPoint>((int)ConfigManager.Config.Server.Network.MaximumAllowedSessions);
        private static readonly PhysicsEngine Physics;

        public static List<Player> AllPlayers;

        public static bool Concurrency = false;

        /// <summary>
        /// Seconds until a session will timeout. 
        /// Raising this value allows connections to remain active for a longer period of time. 
        /// </summary>
        /// <remarks>
        /// If you're experiencing network dropouts or frequent disconnects, try increasing this value.
        /// </remarks>
        public static uint DefaultSessionTimeout = ConfigManager.Config.Server.Network.DefaultSessionTimeout;

        private static volatile bool pendingWorldStop;
        public static bool WorldActive { get; private set; }

        public static DateTime WorldStartTime { get; } = DateTime.UtcNow;

        public static DerethDateTime WorldStartFromTime { get; } = new DerethDateTime().UTCNowToLoreTime;

        public static double PortalYearTicks { get; private set; } = WorldStartFromTime.Ticks;

        /// <summary>
        /// Handles ClientMessages in InboundMessageManager
        /// </summary>
        public static readonly ActionQueue InboundMessageQueue = new ActionQueue();

        public static readonly ActionQueue LandblockActionQueue = new ActionQueue();

        public static readonly DelayManager DelayManager = new DelayManager();

        static WorldManager()
        {
            Physics = new PhysicsEngine(new ObjectMaint(), new SmartBox());
            Physics.Server = true;

            LoadAllPlayers();
        }

        /// <summary>
        /// Populate a list of all players on the server
        /// This includes offline players, and these records are technically separate from the online records
        /// This method is a placeholder until syncing the offline data with the online Players is sorted out...
        /// </summary>
        public static void LoadAllPlayers()
        {
            // FIXME: this is a placeholder for offline players

            // probably bugged when players are added/removed...
            AllPlayers = new List<Player>();

            // get all character ids
            DatabaseManager.Shard.GetAllCharacters(characters =>
            {
                foreach (var character in characters)
                {
                    DatabaseManager.Shard.GetPlayerBiotas(character.Id, biotas =>
                    {
                        var session = new Session();
                        var player = new Player(biotas.Player, biotas.Inventory, biotas.WieldedItems, character, session);
                        AllPlayers.Add(player);
                    });
                }
            });
        }

        /// <summary>
        /// Returns an offline player record from the AllPlayers list
        /// </summary>
        /// <param name="playerGuid"></param>
        /// <returns></returns>
        public static Player GetOfflinePlayer(ObjectGuid playerGuid)
        {
            return AllPlayers.FirstOrDefault(p => p.Guid.Equals(playerGuid));
        }

        /// <summary>
        /// Syncs the cached offline player fields
        /// </summary>
        /// <param name="player">An online player</param>
        public static void SyncOffline(Player player)
        {
            var offlinePlayer = AllPlayers.FirstOrDefault(p => p.Guid.Full == player.Guid.Full);
            if (offlinePlayer == null) return;

            // FIXME: this is a placeholder for offline players
            offlinePlayer.Monarch = player.Monarch;
            offlinePlayer.Patron = player.Patron;

            offlinePlayer.AllegianceCPPool = player.AllegianceCPPool;
        }

        /// <summary>
        /// Syncs an online player with the cached offline fields
        /// </summary>
        /// <param name="player">An online player</param>
        public static void SyncOnline(Player player)
        {
            var offlinePlayer = AllPlayers.FirstOrDefault(p => p.Guid.Full == player.Guid.Full);
            if (offlinePlayer == null) return;

            // FIXME: this is a placeholder for offline players
            player.AllegianceCPPool = offlinePlayer.AllegianceCPPool;
        }

        public static void Initialize()
        {
            var thread = new Thread(UpdateWorld);
            thread.Name = "World Manager";
            thread.Start();
            log.DebugFormat("ServerTime initialized to {0}", WorldStartFromTime);
            log.DebugFormat($"Current maximum allowed sessions: {ConfigManager.Config.Server.Network.MaximumAllowedSessions}");
        }

        public static void ProcessPacket(ClientPacket packet, IPEndPoint endPoint)
        {
            if (packet.Header.HasFlag(PacketHeaderFlags.LoginRequest) && !loggedInClients.Contains(endPoint) &&
                loggedInClients.Count < ConfigManager.Config.Server.Network.MaximumAllowedSessions)
            {
                log.DebugFormat("Login Request from {0}", endPoint);
                var session = FindOrCreateSession(endPoint);
                if (session != null)
                    session.ProcessPacket(packet);
                
            }
            else if (sessionMap.Length > packet.Header.Id && loggedInClients.Contains(endPoint))
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

        public static Session FindOrCreateSession(IPEndPoint endPoint)
        {
            Session session = null;
            sessionLock.EnterWriteLock();
            try
            {
                session = sessions.SingleOrDefault(s => endPoint.Equals(s.EndPoint));
                if (session == null)
                {
                    for (ushort i = 0; i < sessionMap.Length; i++)
                    {
                        if (sessionMap[i] == null)
                        {
                            log.InfoFormat("Creating new session for {0} with id {1}", endPoint, i);
                            session = new Session(endPoint, i, ServerId);
                            sessions.Add(session);
                            sessionMap[i] = session;
                            loggedInClients.Add(endPoint);
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
                var errorSession = new Session(endPoint, (ushort)(sessionMap.Length + 1), ServerId);
                errorSession.SendCharacterError(Network.Enum.CharacterError.LogonServerFull);
            }
            return session;
        }

        /// <summary>
        /// Removes a player or worldobject from the active world.
        /// </summary>
        /// <param name="session"></param>
        public static void RemoveSession(Session session)
        {
            sessionLock.EnterWriteLock();
            try
            {
                log.InfoFormat("Removing session for {0} with id {1}", session.EndPoint, session.Network.ClientId);
                if (sessions.Contains(session))
                    sessions.Remove(session);
                if (sessionMap[session.Network.ClientId] == session)
                    sessionMap[session.Network.ClientId] = null;
                if (loggedInClients.Contains(session.EndPoint))
                    loggedInClients.Remove(session.EndPoint);
            }
            finally
            {
                sessionLock.ExitWriteLock();
            }
        }

        public static Session Find(uint accountId)
        {
            sessionLock.EnterReadLock();
            try
            {
                return sessions.SingleOrDefault(s => s.Id == accountId);
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
                return sessions.SingleOrDefault(s => s.Account == account);
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

        public static Player GetPlayerByGuidId(uint playerId, bool isOnlineRequired = true)
        {
            sessionLock.EnterReadLock();
            try
            {
                Session session;

                if (isOnlineRequired)
                    session = sessions.SingleOrDefault(s => s.Player != null && s.Player.IsOnline && s.Player.Guid.Full == playerId);
                else
                    session = sessions.SingleOrDefault(s => s.Player != null && s.Player.Guid.Full == playerId);

                return session != null ? session.Player : null;
            }
            finally
            {
                sessionLock.ExitReadLock();
            }
        }


        public static Player GetOfflinePlayerByGuidId(uint playerId)
        {
            return AllPlayers.FirstOrDefault(p => p.Guid.Full.Equals(playerId));
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

        /// <summary>
        /// Returns a list of all players currently online
        /// </summary>
        /// <param name="isOnlineRequired">false returns all players (offline or online)</param>
        /// <returns>List of all online players on the server</returns>
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

        /// <summary>
        /// Returns a list of all players who are under a monarch
        /// </summary>
        /// <param name="monarch">The monarch of an allegiance</param>
        public static List<Player> GetAllegiance(Player monarch)
        {
            sessionLock.EnterReadLock();
            try
            {
                return AllPlayers.Where(p => p.Monarch == monarch.Guid.Full).ToList();
            }
            finally
            {
                sessionLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Broadcasts GameMessage to all online sessions.
        /// </summary>
        public static void BroadcastToAll(GameMessage msg)
        {
            sessionLock.EnterReadLock();
            try
            {
                foreach (Session session in sessions.Where(s => s.Player != null && s.Player.IsOnline).ToList())
                {
                    session.Network.EnqueueSend(msg);
                }
            }
            finally
            {
                sessionLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Function to begin ending the operations inside of an active world.
        /// </summary>
        public static void StopWorld() { pendingWorldStop = true; }

        /// <summary>
        /// Manages updating all entities on the world.
        ///  - Server-side command-line commands are handled in their own thread.
        ///  - Network commands come from their own listener threads, and are queued in world objects
        ///  - This thread does the rest of the work!
        /// </summary>
        private static void UpdateWorld()
        {
            log.DebugFormat("Starting UpdateWorld thread");
            double lastTick = 0d;
            WorldActive = true;
            var worldTickTimer = new Stopwatch();
            while (!pendingWorldStop)
            {
                worldTickTimer.Restart();

                // handle time-based actions
                DelayManager.RunActions();

                // update positions through physics engine
                var movedObjects = HandlePhysics(PortalYearTicks);

                // iterate through objects that have changed landblocks
                foreach (var movedObject in movedObjects)
                {
                    // NOTE: The object's Location can now be null, if a player logs out, or an item is picked up
                    if (movedObject.Location == null) continue;

                    // assume adjacency move here?
                    LandblockManager.RelocateObjectForPhysics(movedObject, true);
                }

                InboundMessageQueue.RunActions();

                // Now, update actions within landblocks
                //   This is responsible for updating all "actors" residing within the landblock. 
                //   Objects and landblocks are "actors"
                //   "actors" decide if they want to read/modify their own state (set desired velocity), move-to positions, move items, read vitals, etc
                // N.B. -- Broadcasts are enqueued for sending at the end of the landblock's action time
                // FIXME(ddevec): Goal is to eventually migrate to an "Act" function of the LandblockManager ActiveLandblocks
                //    Inactive landblocks will be put on TimeoutManager queue for timeout killing
                LandblockActionQueue.RunActions();

                // XXX(ddevec): Should this be its own step in world-update thread?
                sessionLock.EnterReadLock();
                try
                {
                    // Send the current time ticks to allow sessions to declare themselves bad
                    Parallel.ForEach(sessions, s => s.Update(lastTick, DateTime.UtcNow.Ticks));
                }
                finally
                {
                    sessionLock.ExitReadLock();
                }

                // Removes sessions in the NetworkTimeout state, incuding sessions that have reached a timeout limit.
                var deadSessions = sessions.FindAll(s => s.State == Network.Enum.SessionState.NetworkTimeout);
                foreach (var session in deadSessions)
                    RemoveSession(session);

                // clean up inactive landblocks
                LandblockManager.UnloadLandblocks();

                Thread.Sleep(1);

                lastTick = (double)worldTickTimer.ElapsedTicks / Stopwatch.Frequency;
                PortalYearTicks += lastTick;
            }

            // World has finished operations and concedes the thread to garbage collection
            WorldActive = false;
        }

        /// <summary>
        /// A list of WorldObjects that have transitioned to adjacent landblocks for this update frame
        /// </summary>
        public static List<WorldObject> UpdateLandblock = new List<WorldObject>();

        /// <summary>
        /// The number of times per second physics updates are processed (inverted)
        /// </summary>
        public static double PhysicsRate = 1.0f / 60.0f;

        public static double LastPhysicsUpdate;

        /// <summary>
        /// Processes physics objects in all active landblocks for updating
        /// </summary>
        private static IEnumerable<WorldObject> HandlePhysics(double timeTick)
        {
            ConcurrentQueue<WorldObject> movedObjects = new ConcurrentQueue<WorldObject>();

            if (Server.Physics.Common.Timer.CurrentTime < LastPhysicsUpdate + PhysicsRate)
                return movedObjects;

            try
            {
                if (Concurrency)
                {
                    // Access ActiveLandblocks should be safe here, but sometimes crashes with
                    // System.InvalidOperationException: 'Collection was modified; enumeration operation may not execute.'
                    Parallel.ForEach(LandblockManager.ActiveLandblocks.Keys, landblock =>
                    {
                        HandlePhysicsLandblock(landblock, timeTick, movedObjects);
                    });
                }
                else
                {
                    foreach (var landblock in LandblockManager.ActiveLandblocks.Keys)
                        HandlePhysicsLandblock(landblock, timeTick, movedObjects);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);   // FIXME: concurrency + collection was modified
            }

            LastPhysicsUpdate = Server.Physics.Common.Timer.CurrentTime;

            return movedObjects;
        }

        public static void HandlePhysicsLandblock(Landblock landblock, double timeTick, ConcurrentQueue<WorldObject> movedObjects)
        {
            foreach (WorldObject wo in landblock.GetPhysicsWorldObjects())
            {
                Position newPosition = null;

                // set to TRUE if object changes landblock
                var landblockUpdate = false;

                // detect player movement
                // TODO: handle players the same as everything else
                var player = wo as Player;
                if (player != null)
                {
                    wo.InUpdate = true;

                    newPosition = HandlePlayerPhysics(player, timeTick);

                    // update position through physics engine
                    if (newPosition != null)
                        landblockUpdate = wo.UpdatePlayerPhysics(newPosition);

                    wo.InUpdate = false;
                }
                else
                    landblockUpdate = wo.UpdateObjectPhysics();

                if (landblockUpdate)
                    movedObjects.Enqueue(wo);
            }
        }

        /// <summary>
        /// Detects if player has moved through ForcedLocation or RequestedLocation
        /// </summary>
        private static Position HandlePlayerPhysics(Player player, double timeTick)
        {
            Position newPosition = null;

            if (player.ForcedLocation != null)
                newPosition = player.ForcedLocation;

            else if (player.RequestedLocation != null)
                newPosition = player.RequestedLocation;

            if (newPosition != null)
                player.ClearRequestedPositions();

            return newPosition;
        }
    }
}
