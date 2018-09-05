using System;
using ACE.Server.Network.Structure;

namespace ACE.Server.Network.GameEvent.Events
{
    /// <summary>
    /// House panel information for owners
    /// </summary>
    public class GameEventHouseData : GameEventMessage
    {
        public GameEventHouseData(Session session, HouseData data)
            : base(GameEventType.HouseData, GameMessageGroup.UIQueue, session)
        {
            //Console.WriteLine("Sending 0x225 - GameEventHouseData");

            Writer.Write(data);
        }
    }
}
