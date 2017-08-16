namespace ACE.DatLoader.Entity.AnimationHooks
{
    public class SoundHook : IHook
    {
        public uint Id { get; private set; }

        public static SoundHook ReadHookType(DatReader datReader)
        {
            SoundHook s = new SoundHook();
            s.Id = datReader.ReadUInt32();
            return s;
        }
    }
}
