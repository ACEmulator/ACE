using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

using ACE.Common;
using ACE.Database;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
using ACE.Server.Entity;
using ACE.Server.Factories;
using ACE.Server.Factories.Entity;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Structure;
using ACE.Server.Managers;
using ACE.Server.Entity.Actions;
using ACE.Server.Physics;
using ACE.Server.Physics.Extensions;
using ACE.Server.WorldObjects.Managers;

namespace ACE.Server.WorldObjects
{
    partial class WorldObject
    {
        /// <summary>
        /// Instantly casts a spell for a WorldObject (ie. spell traps)
        /// </summary>
        public void TryCastSpell(Spell spell, WorldObject target, WorldObject itemCaster = null, WorldObject weapon = null, bool isWeaponSpell = false, bool fromProc = false, bool tryResist = true)
        {
            // TODO: look into further normalizing this / caster / weapon

            // verify spell exists in database
            if (spell._spell == null)
            {
                if (target is Player targetPlayer)
                    targetPlayer.Session.Network.EnqueueSend(new GameMessageSystemChat($"{spell.Name} spell not implemented, yet!", ChatMessageType.System));

                return;
            }

            if (spell.Flags.HasFlag(SpellFlags.FellowshipSpell))
            {
                if (target is not Player targetPlayer || targetPlayer.Fellowship == null)
                    return;

                var fellows = targetPlayer.Fellowship.GetFellowshipMembers();

                foreach (var fellow in fellows.Values)
                    TryCastSpell_Inner(spell, fellow, itemCaster, weapon, isWeaponSpell, fromProc, tryResist);
            }
            else
                TryCastSpell_Inner(spell, target, itemCaster, weapon, isWeaponSpell, fromProc, tryResist);
        }

        public void TryCastSpell_Inner(Spell spell, WorldObject target, WorldObject itemCaster = null, WorldObject weapon = null, bool isWeaponSpell = false, bool fromProc = false, bool tryResist = true)
        {
            // verify before resist, still consumes source item
            if (spell.MetaSpellType == SpellType.Dispel && !VerifyDispelPKStatus(itemCaster, target))
                return;

            // perform resistance check, if applicable
            if (tryResist && TryResistSpell(target, spell, itemCaster))
                return;

            // if not resisted, cast spell
            HandleCastSpell(spell, target, itemCaster, weapon, isWeaponSpell, fromProc);
        }

        /// <summary>
        /// Instantly casts a spell for a WorldObject, with optional redirects for item enchantments
        /// </summary>
        public bool TryCastSpell_WithRedirects(Spell spell, WorldObject target, WorldObject itemCaster = null, WorldObject weapon = null, bool isWeaponSpell = false, bool fromProc = false, bool tryResist = true)
        {
            if (target is Creature creatureTarget)
            {
                var targets = GetNonComponentTargetTypes(spell, creatureTarget);

                if (targets != null)
                {
                    foreach (var itemTarget in targets)
                        TryCastSpell(spell, itemTarget, itemCaster, weapon, isWeaponSpell, fromProc, tryResist);

                    return targets.Count > 0;
                }
            }

            TryCastSpell(spell, target, itemCaster, weapon, isWeaponSpell, fromProc, tryResist);

            return true;
        }

        /// <summary>
        /// Determines whether a spell will be resisted,
        /// based upon the caster's magic skill vs target's magic defense skill
        /// </summary>
        /// <returns>TRUE if spell is resisted</returns>
        public static bool MagicDefenseCheck(uint casterMagicSkill, uint targetMagicDefenseSkill, out float resistChance)
        {
            // uses regular 0.03 factor, and not magic casting 0.07 factor
            var chance = SkillCheck.GetSkillChance((int)casterMagicSkill, (int)targetMagicDefenseSkill);
            var rng = ThreadSafeRandom.Next(0.0f, 1.0f);

            resistChance = (float)(1.0f - chance);

            return chance <= rng;
        }

        /// <summary>
        /// If this spell has a chance to be resisted, rolls for a chance
        /// Returns TRUE if spell is resistable and was resisted for this attempt
        /// </summary>
        public bool TryResistSpell(WorldObject target, Spell spell, WorldObject itemCaster = null, bool projectileHit = false)
        {
            // fix hermetic void?
            if (!spell.IsResistable && spell.Category != SpellCategory.ManaConversionModLowering || spell.IsSelfTargeted)
            //if (!spell.IsResistable || spell.IsSelfTargeted)
                return false;

            if (spell.MetaSpellType == SpellType.Dispel && spell.Align == DispelType.Negative && !PropertyManager.GetBool("allow_negative_dispel_resist").Item)
                return false;

            if (spell.NumProjectiles > 0 && !projectileHit)
                return false;

            if (itemCaster != null && Cloak.IsCloak(itemCaster))
                return false;

            uint magicSkill = 0;

            var caster = itemCaster ?? this;

            var casterCreature = caster as Creature;

            if (casterCreature != null)
            {
                // Retrieve caster's skill level in the Magic School
                magicSkill = casterCreature.GetCreatureSkill(spell.School).Current;

            }
            else if (caster.ItemSpellcraft != null)
            {
                // Retrieve casting item's spellcraft
                magicSkill = (uint)caster.ItemSpellcraft;
            }
            else if (caster.Wielder is Creature wielder)
            {
                // Receive wielder's skill level in the Magic School?
                magicSkill = wielder.GetCreatureSkill(spell.School).Current;
            }

            //Console.WriteLine($"Magic skill: {magicSkill}");

            // only creatures can resist spells?
            if (target is not Creature targetCreature)
                return false;

            // Retrieve target's Magic Defense Skill
            var difficulty = targetCreature.GetEffectiveMagicDefense();

            //Console.WriteLine($"{target.Name}.ResistSpell({Name}, {spell.Name}): magicSkill: {magicSkill}, difficulty: {difficulty}");
            bool resisted = MagicDefenseCheck(magicSkill, difficulty, out float resistChance);

            var player = this as Player;
            var targetPlayer = target as Player;

            if (targetPlayer != null)
            {
                if (targetPlayer.Invincible)
                    resisted = true;

                if (targetPlayer.UnderLifestoneProtection)
                {
                    targetPlayer.HandleLifestoneProtection();
                    resisted = true;
                }
            }

            if (caster == target)
                resisted = false;

            if (resisted)
            {
                if (player != null)
                {
                    player.SendChatMessage(targetCreature, $"{targetCreature.Name} resists your spell", ChatMessageType.Magic);

                    player.Session.Network.EnqueueSend(new GameMessageSound(player.Guid, Sound.ResistSpell, 1.0f));
                }

                if (targetPlayer != null)
                {
                    targetPlayer.SendChatMessage(this, $"You resist the spell cast by {Name}", ChatMessageType.Magic);

                    targetPlayer.Session.Network.EnqueueSend(new GameMessageSound(targetPlayer.Guid, Sound.ResistSpell, 1.0f));

                    if (casterCreature != null)
                        targetPlayer.SetCurrentAttacker(casterCreature);

                    Proficiency.OnSuccessUse(targetPlayer, targetPlayer.GetCreatureSkill(Skill.MagicDefense), magicSkill);
                }

                if (this is Creature creature)
                    targetCreature.EmoteManager.OnResistSpell(creature);
            }

            if (player != null && player.DebugDamage.HasFlag(Creature.DebugDamageType.Attacker))
            {
                ShowResistInfo(player, this, target, spell, magicSkill, difficulty, resistChance, resisted);
            }
            if (targetCreature != null && targetCreature.DebugDamage.HasFlag(Creature.DebugDamageType.Defender))
            {
                ShowResistInfo(targetCreature, this, target, spell, magicSkill, difficulty, resistChance, resisted);
            }

            return resisted;
        }

        public static void ShowResistInfo(Creature observed, WorldObject attacker, WorldObject defender, Spell spell, uint attackSkill, uint defenseSkill, float resistChance, bool resisted)
        {
            var targetInfo = PlayerManager.GetOnlinePlayer(observed.DebugDamageTarget);

            if (targetInfo == null)
            {
                observed.DebugDamage = Creature.DebugDamageType.None;
                return;
            }

            // initial info / resist chance
            var info = $"Attacker: {attacker.Name} ({attacker.Guid})\n";
            info += $"Defender: {defender.Name} ({defender.Guid})\n";

            info += $"CombatType: Magic\n";

            info += $"Spell: {spell.Name} ({spell.Id})\n";

            info += $"EffectiveAttackSkill: {attackSkill}\n";
            info += $"EffectiveDefenseSkill: {defenseSkill}\n";

            info += $"ResistChance: {resistChance}\n";

            info += $"Resisted: {resisted}";

            if (resisted || spell.NumProjectiles == 0)
                targetInfo.Session.Network.EnqueueSend(new GameMessageSystemChat(info, ChatMessageType.Broadcast));
            else
                targetInfo.DebugDamageBuffer = $"{info}\n";
        }

