using System;

namespace ACE.Server.Network.GameAction.Actions
{
    /// <summary>
    /// Modify whether allegiance members are guests, /house guest add_allegiance / remove_allegiance
    /// </summary>
    public static class GameActionHouseModifyAllegianceGuestPermission
    {
        [GameAction(GameActionType.ModifyAllegianceGuestPermission)]
        public static void Handle(ClientMessage message, Session session)
        {
            //Console.WriteLine("Received 0x267 - House - ModifyAllegianceGuestPermission");

            // whether we are adding or removing permissions
            var add = Convert.ToBoolean(message.Payload.ReadUInt32());

            session.Player.HandleActionModifyAllegianceGuestPermission(add);
        }
    }
}
