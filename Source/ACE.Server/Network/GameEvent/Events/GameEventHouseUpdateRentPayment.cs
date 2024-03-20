using System;
using System.Collections.Generic;
using ACE.Server.Network.Structure;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventHouseUpdateRentPayment : GameEventMessage
    {
        public GameEventHouseUpdateRentPayment(Session session)
            : base(GameEventType.UpdateRentPayment, GameMessageGroup.UIQueue, session, 80) // Only 32 and 80 seen in retail pcaps
        {
            //Console.WriteLine("Sending 0x228 - House - UpdateRentPayment");

            var payments = new List<HousePayment>();

            Writer.Write(payments);
        }
    }
}
