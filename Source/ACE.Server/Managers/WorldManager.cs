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
using ACE.Database;
using ACE.Database.Entity;
using ACE.Database.Models.Shard;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity.Actions;
using ACE.Server.WorldObjects;
using ACE.Server.Network;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Physics;
using ACE.Server.Physics.Common;

using Landblock = ACE.Server.Entity.Landblock;
using Position = ACE.Entity.Position;

namespace ACE.Server.Managers
{
    public static class WorldManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
        private static readonly List<Session> sessions = new List<Session>();
        private static readonly List<IPEndPoint> loggedInClients = new List<IPEndPoint>((int)ConfigManager.Config.Server.Network.MaximumAllowedSessions);

        public static bool Concurrency = false;

        private static readonly PhysicsEngine Physics;

        public static DateTime WorldStartTime { get; } = DateTime.UtcNow;
        public static DerethDateTime WorldStartFromTime { get; } = new DerethDateTime().UTCNowToLoreTime;
        public static double PortalYearTicks { get; private set; } = WorldStartFromTime.Ticks;

        public static bool WorldActive { get; private set; }
        private static volatile bool pendingWorldStop;

        /// <summary>
        /// Handles ClientMessages in InboundMessageManager
        /// </summary>
        public static readonly ActionQueue InboundClientMessageQueue = new ActionQueue();
        private static readonly ActionQueue playerEnterWorldQueue = new ActionQueue();
        public static readonly DelayManager DelayManager = new DelayManager(); // TODO get rid of this. Each WO should have its own delayManager

        public static List<Player> AllPlayers;

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
            if (packet.Header.HasFlag(PacketHeaderFlags.LoginRequest) && !loggedInClients.Contains(endPoint) && loggedInClients.Count < ConfigManager.Config.Server.Network.MaximumAllowedSessions)
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
            Session session;

