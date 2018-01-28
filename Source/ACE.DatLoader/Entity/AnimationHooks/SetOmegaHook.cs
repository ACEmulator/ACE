using System.IO;

using ACE.Entity;

namespace ACE.DatLoader.Entity.AnimationHooks
{
    public class SetOmegaHook : AnimationHook
    {
        public AceVector3 Axis { get; private set; }

        public override void Unpack(BinaryReader reader)
        {
            base.Unpack(reader);

            Axis = new AceVector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }
    }
}
