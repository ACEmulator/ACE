using ACE.Entity;
using ACE.Entity.Enum.Properties;
using ACE.Network.Enum;
using ACE.Network.GameMessages.Messages;


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
            MotionData motionData = new MotionData
            {
                // TODO: Need to setup enums for motion data
                MotionStateFlag = MotionStateFlag.ForwardCommand,
                ForwardCommand = 24
            };
            var movementMessage1 = new GameMessageMotion(Session.Player, Session, MotionActivity.Idle, MotionType.General, MotionFlags.None, MotionStance.Standing, MotionCommand.MotionInvalid, motionData, 1.00f);

            motionData.MotionStateFlag = MotionStateFlag.NoMotionState;
            motionData.ForwardCommand = 0;

            var movementMessage2 = new GameMessageMotion(Session.Player, Session, MotionActivity.Idle, MotionType.General, MotionFlags.None, MotionStance.Standing, MotionCommand.MotionInvalid, motionData, 1.00f);
            var targetContainer = new ObjectGuid(0);

            Session.Network.EnqueueSend(
                new GameMessagePrivateUpdatePropertyInt(this.Session,
                    PropertyInt.EncumbVal,
                    (uint)Session.Player.GameData.Burden),
                movementMessage1,
                new GameMessageUpdateInstanceId(this.objectGuid, targetContainer),
                movementMessage2,
                new GameMessagePutObjectIn3D(Session, Session.Player, objectGuid),
                new GameMessageSound(Session.Player.Guid, Sound.DropItem, (float)1.0),
                new GameMessageUpdateInstanceId(objectGuid, targetContainer));

            Session.Player.HandleDropItem(objectGuid, Session);
        }

        public override void Read() => objectGuid = new ObjectGuid(Fragment.Payload.ReadUInt32());
    }
}