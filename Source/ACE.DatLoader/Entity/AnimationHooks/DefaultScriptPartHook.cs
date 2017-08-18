using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity.AnimationHooks
{
    public class DefaultScriptPartHook : IHook
    {
        public uint PartIndex { get; private set; }

        public static DefaultScriptPartHook ReadHookType(DatReader datReader)
        {
            DefaultScriptPartHook dsp = new DefaultScriptPartHook();
            dsp.PartIndex = datReader.ReadUInt32();
            return dsp;
        }
    }
}
