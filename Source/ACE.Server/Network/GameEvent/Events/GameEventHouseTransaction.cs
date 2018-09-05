using System;

namespace ACE.Server.Network.GameEvent.Events
{
    /// <summary>
    /// House status?
    /// </summary>
    public class GameEventHouseTransaction : GameEventMessage
    {
        public GameEventHouseTransaction(Session session)
            : base(GameEventType.HouseTransaction, GameMessageGroup.UIQueue, session)
        {
            //Console.WriteLine("Sending 0x259 - GameEventHouseTransaction");

            var noticeType = 2u;    // type of message to display

            Writer.Write(noticeType);
        }
    }
}
