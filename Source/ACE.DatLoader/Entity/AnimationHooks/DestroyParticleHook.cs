namespace ACE.DatLoader.Entity.AnimationHooks
{
    public class DestroyParticleHook : IHook
    {
        public uint EmitterId { get; private set; }

        public static DestroyParticleHook ReadHookType(DatReader datReader)
        {
            DestroyParticleHook dp = new DestroyParticleHook();
            dp.EmitterId = datReader.ReadUInt32();
            return dp;
        }
    }
}
