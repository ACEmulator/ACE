namespace ACE.DatLoader.Entity.AnimationHooks
{
    public class StopParticleHook : IHook
    {
        public uint EmitterId { get; set; }

        public static StopParticleHook ReadHookType(DatReader datReader)
        {
            StopParticleHook sp = new StopParticleHook();
            sp.EmitterId = datReader.ReadUInt32();
            return sp;
        }
    }
}
