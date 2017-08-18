using ACE.Entity;

namespace ACE.DatLoader.Entity.AnimationHooks
{
    public class SetOmegaHook : IHook
    {
        public AceVector3 Axis { get; private set; }

        public static SetOmegaHook ReadHookType(DatReader datReader)
        {
            SetOmegaHook so = new SetOmegaHook();
            so.Axis = new AceVector3(datReader.ReadSingle(), datReader.ReadSingle(), datReader.ReadSingle());
            return so;
        }
    }
}
