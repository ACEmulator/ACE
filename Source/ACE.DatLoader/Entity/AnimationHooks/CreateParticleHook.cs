using System.IO;

namespace ACE.DatLoader.Entity.AnimationHooks
{
    public class CreateParticleHook : AnimationHook
    {
        public uint EmitterInfoId { get; private set; }
        public uint PartIndex { get; private set; }
        public Frame Offset { get; } = new Frame();
        public uint EmitterId { get; private set; }

        public override void Unpack(BinaryReader reader)
        {
            base.Unpack(reader);

            EmitterInfoId   = reader.ReadUInt32();
            PartIndex       = reader.ReadUInt32();
            Offset.Unpack(reader);
            EmitterId       = reader.ReadUInt32();
        }
    }
}
