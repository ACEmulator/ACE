using ACE.Common.Extensions;

namespace ACE.Server.Network.GameAction.Actions
{
    /// <summary>
    /// This method processes the Game Action (F7B1) Writing_SetInscription (0x00BF) and calls
    /// the HandleActionSetInscription method on the player object. Og II
    /// </summary>
    public static class GameActionSetInscription
    {
        [GameAction(GameActionType.SetInscription)]
        public static void Handle(ClientMessage message, Session session)
        {
            uint objectGuid = message.Payload.ReadUInt32();
            string inscriptionText = message.Payload.ReadString16L();

            session.Player.HandleActionSetInscription(objectGuid, inscriptionText);
        }
    }
}
