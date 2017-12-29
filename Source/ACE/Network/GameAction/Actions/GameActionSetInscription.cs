using System.Threading.Tasks;

using ACE.Common.Extensions;
using ACE.Entity;

namespace ACE.Network.GameAction.Actions
{
    /// <summary>
    /// This method processes the Game Action (F7B1) Writing_SetInscription (0x00BF) and calls
    /// the HandleActionSetInscription method on the player object. Og II
    /// </summary>
    public static class GameActionSetInscription
    {
        [GameAction(GameActionType.SetInscription)]
        #pragma warning disable 1998
        public static async Task Handle(ClientMessage message, Session session)
        {
            uint objectId = message.Payload.ReadUInt32();
            string inscriptionText = message.Payload.ReadString16L();
            session.Player.SetInscription(new ObjectGuid(objectId), inscriptionText);
        }
        #pragma warning restore 1998
    }
}
