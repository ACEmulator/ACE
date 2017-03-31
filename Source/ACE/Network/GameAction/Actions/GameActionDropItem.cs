using ACE.Entity;
using ACE.Entity.Enum.Properties;
using ACE.Network.Enum;
using ACE.Network.GameMessages.Messages;


namespace ACE.Network.GameAction.Actions
{

    [GameAction(GameActionType.DropItem)]
    public class GameActionDropItem : GameActionPacket
    {
        private ObjectGuid objectGuid;
        public GameActionDropItem(Session session, ClientPacketFragment fragment) : base(session, fragment)
        {
        }

        private void MotionForwardBend()
        {
            var movement = new MovementData { ForwardCommand = 24, MovementStateFlag = MovementStateFlag.ForwardCommand };
            Session.Network.EnqueueSend(new GameMessageMotion(Session.Player, Session, MotionActivity.Idle, MovementTypes.Invalid, MotionFlags.None, MotionStance.Standing, movement));
        }

        private void MotionStandUp()
        {
            var movement = new MovementData { ForwardCommand = 0, MovementStateFlag = MovementStateFlag.NoMotionState };
            Session.Network.EnqueueSend(new GameMessageMotion(Session.Player, Session, MotionActivity.Idle, MovementTypes.Invalid, MotionFlags.None, MotionStance.Standing, movement));
        }

        private void UpdatePlayerBurden()
        {
            Session.Network.EnqueueSend(
                new GameMessagePrivateUpdatePropertyInt(
                    this.Session,
                    PropertyInt.EncumbVal,
                    (uint)Session.Player.GameData.Burden));
        }

        private void UpdateContainerId()
        {
            var targetContainer = new ObjectGuid(0);
            // Set Container id to 0 - you are free
            Session.Network.EnqueueSend(
                new GameMessageUpdateInstanceId(this.objectGuid, targetContainer));
        }

        public override void Handle()
        {
            var targetContainer = new ObjectGuid(0);

            this.UpdatePlayerBurden();
            this.MotionForwardBend();
            this.UpdateContainerId();
            this.MotionStandUp();

            // Ok, we can do the last 3 steps together.   Not sure if it is better to break this stuff our for clarity
            // Put the darn thing in 3d space
            // Make the thud sound
            // Send the container update again.   I have no idea why, but that is what they did in live.
            Session.Network.EnqueueSend(
                new GameMessagePutObjectIn3d(Session, Session.Player, objectGuid),
                new GameMessageSound(Session.Player.Guid, Sound.DropItem, (float)1.0),
                new GameMessageUpdateInstanceId(objectGuid, targetContainer));

            Session.Player.HandleDropItem(objectGuid, Session);
        }

        public override void Read() => objectGuid = new ObjectGuid(Fragment.Payload.ReadUInt32());
    }
}