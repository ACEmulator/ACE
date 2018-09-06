using System;
using System.Collections.Generic;
using ACE.Entity.Enum;
using ACE.Server.Network.Structure;

namespace ACE.Server.Network.GameEvent.Events
{
    /// <summary>
    /// Displays a list of available houses
    /// </summary>
    public class GameEventHouseAvailableHouses : GameEventMessage
    {
        public GameEventHouseAvailableHouses(Session session, HouseType type, List<uint> locations, int totalAvailable)
            : base(GameEventType.AvailableHouses, GameMessageGroup.UIQueue, session)
        {
            //Console.WriteLine("Sending 0x271 - GameEvent - AvailableHouses");

            Writer.Write((uint)type);
            Writer.Write(locations);
            Writer.Write(totalAvailable);
        }
    }
}
