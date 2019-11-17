using ACE.Entity;
using ACE.Server.WorldObjects;
using System.IO;

namespace ACE.Server.Network.Structure
{
    /// <summary>
    /// Client sends this structure in F61C - MoveToState
    /// </summary>
    public class MoveToState
    {
        public WorldObject WorldObject;

        public RawMotionState RawMotionState;       // the raw movement commands sent by the client
                                                    // these are in turn translated to an InterpretedMotionState
        public Position Position;

        public ushort InstanceSequence;
        public ushort ServerControlSequence;
        public ushort TeleportSequence;
        public ushort ForcePositionSequence;

        public byte ContactLongJump;

        // not sent in packet directly as bools, parsed from above
        public bool Contact;                // verify: contact (indicates if player is on ground), or sticky bit?
        public bool StandingLongJump;

        public MoveToState() { }

        public MoveToState(WorldObject wo, BinaryReader reader)
        {
            WorldObject = wo;

            RawMotionState = new RawMotionState(this, reader);
            Position = new Position(reader);

            InstanceSequence = reader.ReadUInt16();
            ServerControlSequence = reader.ReadUInt16();
            TeleportSequence = reader.ReadUInt16();
            ForcePositionSequence = reader.ReadUInt16();

            ContactLongJump = reader.ReadByte();
            if ((ContactLongJump & 0x1) != 0)
                Contact = true;
            if ((ContactLongJump & 0x2) != 0)
                StandingLongJump = true;

            // align to DWORD boundary
            reader.Align();
        }
    }
}
