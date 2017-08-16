using ACE.Entity;

namespace ACE.DatLoader.Entity.AnimationHooks
{
    public class CreateParticleHook : IHook
    {
        public uint EmitterInfoId { get; set; }
        public uint PartIndex { get; set; }
        public Position Offset { get; set; }
        public uint EmitterId { get; set; }

        public static CreateParticleHook ReadHookType(DatReader datReader)
        {
            CreateParticleHook hook = new CreateParticleHook();
            hook.EmitterInfoId = datReader.ReadUInt32();

            hook.PartIndex = datReader.ReadUInt32();

            Position p = new Position();
            p.PositionX = datReader.ReadSingle();
            p.PositionY = datReader.ReadSingle();
            p.PositionZ = datReader.ReadSingle();
            p.RotationW = datReader.ReadSingle();
            p.RotationX = datReader.ReadSingle();
            p.RotationY = datReader.ReadSingle();
            p.RotationZ = datReader.ReadSingle();
            hook.Offset = p;

            hook.EmitterId = datReader.ReadUInt32();
            return hook;
        }
    }
}
