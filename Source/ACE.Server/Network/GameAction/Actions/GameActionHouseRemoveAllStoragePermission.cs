using System;

namespace ACE.Server.Network.GameAction.Actions
{
    /// <summary>
    /// Removes all storage permissions, /house storage remove_all
    /// </summary>
    public static class GameActionHouseRemoveAllStoragePermission
    {
        [GameAction(GameActionType.RemoveAllStoragePermission)]
        public static void Handle(ClientMessage message, Session session)
        {
            //Console.WriteLine("Received 0x24C - House - RemoveAllStoragePermission");
            session.Player.HandleActionRemoveAllStorage();
        }
    }
}
