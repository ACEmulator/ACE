namespace ACE.DatLoader.Entity.AnimationHooks
{
    public class SoundTableHook : IHook
    {
        public uint SoundType { get; private set; }

        public static SoundTableHook ReadHookType(DatReader datReader)
        {
            SoundTableHook s = new SoundTableHook();
            s.SoundType = datReader.ReadUInt32();
            return s;
        }
    }
}
