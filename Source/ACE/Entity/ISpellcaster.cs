using System;
using ACE.Entity.Enum;
using ACE.Entity.Events;

namespace ACE.Entity
{
    /// <summary>
    /// object that can cast spells
    /// </summary>
    public interface ISpellcaster
    { 
        event EventHandler<SpellCastArgs> OnSpellCast;
    }
}
