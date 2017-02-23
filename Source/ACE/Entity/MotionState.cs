using ACE.Network.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity
{
    public class MotionState
    {
        private MotionStateFlag flags = MotionStateFlag.None;
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

        public Position Position
        { get { return position; } }

        public MotionState(BinaryReader payload)
        {
            MotionStateFlag flags = (MotionStateFlag)payload.ReadUInt32();

            if ((flags & MotionStateFlag.CurrentHoldKey) != 0)
                currentHoldkey = payload.ReadUInt32();

            if ((flags & MotionStateFlag.CurrentStyle) != 0)
                currentStyle = payload.ReadUInt32();

            if ((flags & MotionStateFlag.ForwardCommand) != 0)
                forward.Command = payload.ReadUInt32();

            if ((flags & MotionStateFlag.ForwardHoldKey) != 0)
                forward.HoldKey = payload.ReadUInt32();

            if ((flags & MotionStateFlag.ForwardSpeed) != 0)
                forward.Speed = payload.ReadSingle();

            if ((flags & MotionStateFlag.SideStepCommand) != 0)
                sideStep.Command = payload.ReadUInt32();

            if ((flags & MotionStateFlag.SideStepHoldKey) != 0)
                sideStep.HoldKey = payload.ReadUInt32();

            if ((flags & MotionStateFlag.SideStepSpeed) != 0)
                sideStep.Speed = payload.ReadSingle();

            if ((flags & MotionStateFlag.TurnCommand) != 0)
                turn.Command = payload.ReadUInt32();

            if ((flags & MotionStateFlag.TurnHoldKey) != 0)
                turn.HoldKey = payload.ReadUInt32();

            if ((flags & MotionStateFlag.TurnSpeed) != 0)
                turn.Speed = payload.ReadSingle();

            position = new Position(payload);

            instanceTimestamp = payload.ReadUInt16();
            serverControlTimestamp = payload.ReadUInt16();
            teleportTimestamp = payload.ReadUInt16();
            forcePositionTimestamp = payload.ReadUInt16();

            // unknown
            payload.ReadByte();
        }
    }
}