        /// <summary>
        /// Creates a spell based on MetaSpellType
        /// </summary>
        protected bool HandleCastSpell(Spell spell, WorldObject target, WorldObject itemCaster = null, WorldObject weapon = null, bool isWeaponSpell = false, bool fromProc = false, bool equip = false)
        {
            var targetCreature = !spell.IsSelfTargeted || spell.IsFellowshipSpell ? target as Creature : this as Creature;

            if (this is Gem || this is Food || this is Hook)
                targetCreature = target as Creature;

            if (spell.School == MagicSchool.LifeMagic || spell.MetaSpellType == SpellType.Dispel)
            {
                // NonComponentTargetType should be 0 for untargeted spells.
                // Return if the spell type is targeted with no target defined or the target is already dead.
                if ((targetCreature == null || !targetCreature.IsAlive) && spell.NonComponentTargetType != ItemType.None
                    && spell.DispelSchool != MagicSchool.ItemEnchantment)
                {
                    return false;
                }
            }

            switch (spell.MetaSpellType)
            {
                case SpellType.Enchantment:
                case SpellType.FellowEnchantment:

                    // TODO: replace with some kind of 'rootOwner unless equip' concept?
                    if (itemCaster != null && (equip || itemCaster is Gem || itemCaster is Food))
                        CreateEnchantment(targetCreature ?? target, itemCaster, itemCaster, spell, equip);
                    else
                        CreateEnchantment(targetCreature ?? target, this, this, spell, equip);

                    break;

                case SpellType.Boost:
                case SpellType.FellowBoost:

                    HandleCastSpell_Boost(spell, targetCreature);
                    break;

                case SpellType.Transfer:

                    HandleCastSpell_Transfer(spell, targetCreature);
                    break;

                case SpellType.Projectile:
                case SpellType.LifeProjectile:
                case SpellType.EnchantmentProjectile:

                    HandleCastSpell_Projectile(spell, targetCreature, itemCaster, weapon, isWeaponSpell, fromProc);
                    break;

                case SpellType.PortalLink:

                    HandleCastSpell_PortalLink(spell, target);
                    break;

                case SpellType.PortalRecall:

                    HandleCastSpell_PortalRecall(spell, targetCreature);
                    break;

                case SpellType.PortalSummon:

                    HandleCastSpell_PortalSummon(spell, targetCreature, itemCaster);
                    break;

                case SpellType.PortalSending:

                    HandleCastSpell_PortalSending(spell, targetCreature, itemCaster);
                    break;

                case SpellType.FellowPortalSending:

                    HandleCastSpell_FellowPortalSending(spell, targetCreature, itemCaster);
                    break;

                case SpellType.Dispel:
                case SpellType.FellowDispel:

                    HandleCastSpell_Dispel(spell, targetCreature ?? target);
                    break;

                default:

                    if (this is Player player)
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat("Spell not implemented, yet!", ChatMessageType.Magic));

                    return false;
            }

            // play spell effects
            DoSpellEffects(spell, this, target);

            return true;
        }

        /// <summary>
        /// Plays the caster/target effects for a spell
        /// </summary>
        protected void DoSpellEffects(Spell spell, WorldObject caster, WorldObject target, bool projectileHit = false)
        {
            if (spell.CasterEffect != 0 && (!spell.IsProjectile || !projectileHit))
                caster.EnqueueBroadcast(new GameMessageScript(caster.Guid, spell.CasterEffect, spell.Formula.Scale));

            if (spell.TargetEffect != 0 && (!spell.IsProjectile || projectileHit))
            {
                var targetBroadcaster = target.Wielder ?? target;

                targetBroadcaster.EnqueueBroadcast(new GameMessageScript(target.Guid, spell.TargetEffect, spell.Formula.Scale));
            }
        }

        /// <summary>
        /// Handles casting SpellType.Enchantment / FellowEnchantment spells
        /// this is also called if SpellType.EnchantmentProjectile successfully hits
        /// </summary>
        public void CreateEnchantment(WorldObject target, WorldObject caster, WorldObject weapon, Spell spell, bool equip = false, bool fromProc = false)
        {
            // weird itemCaster -> caster collapsing going on here -- fixme

            var player = this as Player;

            var aetheriaProc = false;
            var cloakProc = false;

            // technically unsafe, should be using fromProc
            if (caster.ProcSpell == spell.Id)
            {
                if (caster is Gem && Aetheria.IsAetheria(caster.WeenieClassId))
                {
                    caster = this;
                    aetheriaProc = true;
                }
                else if (Cloak.IsCloak(caster))
                {
                    caster = this;
                    cloakProc = true;
                }
            }
            else if (fromProc)
            {
                // fromProc is assumed to be cloakProc currently
                // todo: change fromProc from bool to WorldObject
                // do we need separate concepts for itemCaster and fromProc objects?
                caster = this;
                cloakProc = true;
            }

            // create enchantment
            var addResult = target.EnchantmentManager.Add(spell, caster, weapon, equip);

            // build message
            var suffix = "";
            switch (addResult.StackType)
            {
                case StackType.Surpass:
                    suffix = $", surpassing {addResult.SurpassSpell.Name}";
                    break;
                case StackType.Refresh:
                    suffix = $", refreshing {addResult.RefreshSpell.Name}";
                    break;
                case StackType.Surpassed:
                    suffix = $", but it is surpassed by {addResult.SurpassedSpell.Name}";
                    break;
            }

            if (aetheriaProc)
            {
                var message = new GameMessageSystemChat($"Aetheria surges on {target.Name} with the power of {spell.Name}!", ChatMessageType.Spellcasting);

                EnqueueBroadcast(message, LocalBroadcastRange, ChatMessageType.Spellcasting);
            }
            else if (player != null && !cloakProc)
            {
                // TODO: replace with some kind of 'rootOwner unless equip' concept?
                // for item casters where the message should be 'You cast', we still need pass the caster as item
                // down this far, to prevent using player's AugmentationIncreasedSpellDuration
                var casterCheck = caster == this || caster is Gem || caster is Food;

                if (casterCheck || target == this || caster != target)
                {
                    var casterName = casterCheck ? "You" : caster.Name;
                    var targetName = target.Name;
                    if (target == this)
                        targetName = casterCheck ? "yourself" : "you";

                    player.SendChatMessage(player, $"{casterName} cast {spell.Name} on {targetName}{suffix}", ChatMessageType.Magic);
                }
            }

            var playerTarget = target as Player;

            if (playerTarget != null)
            {
                playerTarget.Session.Network.EnqueueSend(new GameEventMagicUpdateEnchantment(playerTarget.Session, new Enchantment(playerTarget, addResult.Enchantment)));

                playerTarget.HandleSpellHooks(spell);

                if (!spell.IsBeneficial && this is Creature creatureCaster)
                    playerTarget.SetCurrentAttacker(creatureCaster);
            }

            if (playerTarget == null && target.Wielder is Player wielder)
                playerTarget = wielder;

            if (playerTarget != null && playerTarget != this && !cloakProc)
            {
                var targetName = target == playerTarget ? "you" : $"your {target.Name}";

                playerTarget.SendChatMessage(this, $"{caster.Name} cast {spell.Name} on {targetName}{suffix}", ChatMessageType.Magic);
            }
        }

        /// <summary>
        /// Handles casting SpellType.Boost / FellowBoost spells
        /// typically for Life Magic, ie. Heal, Harm
        /// </summary>
        private void HandleCastSpell_Boost(Spell spell, Creature targetCreature)
        {
            var player = this as Player;
            var creature = this as Creature;

            // double check caster and target are alive
            if (creature != null && creature.IsDead || targetCreature != null && targetCreature.IsDead)
                return;

            // handle negatives?
            int minBoostValue = Math.Min(spell.Boost, spell.MaxBoost);
            int maxBoostValue = Math.Max(spell.Boost, spell.MaxBoost);

            var resistanceType = minBoostValue > 0 ? GetBoostResistanceType(spell.VitalDamageType) : GetDrainResistanceType(spell.VitalDamageType);

            int tryBoost = ThreadSafeRandom.Next(minBoostValue, maxBoostValue);
            tryBoost = (int)Math.Round(tryBoost * targetCreature.GetResistanceMod(resistanceType));

            int boost = tryBoost;

            // handle cloak damage proc for harm other
            var equippedCloak = targetCreature?.EquippedCloak;

            if (targetCreature != this && spell.VitalDamageType == DamageType.Health && tryBoost < 0)
            {
                var percent = (float)-tryBoost / targetCreature.Health.MaxValue;

                if (equippedCloak != null && Cloak.HasDamageProc(equippedCloak) && Cloak.RollProc(equippedCloak, percent))
                {
                    var reduced = -Cloak.GetReducedAmount(this, -tryBoost);

                    Cloak.ShowMessage(targetCreature, this, -tryBoost, -reduced);

                    tryBoost = boost = reduced;
                }
            }

            string srcVital;

            switch (spell.VitalDamageType)
            {
                case DamageType.Mana:
                    boost = targetCreature.UpdateVitalDelta(targetCreature.Mana, tryBoost);
                    srcVital = "mana";
                    break;
                case DamageType.Stamina:
                    boost = targetCreature.UpdateVitalDelta(targetCreature.Stamina, tryBoost);
                    srcVital = "stamina";
                    break;
                default:   // Health
                    boost = targetCreature.UpdateVitalDelta(targetCreature.Health, tryBoost);
                    srcVital = "health";

                    if (boost >= 0)
                        targetCreature.DamageHistory.OnHeal((uint)boost);
                    else
                        targetCreature.DamageHistory.Add(this, DamageType.Health, (uint)-boost);

                    //if (targetPlayer != null && targetPlayer.Fellowship != null)
                        //targetPlayer.Fellowship.OnVitalUpdate(targetPlayer);

                    break;
            }

            if (player != null)
            {
                string casterMessage;

                if (player != targetCreature)
                {
                    if (spell.IsBeneficial)
                        casterMessage = $"With {spell.Name} you restore {boost} points of {srcVital} to {targetCreature.Name}.";
                    else
                        casterMessage = $"With {spell.Name} you drain {Math.Abs(boost)} points of {srcVital} from {targetCreature.Name}.";
                }
                else
                {
                    var verb = spell.IsBeneficial ? "restore" : "drain";

                    casterMessage = $"You cast {spell.Name} and {verb} {Math.Abs(boost)} points of your {srcVital}.";
                }

                player.SendChatMessage(player, casterMessage, ChatMessageType.Magic);
            }

            if (targetCreature is Player targetPlayer && player != targetPlayer)
            {
                string targetMessage;

                if (spell.IsBeneficial)
                    targetMessage = $"{Name} casts {spell.Name} and restores {boost} points of your {srcVital}.";
                else
                {
                    targetMessage = $"{Name} casts {spell.Name} and drains {Math.Abs(boost)} points of your {srcVital}.";

                    if (creature != null)
                        targetPlayer.SetCurrentAttacker(creature);
                }

                targetPlayer.SendChatMessage(player, targetMessage, ChatMessageType.Magic);
            }

            if (targetCreature != this && targetCreature.IsAlive && spell.VitalDamageType == DamageType.Health && boost < 0)
            {
                // handle cloak spell proc
                if (equippedCloak != null && Cloak.HasProcSpell(equippedCloak))
                {
                    var pct = (float)-boost / targetCreature.Health.MaxValue;

                    // ensure message is sent after enchantment.Message
                    var actionChain = new ActionChain();
                    actionChain.AddDelayForOneTick();
                    actionChain.AddAction(this, () => Cloak.TryProcSpell(targetCreature, this, equippedCloak, pct));
                    actionChain.EnqueueChain();
                }

                // ensure emote process occurs after damage msg
                var emoteChain = new ActionChain();
                emoteChain.AddDelayForOneTick();
                emoteChain.AddAction(targetCreature, () => targetCreature.EmoteManager.OnDamage(creature));
                //if (critical)
                //    emoteChain.AddAction(target, () => target.EmoteManager.OnReceiveCritical(creature));
                emoteChain.EnqueueChain();
            }

            HandleBoostTransferDeath(creature, targetCreature);
        }

