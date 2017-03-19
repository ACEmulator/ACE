using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Network.Enum;
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
            // TODO: animation bend down --> update container to 0 for ground or guid of chest or copse --> Set age for decay countdown --> Animation Stand up --> send put inventory in 3d space 
            var dropSound = new GameMessageSound(Session.Player.Guid, Sound.DropItem, (float)1.0);
            Session.Network.EnqueueSend(burdenUpdate, dropSound);
        }
    }
}