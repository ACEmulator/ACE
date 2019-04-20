using System.IO;

namespace ACE.DatLoader.Entity.AnimationHooks
{
    public class LuminousHook : AnimationHook
    {
        public float Start { get; private set; }
        public float End { get; private set; }
        public float Time { get; private set; }

        public override void Unpack(BinaryReader reader)
        {
            base.Unpack(reader);

            Start   = reader.ReadSingle();
            End     = reader.ReadSingle();
            Time    = reader.ReadSingle();
        }
    }
}
