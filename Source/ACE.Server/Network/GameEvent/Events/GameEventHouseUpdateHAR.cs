using System;
using ACE.Server.Network.Structure;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameEvent.Events
{
    /// <summary>
    /// Update house access records, aka guest list
    /// </summary>
    public class GameEventUpdateHAR : GameEventMessage
    {
        public GameEventUpdateHAR(Session session, House house)
            : base(GameEventType.UpdateHAR, GameMessageGroup.UIQueue, session)
        {
            //Console.WriteLine("Sending 0x257 - Update House Access Records (HAR)");

            var har = new HouseAccess(house);

            Writer.Write(har);
        }
    }
}
