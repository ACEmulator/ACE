using System;

namespace ACE.Server.Network.GameAction.Actions
{
    /// <summary>
    /// Adds all to your storage permissions, /house storage add -all
    /// </summary>
    public static class GameActionHouseAddAllStoragePermission
    {
        [GameAction(GameActionType.AddAllStoragePermission)]
        public static void Handle(ClientMessage message, Session session)
        {
            //Console.WriteLine("Received 0x25C - House - AddAllStoragePermission");

            session.Player.HandleActionAllStorage();
        }
    }
}
