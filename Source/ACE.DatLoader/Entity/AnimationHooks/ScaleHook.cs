using System.IO;

namespace ACE.DatLoader.Entity.AnimationHooks
{
    public class ScaleHook : AnimationHook
    {
        public float End { get; private set; }
        public float Time { get; private set; }

        public override void Unpack(BinaryReader reader)
        {
            base.Unpack(reader);

            End     = reader.ReadSingle();
            Time    = reader.ReadSingle();
        }
    }
}
