using ACE.Entity;

namespace ACE.DatLoader.Entity.AnimationHooks
{
    public class SetOmegaHook : IHook
    {
        public AceVector3 Axis { get; set; }

        public static SetOmegaHook ReadHookType(DatReader datReader)
        {
            SetOmegaHook so = new SetOmegaHook();
            AceVector3 v = new AceVector3(datReader.ReadSingle(), datReader.ReadSingle(), datReader.ReadSingle());
            so.Axis = v;
            return so;
        }
    }
}
