namespace ACE.DatLoader.Entity.AnimationHooks
{
    public class EtherealHook : IHook
    {
        public int Ethereal;

        public static EtherealHook ReadHookType(DatReader datReader)
        {
            EtherealHook e = new EtherealHook();
            e.Ethereal = datReader.ReadInt32();
            return e;
        }
    }
}
