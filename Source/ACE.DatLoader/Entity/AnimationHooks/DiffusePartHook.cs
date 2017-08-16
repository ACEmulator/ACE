namespace ACE.DatLoader.Entity.AnimationHooks
{
    public class DiffusePartHook : IHook
    {
        public uint Part { get; set; }
        public float Start { get; set; }
        public float End { get; set; }
        public float Time { get; set; }

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
