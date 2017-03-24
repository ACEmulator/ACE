
using ACE.Entity.Enum;


namespace ACE.Network.GameAction.Actions
{
    [GameAction(GameActionType.QueryItemMana)]
    public class GameActionQueryItemMana : GameActionPacket
    {
        public GameActionQueryItemMana(Session session, ClientPacketFragment fragment) : base(session, fragment) { }

        public override void Handle()
        {
            ChatPacket.SendServerMessage(Session, $"You want to select that?", ChatMessageType.Broadcast);
        }
    }
}