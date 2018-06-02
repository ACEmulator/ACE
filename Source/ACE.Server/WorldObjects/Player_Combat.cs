using System;
using System.Linq;
using ACE.Database.Models.Shard;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Network.Enum;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    public enum AttackType
    {
        Melee,
        Missile,
        Magic
    };

    partial class Player
    {
        public Skill GetCurrentWeaponSkill()
        {
            var weapon = GetEquippedWeapon();

            if (weapon == null)
                return GetCreatureSkill(Skill.FinesseWeapons).Skill;    // Not sure what skill should be used for bare knuckle melee :: Change if incorrect

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

        public override AttackType GetAttackType()
        {
            AttackType attackType;
            if (GetEquippedWeapon() == null)
            {
                attackType = AttackType.Melee;
            }
            else
                attackType = GetEquippedWeapon().CurrentWieldedLocation == EquipMask.MeleeWeapon ? AttackType.Melee : AttackType.Missile;

            return attackType;
        }

        public float DamageTarget(WorldObject target, WorldObject damageSource)
        {
            var creature = target as Creature;

            if (creature.Health.Current <= 0)
                return 0.0f;

            var critical = false;
            var damage = CalculateDamage(target, damageSource, ref critical);
            if (damage > 0.0f)
                target.TakeDamage(this, damage, critical);
            else
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{target.Name} evaded your attack.", ChatMessageType.CombatSelf));

            if (damage > 0.0f && creature.Health.Current > 0)
            {
                // notify attacker
                var intDamage = (uint)Math.Round(damage);
                if (damageSource?.ItemType == ItemType.MissileWeapon)
                {
                    var damageType = (DamageType)damageSource.GetProperty(PropertyInt.DamageType);
                    Session.Network.EnqueueSend(new GameEventAttackerNotification(Session, target.Name, damageType, (float)intDamage / creature.Health.MaxValue, intDamage, critical, new AttackConditions()));
                }
                else
                {
                    Session.Network.EnqueueSend(new GameEventAttackerNotification(Session, target.Name, GetDamageType(), (float)intDamage / creature.Health.MaxValue, intDamage, critical, new AttackConditions()));
                }

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
            return damage;
        }

        public float GetEvadeChance(WorldObject target)
        {
            // get player attack skill
            var attackSkill = GetCreatureSkill(GetCurrentWeaponSkill());

            // get target defense skill
            var creature = target as Creature;
            var defenseSkill = GetAttackType() == AttackType.Melee ? Skill.MeleeDefense : Skill.MissileDefense;
            var difficulty = creature.GetCreatureSkill(defenseSkill).Current;

            //Console.WriteLine("Attack skill: " + attackSkill.Current);
            //Console.WriteLine("Defense skill: " + difficulty);

            var evadeChance = 1.0f - SkillCheck.GetSkillChance((int)attackSkill.Current, (int)difficulty);
            return (float)evadeChance;
        }

        public override Range GetBaseDamage()
        {
            var attackType = GetAttackType();
            var damageSource = attackType == AttackType.Melee ? GetEquippedWeapon() : GetEquippedAmmo();

            return damageSource != null ? damageSource.GetDamageMod(this) : new Range(1, 5);
        }

        public float CalculateDamage(WorldObject target, WorldObject damageSource, ref bool criticalHit)
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
            DamageType damageType;
            if (damageSource?.ItemType == ItemType.MissileWeapon)
            {
                damageType = (DamageType)damageSource.GetProperty(PropertyInt.DamageType);
            }
            else
                damageType = GetDamageType();
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

        public double GetLifeResistance(DamageType damageType)
        {
            double resistance = 1.0;

            switch (damageType)
            {
                case DamageType.Slash:
                    resistance = ResistAcidMod;
                    break;

                case DamageType.Pierce:
                    resistance = ResistPierceMod;
                    break;

                case DamageType.Bludgeon:
                    resistance = ResistBludgeonMod;
                    break;

                case DamageType.Fire:
                    resistance = ResistFireMod;
                    break;

                case DamageType.Cold:
                    resistance = ResistColdMod;
                    break;

                case DamageType.Acid:
                    resistance = ResistAcidMod;
                    break;

                case DamageType.Electric:
                    resistance = ResistElectricMod;
                    break;

                case DamageType.Nether:
                    resistance = ResistNetherMod;
                    break;
            }

            return resistance;
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

        public Sound GetHitSound(WorldObject source, BodyPart bodyPart)
        {
            var creature = source as Creature;
            var armors = creature.GetArmor(bodyPart);

            foreach (var armor in armors)
            {
                var material = armor.GetProperty(PropertyInt.MaterialType) ?? 0;
                //Console.WriteLine("Name: " + armor.Name + " | Material: " + material);
            }
            return Sound.HitFlesh1;
        }

        public void TakeDamage(WorldObject source, DamageType damageType, float _amount, BodyPart bodyPart, bool crit = false)
        {
            if (Invincible.HasValue && Invincible.Value) return;
            var amount = (uint)Math.Round(_amount);
            var percent = (float)amount / Health.MaxValue;

            // update health
            Health.Current = (uint)Math.Max(0, (int)Health.Current - amount);
            if (Health.Current == 0)
            {
                HandleActionDie();
                return;
            }

            // send network messages
            var msgHealth = new GameMessagePrivateUpdateAttribute2ndLevel(this, Vital.Health, Health.Current);

            var hitSound = new GameMessageSound(Guid, GetHitSound(source, bodyPart), 1.0f);

            var creature = source as Creature;
            var splatter = new GameMessageScript(Guid, (PlayScript)Enum.Parse(typeof(PlayScript), "Splatter" + GetSplatterHeight() + creature.GetSplatterDir(this)));

            var damageLocation = (DamageLocation)BodyParts.Indices[bodyPart];
            var text = new GameEventDefenderNotification(Session, creature.Name, damageType, percent, amount, damageLocation, crit, AttackConditions.None);

            Session.Network.EnqueueSend(text, msgHealth, hitSound, splatter);

            if (percent >= 0.1f)
            {
                var wound = new GameMessageSound(Guid, Sound.Wound1, 1.0f);
                Session.Network.EnqueueSend(wound);
            }
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

        public string GetArmorType(BodyPart bodyPart)
        {
            // Flesh, Leather, Chain, Plate
            // for hit sounds
            return null;
        }
    }
}
