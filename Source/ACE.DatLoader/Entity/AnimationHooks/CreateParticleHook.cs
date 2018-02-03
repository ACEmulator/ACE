using System.IO;

using ACE.Entity;

namespace ACE.DatLoader.Entity.AnimationHooks
{
    public class CreateParticleHook : AnimationHook
    {
        public uint EmitterInfoId { get; private set; }
        public uint PartIndex { get; private set; }
        public Position Offset { get; } = new Position();
        public uint EmitterId { get; private set; }

        public override void Unpack(BinaryReader reader)
        {
            base.Unpack(reader);

            EmitterInfoId   = reader.ReadUInt32();
            PartIndex       = reader.ReadUInt32();
            Offset.ReadFrame(reader);
            EmitterId       = reader.ReadUInt32();
        }
    }
}
