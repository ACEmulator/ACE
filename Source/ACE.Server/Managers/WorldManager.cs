using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using log4net;

using ACE.Common;
using ACE.Database;
using ACE.Database.Entity;
using ACE.Database.Models.Shard;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.WorldObjects;
using ACE.Server.Network;
using ACE.Server.Network.Packets;
using ACE.Server.Network.Handlers;
using ACE.Server.Network.Enum;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Physics;
using ACE.Server.Physics.Common;

using Landblock = ACE.Server.Entity.Landblock;
using Position = ACE.Entity.Position;
using ACE.Server.Network.Managers;

namespace ACE.Server.Managers
{
    public static class WorldManager
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

        public static bool Concurrency = false;

        private static readonly PhysicsEngine Physics;

        public static bool WorldActive { get; private set; }
        private static volatile bool pendingWorldStop;

        /// <summary>
        /// Handles ClientMessages in InboundMessageManager
        /// </summary>
        public static readonly ActionQueue InboundMessageQueue = new ActionQueue();

        private static readonly ActionQueue actionQueue = new ActionQueue();
        public static readonly DelayManager DelayManager = new DelayManager(); // TODO get rid of this. Each WO should have its own delayManager
        private static readonly OutboundPacketQueue OutboundQueue = null;

        static WorldManager()
        {
            OutboundQueue = SocketManager.OutboundQueue;
            Physics = new PhysicsEngine(new ObjectMaint(), new SmartBox());
            Physics.Server = true;
        }

        public static void Initialize()
        {
            var thread = new Thread(() =>
            {
                LandblockManager.PreloadConfigLandblocks();
                UpdateWorld();
            });
            thread.Name = "World Manager";
            thread.Start();
            log.DebugFormat("ServerTime initialized to {0}", Timers.WorldStartLoreTime);
            log.DebugFormat($"Current maximum allowed sessions: {ConfigManager.Config.Server.Network.MaximumAllowedSessions}");
        }

