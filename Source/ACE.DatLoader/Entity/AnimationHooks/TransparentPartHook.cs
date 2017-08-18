namespace ACE.DatLoader.Entity.AnimationHooks
{
    public class TransparentPartHook : IHook
    {
        public uint Part { get; private set; }
        public float Start { get; private set; }
        public float End { get; private set; }
        public float Time { get; private set; }

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
