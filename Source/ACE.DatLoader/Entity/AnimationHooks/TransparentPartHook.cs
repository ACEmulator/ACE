namespace ACE.DatLoader.Entity.AnimationHooks
{
    public class TransparentPartHook : IHook
    {
        public uint Part { get; set; }
        public float Start { get; set; }
        public float End { get; set; }
        public float Time { get; set; }

        public static TransparentPartHook ReadHookType(DatReader datReader)
        {
            TransparentPartHook tp = new TransparentPartHook();
            tp.Part = datReader.ReadUInt32();
            tp.Start = datReader.ReadSingle();
            tp.End = datReader.ReadSingle();
            tp.Time = datReader.ReadSingle();
            return tp;
        }
    }
}
