namespace ACE.DatLoader.Entity.AnimationHooks
{
    public class DiffuseHook : IHook
    {
        public float Start { get; set; }
        public float End { get; set; }
        public float Time { get; set; }

        public static DiffuseHook ReadHookType(DatReader datReader)
        {
            DiffuseHook d = new DiffuseHook();
            d.Start = datReader.ReadSingle();
            d.End = datReader.ReadSingle();
            d.Time = datReader.ReadSingle();
            return d;
        }
    }
}
