using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Common;
using ACE.DatLoader.Entity;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Managers;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.Enum;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    public enum CombatType
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
        public int AttackSequence;
        public bool Attacking;
        public bool AttackCancelled;

        public DateTime NextRefillTime;

        public double LastPkAttackTimestamp
        {
            get => GetProperty(PropertyFloat.LastPkAttackTimestamp) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyFloat.LastPkAttackTimestamp); else SetProperty(PropertyFloat.LastPkAttackTimestamp, value); }
        }

        public double PkTimestamp
        {
            get => GetProperty(PropertyFloat.PkTimestamp) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyFloat.PkTimestamp); else SetProperty(PropertyFloat.PkTimestamp, value); }
        }

        /// <summary>
        /// Returns the current attack skill for the player
        /// </summary>
        public override Skill GetCurrentAttackSkill()
        {
            if (CombatMode == CombatMode.Magic)
                return GetCurrentMagicSkill();
            else
                return GetCurrentWeaponSkill();
        }

        /// <summary>
        /// Returns the current weapon skill for the player
        /// </summary>
        public override Skill GetCurrentWeaponSkill()
        {
            var weapon = GetEquippedWeapon();

            if (weapon?.WeaponSkill == null)
                return GetHighestMeleeSkill();

            var skill = ConvertToMoASkill(weapon.WeaponSkill);

            // DualWieldAlternate will be TRUE if *next* attack is offhand
            if (IsDualWieldAttack && !DualWieldAlternate)
            {
                var weaponSkill = GetCreatureSkill(skill);
                var dualWield = GetCreatureSkill(Skill.DualWield);

                // offhand attacks use the lower skill level between dual wield and weapon skill
                if (dualWield.Current < weaponSkill.Current)
                    skill = Skill.DualWield;
            }
            //Console.WriteLine($"{Name}.GetCurrentWeaponSkill - {skill}");
            return skill;
        }

        /// <summary>
        /// Returns the highest melee skill for the player
        /// (light / heavy / finesse)
        /// </summary>
        public Skill GetHighestMeleeSkill()
        {
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

        public override CombatType GetCombatType()
        {
            // this is an unsafe function, move away from this
            var weapon = GetEquippedWeapon();

            if (weapon == null || weapon.CurrentWieldedLocation != EquipMask.MissileWeapon)
                return CombatType.Melee;
            else
                return CombatType.Missile;
        }

        public DamageEvent DamageTarget(Creature target, WorldObject damageSource)
        {
            if (target.Health.Current <= 0)
                return null;

            var targetPlayer = target as Player;

            // check PK status
            var pkError = CheckPKStatusVsTarget(target, null);
            if (pkError != null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieErrorWithString(Session, pkError[0], target.Name));
                if (targetPlayer != null)
                    targetPlayer.Session.Network.EnqueueSend(new GameEventWeenieErrorWithString(targetPlayer.Session, pkError[1], Name));
                return null;
            }

            var damageEvent = DamageEvent.CalculateDamage(this, target, damageSource);

            if (damageEvent.HasDamage)
            {
                OnDamageTarget(target, damageEvent.CombatType, damageEvent.IsCritical);

                if (targetPlayer != null)
                    targetPlayer.TakeDamage(this, damageEvent);
                else
                    target.TakeDamage(this, damageEvent.DamageType, damageEvent.Damage, damageEvent.IsCritical);
            }
            else
            {
                if (damageEvent.LifestoneProtection)
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"The Lifestone's magic protects {target.Name} from the attack!", ChatMessageType.Magic));

                else if (!SquelchManager.Squelches.Contains(target, ChatMessageType.CombatSelf))
                    Session.Network.EnqueueSend(new GameEventEvasionAttackerNotification(Session, target.Name));

                if (targetPlayer != null)
                    targetPlayer.OnEvade(this, damageEvent.CombatType);
            }

            if (damageEvent.HasDamage && target.IsAlive)
            {
                // notify attacker
                var intDamage = (uint)Math.Round(damageEvent.Damage);

                if (!SquelchManager.Squelches.Contains(this, ChatMessageType.CombatSelf))
                    Session.Network.EnqueueSend(new GameEventAttackerNotification(Session, target.Name, damageEvent.DamageType, (float)intDamage / target.Health.MaxValue, intDamage, damageEvent.IsCritical, damageEvent.AttackConditions));

                // splatter effects
                if (targetPlayer == null)
                {
                    Session.Network.EnqueueSend(new GameMessageSound(target.Guid, Sound.HitFlesh1, 0.5f));
                    if (damageEvent.Damage >= target.Health.MaxValue * 0.25f)
                    {
                        var painSound = (Sound)Enum.Parse(typeof(Sound), "Wound" + ThreadSafeRandom.Next(1, 3), true);
                        Session.Network.EnqueueSend(new GameMessageSound(target.Guid, painSound, 1.0f));
                    }
                    var splatter = (PlayScript)Enum.Parse(typeof(PlayScript), "Splatter" + GetSplatterHeight() + GetSplatterDir(target));
                    Session.Network.EnqueueSend(new GameMessageScript(target.Guid, splatter));
                }

                // handle Dirty Fighting
                if (GetCreatureSkill(Skill.DirtyFighting).AdvancementClass >= SkillAdvancementClass.Trained)
                    FightDirty(target, damageEvent.Weapon);
                
                target.EmoteManager.OnDamage(this);

                if (damageEvent.IsCritical)
                    target.EmoteManager.OnReceiveCritical(this);
            }
            
            if (targetPlayer == null)
                OnAttackMonster(target);

            return damageEvent;
        }

        /// <summary>
        /// Sets the creature that last attacked a player
        /// This is called when the player takes damage, evades, or resists a spell from a creature
        /// If the CurrentAttacker has changed, sends a network message to the player's client
        /// This enables the 'last attacker' functionality in the client, which is bound to the 'home' key by default
        /// </summary>
        public void SetCurrentAttacker(Creature currentAttacker)
        {
            if (currentAttacker == this || CurrentAttacker == currentAttacker.Guid.Full)
                return;

            CurrentAttacker = currentAttacker.Guid.Full;

            Session.Network.EnqueueSend(new GameMessagePrivateUpdateInstanceID(this, PropertyInstanceId.CurrentAttacker, currentAttacker.Guid.Full));
        }

        /// <summary>
        /// Called when a player hits a target
        /// </summary>
        public override void OnDamageTarget(WorldObject target, CombatType attackType, bool critical)
        {
            var attackSkill = GetCreatureSkill(GetCurrentWeaponSkill());
            var difficulty = GetTargetEffectiveDefenseSkill(target);

            Proficiency.OnSuccessUse(this, attackSkill, difficulty);
        }

        public override uint GetEffectiveAttackSkill()
        {
            var weapon = GetEquippedWeapon();
            var attackSkill = GetCreatureSkill(GetCurrentWeaponSkill()).Current;
            var offenseMod = GetWeaponOffenseModifier(this);
            var accuracyMod = GetAccuracyMod(weapon);

            attackSkill = (uint)Math.Round(attackSkill * accuracyMod * offenseMod);

            //if (IsExhausted)
                //attackSkill = GetExhaustedSkill(attackSkill);

            //var baseStr = offenseMod != 1.0f ? $" (base: {GetCreatureSkill(GetCurrentWeaponSkill()).Current})" : "";
            //Console.WriteLine("Attack skill: " + attackSkill + baseStr);

            return attackSkill;
        }

        public uint GetTargetEffectiveDefenseSkill(WorldObject target)
        {
            var creature = target as Creature;
            if (creature == null) return 0;

            var attackType = GetCombatType();
            var defenseSkill = attackType == CombatType.Missile ? Skill.MissileDefense : Skill.MeleeDefense;
            var defenseMod = defenseSkill == Skill.MeleeDefense ? GetWeaponMeleeDefenseModifier(creature) : 1.0f;
            var effectiveDefense = (uint)Math.Round(creature.GetCreatureSkill(defenseSkill).Current * defenseMod);

            if (creature.IsExhausted) effectiveDefense = 0;

            //var baseStr = defenseMod != 1.0f ? $" (base: {creature.GetCreatureSkill(defenseSkill).Current})" : "";
            //Console.WriteLine("Defense skill: " + effectiveDefense + baseStr);

            return effectiveDefense;
        }

        /// <summary>
        /// Returns a modifier to the player's defense skill, based on current motion state
        /// </summary>
        /// <returns></returns>
        public float GetDefenseStanceMod()
        {
            if (IsJumping)
                return 0.5f;

            if (IsLoggingOut)
                return 0.8f;

            if (CombatMode != CombatMode.NonCombat)
                return 1.0f;

            var forwardCommand = CurrentMovementData.MovementType == MovementType.Invalid && CurrentMovementData.Invalid != null ?
                CurrentMovementData.Invalid.State.ForwardCommand : MotionCommand.Invalid;

            switch (forwardCommand)
            {
                // TODO: verify multipliers
                case MotionCommand.Crouch:
                    return 0.4f;
                case MotionCommand.Sitting:
                    return 0.3f;
                case MotionCommand.Sleeping:
                    return 0.2f;
                default:
                    return 1.0f;
            }
        }

        /// <summary>
        /// Called when player successfully avoids an attack
        /// </summary>
        public override void OnEvade(WorldObject attacker, CombatType attackType)
        {
            var creatureAttacker = attacker as Creature;

            if (creatureAttacker != null)
                SetCurrentAttacker(creatureAttacker);

            if (UnderLifestoneProtection)
                return;

            // http://asheron.wikia.com/wiki/Attributes

            // Endurance will also make it less likely that you use a point of stamina to successfully evade a missile or melee attack.
            // A player is required to have Melee Defense for melee attacks or Missile Defense for missile attacks trained or specialized
            // in order for this specific ability to work. This benefit is tied to Endurance only, and it caps out at around a 75% chance
            // to avoid losing a point of stamina per successful evasion.

            var defenseSkillType = attackType == CombatType.Missile ? Skill.MissileDefense : Skill.MeleeDefense;
            var defenseSkill = GetCreatureSkill(defenseSkillType);

            if (CombatMode != CombatMode.NonCombat)
            {
                if (defenseSkill.AdvancementClass >= SkillAdvancementClass.Trained)
                {
                    var enduranceBase = (int)Endurance.Base;

                    // TODO: find exact formula / where it caps out at 75%

                    // more literal / linear formula
                    //var noStaminaUseChance = (enduranceBase - 50) / 320.0f;

                    // gdle curve-based formula, caps at 300 instead of 290
                    var noStaminaUseChance = (enduranceBase * enduranceBase * 0.000005f) + (enduranceBase * 0.00124f) - 0.07f;

                    noStaminaUseChance = Math.Clamp(noStaminaUseChance, 0.0f, 0.75f);

                    //Console.WriteLine($"NoStaminaUseChance: {noStaminaUseChance}");

                    if (noStaminaUseChance <= ThreadSafeRandom.Next(0.0f, 1.0f))
                        UpdateVitalDelta(Stamina, -1);
                }
                else
                    UpdateVitalDelta(Stamina, -1);
            }
            else
            {
                // if the player is in non-combat mode, no stamina is consumed on evade
                // reference: https://youtu.be/uFoQVgmSggo?t=145
                // from the dm guide, page 147: "if you are not in Combat mode, you lose no Stamina when an attack is thrown at you"

                //UpdateVitalDelta(Stamina, -1);
            }

            if (!SquelchManager.Squelches.Contains(attacker, ChatMessageType.CombatEnemy))
                Session.Network.EnqueueSend(new GameEventEvasionDefenderNotification(Session, attacker.Name));

            if (creatureAttacker == null)
                return;

            var difficulty = creatureAttacker.GetCreatureSkill(creatureAttacker.GetCurrentWeaponSkill()).Current;
            // attackMod?
            Proficiency.OnSuccessUse(this, defenseSkill, difficulty);
        }

        public BaseDamageMod GetBaseDamageMod(WorldObject damageSource)
        {
            if (damageSource == this)
            {
                if (AttackType == AttackType.Punch)
                    damageSource = HandArmor;
                else if (AttackType == AttackType.Kick)
                    damageSource = FootArmor;

                // no weapon, no hand or foot armor
                if (damageSource == null)
                {
                    var baseDamage = new BaseDamage(5, 0.2f);   // 1-5
                    return new BaseDamageMod(baseDamage);
                }
                else
                    return damageSource.GetDamageMod(this, damageSource);
            }
            return damageSource.GetDamageMod(this);
        }

        public override float GetPowerMod(WorldObject weapon)
        {
            if (weapon == null || !weapon.IsRanged)
                return PowerLevel + 0.5f;
            else
                return 1.0f;
        }

        public override float GetAccuracyMod(WorldObject weapon)
        {
            if (weapon != null && weapon.IsRanged)
                return AccuracyLevel + 0.6f;
            else
                return 1.0f;
        }

        public float GetPowerAccuracyBar()
        {
            return GetCombatType() == CombatType.Missile ? AccuracyLevel : PowerLevel;
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

        /// <summary>
        /// Simplified player take damage function, only called for DoTs currently
        /// </summary>
        public override void TakeDamageOverTime(float _amount, DamageType damageType)
        {
            if (Invincible || IsDead) return;

            // check lifestone protection
            if (UnderLifestoneProtection)
            {
                HandleLifestoneProtection();
                return;
            }

            var amount = (uint)Math.Round(_amount);
            var percent = (float)amount / Health.MaxValue;

            // update health
            var damageTaken = (uint)-UpdateVitalDelta(Health, (int)-amount);

            // update stamina
            //UpdateVitalDelta(Stamina, -1);

            //if (Fellowship != null)
                //Fellowship.OnVitalUpdate(this);

            // send damage text message
            //if (PropertyManager.GetBool("show_dot_messages").Item)
            //{
                var nether = damageType == DamageType.Nether ? "nether " : "";
                var chatMessageType = damageType == DamageType.Nether ? ChatMessageType.Magic : ChatMessageType.Combat;
                var text = $"You receive {amount} points of periodic {nether}damage.";
                SendMessage(text, chatMessageType);
            //}

            // splatter effects
            //var splatter = new GameMessageScript(Guid, (PlayScript)Enum.Parse(typeof(PlayScript), "Splatter" + creature.GetSplatterHeight() + creature.GetSplatterDir(this)));  // not sent in retail, but great visual indicator?
            var splatter = new GameMessageScript(Guid, damageType == DamageType.Nether ? PlayScript.HealthDownVoid : PlayScript.DirtyFightingDamageOverTime);
            EnqueueBroadcast(splatter);

            if (Health.Current <= 0)
            {
                // since damage over time is possibly combined from multiple sources,
                // sending a message to the last damager here could be tricky..

                // TODO: get last damager from dot stack instead? 
                OnDeath(DamageHistory.LastDamager, damageType, false);
                Die();

                return;
            }

            if (percent >= 0.1f)
                EnqueueBroadcast(new GameMessageSound(Guid, Sound.Wound1, 1.0f));
        }

        public int TakeDamage(WorldObject source, DamageEvent damageEvent)
        {
            return TakeDamage(source, damageEvent.DamageType, damageEvent.Damage, damageEvent.BodyPart, damageEvent.IsCritical, damageEvent.AttackConditions);
        }

        /// <summary>
        /// Applies damages to a player from a physical damage source
        /// </summary>
        public int TakeDamage(WorldObject source, DamageType damageType, float _amount, BodyPart bodyPart, bool crit = false, AttackConditions attackConditions = AttackConditions.None)
        {
            if (Invincible || IsDead) return 0;

            if (source is Creature creatureAttacker)
                SetCurrentAttacker(creatureAttacker);

            // check lifestone protection
            if (UnderLifestoneProtection)
            {
                HandleLifestoneProtection();
                return 0;
            }

            if (_amount < 0)
            {
                log.Error($"{Name}.TakeDamage({source?.Name} ({source?.Guid}), {damageType}, {_amount}) - negative damage, this shouldn't happen");
                return 0;
            }

            var amount = (uint)Math.Round(_amount);
            var percent = (float)amount / Health.MaxValue;

            var equippedCloak = EquippedCloak;

            if (equippedCloak != null && Cloak.HasDamageProc(equippedCloak) && Cloak.RollProc(equippedCloak, percent))
            {
                var reducedAmount = Cloak.GetReducedAmount(source, amount);

                Cloak.ShowMessage(this, source, amount, reducedAmount);

                amount = reducedAmount;
                percent = (float)amount / Health.MaxValue;
            }

            // update health
            var damageTaken = (uint)-UpdateVitalDelta(Health, (int)-amount);
            DamageHistory.Add(source, damageType, damageTaken);

            // update stamina
            if (CombatMode != CombatMode.NonCombat)
            {
                // if the player is in non-combat mode, no stamina is consumed on evade
                // reference: https://youtu.be/uFoQVgmSggo?t=145
                // from the dm guide, page 147: "if you are not in Combat mode, you lose no Stamina when an attack is thrown at you"

                UpdateVitalDelta(Stamina, -1);
            }

            //if (Fellowship != null)
                //Fellowship.OnVitalUpdate(this);

            if (Health.Current <= 0)
            {
                OnDeath(new DamageHistoryInfo(source), damageType, crit);
                Die();
                return (int)damageTaken;
            }

            if (!BodyParts.Indices.TryGetValue(bodyPart, out var iDamageLocation))
            {
                log.Error($"{Name}.TakeDamage({source.Name}, {damageType}, {amount}, {bodyPart}, {crit}): avoided crash for bad damage location");
                return 0;
            }
            var damageLocation = (DamageLocation)iDamageLocation;

            // send network messages
            if (source is Creature creature)
            {
                if (!SquelchManager.Squelches.Contains(source, ChatMessageType.CombatEnemy))
                    Session.Network.EnqueueSend(new GameEventDefenderNotification(Session, creature.Name, damageType, percent, amount, damageLocation, crit, attackConditions));

                var hitSound = new GameMessageSound(Guid, GetHitSound(source, bodyPart), 1.0f);
                var splatter = new GameMessageScript(Guid, (PlayScript)Enum.Parse(typeof(PlayScript), "Splatter" + creature.GetSplatterHeight() + creature.GetSplatterDir(this)));
                EnqueueBroadcast(hitSound, splatter);
            }

            if (percent >= 0.1f)
            {
                // Wound1 - Aahhh!    - elemental attacks above some threshold
                // Wound2 - Deep Ugh! - bludgeoning attacks above some threshold
                // Wound3 - Ooh!      - slashing / piercing / undef attacks above some threshold

                var woundSound = Sound.Wound3;

                if (damageType == DamageType.Bludgeon)
                    woundSound = Sound.Wound2;

                else if ((damageType & DamageType.Elemental) != 0)
                    woundSound = Sound.Wound1;

                EnqueueBroadcast(new GameMessageSound(Guid, woundSound, 1.0f));
            }

            if (equippedCloak != null && Cloak.HasProcSpell(equippedCloak))
                Cloak.TryProcSpell(this, source, equippedCloak, percent);

            // if player attacker, update PK timer
            if (source is Player attacker)
                UpdatePKTimers(attacker, this);

            return (int)damageTaken;
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
        public int GetHeldItemBurden()
        {
            var mainhand = GetEquippedMainHand();
            var offhand = GetEquippedOffHand();

            var mainhandBurden = mainhand?.EncumbranceVal ?? 0;
            var offhandBurden = offhand?.EncumbranceVal ?? 0;

            return mainhandBurden + offhandBurden;
        }

        public float GetStaminaMod()
        {
            var endurance = (int)Endurance.Base;

            // more literal / linear formula
            var staminaMod = 1.0f - (endurance - 50) / 480.0f;

            // gdle curve-based formula, caps at 300 instead of 290
            //var staminaMod = (endurance * endurance * -0.000003175f) - (endurance * 0.0008889f) + 1.052f;

            staminaMod = Math.Clamp(staminaMod, 0.5f, 1.0f);

            // this is also specific to gdle,
            // additive luck which can send the base stamina way over 1.0
            /*var luck = ThreadSafeRandom.Next(0.0f, 1.0f);
            staminaMod += luck;*/

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

            var burden = GetHeldItemBurden();

            var baseCost = StaminaTable.GetStaminaCost(powerAccuracy, burden);

            var staminaMod = GetStaminaMod();

            var staminaCost = Math.Max(baseCost * staminaMod, 1);

            //Console.WriteLine($"GetAttackStamina({powerAccuracy}) - burden: {burden}, baseCost: {baseCost}, staminaMod: {staminaMod}, staminaCost: {staminaCost}");

            return (int)Math.Round(staminaCost);
        }

        /// <summary>
        /// Returns the damage rating modifier for an applicable Recklessness attack
        /// </summary>
        /// <param name="powerAccuracyBar">The 0.0 - 1.0 power/accurary bar</param>
        public float GetRecklessnessMod(/*float powerAccuracyBar*/)
        {
            // ensure melee or missile combat mode
            if (CombatMode != CombatMode.Melee && CombatMode != CombatMode.Missile)
                return 1.0f;

            var skill = GetCreatureSkill(Skill.Recklessness);

            // recklessness skill must be either trained or specialized to use
            if (skill.AdvancementClass < SkillAdvancementClass.Trained)
                return 1.0f;

            // recklessness is active when attack bar is between 20% and 80% (according to wiki)
            // client attack bar range seems to indicate this might have been updated, between 10% and 90%?
            var powerAccuracyBar = GetPowerAccuracyBar();
            //if (powerAccuracyBar < 0.2f || powerAccuracyBar > 0.8f)
            if (powerAccuracyBar < 0.1f || powerAccuracyBar > 0.9f)
                return 1.0f;

            // recklessness only applies to non-critical hits,
            // which is handled outside of this method.

            // damage rating is increased by 20 for specialized, and 10 for trained.
            // incoming non-critical damage from all sources is increased by the same.
            var damageRating = skill.AdvancementClass == SkillAdvancementClass.Specialized ? 20 : 10;

            // if recklessness skill is lower than current attack skill (as determined by your equipped weapon)
            // then the damage rating is reduced proportionately. The damage rating caps at 10 for trained
            // and 20 for specialized, so there is no reason to raise the skill above your attack skill.
            var attackSkill = GetCreatureSkill(GetCurrentAttackSkill());

            if (skill.Current < attackSkill.Current)
            {
                var scale = (float)skill.Current / attackSkill.Current;
                damageRating = (int)Math.Round(damageRating * scale);
            }

            // The damage rating adjustment for incoming damage is also adjusted proportinally if your Recklessness skill
            // is lower than your active attack skill

            var recklessnessMod = GetDamageRating(damageRating);    // trained DR 1.10 = 10% additional damage
                                                                    // specialized DR 1.20 = 20% additional damage
            return recklessnessMod;
        }

        /// <summary>
        /// Returns TRUE if this player is PK and died to another player
        /// </summary>
        public bool IsPKDeath(DamageHistoryInfo topDamager)
        {
            return IsPKDeath(topDamager?.Guid.Full);
        }

        public bool IsPKDeath(uint? killerGuid)
        {
            return PlayerKillerStatus.HasFlag(PlayerKillerStatus.PK) && new ObjectGuid(killerGuid ?? 0).IsPlayer() && killerGuid != Guid.Full;
        }

        /// <summary>
        /// Returns TRUE if this player is PKLite and died to another player
        /// </summary>
        public bool IsPKLiteDeath(DamageHistoryInfo topDamager)
        {
            return IsPKLiteDeath(topDamager?.Guid.Full);
        }

        public bool IsPKLiteDeath(uint? killerGuid)
        {
            return PlayerKillerStatus.HasFlag(PlayerKillerStatus.PKLite) && new ObjectGuid(killerGuid ?? 0).IsPlayer() && killerGuid != Guid.Full;
        }

        public CombatMode LastCombatMode;

        public static readonly float UseTimeEpsilon = 0.05f;

        /// <summary>
        /// This method processes the Game Action (F7B1) Change Combat Mode (0x0053)
        /// </summary>
        public void HandleActionChangeCombatMode(CombatMode newCombatMode, bool forceHandCombat = false, Action callback = null)
        {
            //log.Info($"{Name}.HandleActionChangeCombatMode({newCombatMode})");

            LastCombatMode = newCombatMode;
            
            if (DateTime.UtcNow >= NextUseTime.AddSeconds(UseTimeEpsilon))
                HandleActionChangeCombatMode_Inner(newCombatMode, forceHandCombat, callback);
            else
            {
                var actionChain = new ActionChain();
                actionChain.AddDelaySeconds((NextUseTime - DateTime.UtcNow).TotalSeconds + UseTimeEpsilon);
                actionChain.AddAction(this, () => HandleActionChangeCombatMode_Inner(newCombatMode, forceHandCombat, callback));
                actionChain.EnqueueChain();
            }

            if (IsAfk)
                HandleActionSetAFKMode(false);
        }

        public void HandleActionChangeCombatMode_Inner(CombatMode newCombatMode, bool forceHandCombat = false, Action callback = null)
        {
            //log.Info($"{Name}.HandleActionChangeCombatMode_Inner({newCombatMode})");

            var currentCombatStance = GetCombatStance();

            var missileWeapon = GetEquippedMissileWeapon();
            var caster = GetEquippedWand();

            if (CombatMode == CombatMode.Magic && MagicState.IsCasting)
                FailCast();

            HandleActionCancelAttack();

            float animTime = 0.0f, queueTime = 0.0f;

            switch (newCombatMode)
            {
                case CombatMode.NonCombat:
                    {
                        switch (currentCombatStance)
                        {
                            case MotionStance.BowCombat:
                            case MotionStance.CrossbowCombat:
                            case MotionStance.AtlatlCombat:
                                {
                                    var equippedAmmo = GetEquippedAmmo();
                                    if (equippedAmmo != null)
                                        ClearChild(equippedAmmo); // We must clear the placement/parent when going back to peace
                                    break;
                                }
                        }
                        break;
                    }
                case CombatMode.Melee:

                    // todo expand checks
                    if (!forceHandCombat && (missileWeapon != null || caster != null))
                        return;

                    break;

                case CombatMode.Missile:
                    {
                        if (missileWeapon == null)
                            return;

                        switch (currentCombatStance)
                        {
                            case MotionStance.BowCombat:
                            case MotionStance.CrossbowCombat:
                            case MotionStance.AtlatlCombat:
                                {
                                    var equippedAmmo = GetEquippedAmmo();
                                    if (equippedAmmo == null)
                                    {
                                        animTime = SetCombatMode(newCombatMode, out queueTime);

                                        var actionChain = new ActionChain();
                                        actionChain.AddDelaySeconds(animTime);
                                        actionChain.AddAction(this, () =>
                                        {
                                            Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "You are out of ammunition!"));
                                            SetCombatMode(CombatMode.NonCombat);
                                        });
                                        actionChain.EnqueueChain();

                                        NextUseTime = DateTime.UtcNow.AddSeconds(animTime);
                                        return;
                                    }
                                    else
                                    {
                                        // We must set the placement/parent when going into combat
                                        equippedAmmo.Placement = ACE.Entity.Enum.Placement.RightHandCombat;
                                        equippedAmmo.ParentLocation = ACE.Entity.Enum.ParentLocation.RightHand;
                                    }
                                    break;
                                }
                        }
                        break;
                    }

                case CombatMode.Magic:

                    // todo expand checks
                    if (caster == null)
                        return;

                    break;

            }

            // animTime already includes queueTime
            animTime = SetCombatMode(newCombatMode, out queueTime, forceHandCombat);
            //log.Info($"{Name}.HandleActionChangeCombatMode_Inner({newCombatMode}) - animTime: {animTime}, queueTime: {queueTime}");

            NextUseTime = DateTime.UtcNow.AddSeconds(animTime);

            if (MagicState.IsCasting && RecordCast.Enabled)
                RecordCast.OnSetCombatMode(newCombatMode);

            if (callback != null)
            {
                var callbackChain = new ActionChain();
                callbackChain.AddDelaySeconds(animTime);
                callbackChain.AddAction(this, callback);
                callbackChain.EnqueueChain();
            }
        }

        public override bool CanDamage(Creature target)
        {
            return target.Attackable && !target.Teleporting && !(target is CombatPet);
        }

        // http://acpedia.org/wiki/Announcements_-_2002/04_-_Betrayal

        // Some combination of strength and endurance (the two are roughly of equivalent importance) now allows one to have a level of "natural resistances" to the 7 damage types,
        // and to partially resist drain health and harm attacks.

        // This caps out at a 50% resistance (the equivalent to level 5 life prots) to these damage types.

        // This resistance is not additive to life protections: higher level life protections will overwrite these natural resistances,
        // although life vulns will take these natural resistances into account, if the player does not have a higher level life protection cast upon them.

        // For example, a player will not get a free protective bonus from natural resistances if they have both Prot 7 and Vuln 7 cast upon them.
        // The Prot and Vuln will cancel each other out, and since the Prot has overwritten the natural resistances, there will be no resistance bonus.

        // The natural resistances, drain resistances, and regeneration rate info are now visible on the Character Information Panel, in what was once the Burden panel.

        // The 5 categories for the endurance benefits are, in order from lowest benefit to highest: Poor, Mediocre, Hardy, Resilient, and Indomitable,
        // with each range of benefits divided up equally amongst the 5 (e.g. Poor describes having anywhere from 1-10% resistance against drain health attacks, etc.).

        // A few other important notes:

        // - The abilities that Endurance or Endurance/Strength conveys are not increased by Strength or Endurance buffs.
        //   It is the raw Strength and/or Endurance scores that determine the various bonuses.
        // - For April, natural resistances will offer some protection versus hollow type damage, whether it is from a Hollow Minion or a Hollow weapon. This will be changed in May.
        // - These abilities are player-only, creatures with high endurance will not benefit from any of these changes.
        // - Come May, you can type @help endurance for a summary of the April changes to Endurance.

        public override float GetNaturalResistance(DamageType damageType)
        {
            if (damageType == DamageType.Undef)
                return 1.0f;

            // http://acpedia.org/wiki/Announcements_-_11th_Anniversary_Preview#Void_Magic_and_You.21
            // Creatures under Asheron’s protection take half damage from any nether type spell.
            if (damageType == DamageType.Nether)
                return 0.5f;

            // base strength and endurance give the player a natural resistance to damage,
            // which caps at 50% (equivalent to level 5 life prots)
            // these do not stack with life protection spells

            // - natural resistances are ignored by hollow damage

            var strAndEnd = Strength.Base + Endurance.Base;

            if (strAndEnd <= 200)
                return 1.0f;

            var naturalResistance = 1.0f - (float)(strAndEnd - 200) / 300 * 0.5f;
            naturalResistance = Math.Max(naturalResistance, 0.5f);

            return naturalResistance;
        }

        public string GetNaturalResistanceString(ResistanceType resistanceType)
        {
            var strAndEnd = Strength.Base + Endurance.Base;

            if (strAndEnd > 440)        return "Indomitable";
            else if (strAndEnd > 380)   return "Resilient";
            else if (strAndEnd > 320)   return "Hardy";
            else if (strAndEnd > 260)   return "Mediocre";
            else if (strAndEnd > 200)   return "Poor";
            else
                return "None";
        }

        public string GetRegenBonusString()
        {
            var strAndEnd = Strength.Base + 2 * Endurance.Base;

            if (strAndEnd > 690)        return "Indomitable";
            else if (strAndEnd > 580)   return "Resilient";
            else if (strAndEnd > 470)   return "Hardy";
            else if (strAndEnd > 346)   return "Mediocre";
            else if (strAndEnd > 200)   return "Poor";
            else
                return "None";
        }

        /// <summary>
        /// If a player has been involved in a PK battle this recently,
        /// logging off leaves their character in a frozen state for 20 seconds
        /// </summary>
        public static TimeSpan PKLogoffTimer = TimeSpan.FromMinutes(2);

        public void UpdatePKTimer()
        {
            //log.Info($"Updating PK timer for {Name}");

            LastPkAttackTimestamp = Time.GetUnixTime();
        }

        /// <summary>
        /// Called when a successful attack is landed in PVP
        /// The timestamp for both PKs are updated
        /// 
        /// If a physical attack is evaded, or a magic spell is resisted,
        /// this function should NOT be called.
        /// </summary>
        public static void UpdatePKTimers(Player attacker, Player defender)
        {
            if (attacker == defender) return;

            if (attacker.PlayerKillerStatus == PlayerKillerStatus.Free || defender.PlayerKillerStatus == PlayerKillerStatus.Free)
                return;

            attacker.UpdatePKTimer();
            defender.UpdatePKTimer();
        }

        public bool PKTimerActive => IsPKType && Time.GetUnixTime() - LastPkAttackTimestamp < PropertyManager.GetLong("pk_timer").Item;

        public bool PKLogoutActive => IsPKType && Time.GetUnixTime() - LastPkAttackTimestamp < PKLogoffTimer.TotalSeconds;

        public bool IsPKType => PlayerKillerStatus == PlayerKillerStatus.PK || PlayerKillerStatus == PlayerKillerStatus.PKLite;

        public bool IsPK => PlayerKillerStatus == PlayerKillerStatus.PK;

        public bool IsPKL => PlayerKillerStatus == PlayerKillerStatus.PKLite;

        public bool IsNPK => PlayerKillerStatus == PlayerKillerStatus.NPK;

        public bool CheckHouseRestrictions(Player player)
        {
            if (Location.Cell == player.Location.Cell)
                return true;

            // dealing with outdoor cell equivalents at this point, if applicable
            var cell = (CurrentLandblock?.IsDungeon ?? false) ? Location.Cell : Location.GetOutdoorCell();
            var playerCell = (player.CurrentLandblock?.IsDungeon ?? false) ? player.Location.Cell : player.Location.GetOutdoorCell();

            if (cell == playerCell)
                return true;

            HouseCell.HouseCells.TryGetValue(cell, out var houseGuid);
            HouseCell.HouseCells.TryGetValue(playerCell, out var playerHouseGuid);

            // pass if both of these players aren't in a house cell
            if (houseGuid == 0 && playerHouseGuid == 0)
                return true;

            var houses = new HashSet<House>();
            CheckHouseRestrictions_GetHouse(houseGuid, houses);
            player.CheckHouseRestrictions_GetHouse(playerHouseGuid, houses);

            foreach (var house in houses)
            {
                if (!house.HasPermission(this) || !house.HasPermission(player))
                    return false;
            }
            return true;
        }

        public void CheckHouseRestrictions_GetHouse(uint houseGuid, HashSet<House> houses)
        {
            if (houseGuid == 0)
                return;

            var house = CurrentLandblock.GetObject(houseGuid) as House;
            if (house != null)
            {
                var rootHouse = house.LinkedHouses.Count > 0 ? house.LinkedHouses[0] : house;

                if (rootHouse.HouseOwner == null || rootHouse.OpenStatus || houses.Contains(rootHouse))
                    return;

                //Console.WriteLine($"{Name}.CheckHouseRestrictions_GetHouse({houseGuid:X8}): found root house {house.Name} ({house.HouseId})");
                houses.Add(rootHouse);
            }
            else
                log.Error($"{Name}.CheckHouseRestrictions_GetHouse({houseGuid:X8}): couldn't find house from {CurrentLandblock.Id.Raw:X8}");
        }

        /// <summary>
        /// Returns the damage type for the currently equipped weapon / ammo
        /// </summary>
        /// <param name="multiple">If true, returns all of the damage types for the weapon</param>
        public override DamageType GetDamageType(bool multiple = false, CombatType? combatType = null)
        {
            // player override
            if (combatType == null)
                combatType = GetCombatType();

            var weapon = GetEquippedWeapon();
            var ammo = GetEquippedAmmo();

            if (weapon == null && combatType == CombatType.Melee)
            {
                // handle gauntlets/ boots
                if (AttackType == AttackType.Punch)
                    weapon = HandArmor;
                else if (AttackType == AttackType.Kick)
                    weapon = FootArmor;
                else
                {
                    log.Warn($"{Name}.GetDamageType(): no weapon, AttackType={AttackType}");
                    return DamageType.Undef;
                }

                if (weapon != null && weapon.W_DamageType == DamageType.Undef)
                    return DamageType.Bludgeon;
            }

            if (weapon == null)
                return DamageType.Bludgeon;

            var damageSource = combatType == CombatType.Melee || ammo == null || !weapon.IsAmmoLauncher ? weapon : ammo;

            var damageType = damageSource.W_DamageType;

            if (damageType == DamageType.Undef)
            {
                log.Warn($"{Name}.GetDamageType(): {damageSource} ({damageSource.Guid}, {damageSource.WeenieClassId}): no DamageType");
                return DamageType.Bludgeon;
            }

            // return multiple damage types
            if (multiple || !damageType.IsMultiDamage())
                return damageType;

            // get single damage type
            if (damageType == (DamageType.Pierce | DamageType.Slash))
            {
                if ((AttackType & AttackType.Punches) != 0)
                {
                    if (PowerLevel < ThrustThreshold)
                        return DamageType.Pierce;
                    else
                        return DamageType.Slash;
                }

                if ((AttackType & AttackType.Thrusts) != 0)
                    return DamageType.Pierce;
                else
                    return DamageType.Slash;
            }

            var powerLevel = combatType == CombatType.Melee ? (float?)PowerLevel : null;

            return damageType.SelectDamageType(powerLevel);
        }

        public WorldObject HandArmor => EquippedObjects.Values.FirstOrDefault(i => (i.ClothingPriority & CoverageMask.Hands) > 0);

        public WorldObject FootArmor => EquippedObjects.Values.FirstOrDefault(i => (i.ClothingPriority & CoverageMask.Feet) > 0);


        /// <summary>
        /// Determines if player can damage a target via PlayerKillerStatus
        /// </summary>
        /// <returns>null if no errors, else pk error list</returns>
        public override List<WeenieErrorWithString> CheckPKStatusVsTarget(WorldObject target, Spell spell)
        {
            if (target == null ||target == this)
                return null;

            var targetCreature = target as Creature;
            if (targetCreature == null && target.WielderId != null)
            {
                // handle casting item spells
                targetCreature = CurrentLandblock.GetObject(target.WielderId.Value) as Creature;
            }
            if (targetCreature == null)
                return null;

            if (PlayerKillerStatus == PlayerKillerStatus.Free || targetCreature.PlayerKillerStatus == PlayerKillerStatus.Free)
                return null;

            var targetPlayer = target as Player;

            if (targetPlayer != null)
            {
                if (spell == null || spell.IsHarmful)
                {
                    // Ensure that a non-PK cannot cast harmful spells on another player
                    if (PlayerKillerStatus == PlayerKillerStatus.NPK)
                        return new List<WeenieErrorWithString>() { WeenieErrorWithString.YouFailToAffect_YouAreNotPK, WeenieErrorWithString._FailsToAffectYou_TheyAreNotPK };

                    if (targetPlayer.PlayerKillerStatus == PlayerKillerStatus.NPK)
                        return new List<WeenieErrorWithString>() { WeenieErrorWithString.YouFailToAffect_TheyAreNotPK, WeenieErrorWithString._FailsToAffectYou_YouAreNotPK };

                    // Ensure not attacking across housing boundary
                    if (!CheckHouseRestrictions(targetPlayer))
                        return new List<WeenieErrorWithString>() { WeenieErrorWithString.YouFailToAffect_AcrossHouseBoundary, WeenieErrorWithString._FailsToAffectYouAcrossHouseBoundary };
                }

                // additional checks for different PKTypes
                if (PlayerKillerStatus != targetPlayer.PlayerKillerStatus)
                {
                    // require same pk status, unless beneficial spell being cast on NPK
                    // https://asheron.fandom.com/wiki/Player_Killer
                    // https://asheron.fandom.com/wiki/Player_Killer_Lite

                    if (spell == null || spell.IsHarmful || targetPlayer.PlayerKillerStatus != PlayerKillerStatus.NPK)
                        return new List<WeenieErrorWithString>() { WeenieErrorWithString.YouFailToAffect_NotSamePKType, WeenieErrorWithString._FailsToAffectYou_NotSamePKType };
                }
            }
            else
            {
                // if monster has a non-default pk status, ensure pk types match up
                if (targetCreature.PlayerKillerStatus != PlayerKillerStatus.NPK && PlayerKillerStatus != targetCreature.PlayerKillerStatus)
                {
                    return new List<WeenieErrorWithString>() { WeenieErrorWithString.YouFailToAffect_NotSamePKType, WeenieErrorWithString._FailsToAffectYou_NotSamePKType };
                }
            }
            return null;
        }
    }
}
