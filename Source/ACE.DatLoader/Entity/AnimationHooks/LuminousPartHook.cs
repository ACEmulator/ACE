namespace ACE.DatLoader.Entity.AnimationHooks
{
    public class LuminousPartHook : IHook
    {
        public uint Part { get; private set; }
        public float Start { get; private set; }
        public float End { get; private set; }
        public float Time { get; private set; }

        public static LuminousPartHook ReadHookType(DatReader datReader)
        {
            LuminousPartHook lp = new LuminousPartHook();
            lp.Part = datReader.ReadUInt32();
            lp.Start = datReader.ReadSingle();
            lp.End = datReader.ReadSingle();
            lp.Time = datReader.ReadSingle();
            return lp;
        }
    }
}
