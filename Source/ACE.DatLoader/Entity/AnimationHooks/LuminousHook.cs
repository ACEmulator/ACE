namespace ACE.DatLoader.Entity.AnimationHooks
{
    public class LuminousHook : IHook
    {
        public float Start { get; set; }
        public float End { get; set; }
        public float Time { get; set; }

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
