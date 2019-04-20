using System;

using ACE.Server.Network.Structure;

namespace ACE.Server.Network.GameAction.Actions
{
    /// <summary>
    /// Pay rent for a house
    /// </summary>
    public static class GameActionHouseRentHouse
    {
        [GameAction(GameActionType.RentHouse)]
        public static void Handle(ClientMessage message, Session session)
        {
            //Console.WriteLine("Received 0x221 - RentHouse");

            var slumlord = message.Payload.ReadUInt32();
            var items = message.Payload.ReadListUInt32();   // items being used to pay rent

            session.Player.HandleActionRentHouse(slumlord, items);
        }
    }
}
