using System;
using System.IO;

namespace ACE.DatLoader.Entity.AnimationHooks
{
    public class ReplaceObjectHook : AnimationHook
    {
        public AnimationPartChange APChange { get; } = new AnimationPartChange();

        public override void Unpack(BinaryReader reader)
        {
            base.Unpack(reader);

            // The structure of AnimationPartChange here is slightly different for some reason than the other imeplementations.
            APChange.PartIsOneByte = false;

            APChange.Unpack(reader);
        }
    }
}
