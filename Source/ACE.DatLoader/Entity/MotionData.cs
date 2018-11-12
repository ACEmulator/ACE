using System.Collections.Generic;
using System.IO;
using System.Numerics;

using ACE.Entity.Enum;

namespace ACE.DatLoader.Entity
{
    public class MotionData : IUnpackable
    {
        public byte Bitfield { get; private set; }
        public MotionDataFlags Flags { get; private set; }
        public List<AnimData> Anims { get; } = new List<AnimData>();
        public Vector3 Velocity { get; private set; }
        public Vector3 Omega { get; private set; }

        public void Unpack(BinaryReader reader)
        {
            var numAnims    = reader.ReadByte();
            Bitfield        = reader.ReadByte();
            Flags           = (MotionDataFlags)reader.ReadByte();
            reader.AlignBoundary();

            Anims.Unpack(reader, numAnims);

            if ((Flags & MotionDataFlags.HasVelocity) != 0)
                Velocity = reader.ReadVector3();

            if ((Flags & MotionDataFlags.HasOmega) != 0)
                Omega = reader.ReadVector3();
        }
    }
}