        /// <summary>
        /// Returns the boost resistance for a damage type
        /// </summary>
        private static ResistanceType GetBoostResistanceType(DamageType damageType)
        {
            switch (damageType)
            {
                case DamageType.Health:
                    return ResistanceType.HealthBoost;
                case DamageType.Stamina:
                    return ResistanceType.StaminaBoost;
                case DamageType.Mana:
                    return ResistanceType.ManaBoost;
                default:
                    return ResistanceType.Undef;
            }
        }

        /// <summary>
        /// Returns the drain resistance for a damage type
        /// </summary>
        private static ResistanceType GetDrainResistanceType(DamageType damageType)
        {
            switch (damageType)
            {
                case DamageType.Health:
                    return ResistanceType.HealthDrain;
                case DamageType.Stamina:
                    return ResistanceType.StaminaDrain;
                case DamageType.Mana:
                    return ResistanceType.ManaDrain;
                default:
                    return ResistanceType.Undef;
            }
        }

        /// <summary>
        /// Returns the boost resistance type for a vital
        /// </summary>
        private static ResistanceType GetBoostResistanceType(PropertyAttribute2nd vital)
        {
            switch (vital)
            {
                case PropertyAttribute2nd.Health:
                    return ResistanceType.HealthBoost;
                case PropertyAttribute2nd.Stamina:
                    return ResistanceType.StaminaBoost;
                case PropertyAttribute2nd.Mana:
                    return ResistanceType.ManaBoost;
                default:
                    return ResistanceType.Undef;
            }
        }

        /// <summary>
        /// Returns the drain resistance type for a vital
        /// </summary>
        private static ResistanceType GetDrainResistanceType(PropertyAttribute2nd vital)
        {
            switch (vital)
            {
                case PropertyAttribute2nd.Health:
                    return ResistanceType.HealthDrain;
                case PropertyAttribute2nd.Stamina:
                    return ResistanceType.StaminaDrain;
                case PropertyAttribute2nd.Mana:
                    return ResistanceType.ManaDrain;
                default:
                    return ResistanceType.Undef;
            }
        }

        /// <summary>
        /// Checks for death from a boost / transfer spell
        /// </summary>
        private void HandleBoostTransferDeath(Creature caster, Creature target)
        {
            if (caster != null && caster.IsDead)
            {
                caster.OnDeath(caster.DamageHistory.LastDamager, DamageType.Health, false);
                caster.Die();
            }

            if (target != null && target.IsDead && target != caster)
            {
                target.OnDeath(target.DamageHistory.LastDamager, DamageType.Health, false);
                target.Die();
            }
        }

        /// <summary>
        /// Handles casting SpellType.Transfer spells
        /// usually for Life Magic, ie. Stamina to Mana, Drain
        /// </summary>
        private void HandleCastSpell_Transfer(Spell spell, Creature targetCreature)
        {
            var player = this as Player;
            var creature = this as Creature;

            var targetPlayer = targetCreature as Player;

            // double check caster and target are alive
            if (creature != null && creature.IsDead || targetCreature != null && targetCreature.IsDead)
                return;

            // source and destination can be the same creature, or different creatures
            var caster = this as Creature;
            var transferSource = spell.TransferFlags.HasFlag(TransferFlags.CasterSource) ? caster : targetCreature;
            var destination = spell.TransferFlags.HasFlag(TransferFlags.CasterDestination) ? caster : targetCreature;

            // Calculate vital changes
            uint srcVitalChange, destVitalChange;

            // Drain Resistances - allows one to partially resist drain health/stamina/mana and harm attacks (not including other life transfer spells).
            var isDrain = spell.TransferFlags.HasFlag(TransferFlags.TargetSource | TransferFlags.CasterDestination);
            var drainMod = isDrain ? (float)transferSource.GetResistanceMod(GetDrainResistanceType(spell.Source)) : 1.0f;

            srcVitalChange = (uint)Math.Round(transferSource.GetCreatureVital(spell.Source).Current * spell.Proportion * drainMod);

            // TransferCap caps both srcVitalChange and destVitalChange
            // https://asheron.fandom.com/wiki/Announcements_-_2003/01_-_The_Slumbering_Giant#Letter_to_the_Players

            if (spell.TransferCap != 0 && srcVitalChange > spell.TransferCap)
                srcVitalChange = (uint)spell.TransferCap;

            // should healing resistances be applied here?
            var boostMod = isDrain ? (float)destination.GetResistanceMod(GetBoostResistanceType(spell.Destination)) : 1.0f;

            destVitalChange = (uint)Math.Round(srcVitalChange * (1.0f - spell.LossPercent) * boostMod);

            // scale srcVitalChange to destVitalChange?
            var missingDest = destination.GetCreatureVital(spell.Destination).Missing;

            var maxDestVitalChange = missingDest;
            if (spell.TransferCap != 0 && maxDestVitalChange > spell.TransferCap)
                maxDestVitalChange = (uint)spell.TransferCap;

            if (destVitalChange > maxDestVitalChange)
            {
                var scalar = (float)maxDestVitalChange / destVitalChange;

                srcVitalChange = (uint)Math.Round(srcVitalChange * scalar);
                destVitalChange = maxDestVitalChange;
            }

            // handle cloak damage procs for drain health other
            var equippedCloak = targetCreature?.EquippedCloak;

            if (isDrain && spell.Source == PropertyAttribute2nd.Health)
            {
                var percent = (float)srcVitalChange / targetCreature.Health.MaxValue;

                if (equippedCloak != null && Cloak.HasDamageProc(equippedCloak) && Cloak.RollProc(equippedCloak, percent))
                {
                    var reduced = Cloak.GetReducedAmount(this, srcVitalChange);

                    Cloak.ShowMessage(targetCreature, this, srcVitalChange, reduced);

                    srcVitalChange = reduced;
                    destVitalChange = (uint)Math.Round(srcVitalChange * (1.0f - spell.LossPercent) * boostMod);
                }
            }

            string srcVital, destVital;

            // Apply the change in vitals to the source
            switch (spell.Source)
            {
                case PropertyAttribute2nd.Mana:
                    srcVital = "mana";
                    srcVitalChange = (uint)-transferSource.UpdateVitalDelta(transferSource.Mana, -(int)srcVitalChange);
                    break;
                case PropertyAttribute2nd.Stamina:
                    srcVital = "stamina";
                    srcVitalChange = (uint)-transferSource.UpdateVitalDelta(transferSource.Stamina, -(int)srcVitalChange);
                    break;
                default:   // Health
                    srcVital = "health";
                    srcVitalChange = (uint)-transferSource.UpdateVitalDelta(transferSource.Health, -(int)srcVitalChange);

                    transferSource.DamageHistory.Add(this, DamageType.Health, srcVitalChange);

                    //var sourcePlayer = source as Player;
                    //if (sourcePlayer != null && sourcePlayer.Fellowship != null)
                        //sourcePlayer.Fellowship.OnVitalUpdate(sourcePlayer);

                    break;
            }

            // Apply the scaled change in vitals to the caster
            switch (spell.Destination)
            {
                case PropertyAttribute2nd.Mana:
                    destVital = "mana";
                    destVitalChange = (uint)destination.UpdateVitalDelta(destination.Mana, destVitalChange);
                    break;
                case PropertyAttribute2nd.Stamina:
                    destVital = "stamina";
                    destVitalChange = (uint)destination.UpdateVitalDelta(destination.Stamina, destVitalChange);
                    break;
                default:   // Health
                    destVital = "health";
                    destVitalChange = (uint)destination.UpdateVitalDelta(destination.Health, destVitalChange);

                    destination.DamageHistory.OnHeal(destVitalChange);

                    //var destPlayer = destination as Player;
                    //if (destPlayer != null && destPlayer.Fellowship != null)
                        //destPlayer.Fellowship.OnVitalUpdate(destPlayer);

                    break;
            }

            // You gain 52 points of health due to casting Drain Health Other I on Olthoi Warrior
            // You lose 22 points of mana due to casting Incantation of Infuse Mana Other on High-Voltage VI
            // You lose 12 points of mana due to Zofrit Zefir casting Drain Mana Other II on you

            // You cast Stamina to Mana Self I on yourself and lose 50 points of stamina and also gain 45 points of mana
            // You cast Stamina to Health Self VI on yourself and fail to affect your  stamina and also gain 1 point of health

            // unverified:
            // You gain X points of vital due to caster casting spell on you
            // You lose X points of vital due to caster casting spell on you

            var playerSource = transferSource as Player;
            var playerDestination = destination as Player;

            string sourceMsg = null, targetMsg = null;

            if (playerSource != null && playerDestination != null && transferSource.Guid == destination.Guid)
            {
                sourceMsg = $"You cast {spell.Name} on yourself and lose {srcVitalChange} points of {srcVital} and also gain {destVitalChange} points of {destVital}";
            }
            else
            {
                if (playerSource != null)
                {
                    if (transferSource == this)
                        sourceMsg = $"You lose {srcVitalChange} points of {srcVital} due to casting {spell.Name} on {targetCreature.Name}";
                    else
                        targetMsg = $"You lose {srcVitalChange} points of {srcVital} due to {caster.Name} casting {spell.Name} on you";

                    if (destination is Creature creatureDestination)
                        playerSource.SetCurrentAttacker(creatureDestination);
                }

                if (playerDestination != null)
                {
                    if (destination == this)
                        sourceMsg = $"You gain {destVitalChange} points of {destVital} due to casting {spell.Name} on {targetCreature.Name}";
                    else
                        targetMsg = $"You gain {destVitalChange} points of {destVital} due to {caster.Name} casting {spell.Name} on you";
                }
            }

            if (player != null && sourceMsg != null)
                player.SendChatMessage(player, sourceMsg, ChatMessageType.Magic);

            if (targetPlayer != null && targetMsg != null)
                targetPlayer.SendChatMessage(caster, targetMsg, ChatMessageType.Magic);


            if (isDrain && targetCreature.IsAlive && spell.Source == PropertyAttribute2nd.Health)
            {
                // handle cloak spell proc
                if (equippedCloak != null && Cloak.HasProcSpell(equippedCloak))
                {
                    var pct = (float)srcVitalChange / targetCreature.Health.MaxValue;

                    // ensure message is sent after enchantment.Message
                    var actionChain = new ActionChain();
                    actionChain.AddDelayForOneTick();
                    actionChain.AddAction(this, () => Cloak.TryProcSpell(targetCreature, this, equippedCloak, pct));
                    actionChain.EnqueueChain();
                }

                // ensure emote process occurs after damage msg
                var emoteChain = new ActionChain();
                emoteChain.AddDelayForOneTick();
                emoteChain.AddAction(targetCreature, () => targetCreature.EmoteManager.OnDamage(creature));
                //if (critical)
                //    emoteChain.AddAction(targetCreature, () => targetCreature.EmoteManager.OnReceiveCritical(creature));
                emoteChain.EnqueueChain();
            }

            HandleBoostTransferDeath(creature, targetCreature);
        }

