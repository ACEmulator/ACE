using System.IO;

namespace ACE.DatLoader.Entity.AnimationHooks
{
    public class SoundTableHook : AnimationHook
    {
        public uint SoundType { get; private set; }

        public override void Unpack(BinaryReader reader)
        {
            base.Unpack(reader);

            SoundType = reader.ReadUInt32();
        }
    }
}
