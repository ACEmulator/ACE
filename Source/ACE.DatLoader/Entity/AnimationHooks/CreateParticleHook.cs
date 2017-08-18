using ACE.Entity;

namespace ACE.DatLoader.Entity.AnimationHooks
{
    public class CreateParticleHook : IHook
    {
        public uint EmitterInfoId { get; private set; }
        public uint PartIndex { get; private set; }
        public Position Offset { get; private set; }
        public uint EmitterId { get; private set; }

        public static CreateParticleHook ReadHookType(DatReader datReader)
        {
            CreateParticleHook hook = new CreateParticleHook();
            hook.EmitterInfoId = datReader.ReadUInt32();

            hook.PartIndex = datReader.ReadUInt32();

            hook.Offset = PositionExtensions.ReadPosition(datReader);

            hook.EmitterId = datReader.ReadUInt32();
            return hook;
        }
    }
}
