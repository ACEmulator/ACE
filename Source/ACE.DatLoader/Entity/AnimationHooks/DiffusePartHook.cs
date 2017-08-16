namespace ACE.DatLoader.Entity.AnimationHooks
{
    public class DiffusePartHook : IHook
    {
        public uint Part { get; private set; }
        public float Start { get; private set; }
        public float End { get; private set; }
        public float Time { get; private set; }

        public static DiffusePartHook ReadHookType(DatReader datReader)
        {
            DiffusePartHook dp = new DiffusePartHook();
            dp.Part = datReader.ReadUInt32();
            dp.Start = datReader.ReadSingle();
            dp.End = datReader.ReadSingle();
            dp.Time = datReader.ReadSingle();
            return dp;
        }
    }
}
