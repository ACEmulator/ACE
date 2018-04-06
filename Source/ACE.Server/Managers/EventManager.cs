using System;
using System.Collections.Generic;
using System.Text;
using ACE.Server.Entity;

namespace ACE.Server.Managers
{
    public class EventManager
    {
        public static Dictionary<string, double> Events;

        static EventManager()
        {
            Events = new Dictionary<string, double>();
        }

        public static bool StartEvent(string e)
        {
            if (Events.ContainsKey(e))
                return false;

            Events.Add(e, Timer.CurrentTime);
            return true;
        }

        public static bool StopEvent(string e)
        {
            if (!Events.ContainsKey(e))
                return false;

            Events.Remove(e);
            return true;
        }

        public static bool IsEventStarted(string e)
        {
            return Events.ContainsKey(e);
        }
    }
}
