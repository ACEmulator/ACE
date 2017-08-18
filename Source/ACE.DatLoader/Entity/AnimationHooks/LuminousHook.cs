namespace ACE.DatLoader.Entity.AnimationHooks
{
    public class LuminousHook : IHook
    {
        public float Start { get; private set; }
        public float End { get; private set; }
        public float Time { get; private set; }

        public static LuminousHook ReadHookType(DatReader datReader)
        {
            LuminousHook l = new LuminousHook();
            l.Start = datReader.ReadSingle();
            l.End = datReader.ReadSingle();
            l.Time = datReader.ReadSingle();
            return l;
        }
    }
}
