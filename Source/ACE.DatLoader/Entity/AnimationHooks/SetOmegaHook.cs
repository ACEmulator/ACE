using System.IO;
using System.Numerics;

namespace ACE.DatLoader.Entity.AnimationHooks
{
    public class SetOmegaHook : AnimationHook
    {
        public Vector3 Axis { get; private set; }

        public override void Unpack(BinaryReader reader)
        {
            base.Unpack(reader);

            Axis = reader.ReadVector3();
        }
    }
}
