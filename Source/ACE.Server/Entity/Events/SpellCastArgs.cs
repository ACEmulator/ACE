using System;
using ACE.Entity;

namespace ACE.Server.Entity.Events
{
    /// <summary>
    /// this may or may not end up being used.  a spell casting is really a combination of animation, sound, and effects
    /// </summary>
    public class SpellCastArgs : EventArgs
    {
        /// <summary>
        /// TODO: replace with Spell enumeration type
        /// </summary>
        public uint SpellId { get; set; }

        /// <summary>
        /// id of the target of the spell
        /// </summary>
        public ObjectGuid Target { get; set; }
    }
}
