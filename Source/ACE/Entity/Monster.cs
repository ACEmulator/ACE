using System;
using ACE.Entity.Enum;
using ACE.Entity.Events;

namespace ACE.Entity
{
    public class Monster : MutableWorldObject, ISpellcaster, IChatSender
    {
        public Monster(ObjectGuid guid) : base(ObjectType.Creature | ObjectType.Caster, guid)
        {
        }

        public event EventHandler<ChatMessageArgs> OnChat;

        public event EventHandler<SpellCastArgs> OnSpellCast;
    }
}