        public static void ProcessPacket(ClientPacket packet, IPEndPoint endPoint, IPEndPoint listenerEndpoint)
        {
            if (listenerEndpoint.Port == ConfigManager.Config.Server.Network.Port + 1)
            {
                ServerPerformanceMonitor.RegisterEventStart(ServerPerformanceMonitor.MonitorType.ProcessPacket_1);
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
                ServerPerformanceMonitor.RegisterEventStart(ServerPerformanceMonitor.MonitorType.ProcessPacket_0);
                if (packet.Header.HasFlag(PacketHeaderFlags.LoginRequest))
                {
                    packetLog.Debug($"{packet}, {endPoint}");
                    if (GetSessionCount() >= ConfigManager.Config.Server.Network.MaximumAllowedSessions)
                    {
                        log.InfoFormat("Login Request from {0} rejected. Server full.", endPoint);
                        SendLoginRequestReject(endPoint, CharacterError.LogonServerFull);
                    }
                    else if (ServerManager.ShutdownInitiated)
                    {
                        log.InfoFormat("Login Request from {0} rejected. Server shutting down.", endPoint);
                        SendLoginRequestReject(endPoint, CharacterError.ServerCrash);
                    }
                    else
                    {
                        log.DebugFormat("Login Request from {0}", endPoint);
                        var session = FindOrCreateSession(endPoint);
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
                            SendLoginRequestReject(endPoint, CharacterError.LogonServerFull);
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
                            log.WarnFormat("Session for Id {0} has IP {1} but packet has IP {2}", packet.Header.Id, session.EndPoint, endPoint);
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

        private static void SendLoginRequestReject(IPEndPoint endPoint, CharacterError error)
        {
            var tempSession = new Session(endPoint, (ushort)(sessionMap.Length + 1), ServerId);

            // First we must send the connect request response
            var connectRequest = new PacketOutboundConnectRequest(
                tempSession.Network.ConnectionData.ServerTime,
                tempSession.Network.ConnectionData.ConnectionCookie,
                tempSession.Network.ClientId,
                tempSession.Network.ConnectionData.ServerSeed,
                tempSession.Network.ConnectionData.ClientSeed);
            tempSession.Network.ConnectionData.DiscardSeeds();
            tempSession.Network.EnqueueSend(connectRequest);

            // Then we send the error
            tempSession.SendCharacterError(error);

            tempSession.Network.Update();
        }

        public static Session FindOrCreateSession(IPEndPoint endPoint)
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
                                session = new Session(endPoint, i, ServerId);
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

                actionQueue.EnqueueAction(new ActionEventDelegate(() => DoPlayerEnterWorld(session, character, offlinePlayer.Biota, biotas)));
            });
        }

        private static void DoPlayerEnterWorld(Session session, Character character, Biota playerBiota, PossessedBiotas possessedBiotas)
        {
            Player player;

            Player.HandleNoLogLandblock(playerBiota);

            var stripAdminProperties = false;
            var addAdminProperties = false;
            var addSentinelProperties = false;
            if (ConfigManager.Config.Server.Accounts.OverrideCharacterPermissions)
            {
                if (session.AccessLevel <= AccessLevel.Advocate) // check for elevated characters
                {
                    if (playerBiota.WeenieType == (int)WeenieType.Admin || playerBiota.WeenieType == (int)WeenieType.Sentinel) // Downgrade weenie
                    {
                        character.IsPlussed = false;
                        playerBiota.WeenieType = (int)WeenieType.Creature;
                        stripAdminProperties = true;
                    }
                }
                else if (session.AccessLevel >= AccessLevel.Sentinel && session.AccessLevel <= AccessLevel.Envoy)
                {
                    if (playerBiota.WeenieType == (int)WeenieType.Creature || playerBiota.WeenieType == (int)WeenieType.Admin) // Up/downgrade weenie
                    {
                        character.IsPlussed = true;
                        playerBiota.WeenieType = (int)WeenieType.Sentinel;
                        addSentinelProperties = true;
                    }
                }
                else // Developers and Admins
                {
                    if (playerBiota.WeenieType == (int)WeenieType.Creature || playerBiota.WeenieType == (int)WeenieType.Sentinel) // Up/downgrade weenie
                    {
                        character.IsPlussed = true;
                        playerBiota.WeenieType = (int)WeenieType.Admin;
                        addAdminProperties = true;
                    }
                }
            }

            if (playerBiota.WeenieType == (int)WeenieType.Admin)
                player = new Admin(playerBiota, possessedBiotas.Inventory, possessedBiotas.WieldedItems, character, session);
            else if (playerBiota.WeenieType == (int)WeenieType.Sentinel)
                player = new Sentinel(playerBiota, possessedBiotas.Inventory, possessedBiotas.WieldedItems, character, session);
            else
                player = new Player(playerBiota, possessedBiotas.Inventory, possessedBiotas.WieldedItems, character, session);

            session.SetPlayer(player);

            if (stripAdminProperties) // continue stripping properties
            {
                player.CloakStatus = null;
                player.Attackable = true;
                player.SetProperty(ACE.Entity.Enum.Properties.PropertyBool.DamagedByCollisions, true);
                player.AdvocateLevel = null;
                player.ChannelsActive = null;
                player.ChannelsAllowed = null;
                player.Invincible = null;
                player.Cloaked = null;


                player.ChangesDetected = true;
                player.CharacterChangesDetected = true;
            }

            if (addSentinelProperties || addAdminProperties) // continue restoring properties to default
            {
                WorldObject weenie;

                if (addAdminProperties)
                    weenie = Factories.WorldObjectFactory.CreateWorldObject(DatabaseManager.World.GetCachedWeenie("admin"), new ACE.Entity.ObjectGuid(ACE.Entity.ObjectGuid.Invalid.Full)) as Admin;
                else
                    weenie = Factories.WorldObjectFactory.CreateWorldObject(DatabaseManager.World.GetCachedWeenie("sentinel"), new ACE.Entity.ObjectGuid(ACE.Entity.ObjectGuid.Invalid.Full)) as Sentinel;

                if (weenie != null)
                {
                    player.CloakStatus = CloakStatus.Off;
                    player.Attackable = weenie.Attackable;
                    player.SetProperty(ACE.Entity.Enum.Properties.PropertyBool.DamagedByCollisions, false);
                    player.AdvocateLevel = weenie.GetProperty(ACE.Entity.Enum.Properties.PropertyInt.AdvocateLevel);
                    player.ChannelsActive = (Channel?)weenie.GetProperty(ACE.Entity.Enum.Properties.PropertyInt.ChannelsActive);
                    player.ChannelsAllowed = (Channel?)weenie.GetProperty(ACE.Entity.Enum.Properties.PropertyInt.ChannelsAllowed);
                    player.Invincible = false;
                    player.Cloaked = false;


                    player.ChangesDetected = true;
                    player.CharacterChangesDetected = true;
                }
            }

            // If the client is missing a location, we start them off in the starter dungeon
            if (session.Player.Location == null)
            {
                if (session.Player.Instantiation != null)
                    session.Player.Location = new Position(session.Player.Instantiation);
                else
                    session.Player.Location = new Position(2349072813, 12.3199f, -28.482f, 0.0049999995f, 0.0f, 0.0f, -0.9408059f, -0.3389459f);
            }

            session.Player.PlayerEnterWorld();

            LandblockManager.AddObject(session.Player, true);

            var popup_header = PropertyManager.GetString("popup_header").Item;
            var popup_motd = PropertyManager.GetString("popup_motd").Item;
            var popup_welcome = PropertyManager.GetString("popup_welcome").Item;

            if (character.TotalLogins <= 1)
            {
                session.Network.EnqueueSend(new GameEventPopupString(session, AppendLines(popup_header, popup_motd, popup_welcome)));
            }
            else if (!string.IsNullOrEmpty(popup_motd))
            {
                session.Network.EnqueueSend(new GameEventPopupString(session, AppendLines(popup_header, popup_motd)));
            }

            var info = "Welcome to Asheron's Call\n  powered by ACEmulator\n\nFor more information on commands supported by this server, type @acehelp\n";
            session.Network.EnqueueSend(new GameMessageSystemChat(info, ChatMessageType.Broadcast));

            var server_motd = PropertyManager.GetString("server_motd").Item;
            if (!string.IsNullOrEmpty(server_motd))
                session.Network.EnqueueSend(new GameMessageSystemChat($"{server_motd}\n", ChatMessageType.Broadcast));
        }

        private static string AppendLines(params string[] lines)
        {
            var result = "";
            foreach (var line in lines)
                if (!string.IsNullOrEmpty(line))
                    result += $"{line}\n";

            return Regex.Replace(result, "\n$", "");
        }

        public static void EnqueueAction(IAction action)
        {
            actionQueue.EnqueueAction(action);
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

                ServerPerformanceMonitor.RegisterEventStart(ServerPerformanceMonitor.MonitorType.PlayerManager_Tick);
                PlayerManager.Tick();
                ServerPerformanceMonitor.RegisterEventEnd(ServerPerformanceMonitor.MonitorType.PlayerManager_Tick);

                ServerPerformanceMonitor.RegisterEventStart(ServerPerformanceMonitor.MonitorType.InboundClientMessageQueueRun);
                InboundMessageQueue.RunActions();
                ServerPerformanceMonitor.RegisterEventEnd(ServerPerformanceMonitor.MonitorType.InboundClientMessageQueueRun);

                // This will consist of PlayerEnterWorld actions, as well as other game world actions that require thread safety
                ServerPerformanceMonitor.RegisterEventStart(ServerPerformanceMonitor.MonitorType.actionQueue_RunActions);
                actionQueue.RunActions();
                ServerPerformanceMonitor.RegisterEventEnd(ServerPerformanceMonitor.MonitorType.actionQueue_RunActions);

                ServerPerformanceMonitor.RegisterEventStart(ServerPerformanceMonitor.MonitorType.DelayManager_RunActions);
                DelayManager.RunActions();
                ServerPerformanceMonitor.RegisterEventEnd(ServerPerformanceMonitor.MonitorType.DelayManager_RunActions);

                ServerPerformanceMonitor.RegisterEventStart(ServerPerformanceMonitor.MonitorType.UpdateGameWorld);
                var gameWorldUpdated = UpdateGameWorld();
                ServerPerformanceMonitor.RegisterEventEnd(ServerPerformanceMonitor.MonitorType.UpdateGameWorld);

                ServerPerformanceMonitor.RegisterEventStart(ServerPerformanceMonitor.MonitorType.DoSessionWork);
                int sessionCount = DoSessionWork();
                ServerPerformanceMonitor.RegisterEventEnd(ServerPerformanceMonitor.MonitorType.DoSessionWork);

                ServerPerformanceMonitor.Tick();

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

            ServerPerformanceMonitor.RegisterEventStart(ServerPerformanceMonitor.MonitorType.UpdateGameWorld_Entire);

            // update positions through physics engine
            ServerPerformanceMonitor.RegisterEventStart(ServerPerformanceMonitor.MonitorType.UpdateGameWorld_HandlePhysics);
            var movedObjects = HandlePhysics(Timers.PortalYearTicks);
            ServerPerformanceMonitor.RegisterEventEnd(ServerPerformanceMonitor.MonitorType.UpdateGameWorld_HandlePhysics);

            // iterate through objects that have changed landblocks
            ServerPerformanceMonitor.RegisterEventStart(ServerPerformanceMonitor.MonitorType.UpdateGameWorld_RelocateObjectForPhysics);
            foreach (var movedObject in movedObjects)
            {
                // NOTE: The object's Location can now be null, if a player logs out, or an item is picked up
                if (movedObject.Location == null) continue;

                // assume adjacency move here?
                LandblockManager.RelocateObjectForPhysics(movedObject, true);
            }
            ServerPerformanceMonitor.RegisterEventEnd(ServerPerformanceMonitor.MonitorType.UpdateGameWorld_RelocateObjectForPhysics);

            // Tick all of our Landblocks and WorldObjects
            ServerPerformanceMonitor.RegisterEventStart(ServerPerformanceMonitor.MonitorType.UpdateGameWorld_landblock_Tick);
            var activeLandblocks = LandblockManager.GetActiveLandblocks();

            foreach (var landblock in activeLandblocks)
                landblock.Tick(Time.GetUnixTime());
            ServerPerformanceMonitor.RegisterEventEnd(ServerPerformanceMonitor.MonitorType.UpdateGameWorld_landblock_Tick);

            // clean up inactive landblocks
            LandblockManager.UnloadLandblocks();

            HouseManager.Tick();

            ServerPerformanceMonitor.RegisterEventEnd(ServerPerformanceMonitor.MonitorType.UpdateGameWorld_Entire);

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
            foreach (WorldObject wo in landblock.GetWorldObjectsForPhysicsHandling())
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
            int sessionCount = 0;

            sessionLock.EnterUpgradeableReadLock();
            try
            {
                // The session tick outbound processes pending actions and handles outgoing messages
                ServerPerformanceMonitor.RegisterEventStart(ServerPerformanceMonitor.MonitorType.DoSessionWork_TickOutbound);
                foreach (var s in sessionMap)
                    s?.TickOutbound();

                OutboundQueue.SendAll();

                ServerPerformanceMonitor.RegisterEventEnd(ServerPerformanceMonitor.MonitorType.DoSessionWork_TickOutbound);

                // Removes sessions in the NetworkTimeout state, including sessions that have reached a timeout limit.
                ServerPerformanceMonitor.RegisterEventStart(ServerPerformanceMonitor.MonitorType.DoSessionWork_RemoveSessions);
                foreach (var session in sessionMap.Where(k => !Equals(null, k)))
                {
                    var pendingTerm = session.PendingTermination;
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
    }
}
