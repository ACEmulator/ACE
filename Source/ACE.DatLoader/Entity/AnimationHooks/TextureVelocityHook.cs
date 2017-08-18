using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity.AnimationHooks
{
    public class TextureVelocityHook : IHook
    {
        public float USpeed { get; private set; }
        public float VSpeed { get; private set; }

        public static TextureVelocityHook ReadHookType(DatReader datReader)
        {
            TextureVelocityHook tv = new TextureVelocityHook();
            tv.USpeed = datReader.ReadSingle();
            tv.VSpeed = datReader.ReadSingle();
            return tv;
        }
    }
}
