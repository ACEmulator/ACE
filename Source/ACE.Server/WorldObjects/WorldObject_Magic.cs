using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

using ACE.Database;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Structure;
using ACE.Server.Managers;
using ACE.Server.Entity.Actions;
using ACE.Server.Factories;
using ACE.Server.Physics;

namespace ACE.Server.WorldObjects
{
    partial class WorldObject
    {
        public struct EnchantmentStatus
        {
            public StackType stackType;
            public GameMessageSystemChat message;
        }

        /// <summary>
        /// Instantly casts a spell for a WorldObject (ie. spell traps)
        /// </summary>
        public void TryCastSpell(Spell spell, WorldObject target)
        {
            // spells only castable on creatures?
            var targetCreature = target as Creature;
            if (targetCreature == null)
                return;

            // perform resistance check, if applicable
            var resisted = TryResistSpell(spell, target);
            if (resisted)
                return;

            // if not resisted, cast spell
            var status = default(EnchantmentStatus);
            switch (spell.School)
            {
                case MagicSchool.WarMagic:
                    WarMagic(target, spell);
                    break;
                case MagicSchool.LifeMagic:
                    LifeMagic(target, spell, out uint damage, out bool critical, out status);
                    break;
                case MagicSchool.CreatureEnchantment:
                    status = CreatureMagic(target, spell);
                    break;
                case MagicSchool.ItemEnchantment:
                    status = ItemMagic(target, spell);
                    break;
                case MagicSchool.VoidMagic:
                    VoidMagic(target, spell);
                    break;
            }

            // send message to player, if applicable
            var player = this as Player;
            if (player != null && status.message != null)
                player.Session.Network.EnqueueSend(status.message);

            // for invisible spell traps,
            // their effects won't be seen if they broadcast from themselves
            if (spell.TargetEffect != 0)
                target.EnqueueBroadcast(new GameMessageScript(target.Guid, spell.TargetEffect, spell.Formula.Scale));
        }

        /// <summary>
        /// If this spell has a chance to be resisted, rolls for a chance
        /// Returns TRUE if spell is resistable and was resisted for this attempt
        /// </summary>
        public bool TryResistSpell(Spell spell, WorldObject target)
        {
            if (spell.IsBeneficial)
                return false;

            // todo: verify this flag exists for all resistable spells
            //if (!spell.Flags.HasFlag(SpellFlags.Resistable))
                //return false;

            var targetSelf = spell.Flags.HasFlag(SpellFlags.SelfTargeted);
            if (targetSelf) return false;

            switch (spell.School)
            {
                case MagicSchool.WarMagic:
                    // war magic projectiles do the resistance check on projectile collision
                    break;
                case MagicSchool.LifeMagic:
                case MagicSchool.CreatureEnchantment:
                    if (spell.NumProjectiles == 0)  // life magic projectiles
                    {
                        bool? resisted = ResistSpell(target, spell);
                        if (resisted != null && resisted == true)
                            return true;
                    }
                    break;
                case MagicSchool.ItemEnchantment:
                    // ??
                    break;
                case MagicSchool.VoidMagic:
                    if (spell.NumProjectiles == 0)  // void magic projectiles
                    {
                        bool? resisted = ResistSpell(target, spell);
                        if (resisted != null && resisted == true)
                            return true;
                    }
                    break;
            }
            return false;
        }

