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
            // So we'll read in the 2-byte PartIndex and send that to our other implementation of the Unpack function.
            ushort apChangePartIndex = reader.ReadUInt16();
            APChange.Unpack(reader, apChangePartIndex);
        }
    }
}
