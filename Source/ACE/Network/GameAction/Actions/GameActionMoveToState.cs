using System;

using ACE.Entity;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionMoveToState
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

        [GameAction(GameActionType.MoveToState)]
        public static void Handle(ClientMessage message, Session session)
        {
            Position position;
            uint currentHoldkey;
            uint currentStyle;
            MotionStateDirection forward = new MotionStateDirection();
            MotionStateDirection sideStep = new MotionStateDirection();
            MotionStateDirection turn = new MotionStateDirection();
            ushort instanceTimestamp;
            ushort serverControlTimestamp;
            ushort teleportTimestamp;
            ushort forcePositionTimestamp;
            /*bool contact;
            bool longJump;*/

            CommonTasks.WaitForPlayer(session);

            Console.WriteLine("finished Handle");

            MotionStateFlag flags = (MotionStateFlag)message.Payload.ReadUInt32();

            if ((flags & MotionStateFlag.CurrentHoldKey) != 0)
                currentHoldkey = message.Payload.ReadUInt32();

            if ((flags & MotionStateFlag.CurrentStyle) != 0)
                currentStyle = message.Payload.ReadUInt32();

            if ((flags & MotionStateFlag.ForwardCommand) != 0)
                forward.Command = message.Payload.ReadUInt32();

            if ((flags & MotionStateFlag.ForwardHoldKey) != 0)
                forward.HoldKey = message.Payload.ReadUInt32();

            if ((flags & MotionStateFlag.ForwardSpeed) != 0)
                forward.Speed = message.Payload.ReadSingle();

            if ((flags & MotionStateFlag.SideStepCommand) != 0)
                sideStep.Command = message.Payload.ReadUInt32();

            if ((flags & MotionStateFlag.SideStepHoldKey) != 0)
                sideStep.HoldKey = message.Payload.ReadUInt32();

            if ((flags & MotionStateFlag.SideStepSpeed) != 0)
                sideStep.Speed = message.Payload.ReadSingle();

            if ((flags & MotionStateFlag.TurnCommand) != 0)
                turn.Command = message.Payload.ReadUInt32();

            if ((flags & MotionStateFlag.TurnHoldKey) != 0)
                turn.HoldKey = message.Payload.ReadUInt32();

            if ((flags & MotionStateFlag.TurnSpeed) != 0)
                turn.Speed = message.Payload.ReadSingle();

            position = new Position(message.Payload);
            position.LandblockId = new LandblockId(position.Cell);

            instanceTimestamp = message.Payload.ReadUInt16();
            serverControlTimestamp = message.Payload.ReadUInt16();
            teleportTimestamp = message.Payload.ReadUInt16();
            forcePositionTimestamp = message.Payload.ReadUInt16();
            message.Payload.ReadByte();
            session.Player.RequestUpdatePosition(position);
        }
    }
}
