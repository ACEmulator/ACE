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

    /// <summary>
    /// Handles combat with a Player as the attacker
    /// generalized methods for melee / missile
    /// </summary>
    partial class Player
    {
        public Skill GetCurrentWeaponSkill()
        {
            var weapon = GetEquippedWeapon();

            // missile weapon
            if (weapon != null && weapon.CurrentWieldedLocation == EquipMask.MissileWeapon)
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
            var weapon = GetEquippedWeapon();

            if (weapon == null || weapon.CurrentWieldedLocation != EquipMask.MissileWeapon)
                return AttackType.Melee;
            else
                return AttackType.Missile;
        }

        public float DamageTarget(Creature target, WorldObject damageSource)
        {
            if (target.Health.Current <= 0)
                return 0.0f;

            var critical = false;
            var damage = CalculateDamage(target, damageSource, ref critical);
            if (damage > 0.0f)
                target.TakeDamage(this, damage, critical);
            else
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{target.Name} evaded your attack.", ChatMessageType.CombatSelf));

            if (damage > 0.0f && target.Health.Current > 0)
            {
                // notify attacker
                var intDamage = (uint)Math.Round(damage);
                if (damageSource?.ItemType == ItemType.MissileWeapon)
                {
                    var damageType = (DamageType)damageSource.GetProperty(PropertyInt.DamageType);
                    Session.Network.EnqueueSend(new GameEventAttackerNotification(Session, target.Name, damageType, (float)intDamage / target.Health.MaxValue, intDamage, critical, new AttackConditions()));
                }
                else
                {
                    Session.Network.EnqueueSend(new GameEventAttackerNotification(Session, target.Name, GetDamageType(), (float)intDamage / target.Health.MaxValue, intDamage, critical, new AttackConditions()));
                }

                // splatter effects
                Session.Network.EnqueueSend(new GameMessageSound(target.Guid, Sound.HitFlesh1, 0.5f));
                if (damage >= target.Health.MaxValue * 0.25f)
                {
                    var painSound = (Sound)Enum.Parse(typeof(Sound), "Wound" + Physics.Common.Random.RollDice(1, 3), true);
                    Session.Network.EnqueueSend(new GameMessageSound(target.Guid, painSound, 1.0f));
                }
                var splatter = (PlayScript)Enum.Parse(typeof(PlayScript), "Splatter" + GetSplatterHeight() + GetSplatterDir(target));
                Session.Network.EnqueueSend(new GameMessageScript(target.Guid, splatter));

                Session.Network.EnqueueSend(new GameEventUpdateHealth(Session, target.Guid.Full, (float)target.Health.Current / target.Health.MaxValue));
            }

            OnAttackMonster(target);
            return damage;
        }

        public float GetEvadeChance(WorldObject target)
        {
            // get player attack skill
            var attackSkill = GetCreatureSkill(GetCurrentWeaponSkill()).Current;

            if (IsExhausted)
                attackSkill = GetExhaustedSkill(attackSkill);

            // get target defense skill
            var creature = target as Creature;
            var defenseSkill = GetAttackType() == AttackType.Melee ? Skill.MeleeDefense : Skill.MissileDefense;
            var difficulty = creature.GetCreatureSkill(defenseSkill).Current;

            if (creature.IsExhausted) difficulty = 0;

            //Console.WriteLine("Attack skill: " + attackSkill);
            //Console.WriteLine("Defense skill: " + difficulty);

            var evadeChance = 1.0f - SkillCheck.GetSkillChance((int)attackSkill, (int)difficulty);
            return (float)evadeChance;
        }

        /// <summary>
        /// Called when player successfully avoids an attack
        /// </summary>
        public void OnEvade(WorldObject attacker, AttackType attackType)
        {
            // http://asheron.wikia.com/wiki/Attributes

            // Endurance will also make it less likely that you use a point of stamina to successfully evade a missile or melee attack.
            // A player is required to have Melee Defense for melee attacks or Missile Defense for missile attacks trained or specialized
            // in order for this specific ability to work. This benefit is tied to Endurance only, and it caps out at around a 75% chance
            // to avoid losing a point of stamina per successful evasion.

            if (CombatMode != CombatMode.NonCombat)
            {
                var defenseSkillType = attackType == AttackType.Missile ? Skill.MissileDefense : Skill.MeleeDefense;
                var defenseSkill = GetCreatureSkill(defenseSkillType);
                if (defenseSkill.AdvancementClass >= SkillAdvancementClass.Trained)
                {
                    var enduranceBase = Endurance.Base;
                    // TODO: find exact formula / where it caps out at 75%
                    var enduranceCap = 400;
                    var effective = Math.Min(enduranceBase, enduranceCap);
                    var noStaminaUseChance = effective / enduranceCap * 0.75f;
                    if (noStaminaUseChance < Physics.Common.Random.RollDice(0.0f, 1.0f))
                        UpdateVitalDelta(Stamina, -1);
                }
                else
                    UpdateVitalDelta(Stamina, -1);
            }
            else
                UpdateVitalDelta(Stamina, -1);

            Session.Network.EnqueueSend(new GameMessageSystemChat($"You evaded {attacker.Name}!", ChatMessageType.CombatEnemy));
        }

        public override Range GetBaseDamage()
        {
            var attackType = GetAttackType();
            var damageSource = attackType == AttackType.Melee ? GetEquippedWeapon() : GetEquippedAmmo();

            return damageSource != null ? damageSource.GetDamageMod(this) : new Range(1, 5);
        }

        public float CalculateDamage(WorldObject target, WorldObject damageSource, ref bool criticalHit)
        {
            var creature = target as Creature;

            // evasion chance
            var evadeChance = GetEvadeChance(target);
            if (Physics.Common.Random.RollDice(0.0f, 1.0f) < evadeChance)
                return 0.0f;

            // get weapon base damage
            var baseDamageRange = GetBaseDamage();
            var baseDamage = Physics.Common.Random.RollDice(baseDamageRange.Min, baseDamageRange.Max);

            // get damage mods
            var attackType = GetAttackType();
            var attributeMod = GetAttributeMod(attackType);
            var powerAccuracyMod = GetPowerAccuracyMod();

            var damage = baseDamage * attributeMod * powerAccuracyMod;

            // critical hit
            var critical = 0.1f;
            if (Physics.Common.Random.RollDice(0.0f, 1.0f) < critical)
            {
                damage = baseDamageRange.Max * attributeMod * powerAccuracyMod * 2.0f;
                criticalHit = true;
            }

            // get random body part @ attack height
            var bodyPart = BodyParts.GetBodyPart(AttackHeight.Value);

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

            // scale damage for armor and shield
            var armorMod = SkillFormula.CalcArmorMod(resistance);
            var shieldMod = creature.GetShieldMod(this, damageType);

            return damage * armorMod * shieldMod;
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
            /*var creature = source as Creature;
            var armors = creature.GetArmor(bodyPart);

            foreach (var armor in armors)
            {
                var material = armor.GetProperty(PropertyInt.MaterialType) ?? 0;
                //Console.WriteLine("Name: " + armor.Name + " | Material: " + material);
            }*/
            return Sound.HitFlesh1;
        }

        public void TakeDamage(WorldObject source, DamageType damageType, float _amount, BodyPart bodyPart, bool crit = false)
        {
            if (Invincible ?? false) return;

            var amount = (uint)Math.Round(_amount);
            var percent = (float)amount / Health.MaxValue;

            // update health
            var damageTaken = (uint)-UpdateVitalDelta(Health, (int)-amount);
            DamageHistory.Add(source, damageTaken);

            if (Health.Current == 0)
            {
                Die();
                return;
            }

            // update stamina
            UpdateVitalDelta(Stamina, -1);

            var damageLocation = (DamageLocation)BodyParts.Indices[bodyPart];

            // send network messages
            var creature = source as Creature;
            var hotspot = source as Hotspot;
            if (creature != null)
            {
                var hitSound = new GameMessageSound(Guid, GetHitSound(source, bodyPart), 1.0f);
                var splatter = new GameMessageScript(Guid, (PlayScript)Enum.Parse(typeof(PlayScript), "Splatter" + creature.GetSplatterHeight() + creature.GetSplatterDir(this)));
                var text = new GameEventDefenderNotification(Session, creature.Name, damageType, percent, amount, damageLocation, crit, AttackConditions.None);
                Session.Network.EnqueueSend(text, hitSound, splatter);
            }
            else if (hotspot != null)
            {
                if (!string.IsNullOrWhiteSpace(hotspot.ActivationTalkString))
                    Session.Network.EnqueueSend(new GameMessageSystemChat(hotspot.ActivationTalkString.Replace("%i", amount.ToString()), ChatMessageType.Craft));
                if (!(hotspot.Visibility ?? false))
                    CurrentLandblock?.EnqueueBroadcastSound(hotspot, Sound.TriggerActivated);
            }

            if (percent >= 0.1f)
            {
                var wound = new GameMessageSound(Guid, Sound.Wound1, 1.0f);
                Session.Network.EnqueueSend(wound);
            }
        }

        public string GetArmorType(BodyPart bodyPart)
        {
            // Flesh, Leather, Chain, Plate
            // for hit sounds
            return null;
        }

        /// <summary>
        /// Returns the total burden of items held in both hands
        /// (main hand and offhand)
        /// </summary>
        public int GetHandItemBurden()
        {
            // get main hand item
            var weapon = GetEquippedWeapon();

            // get off-hand item
            var shield = GetEquippedShield();

            var weaponBurden = weapon != null ? (weapon.EncumbranceVal ?? 0) : 0;
            var shieldBurden = shield != null ? (shield.EncumbranceVal ?? 0) : 0;

            return weaponBurden + shieldBurden;
        }

        public float GetStaminaMod()
        {
            var endurance = GetCreatureAttribute(PropertyAttribute.Endurance).Base;

            var staminaMod = 1.0f - (endurance - 100.0f) / 600.0f;   // guesstimated formula: 50% reduction at 400 base endurance
            staminaMod = Math.Clamp(staminaMod, 0.5f, 1.0f);

            return staminaMod;
        }

        /// <summary>
        /// Calculates the amount of stamina required to perform this attack
        /// </summary>
        public int GetAttackStamina(PowerAccuracy powerAccuracy)
        {
            // Stamina cost for melee and missile attacks is based on the total burden of what you are holding
            // in your hands (main hand and offhand), and your power/accuracy bar.

            // Attacking(Low power / accuracy bar)   1 point per 700 burden units
            //                                       1 point per 1200 burden units
            //                                       1.5 points per 1600 burden units
            // Attacking(Mid power / accuracy bar)   1 point per 700 burden units
            //                                       2 points per 1200 burden units
            //                                       3 points per 1600 burden units
            // Attacking(High power / accuracy bar)  2 point per 700 burden units
            //                                       4 points per 1200 burden units
            //                                       6 points per 1600 burden units

            // The higher a player's base Endurance, the less stamina one uses while attacking. This benefit is tied to Endurance only,
            // and caps out at 50% less stamina used per attack. Scaling is similar to other Endurance bonuses. Applies only to players.

            // When stamina drops to 0, your melee and missile defenses also drop to 0 and you will be incapable of attacking.
            // In addition, you will suffer a 50% penalty to your weapon skill. This applies to players and creatures.

            var burden = GetHandItemBurden();

            var baseCost = StaminaTable.GetStaminaCost(powerAccuracy, burden);

            var staminaMod = GetStaminaMod();

            var staminaCost = Math.Max(baseCost * staminaMod, 1);

            //Console.WriteLine($"GetAttackStamina({powerAccuracy}) - burden: {burden}, baseCost: {baseCost}, staminaMod: {staminaMod}, staminaCost: {staminaCost}");

            return (int)Math.Round(staminaCost);
        }
    }
}
