using System;
using System.Collections.Generic;

using ACE.Database.Models.World;
using ACE.Entity.Enum;

using log4net;

namespace ACE.Server.Managers
{
    public static class EventManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static Dictionary<string, Event> Events;

        public static bool Debug = false;

        static EventManager()
        {
            Events = new Dictionary<string, Event>(StringComparer.OrdinalIgnoreCase);
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
            var eventName = GetEventName(e);

            if (eventName.Equals("EventIsPKWorld", StringComparison.OrdinalIgnoreCase)) // special event
                return false;

            if (!Events.TryGetValue(eventName, out Event evnt))
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
            var eventName = GetEventName(e);

            if (eventName.Equals("EventIsPKWorld", StringComparison.OrdinalIgnoreCase)) // special event
                return false;

            if (!Events.TryGetValue(eventName, out Event evnt))
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
            var eventName = GetEventName(e);

            if (eventName.Equals("EventIsPKWorld", StringComparison.OrdinalIgnoreCase)) // special event
            {
                var serverPkState = PropertyManager.GetBool("pk_server").Item;

                return serverPkState;
            }

            if (!Events.TryGetValue(eventName, out Event evnt))
                return false;

            return evnt.State == (int)GameEventState.On;
        }

        public static bool IsEventEnabled(string e)
        {
            var eventName = GetEventName(e);

            if (!Events.TryGetValue(eventName, out Event evnt))
                return false;

            return evnt.State == (int)GameEventState.Enabled || evnt.State == (int)GameEventState.On || evnt.State == (int)GameEventState.Off;
        }

        public static bool IsEventAvailable(string e)
        {
            var eventName = GetEventName(e);

            return Events.ContainsKey(eventName);
        }

        public static GameEventState GetEventStatus(string e)
        {
            var eventName = GetEventName(e);

            if (eventName.Equals("EventIsPKWorld", StringComparison.OrdinalIgnoreCase)) // special event
            {
                if (PropertyManager.GetBool("pk_server").Item)
                    return GameEventState.On;
                else
                    return GameEventState.Off;
            }

            if (!Events.TryGetValue(eventName, out Event evnt))
                return GameEventState.Undef;

            return (GameEventState)evnt.State;
        }

        /// <summary>
        /// Returns the event name without the @ comment
        /// </summary>
        /// <param name="eventFormat">A event name with an optional @comment on the end</param>
        public static string GetEventName(string eventFormat)
        {
            var idx = eventFormat.IndexOf('@');     // strip comment
            if (idx == -1)
                return eventFormat;

            var eventName = eventFormat.Substring(0, idx);
            return eventName;
        }
    }
}
