using System;

namespace ACE.Server.Network.GameAction.Actions
{
    /// <summary>
    /// Sets your house open status
    /// </summary>
    public static class GameActionHouseSetOpenStatus
    {
        [GameAction(GameActionType.SetOpenHouseStatus)]
        public static void Handle(ClientMessage message, Session session)
        {
            //Console.WriteLine("Received 0x247 - SetOpenHouseStatus");

            var openHouse = Convert.ToBoolean(message.Payload.ReadUInt32());

            session.Player.HandleActionSetOpenStatus(openHouse);
        }
    }
}
