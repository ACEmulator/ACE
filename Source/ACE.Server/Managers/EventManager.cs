using System;
using System.Collections.Generic;
using System.Linq;
using ACE.Database.Models.World;
using ACE.Entity.Enum;
using log4net;

namespace ACE.Server.Managers
{
    public class EventManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static Dictionary<string, Event> Events;

        public static bool Debug = false;

        static EventManager()
        {
            Events = new Dictionary<string, Event>();
        }

        public static void Initialize()
        {
            var events = Database.DatabaseManager.World.GetAllEvents();

            foreach(var evnt in events)
            {
                Events.Add(evnt.Name, evnt);

                if (evnt.State == (int)GameEventState.On)
                    StartEvent(evnt.Name);
            }

            log.DebugFormat("EventManager Initalized.");
        }

        public static bool StartEvent(string e)
        {
            if (e.Equals("EventIsPKWorld", StringComparison.OrdinalIgnoreCase)) // special event
                return false;

            var evnt = Events.FirstOrDefault(ev => ev.Key.Equals(e, StringComparison.OrdinalIgnoreCase)).Value;

            if (evnt == null)
                return false;

            var state = (GameEventState)evnt.State;

            if (state == GameEventState.Disabled)
                return false;

            if (state == GameEventState.Enabled || state == GameEventState.Off)
            {
                evnt.State = (int)GameEventState.On;
                //evnt.StartTime = DateTime.UtcNow.Ticks;

                if (Debug)
                    Console.WriteLine($"Starting event {evnt.Name}");
            }

            return true;
        }

        public static bool StopEvent(string e)
        {
            if (e.Equals("EventIsPKWorld", StringComparison.OrdinalIgnoreCase)) // special event
                return false;

            var evnt = Events.FirstOrDefault(ev => ev.Key.Equals(e, StringComparison.OrdinalIgnoreCase)).Value;

            if (evnt == null)
                return false;

            var state = (GameEventState)evnt.State;

            if (state == GameEventState.Disabled)
                return false;

            if (state == GameEventState.Enabled || state == GameEventState.On)
            {
                evnt.State = (int)GameEventState.Off;
                //evnt.StartTime = DateTime.UtcNow.Ticks;

                if (Debug)
                    Console.WriteLine($"Stopping event {evnt.Name}");
            }

            return true;
        }

        public static bool IsEventStarted(string e)
        {
            if (e.Equals("EventIsPKWorld", StringComparison.OrdinalIgnoreCase)) // special event
            {
                if (PropertyManager.GetBool("pk_server").Item)
                    return true;
                else
                    return false;
            }

            var evnt = Events.FirstOrDefault(ev => ev.Key.Equals(e, StringComparison.OrdinalIgnoreCase)).Value;

            if (evnt == null)
                return false;

            return evnt.State == (int)GameEventState.On;
        }

        public static bool IsEventEnabled(string e)
        {
            var evnt = Events.FirstOrDefault(ev => ev.Key.Equals(e, StringComparison.OrdinalIgnoreCase)).Value;

            if (evnt == null)
                return false;

            return evnt.State == (int)GameEventState.Enabled || evnt.State == (int)GameEventState.On || evnt.State == (int)GameEventState.Off;
        }

        public static bool IsEventAvailable(string e)
        {
            var evnt = Events.FirstOrDefault(ev => ev.Key.Equals(e, StringComparison.OrdinalIgnoreCase)).Value;

            if (evnt == null)
                return false;

            return true;
        }

        public static GameEventState GetEventStatus(string e)
        {
            if (e.Equals("EventIsPKWorld", StringComparison.OrdinalIgnoreCase)) // special event
            {
                if (PropertyManager.GetBool("pk_server").Item)
                    return GameEventState.On;
                else
                    return GameEventState.Off;
            }

            var evnt = Events.FirstOrDefault(ev => ev.Key.Equals(e, StringComparison.OrdinalIgnoreCase)).Value;

            if (evnt == null)
                return GameEventState.Undef;

            return (GameEventState)evnt.State;
        }
    }
}
