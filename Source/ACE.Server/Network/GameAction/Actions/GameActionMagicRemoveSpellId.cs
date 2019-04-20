
namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionMagicRemoveSpellId
    {
        [GameAction(GameActionType.RemoveSpellC2S)]
        public static void Handle(ClientMessage message, Session session)
        {
            uint spellId = message.Payload.ReadUInt32();

            session.Player.HandleActionMagicRemoveSpellId(spellId);
        }
    }
}
