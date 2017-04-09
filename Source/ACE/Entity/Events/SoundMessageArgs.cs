using ACE.Entity.Enum;
using ACE.Network.Enum;
using System;

namespace ACE.Entity.Events
{
    public class SoundMessageArgs : EventArgs
    {
        public Sound Sound { get; set; }

        public SoundMessageArgs(Sound sound)
        {
            this.Sound = sound;
        }
    }
}
