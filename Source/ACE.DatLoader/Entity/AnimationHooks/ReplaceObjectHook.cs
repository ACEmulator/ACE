using System.IO;

namespace ACE.DatLoader.Entity.AnimationHooks
{
    public class ReplaceObjectHook : AnimationHook
    {
        public AnimationPartChange APChange { get; } = new AnimationPartChange();

        public override void Unpack(BinaryReader reader)
        {
            base.Unpack(reader);

            APChange.Unpack(reader);
        }
    }
}
