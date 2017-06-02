﻿using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using ACE.Common;
using ACE.Entity;
using ACE.Network;

using ACE.Entity.Actions;

using log4net;

namespace ACE.Managers
{
    public static class WorldManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // Hard coded server Id, this will need to change if we move to multi-process or multi-server model
        public const ushort ServerId = 0xB;
        private static Session[] sessionMap = new Session[128]; // TODO Placeholder, should be config MaxSessions
        private static readonly List<Session> sessions = new List<Session>();
        private static readonly ReaderWriterLockSlim sessionLock = new ReaderWriterLockSlim();

        private static bool pendingWorldStop;

        public static DateTime WorldStartTime { get; } = DateTime.UtcNow;

        public static DerethDateTime WorldStartFromTime { get; } = new DerethDateTime().UTCNowToLoreTime;

        public static double PortalYearTicks { get; private set; } = WorldStartFromTime.Ticks;

        public static readonly ActionQueue ActionQueue = new ActionQueue();
        public static readonly ActionQueue MotionQueue = new ActionQueue();
        public static readonly ActionQueue BroadcastQueue = new ActionQueue();

        public static readonly DelayManager DelayManager = new DelayManager();

        public static void Initialize()
        {
            var thread = new Thread(UpdateWorld);
            thread.Start();
            log.DebugFormat("ServerTime initialized to {0}", WorldStartFromTime.ToString());
        }

        public static void ProcessPacket(ClientPacket packet, IPEndPoint endPoint)
        {
            if (packet.Header.HasFlag(PacketHeaderFlags.LoginRequest))
            {
                log.DebugFormat("Login Request from {0}", endPoint);
                var session = FindOrCreateSession(endPoint);
                if (session != null)
                    session.ProcessPacket(packet);
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

        /// <summary>
        /// Manages updating all entities on the world.
        ///  - Nerver-side command-line commands are handled in their own thread.
        ///  - Network commands come from their own listener threads, and are queued in world objects
        ///  - This thread does the rest of the work!
        /// </summary>
        private static void UpdateWorld()
        {
            log.DebugFormat("Starting UpdateWorld thread");
            double lastTick = 0d;

            var worldTickTimer = new Stopwatch();
            while (!pendingWorldStop)
            {
                worldTickTimer.Restart();

                // Handle time-based timeouts
                DelayManager.RunActions();

                // Sequences of update thread:
                // Update positions based on new tick
                // TODO(ddevec): Physics here
                IEnumerable<WorldObject> movedObjects = FakePhysics(PortalYearTicks);

                // Do any pre-calculated landblock transfers --
                Parallel.ForEach(movedObjects, wo =>
                {
                    // If it was picked up, or moved
                    if (wo.PhysicsData.Position.LandblockId != wo.CurrentLandblock.Id)
                    {
                        LandblockManager.RelocateObject(wo);
                    }
                });

                // FIXME(ddevec): This O(n^2) tracking loop is a remenant of the old structure -- we should probably come up with a more efficient tracking scheme
                Parallel.ForEach(movedObjects, mo =>
                {
                    // detect all world objects in ghost range
                    List<WorldObject> woproxghost = new List<WorldObject>();
                    woproxghost.AddRange(mo.CurrentLandblock.GetWorldObjectsInRangeForPhysics(mo, Landblock.maxobjectGhostRange));

                    // for all objects in range of this moving object or in ghost range of moving object update them.
                    Parallel.ForEach(woproxghost, gwo =>
                    {
                        if (mo.Guid.IsPlayer())
                        {
                            // if world object is in active zone then.
                            if (gwo.Location.SquaredDistanceTo(mo.Location) <= Landblock.maxobjectRange)
                            {
                                // if world object is in active zone.
                                if (!(mo as Player).GetTrackedObjectGuids().Contains(gwo.Guid))
                                    (mo as Player).TrackObject(gwo);
                            }
                            // if world object is in ghost zone and outside of active zone
                            else
                            {
                                if ((mo as Player).GetTrackedObjectGuids().Contains(gwo.Guid))
                                {
                                    (mo as Player).StopTrackingObject(gwo, true);
                                }
                            }
                        }
                    });
                });

                // Process between landblock object motions sequentially -- for safety
                // Currently only used for picking items up off a landblock
                MotionQueue.RunActions();

                // Now, update actions within landblocks
                //   This is responsible for updating all "actors" residing within the landblock. 
                //   Objects and landblocks are "actors"
                //   "actors" decide if they want to read/modify their own state (set desired velocity), move-to positions, move items, read vitals, etc
                //   RULE: action functions CANNOT read/modify other objects 
                //      -- unless they "own" the other object: e.g. in a container
                //      -- action objects must send requests (actions) to other actor's action queues, and have them do modification
                // N.B. -- Broadcasts are enqueued for sending at the end of the landblock's action time
                // FIXME(ddevec): Goal is to eventually migrate to an "Act" function of the LandblockManager ActiveLandblocks
                //    Inactive landblocks will be put on TimeoutManager queue for timeout killing
                ActionQueue.RunActionsParallel();

                // Handles sending out all per-landblock broadcasts -- This may rework when we rework tracking -- tbd
                BroadcastQueue.RunActionsParallel();

                // XXX(ddevec): Should this be its own step in world-update thread?
                sessionLock.EnterReadLock();
                try
                {
                    Parallel.ForEach(sessions, s => s.Update(lastTick));
                }
                finally
                {
                    sessionLock.ExitReadLock();
                }

                Thread.Sleep(1);

                lastTick = (double)worldTickTimer.ElapsedTicks / Stopwatch.Frequency;
                PortalYearTicks += lastTick;
            }
        }

        private static IEnumerable<WorldObject> FakePhysics(double timeTick)
        {
            ConcurrentQueue<WorldObject> movedObjects = new ConcurrentQueue<WorldObject>();
            // Accessing ActiveLandblocks is safe here -- nothing can modify the landblocks at this point
            Parallel.ForEach(LandblockManager.ActiveLandblocks, landblock =>
            {
                foreach (WorldObject wo in landblock.GetPhysicsWorldObjects())
                {
                    Position newPosition = wo.Location;
                    if (wo.ForcedLocation != null)
                    {
                        newPosition = wo.ForcedLocation;
                        movedObjects.Enqueue(wo);
                    }
                    else if (wo.RequestedLocation != null)
                    {
                        newPosition = wo.RequestedLocation;
                        movedObjects.Enqueue(wo);
                    }

                    if (newPosition != wo.Location)
                    {
                        wo.PhysicsUpdatePosition(newPosition);
                    }
                }
            });

            return movedObjects;
        }

        public static double SecondsToTicks(double elapsedTimeSeconds)
        {
            return elapsedTimeSeconds;
        }
    }
}
