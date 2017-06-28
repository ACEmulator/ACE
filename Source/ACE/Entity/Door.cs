using ACE.Entity.Enum;
using ACE.Entity.Actions;
using ACE.Network.Enum;
using ACE.Network.GameEvent.Events;
using ACE.Network.Motion;

namespace ACE.Entity
{
    public class Door : UsableObject
    {
        private static readonly MovementData movementOpen = new MovementData();
        private static readonly MovementData movementClosed = new MovementData();

        private static readonly MotionState motionStateOpen = new UniversalMotion(MotionStance.Standing, movementOpen);
        private static readonly MotionState motionStateClosed = new UniversalMotion(MotionStance.Standing, movementClosed);

        private static readonly UniversalMotion motionOpen = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.On));
        private static readonly UniversalMotion motionClosed = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.Off));

        public Door(AceObject aceO)
            : base(aceO)
        {
            PhysicsState |= PhysicsState.HasPhysicsBsp | PhysicsState.ReportCollision;
            CurrentMotionState = motionStateClosed;
            movementOpen.ForwardCommand = (ushort)MotionCommand.On;
            movementClosed.ForwardCommand = (ushort)MotionCommand.Off;
        }

        public override void OnUse(ObjectGuid playerId)
        {
            // TODO: implement auto close timer, check if door is locked, send locked soundfx if locked and fail to open.

            if (this.CurrentMotionState == motionStateClosed)
            {
                Open();
            }
            else
            {
                Close();
            }

            ActionChain chain = new ActionChain();
            CurrentLandblock.ChainOnObject(chain, playerId, (WorldObject wo) =>
            {
                Player player = wo as Player;
                if (player == null)
                {
                    return;
                }
                var sendUseDoneEvent = new GameEventUseDone(player.Session);
                player.Session.Network.EnqueueSend(sendUseDoneEvent);
            });
            chain.EnqueueChain();
        }

        private void Open()
        {
            CurrentLandblock.EnqueueBroadcastMotion(this, motionOpen);
            this.CurrentMotionState = motionStateOpen;
            this.PhysicsState |= PhysicsState.Ethereal;
        }

        private void Close()
        {
            CurrentLandblock.EnqueueBroadcastMotion(this, motionClosed);
            this.CurrentMotionState = motionStateClosed;
            this.PhysicsState ^= PhysicsState.Ethereal;
        }
    }
}
