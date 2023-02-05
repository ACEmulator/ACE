using ACE.Entity.Enum;
using ACE.Server.WorldObjects;

namespace ACE.Server.Entity
{
    public class CastSpellParams
    {
        public Spell Spell { get; set; }
        //public bool IsWeaponSpell { get; set; }
        public WorldObject CasterItem { get; set; }
        public uint MagicSkill { get; set; }
        public uint ManaUsed { get; set; }
        public WorldObject Target { get; set; }
        public Player.CastingPreCheckStatus Status { get; set; }

        public bool HasWindupGestures => !Spell.Flags.HasFlag(SpellFlags.FastCast) && CasterItem == null && Spell.Formula.HasWindupGestures;

        public CastSpellParams(Spell spell, WorldObject casterItem, uint magicSkill, uint manaUsed, WorldObject target, Player.CastingPreCheckStatus status)
        {
            Spell = spell;
            //IsWeaponSpell = isWeaponSpell;
            CasterItem = casterItem;
            MagicSkill = magicSkill;
            ManaUsed = manaUsed;
            Target = target;
            Status = status;
        }

        public override string ToString()
        {
            var targetName = Target != null ? Target.Name : "null";

            return $"{Spell.Name}, {CasterItem?.Name}, {MagicSkill}, {ManaUsed}, {targetName}, {Status}";
        }
    }
}
