using ACE.Entity;
using ACE.Network;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;

namespace ACE.Managers
{
    public static class WorldManager
    {
        private static uint sessionTimeout = 150u; // max time between packets before the client disconnects

        private static List<Session> sessionStore = new List<Session>();
        private static object sessionLock = new object();

        private static volatile bool pendingWorldStop;

        public static DateTime WorldStartTime { get; } = DateTime.Now;

        public static void Initialise()
        {
            var thread = new Thread(UpdateWorld);
            thread.Start();
        }

        public static Session Find(IPEndPoint endPoint)
        {
            lock (sessionLock)
            {
                var session = sessionStore.SingleOrDefault(s => endPoint.Equals(s.EndPoint));
                if (session == null)
                {
                    session = new Session(endPoint);
                    sessionStore.Add(session);
                }

                return session;
            }
        }

        public static Session Find(string account)
        {
            lock (sessionLock)
                return sessionStore.SingleOrDefault(s => s.Account == account);
        }

        public static Session Find(ulong connectionKey)
        {
            lock (sessionLock)
                return sessionStore.SingleOrDefault(s => s.WorldConnectionKey == connectionKey);
        }

        public static Session Find(ObjectGuid characterGuid)
        {
            lock(sessionLock)
                return sessionStore.SingleOrDefault(s => s.Player.Guid.GetLow() == characterGuid.GetLow());
        }

        public static List<Session> FindInverseFriends(ObjectGuid characterGuid)
        {
            lock(sessionLock)
                return sessionStore.Where(s => s.Player.Character.Friends.FirstOrDefault(f => f.Id.GetLow() == characterGuid.GetLow()) != null).ToList();
        }

        public static void Remove(Session session)
        {
            lock (sessionLock)
                sessionStore.Remove(session);
        }

        public static void StopWorld() { pendingWorldStop = true; }

        private static void UpdateWorld()
        {
            double lastTick = 0d;

            var worldTickTimer = new Stopwatch();
            while (!pendingWorldStop)
            {
                worldTickTimer.Restart();

                foreach (var session in sessionStore)
                    session.Update(lastTick);

                Thread.Sleep(1);
                lastTick = (double)worldTickTimer.ElapsedTicks / Stopwatch.Frequency;
            }
        }

        public static ulong GetUnixTime() { return (ulong)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds; }
    }
}