        /// <summary>
        /// Handles casting SpellType.Projectile / LifeProjectile / EnchantmentProjectile spells
        /// </summary>
        private void HandleCastSpell_Projectile(Spell spell, WorldObject target, WorldObject itemCaster, WorldObject weapon, bool isWeaponSpell, bool fromProc)
        {
            uint damage = 0;
            var caster = this as Creature;
            var damageType = DamageType.Undef;

            if (spell.School == MagicSchool.LifeMagic)
            {
                if (spell.Name.Contains("Blight"))
                {
                    var tryDamage = (int)Math.Round(caster.GetCreatureVital(PropertyAttribute2nd.Mana).Current * spell.DrainPercentage);
                    damage = (uint)-caster.UpdateVitalDelta(caster.Mana, -tryDamage);
                    damageType = DamageType.Mana;
                }
                else if (spell.Name.Contains("Tenacity"))
                {
                    var tryDamage = (int)Math.Round(caster.GetCreatureVital(PropertyAttribute2nd.Stamina).Current * spell.DrainPercentage);
                    damage = (uint)-caster.UpdateVitalDelta(caster.Stamina, -tryDamage);
                    damageType = DamageType.Stamina;
                }
                else
                {
                    var tryDamage = (int)Math.Round(caster.GetCreatureVital(PropertyAttribute2nd.Health).Current * spell.DrainPercentage);
                    damage = (uint)-caster.UpdateVitalDelta(caster.Health, -tryDamage);
                    caster.DamageHistory.Add(this, DamageType.Health, damage);
                    damageType = DamageType.Health;

                    //if (player != null && player.Fellowship != null)
                        //player.Fellowship.OnVitalUpdate(player);
                }
            }

            CreateSpellProjectiles(spell, target, weapon, isWeaponSpell, fromProc, damage);

            if (spell.School == MagicSchool.LifeMagic)
            {
                if (caster.Health.Current <= 0)
                {
                    // should this be possible?
                    var lastDamager = caster != null ? new DamageHistoryInfo(caster) : null;

                    caster.OnDeath(lastDamager, damageType, false);
                    caster.Die();
                }
            }
        }

        /// <summary>
        /// Handles casting SpellType.PortalLink spells
        /// </summary>
        private void HandleCastSpell_PortalLink(Spell spell, WorldObject target)
        {
            var player = this as Player;

            if (player == null) return;

            if (player.IsOlthoiPlayer)
            {
                player.Session.Network.EnqueueSend(new GameEventWeenieError(player.Session, WeenieError.OlthoiCanOnlyRecallToLifestone));
                return;
            }

            switch ((SpellId)spell.Id)
            {
                case SpellId.LifestoneTie1:  // Lifestone Tie

                    if (target.WeenieType == WeenieType.LifeStone)
                    {
                        player.SendChatMessage(this, "You have successfully linked with the life stone.", ChatMessageType.Magic);
                        player.LinkedLifestone = target.Location;
                    }
                    else
                        player.SendChatMessage(this, "You cannot link that.", ChatMessageType.Magic);

                    break;

                case SpellId.PortalTie1:    // Primary Portal Tie
                case SpellId.PortalTie2:    // Secondary Portal Tie

                    if (target.WeenieType != WeenieType.Portal)
                    {
                        player.SendChatMessage(this, "You cannot link that.", ChatMessageType.Magic);
                        break;
                    }

                    var targetPortal = target as Portal;

                    var summoned = targetPortal.OriginalPortal != null;

                    var targetDID = summoned ? targetPortal.OriginalPortal : targetPortal.WeenieClassId;

                    var tiePortal = GetPortal(targetDID.Value);

                    if (tiePortal == null)
                    {
                        player.Session.Network.EnqueueSend(new GameEventWeenieError(player.Session, WeenieError.YouCannotLinkToThatPortal));
                        break;
                    }

                    var result = tiePortal.CheckUseRequirements(player);

                    if (!result.Success && result.Message != null)
                        player.Session.Network.EnqueueSend(result.Message);

                    if (tiePortal.NoTie || !result.Success)
                    {
                        player.Session.Network.EnqueueSend(new GameEventWeenieError(player.Session, WeenieError.YouCannotLinkToThatPortal));
                        break;
                    }

                    var isPrimary = spell.Id == (int)SpellId.PortalTie1;

                    if (isPrimary)
                    {
                        player.LinkedPortalOneDID = targetDID;
                        player.SetProperty(PropertyBool.LinkedPortalOneSummon, summoned);
                    }
                    else
                    {
                        player.LinkedPortalTwoDID = targetDID;
                        player.SetProperty(PropertyBool.LinkedPortalTwoSummon, summoned);
                    }

                    player.SendChatMessage(this, "You have successfully linked with the portal.", ChatMessageType.Magic);
                    break;
            }
        }

        /// <summary>
        /// Returns a Portal object for a WCID
        /// </summary>
        private static Portal GetPortal(uint wcid)
        {
            var weenie = DatabaseManager.World.GetCachedWeenie(wcid);

            return WorldObjectFactory.CreateWorldObject(weenie, new ObjectGuid(wcid)) as Portal;
        }

        /// <summary>
        /// Handles casting SpellType.PortalRecall spells
        /// </summary>
        private void HandleCastSpell_PortalRecall(Spell spell, Creature targetCreature)
        {
            var player = this as Player;

            if (player != null && player.IsOlthoiPlayer)
            {
                player.Session.Network.EnqueueSend(new GameEventWeenieError(player.Session, WeenieError.OlthoiCanOnlyRecallToLifestone));
                return;
            }

            var creature = this as Creature;

            var targetPlayer = targetCreature as Player;

            if (player != null && player.PKTimerActive)
            {
                player.Session.Network.EnqueueSend(new GameEventWeenieError(player.Session, WeenieError.YouHaveBeenInPKBattleTooRecently));
                return;
            }

            PositionType recall = PositionType.Undef;
            uint? recallDID = null;

            // verify pre-requirements for recalls

            switch ((SpellId)spell.Id)
            {
                case SpellId.PortalRecall:       // portal recall

                    if (targetPlayer.LastPortalDID == null)
                    {
                        // You must link to a portal to recall it!
                        targetPlayer.Session.Network.EnqueueSend(new GameEventWeenieError(targetPlayer.Session, WeenieError.YouMustLinkToPortalToRecall));
                    }
                    else
                    {
                        recall = PositionType.LastPortal;
                        recallDID = targetPlayer.LastPortalDID;
                    }
                    break;

                case SpellId.LifestoneRecall1:   // lifestone recall

                    if (targetPlayer.GetPosition(PositionType.LinkedLifestone) == null)
                    {
                        // You must link to a lifestone to recall it!
                        targetPlayer.Session.Network.EnqueueSend(new GameEventWeenieError(targetPlayer.Session, WeenieError.YouMustLinkToLifestoneToRecall));
                    }
                    else
                        recall = PositionType.LinkedLifestone;
                    break;

                case SpellId.LifestoneSending1:

                    if (player != null && player.GetPosition(PositionType.Sanctuary) != null)
                        recall = PositionType.Sanctuary;
                    else if (targetPlayer != null && targetPlayer.GetPosition(PositionType.Sanctuary) != null)
                        recall = PositionType.Sanctuary;

                    break;

                case SpellId.PortalTieRecall1:   // primary portal tie recall

                    if (targetPlayer.LinkedPortalOneDID == null)
                    {
                        // You must link to a portal to recall it!
                        targetPlayer.Session.Network.EnqueueSend(new GameEventWeenieError(targetPlayer.Session, WeenieError.YouMustLinkToPortalToRecall));
                    }
                    else
                    {
                        recall = PositionType.LinkedPortalOne;
                        recallDID = targetPlayer.LinkedPortalOneDID;
                    }
                    break;

                case SpellId.PortalTieRecall2:   // secondary portal tie recall

                    if (targetPlayer.LinkedPortalTwoDID == null)
                    {
                        // You must link to a portal to recall it!
                        targetPlayer.Session.Network.EnqueueSend(new GameEventWeenieError(targetPlayer.Session, WeenieError.YouMustLinkToPortalToRecall));
                    }
                    else
                    {
                        recall = PositionType.LinkedPortalTwo;
                        recallDID = targetPlayer.LinkedPortalTwoDID;
                    }
                    break;
            }

            if (recall != PositionType.Undef)
            {
                if (recallDID == null)
                {
                    // lifestone recall
                    ActionChain lifestoneRecall = new ActionChain();
                    lifestoneRecall.AddAction(targetPlayer, () => targetPlayer.DoPreTeleportHide());
                    lifestoneRecall.AddDelaySeconds(2.0f);  // 2 second delay
                    lifestoneRecall.AddAction(targetPlayer, () => targetPlayer.TeleToPosition(recall));
                    lifestoneRecall.EnqueueChain();
                }
                else
                {
                    // portal recall
                    var portal = GetPortal(recallDID.Value);
                    if (portal == null || portal.NoRecall)
                    {
                        // You cannot recall that portal!
                        player.Session.Network.EnqueueSend(new GameEventWeenieError(player.Session, WeenieError.YouCannotRecallPortal));
                        return;
                    }

                    var result = portal.CheckUseRequirements(targetPlayer);
                    if (!result.Success)
                    {
                        if (result.Message != null)
                            targetPlayer.Session.Network.EnqueueSend(result.Message);

                        return;
                    }

                    ActionChain portalRecall = new ActionChain();
                    portalRecall.AddAction(targetPlayer, () => targetPlayer.DoPreTeleportHide());
                    portalRecall.AddDelaySeconds(2.0f);  // 2 second delay
                    portalRecall.AddAction(targetPlayer, () =>
                    {
                        var teleportDest = new Position(portal.Destination);
                        AdjustDungeon(teleportDest);

                        targetPlayer.Teleport(teleportDest);
                    });
                    portalRecall.EnqueueChain();
                }
            }
        }

