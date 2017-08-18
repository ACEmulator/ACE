using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity.AnimationHooks
{
    public class CallPESHook : IHook
    {
        public uint PES { get; private set; }
        public float Pause { get; private set; }

        public static CallPESHook ReadHookType(DatReader datReader)
        {
            CallPESHook hook = new CallPESHook();
            hook.PES = datReader.ReadUInt32();
            hook.Pause = datReader.ReadSingle();
            return hook;
        }
    }
}
