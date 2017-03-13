
using ACE.Common.Extensions;
using ACE.Entity;
using ACE.Entity.Enum;

namespace ACE.Network.GameAction.Actions
{
    [GameAction(GameActionType.AdvocateTeleport)]
    public class GameActionAdvocateTeleport : GameActionPacket
    {
        private string target;
        private Position position;

        public GameActionAdvocateTeleport(Session session, ClientPacketFragment fragment) : base(session, fragment) { }

        public override void Read()
        {
            target   = Fragment.Payload.ReadString16L();
            position = new Position(Fragment.Payload);
        }

        public override void Handle()
        {
            // this check is also done clientside, see: PlayerDesc::PlayerIsPSR
            if (!Session.Player.IsAdmin && !Session.Player.IsArch && !Session.Player.IsPsr)
                return;

            uint cell  = position.Cell;
            uint cellX = (cell >> 3);

            //TODO: Wrap command in a check to confirm session.character IsAdvocate or higher access level
            
            //TODO: Maybe output to chat window coords teleported to.
            //ChatPacket.SendSystemMessage(session, $"Teleporting to: 0.0[N/S], 0.0[E/W]");
            ChatPacket.SendServerMessage(Session, "Teleporting...", ChatMessageType.Broadcast);
            Session.Player.Teleport(position);
        }
    }
}