using System.IO;

namespace ACE.DatLoader.Entity.AnimationHooks
{
    public class SoundTweakedHook : AnimationHook
    {
        public uint SoundID { get; private set; }
        public float Priority { get; private set; }
        public float Probability { get; private set; }
        public float Volume { get; private set; }

        public override void Unpack(BinaryReader reader)
        {
            base.Unpack(reader);

            SoundID = reader.ReadUInt32();
            Priority = reader.ReadSingle();
            Probability = reader.ReadSingle();
            Volume = reader.ReadSingle();
        }
    }
}
