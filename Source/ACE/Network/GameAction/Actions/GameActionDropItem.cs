using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Network.Enum;
using ACE.Network.GameMessages.Messages;
using ACE.Managers;

namespace ACE.Network.GameAction.Actions
{

    [GameAction(GameActionType.DropItem)]
    public class GameActionDropItem : GameActionPacket
    {
        private ObjectGuid objectGuid;
        public GameActionDropItem(Session session, ClientPacketFragment fragment) : base(session, fragment)
        {
        }

        public override void Handle()
        {
            ChatPacket.SendServerMessage(Session, $"You want to drop that don't you?", ChatMessageType.Broadcast);

            Session.Player.HandleDropItem(objectGuid);
            //var burdenUpdate = new GameMessagePrivateUpdatePropertyInt(Session, PropertyInt.EncumbVal,
            //    (uint)(Session.Player.GameData.Burden - inventoryItem.GameData.Burden));
            // TODO: animation bend down --> update container to 0 for ground or guid of chest or copse --> Set age for decay countdown --> Animation Stand up --> send put inventory in 3d space 
            var dropSound = new GameMessageSound(Session.Player.Guid, Sound.DropItem, (float) 1.0);
            Session.Network.EnqueueSend(dropSound);
        }

        public override void Read() => objectGuid = new ObjectGuid(Fragment.Payload.ReadUInt32());
    }
}