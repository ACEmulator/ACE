using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using ACE.Server.Managers;

namespace ACE.Server.Entity
{
    public class PerfTimer
    {
        public static Dictionary<string, MethodTimer> MethodTimers = new Dictionary<string, MethodTimer>();

        public static DateTime NextMethodTime;

        public static bool ShowInfo = true;

        public static Dictionary<string, bool> ShowInfos = new Dictionary<string, bool>()
        {
            { "wo.Tick", true },
            { "Monster", true },
        };

        public static void StartTimer(string methodName)
        {
            if (!ShowInfo || !WorldManager.Profiling) return;

            var methodTimer = GetTimer(methodName);
            methodTimer.Timer.Start();
            methodTimer.Instances++;
        }

        public static void StopTimer(string methodName)
        {
            if (!ShowInfo || !WorldManager.Profiling) return;

            var methodTimer = GetTimer(methodName);
            methodTimer.Instances--;
            if (methodTimer.Instances == 0)
                methodTimer.Timer.Stop();
        }

        public static MethodTimer GetTimer(string methodName)
        {
            MethodTimer methodTimer = null;
            MethodTimers.TryGetValue(methodName, out methodTimer);
            if (methodTimer == null)
            {
                MethodTimers.Add(methodName, new MethodTimer());
                MethodTimers.TryGetValue(methodName, out methodTimer);
            }
            return methodTimer;
        }

        public static void DoTimers()
        {
            if (!ShowInfo || !WorldManager.Profiling) return;

            var currentTime = DateTime.UtcNow;

            if (currentTime < NextMethodTime)
                return;

            foreach (var kvp in MethodTimers)
            {
                var methodName = kvp.Key;
                var methodTimer = kvp.Value;

                var className = methodName.Contains('.') ? methodName.Substring(0, methodName.IndexOf('.')) : methodName;

                if (ShowInfos.ContainsKey(className) && !ShowInfos[className])
                    continue;

                Console.WriteLine(methodName + ": " + methodTimer.Timer.Elapsed.TotalMilliseconds);

                methodTimer.Timer.Reset();
            }

            NextMethodTime = currentTime.AddSeconds(1.0f);
        }

        public class MethodTimer
        {
            public Stopwatch Timer;
            public int Instances;

            public MethodTimer()
            {
                Timer = new Stopwatch();
            }
        }
    }
}