        /// <summary>
        /// Handles casting SpellType.PortalSummon spells
        /// </summary>
        private void HandleCastSpell_PortalSummon(Spell spell, Creature targetCreature, WorldObject itemCaster)
        {
            var player = this as Player;

            if (player != null && player.IsOlthoiPlayer)
            {
                player.Session.Network.EnqueueSend(new GameEventWeenieError(player.Session, WeenieError.OlthoiCanOnlyRecallToLifestone));
                return;
            }

            if (player != null && player.PKTimerActive)
            {
                player.Session.Network.EnqueueSend(new GameEventWeenieError(player.Session, WeenieError.YouHaveBeenInPKBattleTooRecently));
                return;
            }

            var source = player ?? itemCaster;

            uint portalId = 0;
            bool linkSummoned;

            // spell.link = 1 = LinkedPortalOneDID
            // spell.link = 2 = LinkedPortalTwoDID

            if (spell.Link <= 1)
            {
                portalId = source.LinkedPortalOneDID ?? 0;
                linkSummoned = source.GetProperty(PropertyBool.LinkedPortalOneSummon) ?? false;
            }
            else
            {
                portalId = source.LinkedPortalTwoDID ?? 0;
                linkSummoned = source.GetProperty(PropertyBool.LinkedPortalTwoSummon) ?? false;
            }

            Position summonLoc = null;

            if (player != null)
            {
                if (portalId == 0)
                {
                    // You must link to a portal to summon it!
                    player.Session.Network.EnqueueSend(new GameEventWeenieError(player.Session, WeenieError.YouMustLinkToPortalToSummonIt));
                    return;
                }

                var summonPortal = GetPortal(portalId);
                if (summonPortal == null || summonPortal.NoSummon || (linkSummoned && !PropertyManager.GetBool("gateway_ties_summonable").Item))
                {
                    // You cannot summon that portal!
                    player.Session.Network.EnqueueSend(new GameEventWeenieError(player.Session, WeenieError.YouCannotSummonPortal));
                    return;
                }

                var result = summonPortal.CheckUseRequirements(player);
                if (!result.Success)
                {
                    if (result.Message != null)
                        player.Session.Network.EnqueueSend(result.Message);

                    return;
                }

                summonLoc = player.Location.InFrontOf(3.0f);
            }
            else if (itemCaster != null)
            {
                if (itemCaster.PortalSummonLoc != null)
                    summonLoc = new Position(PortalSummonLoc);
                else
                {
                    if (itemCaster.Location != null)
                        summonLoc = itemCaster.Location.InFrontOf(3.0f);
                    else if (targetCreature != null && targetCreature.Location != null)
                        summonLoc = targetCreature.Location.InFrontOf(3.0f);
                }
            }

            if (summonLoc != null)
                summonLoc.LandblockId = new LandblockId(summonLoc.GetCell());

            var success = SummonPortal(portalId, summonLoc, spell.PortalLifetime);

            if (!success && player != null)
                player.Session.Network.EnqueueSend(new GameEventWeenieError(player.Session, WeenieError.YouFailToSummonPortal));
        }

        /// <summary>
        /// Spawns a portal for SpellType.PortalSummon spells
        /// </summary>
        protected static bool SummonPortal(uint portalId, Position location, double portalLifetime)
        {
            var portal = GetPortal(portalId);

            if (portal == null || location == null)
                return false;

            var gateway = WorldObjectFactory.CreateNewWorldObject("portalgateway") as Portal;

            if (gateway == null)
                return false;

            gateway.Location = new Position(location);
            gateway.OriginalPortal = portalId;

            gateway.UpdatePortalDestination(new Position(portal.Destination));

            gateway.TimeToRot = portalLifetime;

            gateway.MinLevel = portal.MinLevel;
            gateway.MaxLevel = portal.MaxLevel;
            gateway.PortalRestrictions = portal.PortalRestrictions;
            gateway.AccountRequirements = portal.AccountRequirements;
            gateway.AdvocateQuest = portal.AdvocateQuest;

            gateway.Quest = portal.Quest;
            gateway.QuestRestriction = portal.QuestRestriction;

            gateway.PortalRestrictions |= PortalBitmask.NoSummon; // all gateways are marked NoSummon but by default ruleset, the OriginalPortal is the one that is checked against

            gateway.EnterWorld();

            return true;
        }

        /// <summary>
        /// Handles casting SpellType.PortalSending spells
        /// </summary>
        private void HandleCastSpell_PortalSending(Spell spell, Creature targetCreature, WorldObject itemCaster)
        {
            if (targetCreature is Player targetPlayer)
            {
                if (targetPlayer.PKTimerActive)
                {
                    targetPlayer.Session.Network.EnqueueSend(new GameEventWeenieError(targetPlayer.Session, WeenieError.YouHaveBeenInPKBattleTooRecently));
                    return;
                }

                ActionChain portalSendingChain = new ActionChain();
                //portalSendingChain.AddDelaySeconds(2.0f);  // 2 second delay
                portalSendingChain.AddAction(targetPlayer, () => targetPlayer.DoPreTeleportHide());
                portalSendingChain.AddAction(targetPlayer, () =>
                {
                    var teleportDest = new Position(spell.Position);
                    AdjustDungeon(teleportDest);

                    targetPlayer.Teleport(teleportDest);

                    targetPlayer.SendTeleportedViaMagicMessage(itemCaster, spell);
                });
                portalSendingChain.EnqueueChain();
            }
            else if (targetCreature != null)
            {
                // monsters can cast some portal spells on themselves too, possibly?
                // under certain circumstances, such as ensuring the destination is the same landblock
                var teleportDest = new Position(spell.Position);
                AdjustDungeon(teleportDest);

                targetCreature.FakeTeleport(teleportDest);
            }
        }

        /// <summary>
        /// Handles casting SpellType.FellowPortalSending spells
        /// </summary>
        private bool HandleCastSpell_FellowPortalSending(Spell spell, Creature targetCreature, WorldObject itemCaster)
        {
            var creature = this as Creature;

            var targetPlayer = targetCreature as Player;

            if (targetPlayer == null || targetPlayer.Fellowship == null)
                return false;

            if (targetPlayer.PKTimerActive)
            {
                targetPlayer.Session.Network.EnqueueSend(new GameEventWeenieError(targetPlayer.Session, WeenieError.YouHaveBeenInPKBattleTooRecently));
                return false;
            }

            var distanceToTarget = creature.GetDistance(targetPlayer);
            var skill = creature.GetCreatureSkill(spell.School);
            var magicSkill = skill.InitLevel + skill.Ranks;     // ?? - this probably isn't right, should be either base or current

            var maxRange = spell.BaseRangeConstant + magicSkill * spell.BaseRangeMod;
            if (maxRange == 0.0f)
                maxRange = float.PositiveInfinity;

            if (distanceToTarget > maxRange)
                return false;

            var portalSendingChain = new ActionChain();
            portalSendingChain.AddAction(targetPlayer, () => targetPlayer.DoPreTeleportHide());
            portalSendingChain.AddAction(targetPlayer, () =>
            {
                var teleportDest = new Position(spell.Position);
                AdjustDungeon(teleportDest);

                targetPlayer.Teleport(teleportDest);

                targetPlayer.SendTeleportedViaMagicMessage(itemCaster, spell);
            });
            portalSendingChain.EnqueueChain();

            return true;
        }

