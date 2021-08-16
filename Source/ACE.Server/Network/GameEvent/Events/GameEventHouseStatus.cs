using ACE.Entity.Enum;
using System;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventHouseStatus : GameEventMessage
    {
        public GameEventHouseStatus(Session session, WeenieError weenieError = WeenieError.BadParam)
            : base(GameEventType.HouseStatus, GameMessageGroup.UIQueue, session)
        {
            //Console.WriteLine("Sending 0x226 - HouseStatus");

            //var noticeType = 2u;    // type of message to display

            Writer.Write((uint)weenieError);
        }
    }
}
