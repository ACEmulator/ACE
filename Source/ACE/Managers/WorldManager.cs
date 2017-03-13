using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;

using ACE.Network;
using ACE.Entity;
using ACE.Common;
using System.Threading.Tasks;

namespace ACE.Managers
{
    public static class WorldManager
    {
        // commented: unused.  uncomment if you'll use it.
        // private static uint sessionTimeout = 150u; // max time between packets before the client disconnects

        private static readonly List<Session> sessionStore = new List<Session>();
        private static readonly ReaderWriterLockSlim sessionLock = new ReaderWriterLockSlim();

        private static volatile bool pendingWorldStop;

        public static DateTime WorldStartTime { get; } = DateTime.UtcNow;

        public static DerethDateTime WorldStartFromTime { get; } = new DerethDateTime().UTCNowToLoreTime;

        public static double PortalYearTicks { get; set; } = WorldStartFromTime.Ticks;

        public static void Initialise()
        {
            var thread = new Thread(UpdateWorld);
            thread.Start();

            Console.WriteLine("");
            Console.WriteLine("ServerTime initialized to " + WorldStartFromTime.ToString());
        }

        public static Session Find(IPEndPoint endPoint)
        {
            Session session;

            sessionLock.EnterReadLock();
            try
            {
                session = sessionStore.SingleOrDefault(s => endPoint.Equals(s.EndPoint));
                if (session != null)
                    return session;
            }
            finally
            {
                sessionLock.ExitReadLock();
            }

            sessionLock.EnterWriteLock();
            try
            {
                // this needs to be checked again, another thread could of created a session between the read and write lock
                session = sessionStore.SingleOrDefault(s => endPoint.Equals(s.EndPoint));
                if (session != null)
                    return session;

                session = new Session(endPoint);
                sessionStore.Add(session);
            }
            finally
            {
                sessionLock.ExitWriteLock();
            }

            return session;
        }

        public static Session Find(string account)
        {
            sessionLock.EnterReadLock();
            try
            {
                return sessionStore.SingleOrDefault(s => s.Account == account);
            }
            finally
            {
                sessionLock.ExitReadLock();
            }
        }

        public static Session Find(ulong connectionKey)
        {
            sessionLock.EnterReadLock();
            try
            {
                return sessionStore.SingleOrDefault(s => s.WorldConnectionKey == connectionKey);
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
                return sessionStore.SingleOrDefault(s => s.Player?.Guid.Low == characterGuid.Low);
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
                    return sessionStore.SingleOrDefault(s => s.Player != null && s.Player.IsOnline && String.Compare(s.Player.Name, name, StringComparison.OrdinalIgnoreCase) == 0);

                return sessionStore.SingleOrDefault(s => s.Player != null && String.Compare(s.Player.Name, name, StringComparison.OrdinalIgnoreCase) == 0);
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
                return sessionStore.Where(s => s.Player?.Friends.FirstOrDefault(f => f.Id.Low == characterGuid.Low) != null).ToList();
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
                    return sessionStore.Where(s => s.Player != null && s.Player.IsOnline).ToList();

                return sessionStore.Where(s => s.Player != null).ToList();
            }
            finally
            {
                sessionLock.ExitReadLock();
            }
        }

        public static void Remove(Session session)
        {
            sessionLock.EnterWriteLock();
            try
            {
                sessionStore.Remove(session);
            }
            finally
            {
                sessionLock.ExitWriteLock();
            }
        }

        public static void StopWorld() { pendingWorldStop = true; }

        private static void UpdateWorld()
        {
            double lastTick = 0d;

            var worldTickTimer = new Stopwatch();
            while (!pendingWorldStop)
            {
                worldTickTimer.Restart();

                sessionLock.EnterReadLock();
                try
                {
                    Parallel.ForEach(sessionStore, s => s.Update(lastTick));
                    LandblockManager.UseTime(lastTick);
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
    }
}