            sessionLock.EnterUpgradeableReadLock();
            try
            {
                session = sessions.SingleOrDefault(s => endPoint.Equals(s.EndPoint));
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
                                session = new Session(endPoint, i, ServerId);
                                sessions.Add(session);
                                sessionMap[i] = session;
                                loggedInClients.Add(endPoint);
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
        /// Removes a session, network client and network endpoint from the various tracker objects.
        /// </summary>
        public static void RemoveSession(Session session)
        {
            sessionLock.EnterWriteLock();
            try
            {
                log.DebugFormat("Removing session for {0} with id {1}", session.EndPoint, session.Network.ClientId);
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

                return session?.Player;
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
        /// Returns a list of all sessions currently connected
        /// </summary>
        /// <param name="isOnlineRequired">false returns all players (offline or online)</param>
        /// <returns>List of all active sessions to the server</returns>
        public static List<Session> GetAll(bool isOnlineRequired = true)
        {
            sessionLock.EnterReadLock();
            try
            {
                if (isOnlineRequired)
                    return sessions.Where(s => s.Player != null && s.Player.IsOnline).ToList();

                return sessions.ToList();
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
                    session.Network.EnqueueSend(msg);
            }
            finally
            {
                sessionLock.ExitReadLock();
            }
        }

        public static void PlayerEnterWorld(Session session, Character character)
        {
            var start = DateTime.UtcNow;
            DatabaseManager.Shard.GetPlayerBiotas(character.Id, biotas =>
            {
                log.Debug($"GetPlayerBiotas for {character.Name} took {(DateTime.UtcNow - start).TotalMilliseconds:N0} ms");

                playerEnterWorldQueue.EnqueueAction(new ActionEventDelegate(() => DoPlayerEnterWorld(session, character, biotas)));
            });
        }

        private static void DoPlayerEnterWorld(Session session, Character character, PlayerBiotas biotas)
        {
            Player player;

            if (biotas.Player.WeenieType == (int)WeenieType.Admin)
                player = new Admin(biotas.Player, biotas.Inventory, biotas.WieldedItems, character, session);
            else if (biotas.Player.WeenieType == (int)WeenieType.Sentinel)
                player = new Sentinel(biotas.Player, biotas.Inventory, biotas.WieldedItems, character, session);
            else
                player = new Player(biotas.Player, biotas.Inventory, biotas.WieldedItems, character, session);

            session.SetPlayer(player);
            session.Player.PlayerEnterWorld();

            if (character.TotalLogins <= 1 || PropertyManager.GetBool("alwaysshowwelcome").Item)
            {
                // check the value of the welcome message. Only display it if it is not empty
                string welcomeHeader = ConfigManager.Config.Server.Welcome ?? "Welcome to Asheron's Call!";
                string msg = "To begin your training, speak to the Society Greeter. Walk up to the Society Greeter using the 'W' key, then double-click on her to initiate a conversation.";

                session.Network.EnqueueSend(new GameEventPopupString(session, $"{welcomeHeader}\n{msg}"));
            }

            LandblockManager.AddObject(session.Player, true);

            var motdString = PropertyManager.GetString("motd_string").Item;
            session.Network.EnqueueSend(new GameMessageSystemChat(motdString, ChatMessageType.Broadcast));
        }

        /// <summary>
        /// Manages updating all entities on the world.
        ///  - Server-side command-line commands are handled in their own thread.
        ///  - Database I/O is handled in its own thread.
        ///  - Network commands come from their own listener threads, and are queued for each sessions which are then processed here.
        ///  - This thread does the rest of the work!
        /// </summary>
        private static void UpdateWorld()
        {
            log.DebugFormat("Starting UpdateWorld thread");

            double lastTickDuration = 0d;
            WorldActive = true;
            var worldTickTimer = new Stopwatch();

            while (!pendingWorldStop)
            {
                worldTickTimer.Restart();

                /*
                When it comes to thread safety for Landblocks and WorldObjects, ACE makes the following assumptions:

                 * Inbound ClientMessages and GameActions are handled on the main UpdateWorld thread.
                   - These actions may load Landblocks and modify other WorldObjects safely.

                 * PlayerEnterWorld queue is run on the main UpdateWorld thread.
                   - These actions may load Landblocks and modify other WorldObjects safely.

                 * Landblock Groups (calculated by LandblockManager) can be processed in parallel.

                 * Adjacent Landblocks will always be run on the same thread.

                 * Non-adjacent landblocks might be run on different threads.
                   - If two non-adjacent landblocks both touch the same landblock, and that landblock is active, they will be run on the same thread.

                 * Database results are returned from a task spawned in SerializedShardDatabase (via callback).
                   - Minimal processing should be done from the callback. Return as quickly as possible to let the database thread do database work.
                   - The processing of these results should be queued to an ActionQueue

                 * The only cases where it's acceptable for to create a new Task, Thread or Parallel loop are the following:
                   - Every scenario must be one where you don't care about breaking ACE
                   - DeveloperCommand Handlers

                 * TODO: We need a thread safe way to handle object transitions between distant landblocks
                */

                InboundClientMessageQueue.RunActions();

                playerEnterWorldQueue.RunActions();

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

                // Tick all of our Landblocks and WorldObjects
                var activeLandblocks = LandblockManager.GetActiveLandblocks();

                foreach (var landblock in activeLandblocks)
                    landblock.Tick(lastTickDuration, Time.GetUnixTime());

                // clean up inactive landblocks
                LandblockManager.UnloadLandblocks();

                // Session Maintenance
                int sessionCount;

                sessionLock.EnterUpgradeableReadLock();
                try
                {
                    sessionCount = sessions.Count;

                    // The session tick processes all inbound GameAction messages
                    foreach (var s in sessions)
                        s.Tick(lastTickDuration);

                    // Send the current time ticks to allow sessions to declare themselves bad
                    Parallel.ForEach(sessions, s => s.TickInParallel(lastTickDuration));

                    // Removes sessions in the NetworkTimeout state, incuding sessions that have reached a timeout limit.
                    var deadSessions = sessions.FindAll(s => s.State == Network.Enum.SessionState.NetworkTimeout);

                    foreach (var session in deadSessions)
                    {
                        log.Info($"client {session.Account} dropped");
                        RemoveSession(session);
                    }
                }
                finally
                {
                    sessionLock.ExitUpgradeableReadLock();
                }

                Thread.Sleep(sessionCount == 0 ? 10 : 1); // Relax the CPU if no sessions are connected

                lastTickDuration = worldTickTimer.Elapsed.TotalSeconds;
                PortalYearTicks += lastTickDuration;
            }

            // World has finished operations and concedes the thread to garbage collection
            WorldActive = false;
        }

        /// <summary>
        /// Function to begin ending the operations inside of an active world.
        /// </summary>
        public static void StopWorld() { pendingWorldStop = true; }

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

            if (PhysicsTimer.CurrentTime < LastPhysicsUpdate + PhysicsRate)
                return movedObjects;

            try
            {
                var activeLandblocks = LandblockManager.GetActiveLandblocks();

                if (Concurrency)
                {
                    // Access ActiveLandblocks should be safe here, but sometimes crashes with
                    // System.InvalidOperationException: 'Collection was modified; enumeration operation may not execute.'
                    Parallel.ForEach(activeLandblocks, landblock =>
                    {
                        HandlePhysicsLandblock(landblock, timeTick, movedObjects);
                    });
                }
                else
                {
                    foreach (var landblock in activeLandblocks)
                        HandlePhysicsLandblock(landblock, timeTick, movedObjects);
                }
            }
            catch (Exception e)
            {
                log.Error(e);
            }

            LastPhysicsUpdate = PhysicsTimer.CurrentTime;

            return movedObjects;
        }

        private static void HandlePhysicsLandblock(Landblock landblock, double timeTick, ConcurrentQueue<WorldObject> movedObjects)
        {
            foreach (WorldObject wo in landblock.GetPhysicsWorldObjects())
            {
                // set to TRUE if object changes landblock
                var landblockUpdate = false;

                // detect player movement
                // TODO: handle players the same as everything else
                if (wo is Player player)
                {
                    wo.InUpdate = true;

                    var newPosition = HandlePlayerPhysics(player, timeTick);

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
