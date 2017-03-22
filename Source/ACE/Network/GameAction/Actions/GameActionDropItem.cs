using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Network.Enum;
using ACE.Network.GameMessages.Messages;
using ACE.Network.GameMessages;

namespace ACE.Network.GameAction.Actions
{
    using global::ACE.Network.GameEvent.Events;

    [GameAction(GameActionType.DropItem)]
    public class GameActionDropItem : GameActionPacket
    {
        private ObjectGuid objectGuid;
        public GameActionDropItem(Session session, ClientPacketFragment fragment) : base(session, fragment)
        {
        }

        public override void Handle()
        {
            //ChatPacket.SendServerMessage(this.Session, $"You want to drop that don't you?", ChatMessageType.Broadcast);

            this.Session.Player.HandleDropItem(this.objectGuid);

            var burdenUpdate = new GameMessagePrivateUpdatePropertyInt(this.Session, PropertyInt.EncumbVal,
                (uint)this.Session.Player.GameData.Burden);
            // TODO: animation bend down --> update container to 0 for ground or guid of chest or copse --> Set age for decay countdown --> Animation Stand up --> send put inventory in 3d space 
            //var movementMessage = new GameMessageMovement(this.Session.Player,1,1,1,0);
            var movementMessage = new GameMessageMotion(this.Session.Player,this.Session, MotionActivity.Active, MotionType.General, MotionFlags.None, MotionStance.Standing, MotionCommand.MotionInvalid, 1.00f);
            var dropSound = new GameMessageSound(this.Session.Player.Guid, Sound.DropItem, (float) 1.0);

            this.Session.Network.EnqueueSend(burdenUpdate);
            this.Session.Network.EnqueueSend(movementMessage);
            this.Session.Network.EnqueueSend(dropSound);
        }

        public override void Read() => objectGuid = new ObjectGuid(Fragment.Payload.ReadUInt32());
    }
}