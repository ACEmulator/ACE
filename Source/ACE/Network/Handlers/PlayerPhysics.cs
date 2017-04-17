using ACE.Common.Extensions;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Network;
using ACE.Network.Enum;
using ACE.Network.GameAction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity
{
    public partial class Player
    {
        [GameAction(GameActionType.LoginComplete)]
        private void LoginCompleteAction(ClientMessage message)
        {
            InWorld = true;
            SetPhysicsState(PhysicsState.ReportCollision | PhysicsState.Gravity | PhysicsState.EdgeSlide);
        }

        [GameAction(GameActionType.AutonomousPosition)]
        private void AutonomousPositionAction(ClientMessage message)
        {
            var position = new Position(message.Payload);
            var instanceTimestamp = message.Payload.ReadUInt16();
            var serverControlTimestamp = message.Payload.ReadUInt16();
            var teleportTimestamp = message.Payload.ReadUInt16();
            var forcePositionTimestamp = message.Payload.ReadUInt16();
            message.Payload.ReadByte();
            UpdatePosition(position);
        }

        private class MotionStateDirection
        {
            public uint Command { get; set; }
            public uint HoldKey { get; set; }
            public float Speed { get; set; } = 1.0f;
        }

        [GameAction(GameActionType.MoveToState)]
        private void MoveToStateAction(ClientMessage message)
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
            position.CharacterId = Guid.Low;
            position.PositionType = Entity.Enum.PositionType.Location;
            position.LandblockId = new LandblockId(position.Cell);

            instanceTimestamp = message.Payload.ReadUInt16();
            serverControlTimestamp = message.Payload.ReadUInt16();
            teleportTimestamp = message.Payload.ReadUInt16();
            forcePositionTimestamp = message.Payload.ReadUInt16();
            message.Payload.ReadByte();

            UpdatePosition(position);
        }

        [GameAction(GameActionType.ChangeCombatMode)]
        private void ChangeCombatModeAction(ClientMessage message)
        {
            var newCombatMode = message.Payload.ReadUInt32();
            SetCombatMode((CombatMode)newCombatMode);
        }
    }
}