        /// <summary>
        /// Handles casting SpellType.Dispel / FellowDispel spells
        /// </summary>
        private void HandleCastSpell_Dispel(Spell spell, WorldObject target)
        {
            var player = this as Player;
            var creature = this as Creature;

            var removeSpells = target.EnchantmentManager.SelectDispel(spell);

            // dispel on server and client
            target.EnchantmentManager.Dispel(removeSpells.Select(s => s.Enchantment).ToList());

            var spellList = BuildSpellList(removeSpells);
            var suffix = "";
            if (removeSpells.Count > 0)
                suffix = $" and dispel: {spellList}.";
            else
                suffix = ", but the dispel fails.";

            if (player != null)
            {
                string casterMsg;

                if (player == target)
                    casterMsg = $"You cast {spell.Name} on yourself{suffix}";
                else
                    casterMsg = $"You cast {spell.Name} on {target.Name}{suffix}";

                player.SendChatMessage(player, casterMsg, ChatMessageType.Magic);
            }

            if (target is Player targetPlayer && targetPlayer != player)
            {
                var targetMsg = $"{Name} casts {spell.Name} on you{suffix.Replace("and dispel", "and dispels")}";

                targetPlayer.SendChatMessage(this, targetMsg, ChatMessageType.Magic);

                // all dispels appear to be listed as non-beneficial, even the ones that only dispel negative spells
                // we filter here to positive or all
                if (creature != null && spell.Align != DispelType.Negative)
                    targetPlayer.SetCurrentAttacker(creature);
            }
        }

        public static bool VerifyDispelPKStatus(WorldObject caster, WorldObject target)
        {
            // https://asheron.fandom.com/wiki/Announcements_-_2004/04_-_A_New_Threat
            // https://asheron.fandom.com/wiki/Dispel_Spells

            // Dispel spells and potions have been revised. All dispels are also now tied to the PK/L timer.

            // The feedback on the suggested dispel timer for PK/L was very mixed. There was no clear majority either for or against.
            // With that in mind, we've gone ahead with the changes that we feel best improve majority of PK/L combat:
            // we've decided to implement the PK/L timer on dispels.

            // If you have been in a PK/L action within the last 20 seconds, you will not be able to:

            // - Use a dispel gem.
            // - Use a dispel potion.
            // - Use the Awakener or Attenuated Awakener on someone else.
            // - Cast any dispel spell on yourself.
            // - Cast any dispel spell on someone else.

            var casterPlayer = caster as Player;

            if (casterPlayer != null && casterPlayer.PKTimerActive)
            {
                casterPlayer.SendWeenieError(WeenieError.YouHaveBeenInPKBattleTooRecently);
                return false;
            }

            if ((target.Wielder ?? target) is Player targetPlayer && targetPlayer.PKTimerActive)
            {
                if (/* casterPlayer != null || */ caster is Gem || caster is Food)
                {
                    if (casterPlayer != null)
                        casterPlayer.Session.Network.EnqueueSend(new GameMessageSystemChat($"{targetPlayer.Name} has been involved in a player killer battle too recently to do that!", ChatMessageType.Magic));
                    else
                        targetPlayer.SendWeenieError(WeenieError.YouHaveBeenInPKBattleTooRecently);

                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Returns a string with the spell list format as:
        /// Spell Name 1, Spell Name 2, and Spell Name 3
        /// </summary>
        private static string BuildSpellList(List<SpellEnchantment> spells)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < spells.Count; i++)
            {
                var spell = spells[i];

                if (i > 0)
                {
                    sb.Append(", ");
                    if (i == spells.Count - 1)
                        sb.Append("and ");
                }

                sb.Append(spell.Spell.Name);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Creates and launches the projectiles for a spell
        /// </summary>
        public List<SpellProjectile> CreateSpellProjectiles(Spell spell, WorldObject target, WorldObject weapon, bool isWeaponSpell = false, bool fromProc = false, uint lifeProjectileDamage = 0)
        {
            if (spell.NumProjectiles == 0)
            {
                log.Error($"{Name} ({Guid}).CreateSpellProjectiles({spell.Id} - {spell.Name}) - spell.NumProjectiles == 0");
                return new List<SpellProjectile>();
            }

            var spellType = SpellProjectile.GetProjectileSpellType(spell.Id);

            var origins = CalculateProjectileOrigins(spell, spellType, target);

            var velocity = CalculateProjectileVelocity(spell, target, spellType, origins[0]);

            return LaunchSpellProjectiles(spell, target, spellType, weapon, isWeaponSpell, fromProc, origins, velocity, lifeProjectileDamage);
        }

        public static readonly float ProjHeight = 2.0f / 3.0f;

        public Vector3 CalculatePreOffset(Spell spell, ProjectileSpellType spellType, WorldObject target)
        {
            var startFactor = spellType == ProjectileSpellType.Arc ? 1.0f : ProjHeight;

            var preOffset = new Vector3(0, 0, Height * startFactor);

            if (target == null)
                return preOffset;

            var startPos = new Physics.Common.Position(PhysicsObj.Position);
            startPos.Frame.Origin.Z += Height * startFactor;

            var endFactor = spellType == ProjectileSpellType.Arc ? ProjHeightArc : ProjHeight;

            var endPos = new Physics.Common.Position(target.PhysicsObj.Position);
            endPos.Frame.Origin.Z += target.Height * endFactor;

            var globOffset = startPos.GetOffset(endPos);

            // align in x
            var rotate = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, (float)Math.Atan2(globOffset.X, globOffset.Y));

            var offset = Vector3.Transform(globOffset, rotate);

            var localDir = Vector3.Normalize(offset);

            var radsum = PhysicsObj.GetPhysicsRadius() + GetProjectileRadius(spell);

            var defaultSpawnPos = Vector3.UnitY * radsum;

            var spawnPos = localDir * radsum;

            var spawnOffset = spawnPos - defaultSpawnPos;

            return preOffset + spawnOffset;
        }

        /// <summary>
        /// Returns a list of positions to spawn projectiles for a spell,
        /// in local space relative to the caster
        /// </summary>
        public List<Vector3> CalculateProjectileOrigins(Spell spell, ProjectileSpellType spellType, WorldObject target)
        {
            var origins = new List<Vector3>();

            var radius = GetProjectileRadius(spell);
            //Console.WriteLine($"Radius: {radius}");

            var vRadius = Vector3.One * radius;

            var baseOffset = spell.CreateOffset;

            var radsum = PhysicsObj.GetPhysicsRadius() * 2.0f + radius * 2.0f;

            var heightOffset = CalculatePreOffset(spell, spellType, target);

            if (target != null)
            {
                var cylDist = GetCylinderDistance(target);
                //Console.WriteLine($"CylDist: {cylDist}");
                if (cylDist < 0.6f)
                    radsum = PhysicsObj.GetPhysicsRadius() + radius;
            }

            if (spell.SpreadAngle == 360)
                radsum *= 0.6f;

            baseOffset.Y += radsum;

            baseOffset += heightOffset;

            var anglePerStep = GetSpreadAnglePerStep(spell);

            // TODO: normalize data
            var dims = new Vector3(spell._spell.DimsOriginX ?? spell.NumProjectiles, spell._spell.DimsOriginY ?? 1, spell._spell.DimsOriginZ ?? 1);

            var i = 0;
            for (var z = 0; z < dims.Z; z++)
            {
                for (var y = 0; y < dims.Y; y++)
                {
                    var oddRow = (int)Math.Min(dims.X, spell.NumProjectiles - i) % 2 == 1;

                    for (var x = 0; x < dims.X; x++)
                    {
                        if (i >= spell.NumProjectiles)
                            break;

                        var curOffset = baseOffset;

                        if (spell.Peturbation != Vector3.Zero)
                        {
                            var rng = new Vector3((float)ThreadSafeRandom.Next(-1.0f, 1.0f), (float)ThreadSafeRandom.Next(-1.0f, 1.0f), (float)ThreadSafeRandom.Next(-1.0f, 1.0f));

                            curOffset += rng * spell.Peturbation * spell.Padding;
                        }

                        if (!oddRow && spell.SpreadAngle == 0)
                            curOffset.X += spell.Padding.X * 0.5f + radius;

                        var xFactor = spell.SpreadAngle == 0 ? oddRow ? (float)Math.Ceiling(x * 0.5f) : (float)Math.Floor(x * 0.5f) : 0;

                        var origin = curOffset + (vRadius * 2.0f + spell.Padding) * new Vector3(xFactor, y, z);

                        if (spell.SpreadAngle == 0)
                        {
                            if (x % 2 == (oddRow ? 1 : 0))
                                origin.X *= -1.0f;
                        }
                        else
                        {
                            // get the rotation matrix to apply to x
                            var numSteps = (x + 1) / 2;
                            if (x % 2 == 0)
                                numSteps *= -1;

                            //Console.WriteLine($"NumSteps: {numSteps}");

                            var curAngle = anglePerStep * numSteps;
                            var rads = curAngle.ToRadians();

                            var rot = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, rads);
                            origin = Vector3.Transform(origin, rot);
                        }

                        origins.Add(origin);
                        i++;
                    }

                    if (i >= spell.NumProjectiles)
                        break;
                }

                if (i >= spell.NumProjectiles)
                    break;
            }

            /*foreach (var origin in origins)
                Console.WriteLine(origin);*/

            return origins;
        }

        /// <summary>
        /// Returns the angle in degrees between projectiles
        /// for spells with SpreadAngle
        /// </summary>
        public static float GetSpreadAnglePerStep(Spell spell)
        {
            if (spell.SpreadAngle == 0.0f || spell.NumProjectiles == 1)
                return 0.0f;

            var numProjectiles = spell.NumProjectiles;

            if (numProjectiles % 2 == 1)
                numProjectiles--;

            return spell.SpreadAngle / numProjectiles;
        }

        public static readonly Quaternion OneEighty = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, (float)Math.PI);

        public static readonly float ProjHeightArc = 5.0f / 6.0f;

