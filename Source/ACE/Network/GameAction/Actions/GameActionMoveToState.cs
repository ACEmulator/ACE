using System;
using System.IO;
using ACE.Entity;
using ACE.Network.Motion;

using log4net;
using ACE.Entity.Enum;

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
            TurnSpeed       = 0x0400,
            AnimationFlag   = 0x0800
        }

        private class MotionStateDirection
        {
            public uint Command { get; set; }
            public uint HoldKey { get; set; }
            public float Speed { get; set; } = 1.0f;
        }

        private static MovementData DeserializeMovement(BinaryReader reader, MotionStateFlag flags)
        {
            MovementData ret = new MovementData();

            if ((flags & MotionStateFlag.CurrentStyle) != 0)
                ret.CurrentStyle = (ushort)reader.ReadUInt32();

            if ((flags & MotionStateFlag.ForwardCommand) != 0)
                ret.ForwardCommand = (ushort)reader.ReadUInt32();

            // FIXME(ddevec): Holdkey?
            if ((flags & MotionStateFlag.ForwardHoldKey) != 0)
                reader.ReadUInt32();

            if ((flags & MotionStateFlag.ForwardSpeed) != 0)
                ret.ForwardSpeed = (ushort)reader.ReadSingle();

            if ((flags & MotionStateFlag.SideStepCommand) != 0)
                ret.SideStepCommand = (ushort)reader.ReadUInt32();

            // FIXME(ddevec): Holdkey?
            if ((flags & MotionStateFlag.SideStepHoldKey) != 0)
                reader.ReadUInt32();

            if ((flags & MotionStateFlag.SideStepSpeed) != 0)
                ret.SideStepSpeed = (ushort)reader.ReadSingle();

            if ((flags & MotionStateFlag.TurnCommand) != 0)
                ret.TurnCommand = (ushort)reader.ReadUInt32();

            // FIXME(ddevec): Holdkey?
            if ((flags & MotionStateFlag.TurnHoldKey) != 0)
                reader.ReadUInt32();

            if ((flags & MotionStateFlag.TurnSpeed) != 0)
                ret.TurnSpeed = (ushort)reader.ReadSingle();

            return ret;
        }

        [GameAction(GameActionType.MoveToState)]
        public static void Handle(ClientMessage message, Session session)
        {
            Position position;
            uint currentHoldkey = 0;
            ushort instanceTimestamp;
            ushort serverControlTimestamp;
            ushort teleportTimestamp;
            ushort forcePositionTimestamp;
            /*bool contact;
            bool longJump;*/

            MotionStateFlag flags = (MotionStateFlag)message.Payload.ReadUInt32();

            if ((flags & MotionStateFlag.CurrentHoldKey) != 0)
                currentHoldkey = message.Payload.ReadUInt32();

            MovementData md = DeserializeMovement(message.Payload, flags);

            uint numAnimations = (uint)flags >> 11;
            MotionItem[] commands = new MotionItem[numAnimations];
            for (int i = 0; i < numAnimations; i++)
            {
                ushort motionCommand = message.Payload.ReadUInt16();
                ushort sequence = message.Payload.ReadUInt16();
                float speed = message.Payload.ReadSingle();
                commands[i] = new MotionItem((MotionCommand)motionCommand, speed);
            }

            position = new Position(message.Payload);
            position.LandblockId = new LandblockId(position.Cell);

            instanceTimestamp = message.Payload.ReadUInt16();
            serverControlTimestamp = message.Payload.ReadUInt16();
            teleportTimestamp = message.Payload.ReadUInt16();
            forcePositionTimestamp = message.Payload.ReadUInt16();
            message.Payload.ReadByte();
            // FIXME(ddevec): If speed values in the motion need to be updated by the player, this will likely need to be adjusted
            // Send the motion to the player

            session.Player.RequestUpdateMotion(currentHoldkey, md, commands);

            session.Player.RequestUpdatePosition(position);
        }
    }
}
