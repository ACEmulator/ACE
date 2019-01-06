using ACE.Server.Network.Structure;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionJump
    {
        [GameAction(GameActionType.Jump)]
        public static void Handle(ClientMessage message, Session session)
        {
            var jumpPack = new JumpPack(message.Payload);

            var objectGuid = message.Payload.ReadUInt32();
            var spellId = message.Payload.ReadUInt32();

            session.Player.HandleActionJump(jumpPack);
        }
    }
}