        /// <summary>
        /// Calculates the spell projectile velocity in global space
        /// </summary>
        public Vector3 CalculateProjectileVelocity(Spell spell, WorldObject target, ProjectileSpellType spellType, Vector3 origin)
        {
            var casterLoc = PhysicsObj.Position.ACEPosition();

            var speed = GetProjectileSpeed(spell);

            if (target == null && this is Creature creature && !(this is Player))
                target = creature.AttackTarget;

            if (target == null)
            {
                // launch along forward vector
                return Vector3.Transform(Vector3.UnitY, casterLoc.Rotation) * speed;
            }

            var targetLoc = target.PhysicsObj.Position.ACEPosition();

            var strikeSpell = spellType == ProjectileSpellType.Strike;

            var crossLandblock = !strikeSpell && casterLoc.Landblock != targetLoc.Landblock;

            var qDir = PhysicsObj.Position.GetOffset(target.PhysicsObj.Position);
            var rotate = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, (float)Math.Atan2(-qDir.X, qDir.Y));

            var startPos = strikeSpell ? targetLoc.Pos : crossLandblock ? casterLoc.ToGlobal(false) : casterLoc.Pos;
            startPos += Vector3.Transform(origin, strikeSpell ? rotate * OneEighty : rotate);

            var endPos = crossLandblock ? targetLoc.ToGlobal(false) : targetLoc.Pos;

            endPos.Z += target.Height * (spellType == ProjectileSpellType.Arc ? ProjHeightArc : ProjHeight);

            var dir = Vector3.Normalize(endPos - startPos);

            var targetVelocity = spell.IsTracking ? target.PhysicsObj.CachedVelocity : Vector3.Zero;

            var useGravity = spellType == ProjectileSpellType.Arc;

            var velocity = Vector3.Zero;

            if (useGravity || targetVelocity != Vector3.Zero)
            {
                var gravity = useGravity ? PhysicsGlobals.Gravity : 0.0f;

                if (!PropertyManager.GetBool("trajectory_alt_solver").Item)
                    Trajectory.solve_ballistic_arc_lateral(startPos, speed, endPos, targetVelocity, gravity, out velocity, out var time, out var impactPoint);
                else
                    velocity = Trajectory2.CalculateTrajectory(startPos, endPos, targetVelocity, speed, useGravity);

                if (velocity == Vector3.Zero && useGravity && targetVelocity != Vector3.Zero)
                {
                    // intractable?
                    // try to solve w/ zero velocity
                    if (!PropertyManager.GetBool("trajectory_alt_solver").Item)
                        Trajectory.solve_ballistic_arc_lateral(startPos, speed, endPos, Vector3.Zero, gravity, out velocity, out var time, out var impactPoint);
                    else
                        velocity = Trajectory2.CalculateTrajectory(startPos, endPos, Vector3.Zero, speed, useGravity);
                }
                if (velocity != Vector3.Zero)
                    return velocity;
            }

