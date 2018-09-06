using System;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventHouseUpdateRentTime : GameEventMessage
    {
        public GameEventHouseUpdateRentTime(Session session)
            : base(GameEventType.UpdateRentTime, GameMessageGroup.UIQueue, session)
        {
            //Console.WriteLine("Sending 0x227 - House - UpdateRentTime");

            var rentTime = 0u;  // when the current maintenance period began (unix timestamp)

            Writer.Write(rentTime);
        }
    }
}
