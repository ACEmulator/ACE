using System.IO;

namespace ACE.DatLoader.Entity.AnimationHooks
{
    public class TextureVelocityPartHook : AnimationHook
    {
        public uint PartIndex { get; private set; }
        public float USpeed { get; private set; }
        public float VSpeed { get; private set; }

        public override void Unpack(BinaryReader reader)
        {
            base.Unpack(reader);

            PartIndex   = reader.ReadUInt32();
            USpeed      = reader.ReadSingle();
            VSpeed      = reader.ReadSingle();
        }
    }
}
