namespace ACE.DatLoader.Entity.AnimationHooks
{
    public class TransparentHook : IHook
    {
        public float Start { get; private set; }
        public float End { get; private set; }
        public float Time { get; private set; }

        public static TransparentHook ReadHookType(DatReader datReader)
        {
            TransparentHook tp = new TransparentHook();
            tp.Start = datReader.ReadSingle();
            tp.End = datReader.ReadSingle();
            tp.Time = datReader.ReadSingle();
            return tp;
        }
    }
}
