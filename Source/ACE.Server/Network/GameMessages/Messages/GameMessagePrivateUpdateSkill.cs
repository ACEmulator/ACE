using System;
using ACE.Entity.Enum;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessagePrivateUpdateSkill : GameMessage
    {
        public GameMessagePrivateUpdateSkill(Session session, Skill skill, SkillStatus status, uint ranks, uint baseValue, uint totalInvestment)
            : base(GameMessageOpcode.PrivateUpdateSkill, GameMessageGroup.UIQueue)
        {
            // TODO We shouldn't be passing session. Insetad, we should pass the value after session.UpdateSkillSequence++.
            // TODO Why is baseValue being passed to this function even though it's not used?

            switch (skill)
            {
                case Skill.Axe:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillAxe));
                    break;
                case Skill.Bow:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillBow));
                    break;
                case Skill.Crossbow:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillCrossBow));
                    break;
                case Skill.Dagger:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillDagger));
                    break;
                case Skill.Mace:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillMace));
                    break;
                case Skill.MeleeDefense:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillMeleeDefense));
                    break;
                case Skill.MissileDefense:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillMissileDefense));
                    break;
                case Skill.Sling:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillSling));
                    break;
                case Skill.Spear:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillSpear));
                    break;
                case Skill.Staff:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillStaff));
                    break;
                case Skill.Sword:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillSword));
                    break;
                case Skill.ThrownWeapon:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillThrownWeapon));
                    break;
                case Skill.UnarmedCombat:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillUnarmedCombat));
                    break;
                case Skill.ArcaneLore:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillArcaneLore));
                    break;
                case Skill.MagicDefense:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillMagicDefense));
                    break;
                case Skill.ManaConversion:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillManaConversion));
                    break;
                case Skill.Spellcraft:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillSpellcraft));
                    break;
                case Skill.AssessPerson:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillPersonalAppraisal));
                    break;
                case Skill.Deception:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillDeception));
                    break;
                case Skill.Healing:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillHealing));
                    break;
                case Skill.Jump:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillJump));
                    break;
                case Skill.Lockpick:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillLockpick));
                    break;
                case Skill.Run:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillRun));
                    break;
                case Skill.Awareness:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillAwareness));
                    break;
                case Skill.ArmsAndArmorRepair:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillArmsAndArmorRepair));
                    break;
                case Skill.AssessCreature:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillCreatureAppraisal));
                    break;
                case Skill.WeaponTinkering:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillWeaponAppraisal));
                    break;
                case Skill.ArmorTinkering:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillArmorAppraisal));
                    break;
                case Skill.MagicItemTinkering:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillMagicItemAppraisal));
                    break;
                case Skill.CreatureEnchantment:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillCreatureEnchantment));
                    break;
                case Skill.ItemEnchantment:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillItemEnchantment));
                    break;
                case Skill.LifeMagic:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillLifeMagic));
                    break;
                case Skill.WarMagic:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillWarMagic));
                    break;
                case Skill.Leadership:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillLeadership));
                    break;
                case Skill.Loyalty:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillLoyalty));
                    break;
                case Skill.Fletching:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillFletching));
                    break;
                case Skill.Alchemy:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillAlchemy));
                    break;
                case Skill.Cooking:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillCooking));
                    break;
                case Skill.Salvaging:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillSalvaging));
                    break;
                case Skill.TwoHandedCombat:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillTwoHandedCombat));
                    break;
                case Skill.Gearcraft:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillGearcraft));
                    break;
                case Skill.VoidMagic:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillVoidMagic));
                    break;
                case Skill.HeavyWeapons:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillHeavyWeapons));
                    break;
                case Skill.LightWeapons:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillLightWeapons));
                    break;
                case Skill.FinesseWeapons:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillFinesseWeapons));
                    break;
                case Skill.MissileWeapons:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillMissileWeapons));
                    break;
                case Skill.Shield:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillShield));
                    break;
                case Skill.DualWield:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillDualWield));
                    break;
                case Skill.Recklessness:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillRecklessness));
                    break;
                case Skill.SneakAttack:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillSneakAttack));
                    break;
                case Skill.DirtyFighting:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillDirtyFighting));
                    break;
                case Skill.Challenge:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillChallenge));
                    break;
                case Skill.Summoning:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillSummoning));
                    break;
            }

            Writer.Write((uint)skill);
            Writer.Write(Convert.ToUInt16(ranks));
            Writer.Write(Convert.ToUInt16(1)); // no clue, but this makes it work.
            Writer.Write((uint)status);
            Writer.Write(totalInvestment);

            // not sure what's in these, but anything in the first DWORD gets added to your current skill value - augmentations perhaps?
            Writer.Write(0u);
            Writer.Write(0u);
            Writer.Write(0u);
            Writer.Write(0u);
        }
    }
}
