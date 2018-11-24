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
using ACE.Server.Entity;
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

        public static bool WorldActive { get; private set; }
        private static volatile bool pendingWorldStop;

        /// <summary>
        /// Handles ClientMessages in InboundMessageManager
        /// </summary>
        public static readonly ActionQueue InboundClientMessageQueue = new ActionQueue();
        private static readonly ActionQueue playerEnterWorldQueue = new ActionQueue();
        public static readonly DelayManager DelayManager = new DelayManager(); // TODO get rid of this. Each WO should have its own delayManager

        static WorldManager()
        {
            Physics = new PhysicsEngine(new ObjectMaint(), new SmartBox());
            Physics.Server = true;
        }

        public static void Initialize()
        {
            var thread = new Thread(UpdateWorld);
            thread.Name = "World Manager";
            thread.Start();
            log.DebugFormat("ServerTime initialized to {0}", Timers.WorldStartLoreTime);
            log.DebugFormat($"Current maximum allowed sessions: {ConfigManager.Config.Server.Network.MaximumAllowedSessions}");
        }

        public static void ProcessPacket(ClientPacket packet, IPEndPoint endPoint)
        {
            if (packet.Header.HasFlag(PacketHeaderFlags.LoginRequest))
            {
                if (!loggedInClients.Contains(endPoint) && loggedInClients.Count >= ConfigManager.Config.Server.Network.MaximumAllowedSessions)
                {
                    log.InfoFormat("Login Request from {0} rejected. Server full.", endPoint);
                    // TODO can we send a message back to the client indicating we're full?
                }
                else
                {
                    log.DebugFormat("Login Request from {0}", endPoint);
                    var session = FindOrCreateSession(endPoint);
                    if (session != null)
                        session.ProcessPacket(packet);
                }
            }
            else if (packet.Header.Id == 0 && packet.Header.HasFlag(PacketHeaderFlags.CICMDCommand))
            {
                // TODO: Not sure what to do with these packets yet
            }
            else if (sessionMap.Length > packet.Header.Id && loggedInClients.Contains(endPoint))
            {
                var session = sessionMap[packet.Header.Id];
                if (session != null)
                {
                    if (session.EndPoint.Equals(endPoint))
                        session.ProcessPacket(packet);
                    else
                        log.WarnFormat("Session for Id {0} has IP {1} but packet has IP {2}", packet.Header.Id, session.EndPoint, endPoint);
                }
                else
                {
                    log.WarnFormat("Null Session for Id {0}", packet.Header.Id);
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

        /// <summary>
        /// This will return a list of sessions that have this guid as a friend.
        /// </summary>
        public static List<Session> FindInverseFriends(ObjectGuid guid)
        {
            sessionLock.EnterReadLock();
            try
            {
                return sessions.Where(s => s.Player?.Character?.HasAsFriend(guid.Full, s.Player.CharacterDatabaseLock) == true).ToList();
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
            var offlinePlayer = PlayerManager.GetOfflinePlayer(character.Id);

            if (offlinePlayer == null)
            {
                log.Error($"PlayerEnterWorld requested for character.Id 0x{character.Id:X8} not found in PlayerManager OfflinePlayers.");
                return;
            }

            var start = DateTime.UtcNow;
            DatabaseManager.Shard.GetPossessedBiotasInParallel(character.Id, biotas =>
            {
                log.Debug($"GetPossessedBiotasInParallel for {character.Name} took {(DateTime.UtcNow - start).TotalMilliseconds:N0} ms");

                playerEnterWorldQueue.EnqueueAction(new ActionEventDelegate(() => DoPlayerEnterWorld(session, character, offlinePlayer.Biota, biotas)));
            });
        }

        private static void DoPlayerEnterWorld(Session session, Character character, Biota playerBiota, PossessedBiotas possessedBiotas)
        {
            Player player;

            if (playerBiota.WeenieType == (int)WeenieType.Admin)
                player = new Admin(playerBiota, possessedBiotas.Inventory, possessedBiotas.WieldedItems, character, session);
            else if (playerBiota.WeenieType == (int)WeenieType.Sentinel)
                player = new Sentinel(playerBiota, possessedBiotas.Inventory, possessedBiotas.WieldedItems, character, session);
            else
                player = new Player(playerBiota, possessedBiotas.Inventory, possessedBiotas.WieldedItems, character, session);

            session.SetPlayer(player);

            PlayerManager.SwitchPlayerFromOfflineToOnline(player);

            session.Player.PlayerEnterWorld();

            if (character.TotalLogins <= 1 || PropertyManager.GetBool("alwaysshowwelcome").Item)
            {
                // check the value of the welcome message. Only display it if it is not empty
                string welcomeHeader = !string.IsNullOrEmpty(ConfigManager.Config.Server.Welcome) ? ConfigManager.Config.Server.Welcome : "Welcome to Asheron's Call!";
                string msg = "To begin your training, speak to the Society Greeter. Walk up to the Society Greeter using the 'W' key, then double-click on her to initiate a conversation.";

                session.Network.EnqueueSend(new GameEventPopupString(session, $"{welcomeHeader}\n{msg}"));
            }

            LandblockManager.AddObject(session.Player, true);

            var motdString = PropertyManager.GetString("motd_string").Item;
            session.Network.EnqueueSend(new GameMessageSystemChat(motdString, ChatMessageType.Broadcast));
        }

        private static readonly RateLimiter updateGameWorldRateLimiter = new RateLimiter(60, TimeSpan.FromSeconds(1));

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

            WorldActive = true;
            var worldTickTimer = new Stopwatch();

            while (!pendingWorldStop)
            {
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

                worldTickTimer.Restart();

                PlayerManager.Tick();

                InboundClientMessageQueue.RunActions();

                playerEnterWorldQueue.RunActions();

                DelayManager.RunActions();

                var gameWorldUpdated = UpdateGameWorld();

                int sessionCount = DoSessionWork();

                // We only relax the CPU if our game world is able to update at the target rate.
                // We do not sleep if our game world just updated. This is to prevent the scenario where our game world can't keep up. We don't want to add further delays.
                // If our game world is able to keep up, it will not be updated on most ticks. It's on those ticks (between updates) that we will relax the CPU.
                if (!gameWorldUpdated)
                    Thread.Sleep(sessionCount == 0 ? 10 : 1); // Relax the CPU more if no sessions are connected

                Timers.PortalYearTicks += worldTickTimer.Elapsed.TotalSeconds;
            }

            // World has finished operations and concedes the thread to garbage collection
            WorldActive = false;
        }

        /// <summary>
        /// Projected to run at a reasonable rate for gameplay (30-60fps)
        /// </summary>
        public static bool UpdateGameWorld()
        {
            if (updateGameWorldRateLimiter.GetSecondsToWaitBeforeNextEvent() > 0)
                return false;

            updateGameWorldRateLimiter.RegisterEvent();

            // update positions through physics engine
            var movedObjects = HandlePhysics(Timers.PortalYearTicks);

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
                landblock.Tick(Time.GetUnixTime());

            // clean up inactive landblocks
            LandblockManager.UnloadLandblocks();

            return true;
        }

        /// <summary>
        /// Function to begin ending the operations inside of an active world.
        /// </summary>
        public static void StopWorld() { pendingWorldStop = true; }

        /// <summary>
        /// Processes physics objects in all active landblocks for updating
        /// </summary>
        private static IEnumerable<WorldObject> HandlePhysics(double timeTick)
        {
            ConcurrentQueue<WorldObject> movedObjects = new ConcurrentQueue<WorldObject>();
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

        /// <summary>
        /// Processes all inbound GameAction messages.<para />
        /// Dispatches all outgoing messages.<para />
        /// Removes dead sessions.
        /// </summary>
        public static int DoSessionWork()
        {
            int sessionCount;

            sessionLock.EnterUpgradeableReadLock();
            try
            {
                sessionCount = sessions.Count;

                // The session tick inbound processes all inbound GameAction messages
                foreach (var s in sessions)
                    s.TickInbound();

                // Do not combine the above and below loops. All inbound messages should be processed first and then all outbound messages should be processed second.

                // The session tick outbound processes pending actions and handles outgoing messages
                foreach (var s in sessions)
                    s.TickOutbound();

                // Removes sessions in the NetworkTimeout state, including sessions that have reached a timeout limit.
                var deadSessions = sessions.FindAll(s => s.State == Network.Enum.SessionState.NetworkTimeout);

                foreach (var session in deadSessions)
                {
                    log.Info($"client {session.Account} dropped");
                    RemoveSession(session); // This will temporarily upgrade our ReadLock to a WriteLock
                }
            }
            finally
            {
                sessionLock.ExitUpgradeableReadLock();
            }
            return sessionCount;
        }
    }
}
