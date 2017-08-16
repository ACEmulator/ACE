using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity.AnimationHooks
{
    public class SoundTweakedHook : IHook
    {
        public uint SoundID { get; set; }
        public float Priority { get; set; }
        public float Probability { get; set; }
        public float Volume { get; set; }

        public static SoundTweakedHook ReadHookType(DatReader datReader)
        {
            SoundTweakedHook st = new SoundTweakedHook();
            st.SoundID = datReader.ReadUInt32();
            st.Priority = datReader.ReadSingle();
            st.Probability = datReader.ReadSingle();
            st.Volume = datReader.ReadSingle();
            return st;
        }
    }
}
