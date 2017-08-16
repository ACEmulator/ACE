namespace ACE.DatLoader.Entity.AnimationHooks
{
    public class ReplaceObjectHook : IHook
    {
        public AnimationPartChange APChange { get; set; }

        public static ReplaceObjectHook ReadHookType(DatReader datReader)
        {
            ReplaceObjectHook r = new ReplaceObjectHook();

            AnimationPartChange ao = new AnimationPartChange();
            ao.PartIndex = (byte)datReader.ReadUInt16();
            ao.PartID = datReader.ReadUInt16();

            r.APChange = ao;

            return r;
        }
    }
}
