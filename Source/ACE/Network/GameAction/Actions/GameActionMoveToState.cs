using System;

using ACE.Entity;
using ACE.Network.Enum;

namespace ACE.Network.GameAction.Actions
{
    [GameAction(GameActionOpcode.MoveToState)]
    public class GameActionMoveToState : GameActionPacket
    {
        private MotionState motionState = null;

        public GameActionMoveToState(Session session, ClientPacketFragment fragment) : base(session, fragment) { }

        public override void Read()
        {
            motionState = new MotionState(Fragment.Payload);
        }

        public override void Handle()
        {
            Session.Player.UpdateMotionState(motionState);
        }
    }
}
