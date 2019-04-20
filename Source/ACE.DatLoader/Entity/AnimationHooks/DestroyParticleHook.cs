using System.IO;

namespace ACE.DatLoader.Entity.AnimationHooks
{
    public class DestroyParticleHook : AnimationHook
    {
        public uint EmitterId { get; private set; }

        public override void Unpack(BinaryReader reader)
        {
            base.Unpack(reader);

            EmitterId = reader.ReadUInt32();
        }
    }
}
