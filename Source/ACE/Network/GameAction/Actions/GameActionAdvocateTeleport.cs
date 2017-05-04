using ACE.Common.Extensions;
using ACE.Entity;
using ACE.Entity.Enum;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionAdvocateTeleport
    {
        [GameAction(GameActionType.AdvocateTeleport)]
        public static void Handle(ClientMessage message, Session session)
        {
            var target = message.Payload.ReadString16L();
            var position = new Position(message.Payload);
            // this check is also done clientside, see: PlayerDesc::PlayerIsPSR
            if (!session.Player.IsAdmin && !session.Player.IsArch && !session.Player.IsPsr)
                return;

            uint cell = position.LandblockId.Raw;
            uint cellX = (cell >> 3);

            // TODO: Wrap command in a check to confirm session.character IsAdvocate or higher access level

            // TODO: Maybe output to chat window coords teleported to.
            // ChatPacket.SendSystemMessage(session, $"Teleporting to: 0.0[N/S], 0.0[E/W]");
            ChatPacket.SendServerMessage(session, "Teleporting...", ChatMessageType.Broadcast);
            session.Player.Teleport(position);
        }
    }
}