        /// <summary>
        /// Determine Player's PK status and whether it matches the target Player
        /// </summary>
        /// <returns>
        /// Returns NULL if either player are target are null
        /// Returns TRUE if player should be allowed to cast the spell on target player
        /// Returns FALSE if player shouldn't be allowed to cast the spell on target player
        /// </returns>
        protected bool? CheckPKStatusVsTarget(Player player, Player target, Spell spell)
        {
            if (player == null || target == null)
                return null;

            if (player == target)
                return true;

            if (spell == null || spell.IsHarmful)
            {
                // Ensure that a non-PK cannot cast harmful spells on another player
                if (player.PlayerKillerStatus == PlayerKillerStatus.NPK)
                    return false;

                // Ensure that a harmful spell isn't being cast on another player that doesn't have the same PK status
                if (player.PlayerKillerStatus != target.PlayerKillerStatus)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Determines whether the target for the spell being cast is invalid
        /// </summary>
        protected bool IsInvalidTarget(Spell spell, WorldObject target)
        {
            var targetPlayer = target as Player;
            var targetCreature = target as Creature;

            // Self targeted spells should have a target of self
            if ((int)Math.Floor(spell.BaseRangeConstant) == 0 && targetPlayer == null)
                return true;

            // Invalidate non Item Enchantment spells cast against non Creatures or Players
            if (spell.School != MagicSchool.ItemEnchantment && targetCreature == null)
                return true;

            // Invalidate beneficial spells against Creature/Non-player targets
            if (targetCreature != null && targetPlayer == null && spell.IsBeneficial)
                return true;

            // Cannot cast Weapon Aura spells on targets that are not players or creatures
            if ((spell.Name.Contains("Aura of")) && (spell.School == MagicSchool.ItemEnchantment))
            {
                if (targetCreature == null)
                    return true;
            }

            // Cannot cast Weapon Aura spells on targets that are not players or creatures
            if ((spell.MetaSpellType == SpellType.Enchantment) && (spell.School == MagicSchool.ItemEnchantment))
            {
                if (targetPlayer != null
                    || (target.WeenieType == WeenieType.Creature)
                    || (target.WeenieType == WeenieType.Clothing)
                    || (target.WeenieType == WeenieType.Caster)
                    || (target.WeenieType == WeenieType.MeleeWeapon)
                    || (target.WeenieType == WeenieType.MissileLauncher)
                    || (target.WeenieType == WeenieType.Missile)
                    || (target.WeenieType == WeenieType.Door)
                    || (target.WeenieType == WeenieType.Chest)
                    || (target.CombatUse != null && target.CombatUse == ACE.Entity.Enum.CombatUse.Shield))
                    return false;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether a spell will be resisted,
        /// based upon the caster's magic skill vs target's magic defense skill
        /// </summary>
        /// <returns>TRUE if spell is resisted</returns>
        public static bool MagicDefenseCheck(uint casterMagicSkill, uint targetMagicDefenseSkill)
        {
            // uses regular 0.03 factor, and not magic casting 0.07 factor
            var chance = SkillCheck.GetSkillChance((int)casterMagicSkill, (int)targetMagicDefenseSkill);
            var rng = Physics.Common.Random.RollDice(0.0f, 1.0f);

            return chance <= rng;
        }

        /// <summary>
        /// Performs the magic defense checks for spell attacks
        /// </summary>
        public bool? ResistSpell(WorldObject target, Spell spell)
        {
            uint magicSkill = 0;
            var caster = this as Creature;
            if (caster != null)
                // Retrieve caster's skill level in the Magic School
                magicSkill = caster.GetCreatureSkill(spell.School).Current;
            else
                // Retrieve casting item's spellcraft
                magicSkill = (uint)ItemSpellcraft;

            var player = caster as Player;
            var targetPlayer = target as Player;

            // only creatures can resist spells?
            var creature = target as Creature;
            if (creature == null) return null;

            // Retrieve target's Magic Defense Skill
            var targetMagicDefenseSkill = creature.GetCreatureSkill(Skill.MagicDefense).Current;

            //Console.WriteLine($"{target.Name}.ResistSpell({Name}, {spell.Name}): magicSkill: {magicSkill}, difficulty: {targetMagicDefenseSkill}");
            bool resisted = MagicDefenseCheck(magicSkill, targetMagicDefenseSkill);

            if (targetPlayer != null && targetPlayer.Invincible == true)
                resisted = true;

            if (resisted)
            {
                if (player != null)
                {
                    player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{creature.Name} resists {spell.Name}", ChatMessageType.Magic));
                    player.Session.Network.EnqueueSend(new GameMessageSound(player.Guid, Sound.ResistSpell, 1.0f));
                }
                if (targetPlayer != null)
                {
                    targetPlayer.Session.Network.EnqueueSend(new GameMessageSystemChat($"You resist the spell cast by {caster.Name}", ChatMessageType.Magic));
                    targetPlayer.Session.Network.EnqueueSend(new GameMessageSound(targetPlayer.Guid, Sound.ResistSpell, 1.0f));

                    Proficiency.OnSuccessUse(targetPlayer, targetPlayer.GetCreatureSkill(Skill.MagicDefense), magicSkill);
                }
                return resisted;
            }
            return resisted;
        }

        /// <summary>
        /// Launches a Life Magic spell
        /// </summary>
        protected bool LifeMagic(WorldObject target, Spell spell, out uint damage, out bool critical, out EnchantmentStatus enchantmentStatus, WorldObject itemCaster = null)
        {
            critical = false;
            string srcVital, destVital;
            enchantmentStatus = default(EnchantmentStatus);
            enchantmentStatus.stackType = StackType.None;
            GameMessageSystemChat targetMsg = null;

            var player = this as Player;
            var creature = this as Creature;

            var spellTarget = spell.BaseRangeConstant > 0 ? target as Creature : creature;

            if (!spellTarget.IsAlive)
            {
                enchantmentStatus.message = null;
                damage = 0;
                return false;
            }

            switch (spell.MetaSpellType)
            {
                case SpellType.Boost:

                    // handle negatives?
                    int minBoostValue = Math.Min(spell.Boost, spell.MaxBoost);
                    int maxBoostValue = Math.Max(spell.Boost, spell.MaxBoost);

                    int tryBoost = Physics.Common.Random.RollDice(minBoostValue, maxBoostValue);
                    int boost = tryBoost;
                    damage = tryBoost < 0 ? (uint)Math.Abs(tryBoost) : 0;

                    switch (spell.VitalDamageType)
                    {
                        case DamageType.Mana:
                            boost = spellTarget.UpdateVitalDelta(spellTarget.Mana, tryBoost);
                            srcVital = "mana";
                            break;
                        case DamageType.Stamina:
                            boost = spellTarget.UpdateVitalDelta(spellTarget.Stamina, tryBoost);
                            srcVital = "stamina";
                            break;
                        default:   // Health
                            boost = spellTarget.UpdateVitalDelta(spellTarget.Health, tryBoost);
                            srcVital = "health";

                            if (boost >= 0)
                                spellTarget.DamageHistory.OnHeal((uint)boost);
                            else
                                spellTarget.DamageHistory.Add(this, DamageType.Health, damage);
                            break;
                    }

                    if (player != null)
                    {
                        if (spell.BaseRangeConstant > 0)
                        {
                            string msg;
                            if (spell.IsBeneficial)
                            {
                                msg = $"You cast {spell.Name} and restore {boost} points of {srcVital} to {spellTarget.Name}.";
                                enchantmentStatus.message = new GameMessageSystemChat(msg, ChatMessageType.Magic);
                            }
                            else
                            {
                                msg = $"You cast {spell.Name} and drain {Math.Abs(boost)} points of {srcVital} from {spellTarget.Name}.";
                                enchantmentStatus.message = new GameMessageSystemChat(msg, ChatMessageType.Combat);
                            }
                        }
                        else
                        {
                            var verb = spell.IsBeneficial ? "restore" : "drain";
                            enchantmentStatus.message = new GameMessageSystemChat($"You cast {spell.Name} and {verb} {Math.Abs(boost)} points of your {srcVital}.", ChatMessageType.Magic);
                        }
                    }
                    else
                        enchantmentStatus.message = null;

                    if (target is Player && spell.BaseRangeConstant > 0)
                    {
                        string msg;
                        if (spell.IsBeneficial)
                        {
                            msg = $"{Name} casts {spell.Name} and restores {boost} points of your {srcVital}.";
                            targetMsg = new GameMessageSystemChat(msg, ChatMessageType.Magic);
                        }
                        else
                        {
                            msg = $"{Name} casts {spell.Name} and drains {Math.Abs(boost)} points of your {srcVital}.";
                            targetMsg = new GameMessageSystemChat(msg, ChatMessageType.Combat);
                        }
                    }

                    if (player != null && srcVital != null && srcVital.Equals("health"))
                        player.Session.Network.EnqueueSend(new GameEventUpdateHealth(player.Session, target.Guid.Full, (float)spellTarget.Health.Current / spellTarget.Health.MaxValue));

                    break;

                case SpellType.Transfer:

                    // source and destination can be the same creature, or different creatures
                    var caster = this as Creature;
                    var source = spell.TransferFlags.HasFlag(TransferFlags.CasterSource) ? caster : spellTarget;
                    var destination = spell.TransferFlags.HasFlag(TransferFlags.CasterDestination) ? caster : spellTarget;

                    // Calculate vital changes
                    uint srcVitalChange, destVitalChange;
                    ResistanceType resistanceDrain, resistanceBoost;
                    resistanceDrain = GetDrainResistanceType(spell.Source);

                    // should drain resistance be taken into account here,
                    // or only after the destVitalChange calc?
                    srcVitalChange = (uint)Math.Round(source.GetCurrentCreatureVital(spell.Source) * spell.Proportion * source.GetNaturalResistance(resistanceDrain));

                    if (spell.TransferCap != 0)
                    {
                        if (srcVitalChange > spell.TransferCap)
                            srcVitalChange = (uint)spell.TransferCap;
                    }
                    resistanceBoost = GetBoostResistanceType(spell.Destination);
                    destVitalChange = (uint)Math.Round(srcVitalChange * (1.0f - spell.LossPercent) * destination.GetNaturalResistance(resistanceBoost));

                    // scale srcVitalChange to destVitalChange?

                    // Apply the change in vitals to the source
                    switch (spell.Source)
                    {
                        case PropertyAttribute2nd.Mana:
                            srcVital = "mana";
                            srcVitalChange = (uint)-source.UpdateVitalDelta(source.Mana, -(int)srcVitalChange);
                            break;
                        case PropertyAttribute2nd.Stamina:
                            srcVital = "stamina";
                            srcVitalChange = (uint)-source.UpdateVitalDelta(source.Stamina, -(int)srcVitalChange);
                            break;
                        default:   // Health
                            srcVital = "health";
                            srcVitalChange = (uint)-source.UpdateVitalDelta(source.Health, -(int)srcVitalChange);

                            source.DamageHistory.Add(this, DamageType.Health, srcVitalChange);
                            break;
                    }
                    damage = srcVitalChange;

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

                    var playerSource = source is Player;
                    var playerDestination = destination is Player;

                    if (playerSource && playerDestination && source.Guid == destination.Guid)
                    {
                        enchantmentStatus.message = new GameMessageSystemChat($"You cast {spell.Name} on yourself and lose {srcVitalChange} points of {srcVital} and also gain {destVitalChange} points of {destVital}", ChatMessageType.Magic);
                    }
                    else
                    {
                        if (playerSource)
                        {
                            if (source == this)
                                enchantmentStatus.message = new GameMessageSystemChat($"You lose {srcVitalChange} points of {srcVital} due to casting {spell.Name} on {spellTarget.Name}", ChatMessageType.Magic);
                            else
                                targetMsg = new GameMessageSystemChat($"You lose {srcVitalChange} points of {srcVital} due to {caster.Name} casting {spell.Name} on you", ChatMessageType.Magic);
                        }

                        if (playerDestination)
                        {
                            if (destination == this)
                                enchantmentStatus.message = new GameMessageSystemChat($"You gain {destVitalChange} points of {destVital} due to casting {spell.Name} on {spellTarget.Name}", ChatMessageType.Magic);
                            else
                                targetMsg = new GameMessageSystemChat($"You gain {destVitalChange} points of {destVital} due to {caster.Name} casting {spell.Name} on you", ChatMessageType.Magic);
                        }
                    }

                    if (player != null && srcVital != null && srcVital.Equals("health"))
                        player.Session.Network.EnqueueSend(new GameEventUpdateHealth(player.Session, target.Guid.Full, (float)spellTarget.Health.Current / spellTarget.Health.MaxValue));

                    break;

                case SpellType.LifeProjectile:

                    caster = this as Creature;
                    var damageType = DamageType.Undef;

                    if (spell.Name.Contains("Blight"))
                    {
                        var tryDamage = (int)Math.Round(caster.GetCurrentCreatureVital(PropertyAttribute2nd.Mana) * spell.DrainPercentage / caster.GetNaturalResistance(ResistanceType.ManaDrain));
                        damage = (uint)-caster.UpdateVitalDelta(caster.Mana, -tryDamage);
                        damageType = DamageType.Mana;
                    }
                    else if (spell.Name.Contains("Tenacity"))
                    {
                        var tryDamage = (int)Math.Round(caster.GetCurrentCreatureVital(PropertyAttribute2nd.Stamina) * spell.DrainPercentage / caster.GetNaturalResistance(ResistanceType.StaminaDrain));
                        damage = (uint)-caster.UpdateVitalDelta(caster.Stamina, -tryDamage);
                        damageType = DamageType.Stamina;
                    }
                    else
                    {
                        var tryDamage = (int)Math.Round(caster.GetCurrentCreatureVital(PropertyAttribute2nd.Health) * spell.DrainPercentage / caster.GetNaturalResistance(ResistanceType.HealthDrain));
                        damage = (uint)-caster.UpdateVitalDelta(caster.Health, -tryDamage);
                        caster.DamageHistory.Add(this, DamageType.Health, damage);
                        damageType = DamageType.Health;
                    }

                    var sp = CreateSpellProjectile(spell, target, damage);
                    LaunchSpellProjectile(sp);

                    if (caster.Health.Current <= 0)
                    {
                        // should this be possible?
                        caster.OnDeath(caster, damageType, false);
                        caster.Die();
                    }

                    enchantmentStatus.message = null;
                    break;

                case SpellType.Dispel:

                    var removeSpells = target.EnchantmentManager.SelectDispel(spell);

                    // dispel on server and client
                    target.EnchantmentManager.Dispel(removeSpells.Select(s => s.Enchantment).ToList());

                    var spellList = BuildSpellList(removeSpells);
                    var suffix = "";
                    if (removeSpells.Count > 0)
                        suffix = $" and dispel: {spellList}.";
                    else
                        suffix = ", but the dispel fails.";

                    damage = 0;
                    if (player != null)
                    {
                        if (player == target)
                            enchantmentStatus.message = new GameMessageSystemChat($"You cast {spell.Name} on yourself{suffix}", ChatMessageType.Magic);
                        else
                            enchantmentStatus.message = new GameMessageSystemChat($"You cast {spell.Name} on {target.Name}{suffix}", ChatMessageType.Magic);
                    }
                    var targetPlayer = target as Player;
                    if (targetPlayer != null && targetPlayer != player)
                    {
                        targetMsg = new GameMessageSystemChat($"{Name} casts {spell.Name} on you{suffix.Replace("and dispel", "and dispels")}", ChatMessageType.Magic);
                    }
                    break;

                case SpellType.Enchantment:
                    damage = 0;
                    if (itemCaster != null)
                        enchantmentStatus = CreateEnchantment(target, itemCaster, spell);
                    else
                        enchantmentStatus = CreateEnchantment(target, this, spell);
                    break;

                default:
                    damage = 0;
                    enchantmentStatus.message = new GameMessageSystemChat("Spell not implemented, yet!", ChatMessageType.Magic);
                    break;
            }

            if (targetMsg != null)
                (target as Player).Session.Network.EnqueueSend(targetMsg);

            if (spellTarget.Health.Current == 0)
                return true;

            return false;
        }

        /// <summary>
        /// Returns a string with the spell list format as:
        /// Spell Name 1, Spell Name 2, and Spell Name 3
        /// </summary>
        public string BuildSpellList(List<SpellEnchantment> spells)
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
        /// Wrapper around CreateEnchantment for Creature Magic
        /// </summary>
        protected EnchantmentStatus CreatureMagic(WorldObject target, Spell spell, WorldObject itemCaster = null)
        {
            // redirect creature dispels to life magic
            if (spell.MetaSpellType == SpellType.Dispel)
            {
                LifeMagic(target, spell, out uint damage, out bool critical, out EnchantmentStatus enchantmentStatus);
                return enchantmentStatus;
            }

            if (itemCaster != null)
                return CreateEnchantment(target, itemCaster, spell);

            return CreateEnchantment(target, this, spell);
        }

        /// <summary>
        /// Handles casting Item Magic spells
        /// </summary>
        protected EnchantmentStatus ItemMagic(WorldObject target, Spell spell, WorldObject itemCaster = null)
        {
            EnchantmentStatus enchantmentStatus = default(EnchantmentStatus);
            enchantmentStatus.message = null;
            enchantmentStatus.stackType = StackType.None;

            // redirect item dispels to life magic
            if (spell.MetaSpellType == SpellType.Dispel)
            {
                LifeMagic(target, spell, out uint damage, out bool critical, out enchantmentStatus);
                return enchantmentStatus;
            }

            var creature = this as Creature;
            var player = this as Player;

            if ((spell.MetaSpellType == SpellType.PortalLink)
                || (spell.MetaSpellType == SpellType.PortalRecall)
                || (spell.MetaSpellType == SpellType.PortalSending)
                || (spell.MetaSpellType == SpellType.PortalSummon))
            {
                var targetPlayer = target as Player;

                switch (spell.MetaSpellType)
                {
                    case SpellType.PortalRecall:
                        PositionType recall = PositionType.Undef;
                        switch ((Network.Enum.Spell)spell.Id)
                        {
                            case Network.Enum.Spell.PortalRecall:       // portal recall
                                if (player.GetPosition(PositionType.LastPortal) == null)
                                {
                                    // You must link to a portal to recall it!
                                    player.Session.Network.EnqueueSend(new GameEventWeenieError(player.Session, WeenieError.YouMustLinkToPortalToRecall));
                                }
                                else
                                    recall = PositionType.LastPortal;
                                break;
                            case Network.Enum.Spell.LifestoneRecall1:   // lifestone recall
                                if (player.GetPosition(PositionType.LinkedLifestone) == null)
                                {
                                    // You must link to a lifestone to recall it!
                                    player.Session.Network.EnqueueSend(new GameEventWeenieError(player.Session, WeenieError.YouMustLinkToLifestoneToRecall));
                                }
                                else
                                    recall = PositionType.LinkedLifestone;
                                break;
                            case Network.Enum.Spell.PortalTieRecall1:   // primary portal tie recall
                                if (player.GetPosition(PositionType.LinkedPortalOne) == null)
                                {
                                    // You must link to a portal to recall it!
                                    player.Session.Network.EnqueueSend(new GameEventWeenieError(player.Session, WeenieError.YouMustLinkToPortalToRecall));
                                }
                                else
                                    recall = PositionType.LinkedPortalOne;
                                break;
                            case Network.Enum.Spell.PortalTieRecall2:   // secondary portal tie recall
                                if (player.GetPosition(PositionType.LinkedPortalTwo) == null)
                                {
                                    // You must link to a portal to recall it!
                                    player.Session.Network.EnqueueSend(new GameEventWeenieError(player.Session, WeenieError.YouMustLinkToPortalToRecall));
                                }
                                else
                                    recall = PositionType.LinkedPortalTwo;
                                break;
                        }

                        if (recall != PositionType.Undef)
                        {
                            ActionChain portalRecallChain = new ActionChain();
                            portalRecallChain.AddDelaySeconds(2.0f);  // 2 second delay
                            portalRecallChain.AddAction(targetPlayer, () => player.TeleToPosition(recall));
                            portalRecallChain.EnqueueChain();
                        }
                        break;
                    case SpellType.PortalSending:
                        if (targetPlayer != null)
                        {
                            ActionChain portalSendingChain = new ActionChain();
                            portalSendingChain.AddDelaySeconds(2.0f);  // 2 second delay
                            portalSendingChain.AddAction(targetPlayer, () => targetPlayer.Teleport(spell.Position));
                            portalSendingChain.EnqueueChain();
                        }
                        break;
                    case SpellType.PortalLink:
                        if (player != null)
                        {
                            switch ((Network.Enum.Spell)spell.Id)
                            {
                                case Network.Enum.Spell.PortalTie1:    // Primary Portal Tie
                                    if (target.WeenieType == WeenieType.Portal)
                                    {
                                        var targetPortal = target as Portal;
                                        if (!targetPortal.NoTie)
                                            player.LinkedPortalOne = targetPortal.Destination;
                                        else
                                            player.Session.Network.EnqueueSend(new GameEventWeenieError(player.Session, WeenieError.YouCannotLinkToThatPortal));
                                    }
                                    else
                                        player.Session.Network.EnqueueSend(new GameEventCommunicationTransientString(player.Session, $"Primary Portal Tie cannot be cast on {target.Name}"));
                                    break;
                                case Network.Enum.Spell.LifestoneTie1:  // Lifestone Tie
                                    if (target.WeenieType == WeenieType.LifeStone)
                                        player.LinkedLifestone = target.Location;
                                    else
                                        player.Session.Network.EnqueueSend(new GameEventCommunicationTransientString(player.Session, $"Lifestone Tie cannot be cast on {target.Name}"));
                                    break;
                                case Network.Enum.Spell.PortalTie2:  // Secondary Portal Tie
                                    if (target.WeenieType == WeenieType.Portal)
                                    {
                                        var targetPortal = target as Portal;
                                        if (!targetPortal.NoTie)
                                            player.LinkedPortalTwo = targetPortal.Destination;
                                        else
                                            player.Session.Network.EnqueueSend(new GameEventWeenieError(player.Session, WeenieError.YouCannotLinkToThatPortal));
                                    }
                                    else
                                        player.Session.Network.EnqueueSend(new GameEventCommunicationTransientString(player.Session, $"Secondary Portal Tie cannot be cast on {target.Name}"));
                                    break;
                            }
                        }
                        break;
                    case SpellType.PortalSummon:
                        uint portalId = 0;
                        Position destination = null;

                        if (player == null)
                            portalId = LinkedPortalOneDID ?? 0;
                        else if (itemCaster != null)
                            portalId = itemCaster.LinkedPortalOneDID ?? 0;
                        else
                        {
                            if (spell.Name.Contains("Summon Primary"))
                            {
                                destination = GetPosition(PositionType.LinkedPortalOne);
                            }
                            if (spell.Name.Contains("Summon Secondary"))
                            {
                                destination = GetPosition(PositionType.LinkedPortalTwo);
                            }

                            if (destination != null)
                                portalId = 1955;
                        }

                        if (portalId == 0)
                        {
                            // You must link to a portal to summon it!
                            player.Session.Network.EnqueueSend(new GameEventWeenieError(player.Session, WeenieError.YouMustLinkToPortalToSummonIt));
                            break;
                        }

                        if (destination != null)
                            SummonPortal(portalId, destination, spell.PortalLifetime);
                        else
                            SummonPortal(portalId, spell.PortalLifetime);

                        break;
                    case SpellType.FellowPortalSending:
                        if (targetPlayer != null)
                            enchantmentStatus.message = new GameMessageSystemChat("Spell not implemented, yet!", ChatMessageType.Magic);
                        break;
                }
            }
            else if (spell.MetaSpellType == SpellType.Enchantment)
            {
                if (itemCaster != null)
                    return CreateEnchantment(target, itemCaster, spell);

                return CreateEnchantment(target, this, spell);
            }

            return enchantmentStatus;
        }

        /// <summary>
        /// Spawns a player-summoned portal from item magic or gems
        /// </summary>
        protected void SummonPortal(uint portalId, Position destination, double portalLifetime)
        {
            var portal = WorldObjectFactory.CreateNewWorldObject(portalId);
            portal.SetupTableId = 33556212;
            portal.RadarBehavior = ACE.Entity.Enum.RadarBehavior.ShowNever;
            portal.Name = "Gateway";
            portal.Location = Location.InFrontOf(3.0f);

            if (portalId == 1955)
                portal.Destination = destination;

            portal.EnterWorld();

            // Create portal decay
            ActionChain despawnChain = new ActionChain();
            despawnChain.AddDelaySeconds(portalLifetime);
            despawnChain.AddAction(portal, () => portal.CurrentLandblock?.RemoveWorldObject(portal.Guid, false));
            despawnChain.EnqueueChain();
        }

        /// <summary>
        /// Spawns a time-based portal from a portal weenie id
        /// </summary>
        protected void SummonPortal(uint wcid, double portalLifetime = 60.0f)   // default?
        {
            var weenie = DatabaseManager.World.GetCachedWeenie(wcid);
            var portal = WorldObjectFactory.CreateNewWorldObject(weenie);
            if (portal == null) return;

            portal.Location = new Position(Location);

            portal.EnterWorld();

            // queue for destruction
            var despawnChain = new ActionChain();
            despawnChain.AddDelaySeconds(portalLifetime);
            //despawnChain.AddAction(portal, () => portal.Destroy());     // smooth fade-out doesn't work for portals?
            despawnChain.AddAction(portal, () => portal.CurrentLandblock?.RemoveWorldObject(portal.Guid, false));
            despawnChain.EnqueueChain();
        }

        /// <summary>
        /// Launches a War Magic spell projectile (untargeted)
        /// </summary>
        public void WarMagic(Spell spell)
        {
            var spellType = SpellProjectile.GetProjectileSpellType(spell.Id);

            if (spellType == SpellProjectile.ProjectileSpellType.Ring)
            {
                var spellProjectiles = CreateRingProjectiles(spell);
                LaunchSpellProjectiles(spellProjectiles);
            }
            else if (spellType == SpellProjectile.ProjectileSpellType.Wall)
            {
                var spellProjectiles = CreateWallProjectiles(spell);
                LaunchSpellProjectiles(spellProjectiles);
            }
            else
            {
                var player = this as Player;
                if (player != null)
                {
                    player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, errorType: WeenieError.None),
                        new GameMessageSystemChat($"{spell.Name} spell not implemented, yet!", ChatMessageType.System));
                }
            }
        }

        /// <summary>
        /// Launches a targeted War Magic spell projectile
        /// </summary>
        protected void WarMagic(WorldObject target, Spell spell)
        {
            var spellType = SpellProjectile.GetProjectileSpellType(spell.Id);
            // Bolt, Streak, Arc
            if (spell.NumProjectiles == 1)
            {
                var sp = CreateSpellProjectile(spell, target);
                LaunchSpellProjectile(sp);
            }
            else if (spellType == SpellProjectile.ProjectileSpellType.Volley)
            {
                var spellProjectiles = CreateVolleyProjectiles(target, spell);
                LaunchSpellProjectiles(spellProjectiles);
            }
            else if (spellType == SpellProjectile.ProjectileSpellType.Blast)
            {
                var spellProjectiles = CreateBlastProjectiles(target, spell);
                LaunchSpellProjectiles(spellProjectiles);
            }
            else
            {
                var player = this as Player;
                if (player != null)
                {
                    player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, errorType: WeenieError.None),
                        new GameMessageSystemChat($"{spell.Name} spell not implemented, yet!", ChatMessageType.System));
                }
            }
        }

