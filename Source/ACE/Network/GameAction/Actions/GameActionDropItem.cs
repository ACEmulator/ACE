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
            var burdenUpdate = new GameMessagePrivateUpdatePropertyInt(Session, PropertyInt.EncumbVal,
                (uint)Session.Player.GameData.Burden);

            // TODO: animation bend down --> update container to 0 for ground or guid of chest or copse --> Set age for decay countdown --> Animation Stand up --> send put inventory in 3d space 

            MotionData motionData = new MotionData
            {
                MotionStateFlag = MotionStateFlag.ForwardCommand,
                ForwardCommand = 24
            };
            var movementMessage1 = new GameMessageMotion(Session.Player,Session, MotionActivity.Idle, MotionType.General, MotionFlags.None, MotionStance.Standing, MotionCommand.MotionInvalid, motionData, 1.00f);

            motionData.MotionStateFlag = MotionStateFlag.NoMotionState;
            motionData.ForwardCommand = 0;

            var movementMessage2 = new GameMessageMotion(Session.Player, Session, MotionActivity.Idle, MotionType.General, MotionFlags.None, MotionStance.Standing, MotionCommand.MotionInvalid, motionData, 1.00f);
            Session.Network.EnqueueSend(movementMessage1, burdenUpdate, movementMessage2, new GameMessagePutObjectIn3D(Session, Session.Player, objectGuid), new GameMessageSound(Session.Player.Guid, Sound.DropItem, (float)1.0), new GameMessageUpdateInstanceId(objectGuid, Session.Player.Guid));
            Session.Player.HandleDropItem(objectGuid, Session);
        }

        public override void Read() => objectGuid = new ObjectGuid(Fragment.Payload.ReadUInt32());
    }
}