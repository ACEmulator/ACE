using System;

namespace ACE.Entity.Events
{
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
