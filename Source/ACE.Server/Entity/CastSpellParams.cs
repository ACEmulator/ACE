using ACE.Entity.Enum;
using ACE.Server.WorldObjects;

namespace ACE.Server.Entity
{
    public class CastSpellParams
    {
        public Spell Spell { get; set; }
        public bool IsWeaponSpell { get; set; }
        public uint MagicSkill { get; set; }
        public uint ManaUsed { get; set; }
        public WorldObject Target { get; set; }
        public Player.CastingPreCheckStatus Status { get; set; }

        public bool HasWindupGestures => !Spell.Flags.HasFlag(SpellFlags.FastCast) && !IsWeaponSpell && Spell.Formula.HasWindupGestures;

        public CastSpellParams(Spell spell, bool isWeaponSpell, uint magicSkill, uint manaUsed, WorldObject target, Player.CastingPreCheckStatus status)
        {
            Spell = spell;
            IsWeaponSpell = isWeaponSpell;
            MagicSkill = magicSkill;
            ManaUsed = manaUsed;
            Target = target;
            Status = status;
        }

        public override string ToString()
        {
            var targetName = Target != null ? Target.Name : "null";

            return $"{Spell.Name}, {IsWeaponSpell}, {MagicSkill}, {ManaUsed}, {targetName}, {Status}";
        }
    }
}
