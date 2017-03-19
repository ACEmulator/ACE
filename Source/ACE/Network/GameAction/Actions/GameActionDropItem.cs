using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Network.GameMessages.Messages;

namespace ACE.Network.GameAction.Actions
{
    [GameAction(GameActionType.DropItem)]
    public class GameActionDropItem : GameActionPacket
    {
        private uint selectedItem;
        public GameActionDropItem(Session session, ClientPacketFragment fragment) : base(session, fragment) { }

        public override void Read()
        {
            selectedItem = Fragment.Payload.ReadUInt32();
        }

        public override void Handle()
        {           
            ChatPacket.SendServerMessage(Session, $"You want to drop that don't you?", ChatMessageType.Broadcast);
            // can't be updated no access to the item we are dropping. Will need to have something in the world manager deal with this.
            //Session.Player.GameData.Burden 
            var burdenUpdate = new GameMessagePrivateUpdatePropertyInt(Session, PropertyInt.EncumbVal, Session.Player.GameData.Burden);
            Session.Network.EnqueueSend(burdenUpdate);
        }
    }
}