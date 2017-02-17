﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;

using ACE.Network;

namespace ACE.Managers
{
    public static class WorldManager
    {
        private static uint sessionTimeout = 150u; // max time between packets before the client disconnects

        private static readonly List<Session> sessionStore = new List<Session>();
        private static readonly ReaderWriterLockSlim sessionLock = new ReaderWriterLockSlim();

        private static volatile bool pendingWorldStop;

        public static DateTime WorldStartTime { get; } = DateTime.Now;

        public static void Initialise()
        {
            var thread = new Thread(UpdateWorld);
            thread.Start();
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
                    foreach (var session in sessionStore)
                        session.Update(lastTick);
                }
                finally
                {
                    sessionLock.ExitReadLock();
                }

                Thread.Sleep(1);
                lastTick = (double)worldTickTimer.ElapsedTicks / Stopwatch.Frequency;
            }
        }
    }
}
