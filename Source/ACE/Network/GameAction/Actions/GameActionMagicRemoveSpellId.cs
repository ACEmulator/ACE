using System.Threading.Tasks;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionMagicRemoveSpellId
    {
        [GameAction(GameActionType.RemoveSpellC2S)]
        #pragma warning disable 1998
        public static async Task Handle(ClientMessage message, Session session)
        {
            uint spellId = message.Payload.ReadUInt32();

            session.Player.MagicRemoveSpellId(spellId);
        }
        #pragma warning restore 1998
    }
}
