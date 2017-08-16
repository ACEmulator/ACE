using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity.AnimationHooks
{
    public class TextureVelocityPartHook : IHook
    {
        public uint PartIndex { get; private set; }
        public float USpeed { get; private set; }
        public float VSpeed { get; private set; }

        public static TextureVelocityPartHook ReadHookType(DatReader datReader)
        {
            TextureVelocityPartHook tv = new TextureVelocityPartHook();
            tv.PartIndex = datReader.ReadUInt32();
            tv.USpeed = datReader.ReadSingle();
            tv.VSpeed = datReader.ReadSingle();
            return tv;
        }
    }
}