            return dir * speed;
        }

        public List<SpellProjectile> LaunchSpellProjectiles(Spell spell, WorldObject target, ProjectileSpellType spellType, WorldObject weapon, bool isWeaponSpell, bool fromProc, List<Vector3> origins, Vector3 velocity, uint lifeProjectileDamage = 0)
        {
            var useGravity = spellType == ProjectileSpellType.Arc;

            var strikeSpell = target != null && spellType == ProjectileSpellType.Strike;

            var spellProjectiles = new List<SpellProjectile>();

            var casterLoc = PhysicsObj.Position.ACEPosition();
            var targetLoc = target?.PhysicsObj.Position.ACEPosition();

            for (var i = 0; i < origins.Count; i++)
            {
                var origin = origins[i];

                var sp = WorldObjectFactory.CreateNewWorldObject(spell.Wcid) as SpellProjectile;

                if (sp == null)
                {
                    log.Error($"{Name} ({Guid}).LaunchSpellProjectiles({spell.Id} - {spell.Name}) - failed to create spell projectile from wcid {spell.Wcid}");
                    break;
                }

                sp.Setup(spell, spellType);

                var rotate = casterLoc.Rotation;
                if (target != null)
                {
                    var qDir = PhysicsObj.Position.GetOffset(target.PhysicsObj.Position);
                    rotate = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, (float)Math.Atan2(-qDir.X, qDir.Y));
                }

                sp.Location = strikeSpell ? new Position(targetLoc) : new Position(casterLoc);
                sp.Location.Pos += Vector3.Transform(origin, strikeSpell ? rotate * OneEighty : rotate);

                sp.PhysicsObj.Velocity = velocity;

                if (spell.SpreadAngle > 0)
                {
                    var n = Vector3.Normalize(origin);
                    var angle = Math.Atan2(-n.X, n.Y);
                    var q = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, (float)angle);
                    sp.PhysicsObj.Velocity = Vector3.Transform(velocity, q);
                }

                // set orientation
                var dir = Vector3.Normalize(sp.Velocity);
                sp.PhysicsObj.Position.Frame.set_vector_heading(dir);
                sp.Location.Rotation = sp.PhysicsObj.Position.Frame.Orientation;

                sp.ProjectileSource = this;
                sp.FromProc = fromProc;

                // side projectiles always untargeted?
                if (i == 0)
                    sp.ProjectileTarget = target;

                sp.ProjectileLauncher = weapon;
                sp.IsWeaponSpell = isWeaponSpell;

                sp.SetProjectilePhysicsState(sp.ProjectileTarget, useGravity);
                sp.SpawnPos = new Position(sp.Location);

                sp.LifeProjectileDamage = lifeProjectileDamage;

                if (!LandblockManager.AddObject(sp))
                {
                    sp.Destroy();
                    continue;
                }

                if (sp.WorldEntryCollision)
                    continue;

                sp.EnqueueBroadcast(new GameMessageScript(sp.Guid, PlayScript.Launch, sp.GetProjectileScriptIntensity(spellType)));

                if (!IsProjectileVisible(sp))
                {
                    sp.OnCollideEnvironment();
                    continue;
                }

                spellProjectiles.Add(sp);
            }

            return spellProjectiles;
        }

        public static void ClearSpellCache()
        {
            ProjectileRadiusCache.Clear();
            ProjectileSpeedCache.Clear();
        }

        public static readonly ConcurrentDictionary<uint, float> ProjectileRadiusCache = new ConcurrentDictionary<uint, float>();

        private float GetProjectileRadius(Spell spell)
        {
            var projectileWcid = spell.WeenieClassId;

            if (ProjectileRadiusCache.TryGetValue(projectileWcid, out var radius))
                return radius;

            var weenie = DatabaseManager.World.GetCachedWeenie(projectileWcid);

            if (weenie == null)
            {
                log.Error($"{Name} ({Guid}).GetSetupRadius({spell.Id} - {spell.Name}): couldn't find weenie {projectileWcid}");
                return 0.0f;
            }

            if (!weenie.PropertiesDID.TryGetValue(PropertyDataId.Setup, out var setupId))
            {
                log.Error($"{Name} ({Guid}).GetSetupRadius({spell.Id} - {spell.Name}): couldn't find setup ID for {weenie.WeenieClassId} - {weenie.ClassName}");
                return 0.0f;
            }

            var setup = DatManager.PortalDat.ReadFromDat<SetupModel>(setupId);

            if (!weenie.PropertiesFloat.TryGetValue(PropertyFloat.DefaultScale, out var scale))
                scale = 1.0f;

            var result = (float)(setup.Spheres[0].Radius * scale);

            ProjectileRadiusCache.TryAdd(projectileWcid, result);

            return result;
        }

        /// <summary>
        /// This is a temporary structure
        /// GetSpellProjectileSpeed() can easily be moved to SpellProjectile.CalculateSpeed()
        /// however the current calling pattern for Rings and Walls needs some work still..
        /// </summary>
        private static readonly ConcurrentDictionary<uint, float> ProjectileSpeedCache = new ConcurrentDictionary<uint, float>();

        /// <summary>
        /// Gets the speed of a projectile based on the distance to the target.
        /// </summary>
        private float GetProjectileSpeed(Spell spell, float? distance = null)
        {
            var projectileWcid = spell.WeenieClassId;

            if (!ProjectileSpeedCache.TryGetValue(projectileWcid, out var baseSpeed))
            {
                var weenie = DatabaseManager.World.GetCachedWeenie(projectileWcid);

                if (weenie == null)
                {
                    log.Error($"{Name} ({Guid}).GetSpellProjectileSpeed({spell.Id} - {spell.Name}, {distance}): couldn't find weenie {projectileWcid}");
                    return 0.0f;
                }

                if (!weenie.PropertiesFloat.TryGetValue(PropertyFloat.MaximumVelocity, out var maxVelocity))
                {
                    log.Error($"{Name} ({Guid}).GetSpellProjectileSpeed({spell.Id} - {spell.Name}, {distance}): couldn't find MaxVelocity for {weenie.WeenieClassId} - {weenie.ClassName}");
                    return 0.0f;
                }

                baseSpeed = (float)maxVelocity;

                ProjectileSpeedCache.TryAdd(projectileWcid, baseSpeed);
            }

            // TODO:
            // Speed seems to increase when target is moving away from the caster and decrease when
            // the target is moving toward the caster. This still needs more research.
            if (distance == null)
                return baseSpeed;

            var speed = (float)((baseSpeed * .9998363f) - (baseSpeed * .62034f) / distance +
                                   (baseSpeed * .44868f) / Math.Pow(distance.Value, 2f) - (baseSpeed * .25256f)
                                   / Math.Pow(distance.Value, 3f));

            speed = Math.Clamp(speed, 1, 50);

            return speed;
        }

        /// <summary>
        /// Returns the epic cantrips from this item's spellbook
        /// </summary>
        public Dictionary<int, float /* probability */> EpicCantrips => Biota.GetMatchingSpells(LootTables.EpicCantrips, BiotaDatabaseLock);

        /// <summary>
        /// Returns the legendary cantrips from this item's spellbook
        /// </summary>
        public Dictionary<int, float /* probability */> LegendaryCantrips => Biota.GetMatchingSpells(LootTables.LegendaryCantrips, BiotaDatabaseLock);

        private int? _maxSpellLevel;

        public int GetMaxSpellLevel()
        {
            if (_maxSpellLevel == null)
            {
                _maxSpellLevel = Biota.PropertiesSpellBook != null && Biota.PropertiesSpellBook.Count > 0 ?
                    Biota.PropertiesSpellBook.Keys.Max(i => SpellLevelCache.GetSpellLevel(i)) : 0;
            }
            return _maxSpellLevel.Value;
        }

        /// <summary>
        /// Calculates the StatModVal x buffs to enter into the enchantment registry
        /// </summary>
        /// <param name="spell">A spell with a DotDuration</param>
        public float CalculateDotEnchantment_StatModValue(Spell spell, WorldObject target, WorldObject weapon, float statModVal)
        {
            // here are all the dots with current content:

            // - 3 void dots (2 projectiles, 1 direct enchantment)
            // - surge of affliction (target loses health over time)
            // - surge of regeneration (caster gains health over time)
            // - dirty fighting bleed

            if (spell.DotDuration == 0)
                return statModVal;

            var enchantment_statModVal = statModVal;

            var creatureTarget = target as Creature;

            if (spell.Category == SpellCategory.AetheriaProcHealthOverTimeRaising)
            {
                // no healing boost rating modifier found in retail pcaps on apply,
                // could there have been one on tick?
                //if (creatureTarget != null)
                //enchantment_statModVal *= creatureTarget.GetHealingRatingMod();

                return enchantment_statModVal;
            }

            if (spell.Category == SpellCategory.AetheriaProcDamageOverTimeRaising)
            {
                // no mods found in retail pcaps
                return enchantment_statModVal;
            }

            var player = this as Player;
            var creatureSource = this as Creature;

            var damageRatingMod = 1.0f;

            if (creatureSource != null)
            {
                // damage rating mod
                var damageRating = creatureSource.GetDamageRating();

                if (player != null)
                {
                    // TODO: merge this with damage rating
                    var equippedWeapon = player.GetEquippedWeapon() ?? player.GetEquippedWand();
                    if (player.GetHeritageBonus(equippedWeapon))
                        damageRating += 5;

                    if (target is Player)
                        damageRating += player.GetPKDamageRating();
                }
                damageRatingMod = Creature.GetPositiveRatingMod(damageRating);
            }

            if (spell.Category == SpellCategory.DFBleedDamage)
            {
                // retail pcaps have modifiers in the range of 1.1x - 1.7x
                return enchantment_statModVal * damageRatingMod;
            }

            if (spell.Category != SpellCategory.NetherDamageOverTimeRaising && spell.Category != SpellCategory.NetherDamageOverTimeRaising2 && spell.Category != SpellCategory.NetherDamageOverTimeRaising3)
            {
                log.Error($"{Name}.CalculateDamageOverTimeBase({spell.Id} - {spell.Name}, {target?.Name}) - unknown dot spell category {spell.Category}");
                return enchantment_statModVal;
            }

            // factors:
            // - damage rating
            // - heritage bonus (universal masteries at end of retail, TODO: merge this with damage rating)
            // - caster damage type bonus (pvm, half for pvp)
            // - skill in magic school vs. spell difficulty (for projectiles)

            // thanks to Xenocide for figuring this part out!

            var elementalDamageMod = 1.0f;
            var skillMod = 1.0f;

            if (creatureSource != null)
            {
                // elemental damage mod
                elementalDamageMod = GetCasterElementalDamageModifier(weapon, creatureSource, creatureTarget, spell.DamageType);

                // skillMod only applied to projectiles -- no destructive curse
                if (player != null && spell.NumProjectiles > 0)
                {
                    // from SpellProjectile, slightly modified
                    // convert this to common function
                    var magicSkill = player.GetCreatureSkill(spell.School).Current;

                    if (magicSkill > spell.Power)
                    {
                        var percentageBonus = (magicSkill - spell.Power) / 1000.0f;

                        skillMod = 1.0f + percentageBonus;
                    }
                }
            }
            enchantment_statModVal *= skillMod * elementalDamageMod * damageRatingMod;

            return enchantment_statModVal;
        }

        public void TryCastItemEnchantment_WithRedirects(Spell spell, WorldObject target, WorldObject itemCaster = null)
        {
            var caster = itemCaster ?? this;

            var creature = this as Creature;
            var player = this as Player;

            var targetCreature = target as Creature;
            var targetPlayer = target as Player;

            // if negative item spell, can be resisted by the wielder
            if (spell.IsHarmful)
            {
                var targetResist = targetCreature;

                if (targetResist == null && target?.WielderId != null)
                    targetResist = CurrentLandblock?.GetObject(target.WielderId.Value) as Creature;

                // skip TryResistSpell() for non-player casters, they already performed it previously
                if (player != null && targetResist != null)
                {
                    if (TryResistSpell(targetResist, spell, caster))
                        return;
                }
                // should this be set if the spell is invalid / 'fails to affect' below?
                if (creature != null && targetResist is Player playerTargetResist)
                    playerTargetResist.SetCurrentAttacker(creature);
            }

            if (spell.IsImpenBaneType)
            {
                // impen / bane / brittlemail / lure

                // a lot of these will already be filtered out by IsInvalidTarget()
                if (targetCreature == null)
                {
                    // targeting an individual item / wo
                    HandleCastSpell(spell, target);
                }
                else
                {
                    // targeting a creature
                    if (targetPlayer == this)
                    {
                        // targeting self
                        if (creature != null)
                        {
                            var items = creature.EquippedObjects.Values.Where(i => (i.WeenieType == WeenieType.Clothing || i.IsShield) && i.IsEnchantable).ToList();

                            foreach (var item in items)
                                HandleCastSpell(spell, item);

                            if (items.Count > 0)
                                DoSpellEffects(spell, this, creature);
                        }
                    }
                    else
                    {
                        // targeting another player or monster
                        var item = targetCreature.EquippedObjects.Values.FirstOrDefault(i => i.IsShield && i.IsEnchantable);

                        if (item != null)
                        {
                            HandleCastSpell(spell, item);
                        }
                        else
                        {
                            // 'fails to affect'?
                            if (player != null && targetCreature != null)
                                player.Session.Network.EnqueueSend(new GameMessageSystemChat($"You fail to affect {targetCreature.Name} with {spell.Name}", ChatMessageType.Magic));

                            if (targetPlayer != null && !targetPlayer.SquelchManager.Squelches.Contains(this, ChatMessageType.Magic))
                                targetPlayer.Session.Network.EnqueueSend(new GameMessageSystemChat($"{Name} fails to affect you with {spell.Name}", ChatMessageType.Magic));
                        }
                    }
                }
            }
            else if (spell.IsOtherNegativeRedirectable || spell.IsItemRedirectableType)
            {
                // blood loather, spirit loather, lure blade, turn blade, leaden weapon, hermetic void
                if (targetCreature == null)
                {
                    // targeting an individual item / wo
                    HandleCastSpell(spell, target);
                }
                else
                {
                    // targeting a creature, try to redirect to primary weapon
                    var weapon = spell.NonComponentTargetType switch
                    {
                        ItemType.Weapon => targetCreature.GetEquippedWeapon(),
                        ItemType.Caster => targetCreature.GetEquippedWand(),
                        ItemType.WeaponOrCaster => targetCreature.GetEquippedWeapon() ?? targetCreature.GetEquippedWand(),
                        ItemType.MeleeWeapon => targetCreature.GetEquippedMeleeWeapon(),
                        ItemType.MissileWeapon => targetCreature.GetEquippedMissileWeapon(),
                        _ => null
                    };

                    if (weapon != null && weapon.IsEnchantable)
                    {
                        HandleCastSpell(spell, weapon);
                    }
                    else
                    {
                        // 'fails to affect'?
                        if (player != null)
                            player.Session.Network.EnqueueSend(new GameMessageSystemChat($"You fail to affect {targetCreature.Name} with {spell.Name}", ChatMessageType.Magic));

                        if (targetPlayer != null && !targetPlayer.SquelchManager.Squelches.Contains(this, ChatMessageType.Magic))
                            targetPlayer.Session.Network.EnqueueSend(new GameMessageSystemChat($"{Name} fails to affect you with {spell.Name}", ChatMessageType.Magic));
                    }
                }
            }
            else
            {
                // all other item spells, cast directly on target
                HandleCastSpell(spell, target);
            }
        }

        public float ItemManaRateAccumulator { get; set; }

        public bool ItemManaDepletionMessage { get; set; }

        public void OnSpellsActivated()
        {
            IsAffecting = true;
            ItemManaRateAccumulator = 0;
            ItemManaDepletionMessage = false;
        }

        public void OnSpellsDeactivated()
        {
            IsAffecting = false;
        }

        private static readonly double defaultIgnoreSomeMagicProjectileDamage = 0.25;

        public double? GetAbsorbMagicDamage()
        {
            var absorbMagicDamage = AbsorbMagicDamage;

            if (absorbMagicDamage == null && HasImbuedEffect(ImbuedEffectType.IgnoreSomeMagicProjectileDamage))
                absorbMagicDamage = defaultIgnoreSomeMagicProjectileDamage;

            return absorbMagicDamage;
        }

        /// <summary>
        /// For spells with NonComponentTargetType, returns the list of equipped items matching the target type
        /// </summary>
        private static List<WorldObject> GetNonComponentTargetTypes(Spell spell, Creature target)
        {
            switch (spell.NonComponentTargetType)
            {
                case ItemType.Vestements:               // impen / bane
                case ItemType.Weapon:                   // blood drinker
                case ItemType.LockableMagicTarget:      // strengthen lock
                case ItemType.Caster:                   // hermetic void
                case ItemType.WeaponOrCaster:           // lure blade, defender cantrip, hermetic link cantrip, mukkir sense
                case ItemType.Item:                     // essence lull

                    return target.EquippedObjects.Values.Where(i => (i.ItemType & spell.NonComponentTargetType) != 0 && (i.ValidLocations & EquipMask.Selectable) != 0 && i.IsEnchantable).ToList();
            }
            return null;
        }
    }
}
