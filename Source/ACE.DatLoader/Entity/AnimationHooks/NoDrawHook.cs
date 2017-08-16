namespace ACE.DatLoader.Entity.AnimationHooks
{
    public class NoDrawHook : IHook
    {
        public uint NoDraw { get; set; }

        public static NoDrawHook ReadHookType(DatReader datReader)
        {
            NoDrawHook nd = new NoDrawHook();
            nd.NoDraw = datReader.ReadUInt32();
            return nd;
        }
    }
}