        /// <summary>
        /// Launches a Void Magic spell attack
        /// </summary>
        protected void VoidMagic(WorldObject target, Spell spell)
        {
            if (spell.NumProjectiles > 0)
                VoidMagicProjectile(target, spell);
            else
                // curses - apply with code similar to creature/life magic?
                TryApplyEnchantment(target, spell);
        }

        /// <summary>
        /// Attempts to apply an enchantment (added for Void Magic)
        /// </summary>
        protected bool TryApplyEnchantment(WorldObject target, Spell spell)
        {
            var player = this as Player;
            var targetPlayer = target as Player;
            var targetCreature = target as Creature;
            if (player != null && targetCreature != null && targetPlayer == null)
                player.OnAttackMonster(targetCreature);

            if (spell.IsHarmful)
            {
                var resisted = ResistSpell(target, spell);
                if (resisted == true)
                    return false;
                if (resisted == null)
                {
                    log.Error("Something went wrong with the Magic resistance check");
                    return false;
                }
            }

            EnqueueBroadcast(new GameMessageScript(target.Guid, (PlayScript)spell.TargetEffect, spell.Formula.Scale));
            var enchantmentStatus = CreatureMagic(target, spell);
            if (enchantmentStatus.message != null)
                player.Session.Network.EnqueueSend(enchantmentStatus.message);

            var difficulty = spell.Power;
            var difficultyMod = Math.Max(difficulty, 25);   // fix difficulty for level 1 spells?

            if (spell.IsHarmful)
                Proficiency.OnSuccessUse(player, player.GetCreatureSkill(Skill.CreatureEnchantment), (target as Creature).GetCreatureSkill(Skill.MagicDefense).Current);
            else
                Proficiency.OnSuccessUse(player, player.GetCreatureSkill(Skill.CreatureEnchantment), difficultyMod);

            return true;
        }

