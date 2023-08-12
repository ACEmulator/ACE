// This file has been edited by Linae of DnF

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionLoginComplete
    {
        /// <summary>
        /// This is called when the client player exits portal space
        /// It includes initial login, as well as portaling / teleporting
        /// </summary>
        [GameAction(GameActionType.LoginComplete)]
        public static void Handle(ClientMessage message, Session session)
        {
            session.Player.OnTeleportComplete();

            if (!session.Player.FirstEnterWorldDone)
            {
                session.Player.FirstEnterWorldDone = true;
                session.Player.SendPropertyUpdatesAndOverrides();

                if (session.Player.IsFrozen == true)  // Added by Linae
                {    
                    session.Player.SetProperty(ACE.Entity.Enum.Properties.PropertyBool.IsFrozen, true);
                    session.Player.EnqueueBroadcastPhysicsState();
                    session.Player.SendFrozenMessage();
                }
            }
        }
    }
}
