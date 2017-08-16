using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity.AnimationHooks
{
    public class SetLightHook : IHook
    {
        public int LightsOn { get; set; }

        public static SetLightHook ReadHookType(DatReader datReader)
        {
            SetLightHook hook = new SetLightHook();
            hook.LightsOn = datReader.ReadInt32();
            return hook;
        }
    }
}