        /// <summary>
        /// Launches a Void Magic spell projectile
        /// </summary>
        protected void VoidMagicProjectile(WorldObject target, Spell spell)
        {
            // starting with same logic from war magic..

            var spellType = SpellProjectile.GetProjectileSpellType(spell.Id);
            // Bolt, Streak, Arc
            if (spell.NumProjectiles == 1)
            {
                var sp = CreateSpellProjectile(spell, target);
                LaunchSpellProjectile(sp);
            }
            else if (spellType == SpellProjectile.ProjectileSpellType.Volley)
            {
                var spellProjectiles = CreateVolleyProjectiles(target, spell);
                LaunchSpellProjectiles(spellProjectiles);
            }
            else if (spellType == SpellProjectile.ProjectileSpellType.Blast)
            {
                var spellProjectiles = CreateBlastProjectiles(target, spell);
                LaunchSpellProjectiles(spellProjectiles);
            }
            else
            {
                var player = this as Player;
                if (player != null)
                {
                    player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, errorType: WeenieError.None),
                        new GameMessageSystemChat($"{spell.Name} spell not implemented, yet!", ChatMessageType.System));
                }
            }
        }

        /// <summary>
        /// Creates an enchantment and interacts with the Enchantment registry.
        /// Used by Life, Creature, Item, and Void magic
        /// </summary>
        public EnchantmentStatus CreateEnchantment(WorldObject target, WorldObject caster, Spell spell)
        {
            EnchantmentStatus enchantmentStatus = default(EnchantmentStatus);
            double duration;

            // what should the default duration be? -1 or 0?
            // changed from spell -> spellStatMod for void magic...
            if (caster is Creature)
                duration = spell.Duration;
            else
            {
                if (caster.WeenieType == WeenieType.Gem)
                    duration = spell.Duration;
                else
                    duration = -1;
            }

            // create enchantment
            var enchantment = new Enchantment(target, caster.Guid, spell.Id, duration, 1, EnchantmentMask.CreatureSpells);
            var addResult = target.EnchantmentManager.Add(enchantment, caster);

            var player = this as Player;
            var playerTarget = target as Player;
            var creatureTarget = target as Creature;

            // build message
            var suffix = "";
            switch (addResult.stackType)
            {
                case StackType.Refresh:
                    suffix = $", refreshing {spell.Name}";
                    break;
                case StackType.Surpass:
                    suffix = $", surpassing {addResult.surpass.Name}";
                    break;
                case StackType.Surpassed:
                    suffix = $", but it is surpassed by {addResult.surpass.Name}";
                    break;
            }

            var targetName = this == target ? "yourself" : target.Name;

            string message;
            if (addResult.stackType == StackType.Undef)
                message = null;
            else
            {
                if (addResult.stackType == StackType.None)
                    message = null;
                else
                {
                    if (caster is Creature)
                    {
                        if (caster.Guid == Guid)
                            message = $"You cast {spell.Name} on {targetName}{suffix}";
                        else
                            message = $"{caster.Name} casts {spell.Name} on {targetName}{suffix}"; // for the sentinel command `/buff [target player name]`
                    }
                    else
                    {
                        if (target.Name != caster.Name)
                            message = $"{caster.Name} casts {spell.Name} on you{suffix}";
                        else
                            message = null;
                    }
                }
            }

            if (target is Player)
            {
                if (addResult.stackType != StackType.Undef)
                {
                    if (addResult.stackType != StackType.Surpassed)
                        playerTarget.Session.Network.EnqueueSend(new GameEventMagicUpdateEnchantment(playerTarget.Session, enchantment));

                    if (playerTarget != this)
                        playerTarget.Session.Network.EnqueueSend(new GameMessageSystemChat($"{Name} cast {spell.Name} on you{suffix}", ChatMessageType.Magic));
                }
            }

            if (message != null)
                enchantmentStatus.message = new GameMessageSystemChat(message, ChatMessageType.Magic);
            else
                enchantmentStatus.message = null;
            enchantmentStatus.stackType = addResult.stackType;
            return enchantmentStatus;
        }


        /// <summary>
        /// Creates the Magic projectile spells for Life, War, and Void Magic
        /// </summary>
        private SpellProjectile CreateSpellProjectile(Spell spell, WorldObject target = null, uint lifeProjectileDamage = 0, Position origin = null, AceVector3 velocity = null)
        {
            SpellProjectile spellProjectile = WorldObjectFactory.CreateNewWorldObject(spell.Wcid) as SpellProjectile;
            spellProjectile.Setup(spell.Id);

            var useGravity = spellProjectile.SpellType == SpellProjectile.ProjectileSpellType.Arc;

            if (target != null)
            {
                var globalDest = target.Location.ToGlobal();
                globalDest.Z += target.Height / 2.0f;
                var globalOrigin = GetSpellProjectileOrigin(this, spellProjectile, globalDest);
                float dist = (globalDest - globalOrigin).Length();
                float speed = GetSpellProjectileSpeed(spellProjectile.SpellType, dist);

                spellProjectile.DistanceToTarget = dist;
                Position localPos = Location.FromGlobal(globalOrigin);
                spellProjectile.Location = new Position(localPos.LandblockId.Raw, localPos.Pos, this.Location.Rotation);
                spellProjectile.Velocity = GetSpellProjectileVelocity(globalOrigin, target, globalDest, speed, useGravity, out var time);
            }
            // We don't have a target and want to override the projectile origin and velocity
            else
            {
                if (velocity == null)
                {
                    log.Warn($"Untargeted or secondary spell projectiles must have a velocity set.");
                    return spellProjectile;
                }
                spellProjectile.Velocity = velocity;

                if (origin == null)
                {
                    log.Warn($"Untargeted or secondary spell projectiles must have an origin (creation location) set.");
                    return spellProjectile;
                }
                spellProjectile.Location = origin;
            }

            spellProjectile.LifeProjectileDamage = lifeProjectileDamage;
            spellProjectile.ProjectileSource = this;
            spellProjectile.ProjectileTarget = target;
            spellProjectile.SetProjectilePhysicsState(spellProjectile.ProjectileTarget, useGravity);
            spellProjectile.SpawnPos = new Position(spellProjectile.Location);

            return spellProjectile;
        }

        /// <summary>
        /// Creates a spell projectile in the world.
        /// </summary>
        private void LaunchSpellProjectile(SpellProjectile sp)
        {
            if (sp.Location == null)
            {
                log.Warn("A spell projectile could not be spawned. Location must not be null.");
                return;
            }

            if (sp.Velocity == null)
            {
                log.Warn("A spell projectile could not be spawned. Velocity must not be null.");
                return;
            }

            LandblockManager.AddObject(sp);
            sp.EnqueueBroadcast(new GameMessageScript(sp.Guid, ACE.Entity.Enum.PlayScript.Launch, sp.GetProjectileScriptIntensity(sp.SpellType)));

            if (sp.ProjectileTarget == null)
                return;

            // Detonate point-blank projectiles immediately
            var radsum = sp.ProjectileTarget.PhysicsObj.GetRadius() + sp.PhysicsObj.GetRadius();
            if (sp.DistanceToTarget < radsum)
                sp.OnCollideObject(sp.ProjectileTarget);
        }

        /// <summary>
        /// Creates multiple spell projectiles in the world.
        /// </summary>
        private void LaunchSpellProjectiles(List<SpellProjectile> spellProjectiles)
        {
            foreach (var sp in spellProjectiles)
            {
                LaunchSpellProjectile(sp);
            }
        }

        /// <summary>
        /// Calculates the spell projectile origin based on the targets global destination.
        /// </summary>
        private Vector3 GetSpellProjectileOrigin(WorldObject caster, SpellProjectile spellProjectile, Vector3 globalDest)
        {
            var globalOrigin = caster.Location.ToGlobal();
            if (spellProjectile.SpellType == SpellProjectile.ProjectileSpellType.Arc)
                globalOrigin.Z += caster.Height;
            else
                globalOrigin.Z += caster.Height * 2.0f / 3.0f;

            var direction = Vector3.Normalize(globalDest - globalOrigin);

            // This is not perfect but is close to values that retail used. TODO: revisit this later.
            globalOrigin += direction * (caster.PhysicsObj.GetRadius() + spellProjectile.PhysicsObj.GetRadius());

            return globalOrigin;
        }

        /// <summary>
        /// Gets the speed of a projectile based on the distance to the target.
        /// </summary>
        private float GetSpellProjectileSpeed(SpellProjectile.ProjectileSpellType spellType, float distance)
        {
            float speed;

            // TODO:
            // Speed seems to increase when target is moving away from the caster and decrease when
            // the target is moving toward the caster. This still needs more research.
            switch (spellType)
            {
                case SpellProjectile.ProjectileSpellType.Bolt:
                case SpellProjectile.ProjectileSpellType.Volley:
                case SpellProjectile.ProjectileSpellType.Blast:
                    speed = GetStationarySpeed(15f, distance);
                    break;
                case SpellProjectile.ProjectileSpellType.Streak:
                    speed = GetStationarySpeed(45f, distance);
                    break;
                case SpellProjectile.ProjectileSpellType.Arc:
                    speed = GetStationarySpeed(40f, distance);
                    break;
                default:
                    speed = 15f;
                    break;
            }

            return speed;
        }

        /// <summary>
        /// Creates a list of volley spell projectiles ready for creation in the world.
        /// </summary>
        private List<SpellProjectile> CreateVolleyProjectiles(WorldObject target, Spell spell)
        {
            var spellProjectiles = new List<SpellProjectile>();
            var centerProjectile = CreateSpellProjectile(spell, target);
            spellProjectiles.Add(centerProjectile);
            var projectileOrigins = GetVolleyProjectileOrigins(centerProjectile, spell.NumProjectiles);

            foreach (var origin in projectileOrigins)
            {
                spellProjectiles.Add(
                    CreateSpellProjectile(spell, velocity: centerProjectile.Velocity, origin: origin)
                );
            }

            return spellProjectiles;
        }

        /// <summary>
        /// Gets volley projectile origins based on the position of the center projectile.
        /// </summary>
        private List<Position> GetVolleyProjectileOrigins(SpellProjectile centerProjectile, int numProjectiles)
        {
            var origins = new List<Position>();
            // Lightning projectiles (WCID 1635) get a little more padding since they have a bigger radius
            var xOffsets = centerProjectile.WeenieClassId == 1635 ? new List<float> { -1.3f, 1.3f, -2.6f, 2.6f } : new List<float> { -1.2f, 1.2f, -2.4f, 2.4f };

            for (int i = 0; i < numProjectiles-1; i++)
            {
                var projOrigin = new Position(centerProjectile.Location);
                // Rotate and add offset to get the new projectile position then rotate back to the original heading
                var originPosition = RotatePosition(projOrigin.Pos, projOrigin.Rotation);
                originPosition += new Vector3(xOffsets[i], 0, 0);
                projOrigin.SetPosition(Vector3.Transform(originPosition, projOrigin.Rotation));
                projOrigin.LandblockId = new LandblockId(projOrigin.GetCell());
                origins.Add(projOrigin);
            }

            return origins;
        }

        /// <summary>
        /// Creates a list of blast spell projectiles ready for creation in the world.
        /// </summary>
        private List<SpellProjectile> CreateBlastProjectiles(WorldObject target, Spell spell)
        {
            var spellProjectiles = GetSpreadProjectiles(spell, target);
            return spellProjectiles;
        }

        /// <summary>
        /// Creates a list of ring spell projectiles ready for creation in the world.
        /// </summary>
        private List<SpellProjectile> CreateRingProjectiles(Spell spell)
        {
            Vector3 originOffset = GetRingOriginOffset(spell);
            AceVector3 velocity = GetRingVelocity(spell);

            var spellProjectiles = GetSpreadProjectiles(spell, originOffset: originOffset, velocity: velocity);

            return spellProjectiles;
        }

        /// <summary>
        /// Gets the XYZ offsets for a ring spell projectile.
        /// </summary>
        private Vector3 GetRingOriginOffset(Spell spell)
        {
            if (spell.Wcid >= 7269 && spell.Wcid <= 7275 || spell.Wcid == 43233 || spell.Id == 6320)
            {
                var zOffset = Height * 2 / 3;
                return new Vector3(0f, 0.82f, zOffset);
            }
            return Vector3.Zero;
        }

        /// <summary>
        /// Gets the default velocity for a ring spell projectile.
        /// </summary>
        private AceVector3 GetRingVelocity(Spell spell)
        {
            if (spell.Wcid >= 7269 && spell.Wcid <= 7275 || spell.Wcid == 43233)
                return new AceVector3(0f, 2f, 0);
            if (spell.Id == 6320)
                return new AceVector3(0, 15, 0);

            return new AceVector3(0, 0, 0);
        }

        /// <summary>
        /// Creates a list of spell projectiles which use spread angles (Blast or Ring spells).
        /// </summary>
        private List<SpellProjectile> GetSpreadProjectiles(Spell spell, WorldObject target = null, Vector3? originOffset = null, AceVector3 velocity = null)
        {
            var spellProjectiles = new List<SpellProjectile>();

            // The first projectile is always created directly in front of the caster
            SpellProjectile centerProjectile;
            var casterLocalOrigin = RotatePosition(Location.Pos, Location.Rotation);

            if (target != null) // Blast spells
            {
                centerProjectile = CreateSpellProjectile(spell, target);
                var localOrigin = RotatePosition(centerProjectile.Location.Pos, Location.Rotation);
                originOffset = new Vector3(0, Math.Abs(localOrigin.Y - casterLocalOrigin.Y), 0);
                var localVelocity = RotatePosition(centerProjectile.Velocity.Get(), Location.Rotation);
                velocity = new AceVector3(localVelocity.X, localVelocity.Y, localVelocity.Z);
            }
            else // Ring spells
            {
                if (originOffset == null)
                {
                    log.Warn($"Untargeted spread angle spell projectiles must have an origin offset set.");
                    return spellProjectiles;
                }
                if (velocity == null)
                {
                    log.Warn($"Untargeted spread angle spell projectiles must have a default velocity set.");
                    return spellProjectiles;
                }

                var projOrigin = new Position(Location);
                projOrigin.SetPosition(Vector3.Transform(casterLocalOrigin + (Vector3) originOffset,
                    Location.Rotation));
                projOrigin.LandblockId = new LandblockId(projOrigin.GetCell());
                var globalVelocity = Vector3.Transform(velocity.Get(),
                    Location.Rotation);
                centerProjectile = CreateSpellProjectile(spell, origin: projOrigin, velocity: new AceVector3(globalVelocity.X, globalVelocity.Y, globalVelocity.Z));
            }

            spellProjectiles.Add(centerProjectile);
            if (spell.NumProjectiles == 1)
                return spellProjectiles;

            float degrees = spell.SpreadAngle / (spell.NumProjectiles - 1);
            int oddEvenCounter = 1;

            for (int i = 1; i < spell.NumProjectiles; i++)
            {
                // Odd numbers are created on the -X axis (left of caster) and even are on the +X axis
                var radians = (float)(oddEvenCounter * degrees * Math.PI / 180);
                Quaternion localProjRotation;
                if (i % 2 != 0)
                {
                    localProjRotation = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, radians);
                }
                else
                {
                    localProjRotation = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, (float)(2 * Math.PI) - radians);
                    oddEvenCounter++;
                }

                var localProjLocation = Vector3.Transform((Vector3)originOffset, localProjRotation);
                var projOrigin = new Position(Location);
                projOrigin.SetPosition(Vector3.Transform(casterLocalOrigin + localProjLocation,
                    Location.Rotation));
                projOrigin.LandblockId = new LandblockId(projOrigin.GetCell());
                // Make sure Z component matches the center projectile
                projOrigin.PositionZ = centerProjectile.Location.PositionZ;
                var localProjVelocity = Vector3.Transform(velocity.Get(), localProjRotation);
                var globalProjVelocity = Vector3.Transform(localProjVelocity, this.Location.Rotation);
                spellProjectiles.Add(
                    CreateSpellProjectile(spell, origin: projOrigin,
                    velocity: new AceVector3(globalProjVelocity.X, globalProjVelocity.Y, globalProjVelocity.Z)
                ));
            }

            return spellProjectiles;
        }

        /// <summary>
        /// Creates a list of wall spell projectiles ready for creation in the world.
        /// </summary>
        private List<SpellProjectile> CreateWallProjectiles(Spell spell)
        {
            var spellProjectiles = new List<SpellProjectile>();
            var projectileOrigins = GetWallProjectileOrigins(spell);
            var velocity = GetWallProjectileVelocity(spell);

            foreach (var origin in projectileOrigins)
            {
                spellProjectiles.Add(CreateSpellProjectile(spell, velocity: velocity, origin: origin));
            }

            return spellProjectiles;
        }

        /// <summary>
        /// Gets the XYZ offsets for wall spell projectiles.
        /// </summary>
        private List<Position> GetWallProjectileOrigins(Spell spell)
        {
            List<Vector3> offsetList;
            var isTuskerFists = spell.Id == 2934;
            var defaultZOffset = Height * 2.0f / 3.0f;
            // Lightning spells get some additional padding
            var zPadding = (spell.Wcid == 7280) ? 1.3f : 1.2f;
            var xPadding = (spell.Wcid == 7280) ? 0.1f : 0f;
            var topRowZOffset = defaultZOffset + zPadding;

            if (spell.Name.Equals("Rolling Death"))
            {
                offsetList = new List<Vector3>()
                {
                    //new Vector3(0, 0.0f, 1.828333f)
                    new Vector3(0, 0.0f, Height)
                };
            }
            else if (isTuskerFists)
            {
                offsetList = new List<Vector3>
                {
                    new Vector3(0f, 3.2f, defaultZOffset), // Bottom row
                    new Vector3(0f, 4.4f, defaultZOffset), // This front bottom row projectile is shifted back 1 meter
                    new Vector3(1f, 3.2f, defaultZOffset),
                    new Vector3(1f, 5.4f, defaultZOffset),
                    new Vector3(-1f, 3.2f, defaultZOffset),
                    new Vector3(-1f, 5.4f, defaultZOffset),
                    new Vector3(2f, 3.2f, defaultZOffset),
                    new Vector3(2f, 5.4f, defaultZOffset),
                    new Vector3(0f, 3.2f, topRowZOffset),  // Top row
                    new Vector3(0f, 5.4f, topRowZOffset),
                    new Vector3(1f, 3.2f, topRowZOffset),
                    new Vector3(1f, 5.4f, topRowZOffset),
                    new Vector3(-1f, 3.2f, topRowZOffset),
                    new Vector3(-1f, 5.4f, topRowZOffset),
                    new Vector3(2f, 3.2f, topRowZOffset),
                    new Vector3(2f, 5.4f, topRowZOffset)
                };
            }
            else
            {
                offsetList = new List<Vector3> {
                    new Vector3(0f, 3.2f, defaultZOffset),                     // Center bottom
                    new Vector3(0f, 3.2f, topRowZOffset),                      // Center top
                    new Vector3(-2f - (2 * xPadding), 3.2f, defaultZOffset),   // Far left bottom
                    new Vector3(-1f - xPadding, 3.2f, defaultZOffset),         // Near left bottom
                    new Vector3(1f + xPadding, 3.2f, defaultZOffset),          // Near right bottom
                    new Vector3(2f + (2 * xPadding), 3.2f, defaultZOffset),    // Far right bottom
                    new Vector3(-2f - (2 * xPadding), 3.2f, topRowZOffset),    // Far left top
                    new Vector3(-1f - xPadding, 3.2f, topRowZOffset),          // Near left top
                    new Vector3(1f + xPadding, 3.2f, topRowZOffset),           // Near right top
                    new Vector3(2f + (2 * xPadding), 3.2f, topRowZOffset),     // Far right top
                };
            }

            var origins = new List<Position>();
            for (int i = 0; i < spell.NumProjectiles; i++)
            {
                var projOrigin = new Position(Location);
                // Rotate and add offset to get the new projectile position then rotate back to the original heading
                var originPosition = RotatePosition(projOrigin.Pos, projOrigin.Rotation);
                originPosition += offsetList[i];
                projOrigin.SetPosition(Vector3.Transform(originPosition, projOrigin.Rotation));
                projOrigin.LandblockId = new LandblockId(projOrigin.GetCell());
                origins.Add(projOrigin);
            }

            return origins;
        }

        /// <summary>
        /// Get the velocity for wall spell projectiles.
        /// </summary>
        private AceVector3 GetWallProjectileVelocity(Spell spell)
        {
            // The Slithering Flames spell does in fact slither slower than other wall spells
            var velocity = (spell.Id == 1841) ? new Vector3(0, 3f, 0) : new Vector3(0, 4f, 0);

            if (spell.Name.Equals("Rolling Death"))
                velocity = new Vector3(0, 2, 0);

            velocity = Vector3.Transform(velocity, Location.Rotation);

            return new AceVector3(velocity.X, velocity.Y, velocity.Z);
        }

        /// <summary>
        /// Rotates a position by the inverse of its rotation.
        /// Useful for getting the local space coordinates of a position.
        /// </summary>
        private static Vector3 RotatePosition(Vector3 position, Quaternion rotation)
        {
            return Vector3.Transform(position, Quaternion.Inverse(rotation));
        }

        /// <summary>
        /// Calculates the velocity of a spell projectile based on distance to the target (assuming it is stationary)
        /// </summary>
        private float GetStationarySpeed(float defaultSpeed, float distance)
        {
            var speed = (float)((defaultSpeed * .9998363f) - (defaultSpeed * .62034f) / distance +
                                   (defaultSpeed * .44868f) / Math.Pow(distance, 2f) - (defaultSpeed * .25256f)
                                   / Math.Pow(distance, 3f));

            speed = Math.Clamp(speed, 1, 50);

            return speed;
        }

        /// <summary>
        /// Calculates the velocity to launch the projectile from origin to dest
        /// </summary>
        private AceVector3 GetSpellProjectileVelocity(Vector3 origin, WorldObject target, Vector3 dest, float speed, bool useGravity, out float time)
        {
            var targetVelocity = Vector3.Zero;
            if (!useGravity)    // no target tracking for arc spells
                targetVelocity = target.PhysicsObj.CachedVelocity;

            var gravity = useGravity ? PhysicsGlobals.Gravity : 0;
            Trajectory.solve_ballistic_arc_lateral(origin, speed, dest, targetVelocity, gravity, out Vector3 velocity, out time, out var impactPoint);

            return new AceVector3(velocity.X, velocity.Y, velocity.Z);
        }

        /// <summary>
        /// Returns the drain resistance type for a vital
        /// </summary>
        private static ResistanceType GetDrainResistanceType(PropertyAttribute2nd vital)
        {
            if (vital == PropertyAttribute2nd.Mana)
                return ResistanceType.ManaDrain;
            else if (vital == PropertyAttribute2nd.Stamina)
                return ResistanceType.StaminaDrain;
            else
                return ResistanceType.HealthDrain;
        }

        /// <summary>
        /// Returns the boost resistance type for a vital
        /// </summary>
        private static ResistanceType GetBoostResistanceType(PropertyAttribute2nd vital)
        {
            if (vital == PropertyAttribute2nd.Mana)
                return ResistanceType.ManaBoost;
            else if (vital == PropertyAttribute2nd.Stamina)
                return ResistanceType.StaminaBoost;
            else
                return ResistanceType.HealthBoost;
        }
    }
}
