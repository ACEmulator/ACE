using System;
using System.Collections.Generic;
using System.Text;
using ACE.Database.Models.World;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using log4net;

namespace ACE.Server.Managers
{
    public class EventManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static Dictionary<string, Event> Events;

        static EventManager()
        {
            Events = new Dictionary<string, Event>();
        }

        public static void Initialize()
        {
            var events = Database.DatabaseManager.World.GetAllEvents();

            foreach(var evnt in events)
            {
                Events.Add(evnt.Name.ToLower(), evnt);
            }

            log.DebugFormat("EventManager Initalized.");
        }

        public static bool StartEvent(string e)
        {
            e = e.ToLower();

            if (!Events.ContainsKey(e))
                return false;

            var evnt = Events[e];

            var state = (GameEventState)evnt.State;

            if (state == GameEventState.Disabled)
                return false;

            if (state == GameEventState.Enabled || state == GameEventState.Off)
            {
                evnt.State = (int)GameEventState.On;
                //evnt.StartTime = DateTime.UtcNow.Ticks;

                Events[e] = evnt;
                return true;
            }

            return true;
        }

        public static bool StopEvent(string e)
        {
            e = e.ToLower();

            if (!Events.ContainsKey(e))
                return false;

            var evnt = Events[e];

            var state = (GameEventState)evnt.State;

            if (state == GameEventState.Disabled)
                return false;

            if (state == GameEventState.Enabled || state == GameEventState.On)
            {
                evnt.State = (int)GameEventState.Off;
                //evnt.StartTime = DateTime.UtcNow.Ticks;

                Events[e] = evnt;
                return true;
            }

            return true;
        }

        public static bool IsEventStarted(string e)
        {
            e = e.ToLower();

            if (!Events.ContainsKey(e))
                return false;

            return Events[e].State == (int)GameEventState.On;
        }

        public static bool IsEventEnabled(string e)
        {
            e = e.ToLower();

            if (!Events.ContainsKey(e))
                return false;

            return Events[e].State == (int)GameEventState.Enabled || Events[e].State == (int)GameEventState.On || Events[e].State == (int)GameEventState.Off;
        }

        public static bool IsEventAvailable(string e)
        {
            e = e.ToLower();

            if (!Events.ContainsKey(e))
                return false;

            return true;
        }

        public static GameEventState GetEventStatus(string e)
        {
            e = e.ToLower();

            if (!Events.ContainsKey(e))
                return GameEventState.Undef;

            return (GameEventState)Events[e].State;
        }
    }
}
