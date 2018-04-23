using System;
using System.Linq;
using System.Numerics;
using ACE.Database.Models.Shard;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Network.Enum;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Physics.Animation;
using ACE.Server.Physics.Extensions;

namespace ACE.Server.WorldObjects
{
    public enum AttackType
    {
        Melee,
        Missile
    };

    partial class Player
    {
        public Skill GetCurrentWeaponSkill()
        {
            var weapon = GetEquippedWeapon();

            // missile weapon
            if (weapon.CurrentWieldedLocation == EquipMask.MissileWeapon)
                return GetCreatureSkill(Skill.MissileWeapons).Skill;

            // hack for converting pre-MoA skills
            var light = GetCreatureSkill(Skill.LightWeapons);
            var heavy = GetCreatureSkill(Skill.HeavyWeapons);
            var finesse = GetCreatureSkill(Skill.FinesseWeapons);

            var maxMelee = light;
            if (heavy.Current > maxMelee.Current)
                maxMelee = heavy;
            if (finesse.Current > maxMelee.Current)
                maxMelee = finesse;

            return maxMelee.Skill;
        }

        public AttackType GetAttackType()
        {
            return GetEquippedWeapon().CurrentWieldedLocation == EquipMask.MeleeWeapon ? AttackType.Melee : AttackType.Missile;
        }

        public void DamageTarget(WorldObject target)
        {
            var creature = target as Creature;

            if (creature.Health.Current <= 0)
                return;

            var critical = false;
            var damage = CalculateDamage(target, ref critical);
            if (damage > 0.0f)
                target.TakeDamage(this, damage, critical);
            else
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{target.Name} evaded your attack.", ChatMessageType.CombatEnemy));

            if (damage > 0.0f && creature.Health.Current > 0)
            {
                // notify attacker
                var intDamage = (uint)Math.Round(damage);
                Session.Network.EnqueueSend(new GameEventAttackerNotification(Session, target.Name, GetDamageType(), (float)intDamage / creature.Health.MaxValue, intDamage, critical, new AttackConditions()));

                // splatter effects
                Session.Network.EnqueueSend(new GameMessageSound(target.Guid, Sound.HitFlesh1, 0.5f));
                if (damage >= creature.Health.MaxValue * 0.25f)
                {
                    var painSound = (Sound)Enum.Parse(typeof(Sound), "Wound" + Physics.Common.Random.RollDice(1, 3), true);
                    Session.Network.EnqueueSend(new GameMessageSound(target.Guid, painSound, 1.0f));
                }
                var splatter = (PlayScript)Enum.Parse(typeof(PlayScript), "Splatter" + GetSplatterHeight() + GetSplatterDir(target));
                Session.Network.EnqueueSend(new GameMessageScript(target.Guid, splatter));
            }
        }

        public float GetEvadeChance(WorldObject target)
        {
            // get player attack skill
            var attackSkill = GetCreatureSkill(GetCurrentWeaponSkill());

            // get target defense skill
            var creature = target as Creature;
            var defenseSkill = GetAttackType() == AttackType.Melee ? Skill.MeleeDefense : Skill.MissileDefense;
            var difficulty = creature.GetCreatureSkill(defenseSkill).Current;

            var evadeChance = 1.0f - SkillCheck.GetSkillChance((int)attackSkill.Current, (int)difficulty);
            return (float)evadeChance;
        }

        public override Range GetBaseDamage()
        {
            var attackType = GetAttackType();
            var damageSource = attackType == AttackType.Melee ? GetEquippedWeapon() : GetEquippedAmmo();

            return damageSource != null ? damageSource.GetDamageMod(this) : new Range(1, 5);
        }

