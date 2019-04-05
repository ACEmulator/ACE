using System;

namespace ACE.Server.Network.GameAction.Actions
{
    /// <summary>
    /// Modify whether allegiance members can access storage, /house storage add_allegiance / remove_allegiance
    /// </summary>
    public static class GameActionHouseModifyAllegianceStoragePermission
    {
        [GameAction(GameActionType.ModifyAllegianceStoragePermission)]
        public static void Handle(ClientMessage message, Session session)
        {
            //Console.WriteLine("Received 0x268 - House - ModifyAllegianceStoragePermission");

            // whether we are adding or removing permissions
            var add = Convert.ToBoolean(message.Payload.ReadUInt32());

            session.Player.HandleActionModifyAllegianceStoragePermission(add);
        }
    }
}
