using System;

namespace ACE.Server.Network.GameAction.Actions
{
    /// <summary>
    /// Modify whether house hooks are visible, /house hooks on/off
    /// </summary>
    public static class GameActionHouseSetHooksVisibility
    {
        [GameAction(GameActionType.SetHooksVisibility)]
        public static void Handle(ClientMessage message, Session session)
        {
            //Console.WriteLine("Received 0x266 - House - SetHooksVisibility");

            var visible = Convert.ToBoolean(message.Payload.ReadUInt32());

            session.Player.HandleActionSetHooksVisible(visible);
        }
    }
}
