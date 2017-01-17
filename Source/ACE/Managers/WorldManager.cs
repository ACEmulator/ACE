using ACE.Network;
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

        public static void Initalise()
        {
            var thread = new Thread(new ThreadStart(UpdateWorld));
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

        public static void Remove(Session session)
        {
            lock (sessionLock)
            {
                sessionStore.Remove(session);
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

                foreach (var session in sessionStore)
                    session.Update(lastTick);

                Thread.Sleep(1);
                lastTick = (double)worldTickTimer.ElapsedTicks / Stopwatch.Frequency;
            }
        }
    }
}
