namespace ACE.DatLoader.Entity.AnimationHooks
{
    public class ScaleHook : IHook
    {
        public float End { get; set; }
        public float Time { get; set; }

        public static ScaleHook ReadHookType(DatReader datReader)
        {
            ScaleHook s = new ScaleHook();
            s.End = datReader.ReadSingle();
            s.Time = datReader.ReadSingle();
            return s;
        }
    }
}
