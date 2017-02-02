using System;

namespace ACE.Network.GameAction
{
    [GameAction(GameActionOpcode.MoveToState)]
    public class GameActionMoveToState : GameActionPacket
    {
        [Flags]
        private enum MotionStateFlag
        {
            None            = 0x0000,
            CurrentHoldKey  = 0x0001,
            CurrentStyle    = 0x0002,
            ForwardCommand  = 0x0004,
            ForwardHoldKey  = 0x0008,
            ForwardSpeed    = 0x0010,
            SideStepCommand = 0x0020,
            SideStepHoldKey = 0x0040,
            SideStepSpeed   = 0x0080,
            TurnCommand     = 0x0100,
            TurnHoldKey     = 0x0200,
            TurnSpeed       = 0x0400
        }

        private class MotionStateDirection
        {
            public uint Command { get; set; }
            public uint HoldKey { get; set; }
            public float Speed { get; set; } = 1.0f;
        }

        private Position position;
        private uint currentHoldkey;
        private uint currentStyle;
        private MotionStateDirection forward = new MotionStateDirection();
        private MotionStateDirection sideStep = new MotionStateDirection();
        private MotionStateDirection turn = new MotionStateDirection();
        private ushort instanceTimestamp;
        private ushort serverControlTimestamp;
        private ushort teleportTimestamp;
        private ushort forcePositionTimestamp;
        /*private bool contact;
        private bool longJump;*/

        public GameActionMoveToState(Session session, ClientPacketFragment fragment) : base(session, fragment) { }

        public override void Read()
        {
            MotionStateFlag flags = (MotionStateFlag)fragment.Payload.ReadUInt32();

            if ((flags & MotionStateFlag.CurrentHoldKey) != 0)
                currentHoldkey = fragment.Payload.ReadUInt32();

            if ((flags & MotionStateFlag.CurrentStyle) != 0)
                currentStyle = fragment.Payload.ReadUInt32();

            if ((flags & MotionStateFlag.ForwardCommand) != 0)
                forward.Command = fragment.Payload.ReadUInt32();

            if ((flags & MotionStateFlag.ForwardHoldKey) != 0)
                forward.HoldKey = fragment.Payload.ReadUInt32();

            if ((flags & MotionStateFlag.ForwardSpeed) != 0)
                forward.Speed = fragment.Payload.ReadSingle();

            if ((flags & MotionStateFlag.SideStepCommand) != 0)
                sideStep.Command = fragment.Payload.ReadUInt32();

            if ((flags & MotionStateFlag.SideStepHoldKey) != 0)
                sideStep.HoldKey = fragment.Payload.ReadUInt32();

            if ((flags & MotionStateFlag.SideStepSpeed) != 0)
                sideStep.Speed = fragment.Payload.ReadSingle();

            if ((flags & MotionStateFlag.TurnCommand) != 0)
                turn.Command = fragment.Payload.ReadUInt32();

            if ((flags & MotionStateFlag.TurnHoldKey) != 0)
                turn.HoldKey = fragment.Payload.ReadUInt32();

            if ((flags & MotionStateFlag.TurnSpeed) != 0)
                turn.Speed = fragment.Payload.ReadSingle();

            position               = new Position(fragment.Payload);
            instanceTimestamp      = fragment.Payload.ReadUInt16();
            serverControlTimestamp = fragment.Payload.ReadUInt16();
            teleportTimestamp      = fragment.Payload.ReadUInt16();
            forcePositionTimestamp = fragment.Payload.ReadUInt16();
            fragment.Payload.ReadByte();
        }

        public override void Handle()
        {
            session.Character.UpdatePosition(position);
        }
    }
}