        public float CalculateDamage(WorldObject target, ref bool criticalHit)
        {
            // evasion chance
            var evadeChance = GetEvadeChance(target);
            if (Physics.Common.Random.RollDice(0.0f, 1.0f) < evadeChance)
                return 0.0f;

            // get weapon base damage
            var baseDamageRange = GetBaseDamage();
            var baseDamage = Physics.Common.Random.RollDice(baseDamageRange.Min, baseDamageRange.Max);

            // get damage mods
            var powerAccuracyMod = GetPowerAccuracyMod();
            var attributeMod = GetAttributeMod();
            var damage = baseDamage * attributeMod * powerAccuracyMod;

            // critical hit
            var critical = 0.1f;
            if (Physics.Common.Random.RollDice(0.0f, 1.0f) < critical)
            {
                damage = baseDamageRange.Max * attributeMod * powerAccuracyMod * 2.0f;
                criticalHit = true;
            }

            // get random body part @ attack height
            var bodyPart = BodyParts.GetBodyPart(AttackHeight);

            // get target armor
            var armor = GetArmor(target, bodyPart);

            // get target resistance
            var damageType = GetDamageType();
            var resistance = GetResistance(target, bodyPart, damageType);

            // scale damage for armor
            damage *= SkillFormula.CalcArmorMod(resistance);

            return damage;
        }

        public float GetAttributeMod()
        {
            var attackType = GetAttackType();
            if (attackType == AttackType.Melee)
                return SkillFormula.GetAttributeMod(PropertyAttribute.Strength, (int)Strength.Current);
            else if (attackType == AttackType.Missile)
                return SkillFormula.GetAttributeMod(PropertyAttribute.Coordination, (int)Coordination.Current);
            else
                return 1.0f;
        }

        public float GetPowerAccuracyMod()
        {
            var attackType = GetAttackType();
            if (attackType == AttackType.Melee)
                return PowerLevel + 0.5f;
            else if (attackType == AttackType.Missile)
                return AccuracyLevel + 0.6f;
            else
                return 1.0f;
        }

        public Creature_BodyPart GetBodyPart(WorldObject target, BodyPart bodyPart)
        {
            var creature = target as Creature;

            BiotaPropertiesBodyPart part = null;
            var idx = BodyParts.Indices[bodyPart];
            if (creature.Biota.BiotaPropertiesBodyPart.Count > idx)
                part = creature.Biota.BiotaPropertiesBodyPart.ElementAt(idx);
            else
                part = creature.Biota.BiotaPropertiesBodyPart.FirstOrDefault();

            return new Creature_BodyPart(creature, part);
        }

        public float GetArmor(WorldObject target, BodyPart bodyPart)
        {
            var part = GetBodyPart(target, bodyPart);

            return part.BaseArmorMod;
        }

        public float GetResistance(WorldObject target, BodyPart bodyPart, DamageType damageType)
        {
            var part = GetBodyPart(target, bodyPart);

            var resistance = 1.0f;

            switch (damageType)
            {
                case DamageType.Slash:
                    resistance = part.ArmorVsSlash;
                    break;

                case DamageType.Pierce:
                    resistance = part.ArmorVsPierce;
                    break;

                case DamageType.Bludgeon:
                    resistance = part.ArmorVsBludgeon;
                    break;

                case DamageType.Fire:
                    resistance = part.ArmorVsFire;
                    break;

                case DamageType.Cold:
                    resistance = part.ArmorVsCold;
                    break;

                case DamageType.Acid:
                    resistance = part.ArmorVsAcid;
                    break;

                case DamageType.Electric:
                    resistance = part.ArmorVsElectric;
                    break;

                case DamageType.Nether:
                    resistance = part.ArmorVsNether;
                    break;
            }

            return resistance;
        }

        public string GetSplatterDir(WorldObject target)
        {
            var sourcePos = new Vector3(Location.PositionX, Location.PositionY, 0);
            var targetPos = new Vector3(target.Location.PositionX, target.Location.PositionY, 0);
            var targetDir = new AFrame(target.Location.Pos, target.Location.Rotation).get_vector_heading();

            targetDir.Z = 0;
            targetDir = targetDir.Normalize();

            var sourceToTarget = (sourcePos - targetPos).Normalize();

            var dir = Vector3.Dot(sourceToTarget, targetDir);
            var angle = Vector3.Cross(sourceToTarget, targetDir);

            var frontBack = dir >= 0 ? "Front" : "Back";
            var leftRight = angle.Z <= 0 ? "Left" : "Right";

            return leftRight + frontBack;
        }

        public string GetSplatterHeight()
        {
            switch (AttackHeight)
            {
                case AttackHeight.Low: return "Low";
                case AttackHeight.Medium: return "Mid";
                case AttackHeight.High: default: return "Up";
            }
        }
    }
}
