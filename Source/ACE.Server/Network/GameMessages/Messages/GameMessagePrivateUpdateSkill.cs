using System;
using ACE.Entity.Enum;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessagePrivateUpdateSkill : GameMessage
    {
        public GameMessagePrivateUpdateSkill(WorldObject worldObject, Skill skill, SkillStatus status, uint ranks, uint baseValue, uint totalInvestment)
            : base(GameMessageOpcode.PrivateUpdateSkill, GameMessageGroup.UIQueue)
        {
            // TODO Why is baseValue being passed to this function even though it's not used?

            switch (skill)
            {
                case Skill.Axe:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillAxe));
                    break;
                case Skill.Bow:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillBow));
                    break;
                case Skill.Crossbow:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillCrossBow));
                    break;
                case Skill.Dagger:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillDagger));
                    break;
                case Skill.Mace:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillMace));
                    break;
                case Skill.MeleeDefense:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillMeleeDefense));
                    break;
                case Skill.MissileDefense:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillMissileDefense));
                    break;
                case Skill.Sling:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillSling));
                    break;
                case Skill.Spear:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillSpear));
                    break;
                case Skill.Staff:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillStaff));
                    break;
                case Skill.Sword:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillSword));
                    break;
                case Skill.ThrownWeapon:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillThrownWeapon));
                    break;
                case Skill.UnarmedCombat:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillUnarmedCombat));
                    break;
                case Skill.ArcaneLore:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillArcaneLore));
                    break;
                case Skill.MagicDefense:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillMagicDefense));
                    break;
                case Skill.ManaConversion:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillManaConversion));
                    break;
                case Skill.Spellcraft:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillSpellcraft));
                    break;
                case Skill.AssessPerson:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillPersonalAppraisal));
                    break;
                case Skill.Deception:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillDeception));
                    break;
                case Skill.Healing:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillHealing));
                    break;
                case Skill.Jump:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillJump));
                    break;
                case Skill.Lockpick:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillLockpick));
                    break;
                case Skill.Run:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillRun));
                    break;
                case Skill.Awareness:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillAwareness));
                    break;
                case Skill.ArmsAndArmorRepair:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillArmsAndArmorRepair));
                    break;
                case Skill.AssessCreature:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillCreatureAppraisal));
                    break;
                case Skill.WeaponTinkering:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillWeaponAppraisal));
                    break;
                case Skill.ArmorTinkering:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillArmorAppraisal));
                    break;
                case Skill.MagicItemTinkering:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillMagicItemAppraisal));
                    break;
                case Skill.CreatureEnchantment:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillCreatureEnchantment));
                    break;
                case Skill.ItemEnchantment:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillItemEnchantment));
                    break;
                case Skill.LifeMagic:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillLifeMagic));
                    break;
                case Skill.WarMagic:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillWarMagic));
                    break;
                case Skill.Leadership:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillLeadership));
                    break;
                case Skill.Loyalty:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillLoyalty));
                    break;
                case Skill.Fletching:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillFletching));
                    break;
                case Skill.Alchemy:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillAlchemy));
                    break;
                case Skill.Cooking:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillCooking));
                    break;
                case Skill.Salvaging:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillSalvaging));
                    break;
                case Skill.TwoHandedCombat:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillTwoHandedCombat));
                    break;
                case Skill.Gearcraft:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillGearcraft));
                    break;
                case Skill.VoidMagic:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillVoidMagic));
                    break;
                case Skill.HeavyWeapons:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillHeavyWeapons));
                    break;
                case Skill.LightWeapons:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillLightWeapons));
                    break;
                case Skill.FinesseWeapons:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillFinesseWeapons));
                    break;
                case Skill.MissileWeapons:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillMissileWeapons));
                    break;
                case Skill.Shield:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillShield));
                    break;
                case Skill.DualWield:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillDualWield));
                    break;
                case Skill.Recklessness:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillRecklessness));
                    break;
                case Skill.SneakAttack:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillSneakAttack));
                    break;
                case Skill.DirtyFighting:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillDirtyFighting));
                    break;
                case Skill.Challenge:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillChallenge));
                    break;
                case Skill.Summoning:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateSkillSummoning));
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
