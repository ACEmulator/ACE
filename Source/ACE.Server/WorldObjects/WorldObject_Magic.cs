using System;
using System.Collections.Generic;
using System.Numerics;
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
        /// Determine Player's PK status and whether it matches the target Player
        /// </summary>
        /// <returns>
        /// A null return signifies either player or target are not Player World objects, so check does not apply
        /// A true return value indicates that the Player passed the PK status check
        /// A false return value indicates that the Player failed the PK status check
        /// </returns>
        protected bool? CheckPKStatusVsTarget(Player player, Player target, Spell spell)
        {
            if (player == null || target == null)
                return null;

            if (spell.IsHarmful)
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
            // only creatures can resist spells
            var caster = this as Creature;
            if (caster == null) return null;

            var player = caster as Player;
            var targetPlayer = target as Player;

            // Retrieve caster's skill level in the Magic School
            var magicSkill = caster.GetCreatureSkill(spell.School).Current;

            // Retrieve target's Magic Defense Skill
            var creature = target as Creature;
            var targetMagicDefenseSkill = creature.GetCreatureSkill(Skill.MagicDefense).Current;

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

            Player player = null;
            Creature creature = null;
            if (this is Player)
                player = this as Player;
            else if (this is Creature)
                creature = this as Creature;

            Creature spellTarget;
            if (spell.BaseRangeConstant > 0)
                spellTarget = target as Creature;
            else
                spellTarget = this as Creature;

            // Target already dead
            if (spellTarget.Health.Current <= 0)
            {
                enchantmentStatus.message = null;
                damage = 0;
                return false;
            }

            int newSpellTargetVital;
            switch (spell.MetaSpellType)
            {
                case SpellType.Boost:

                    // handle negatives?
                    int minBoostValue = Math.Min(spell.Boost, spell.MaxBoost);
                    int maxBoostValue = Math.Max(spell.Boost, spell.MaxBoost);

                    int boost = Physics.Common.Random.RollDice(minBoostValue, maxBoostValue);
                    damage = boost < 0 ? (uint)Math.Abs(boost) : 0;

                    switch (spell.DamageType2)
                    {
                        case DamageType.Mana:
                            newSpellTargetVital = (int)Math.Min(spellTarget.Mana.Current + boost, spellTarget.Mana.MaxValue);
                            srcVital = "mana";
                            spellTarget.UpdateVital(spellTarget.Mana, (uint)newSpellTargetVital);
                            break;
                        case DamageType.Stamina:
                            newSpellTargetVital = (int)Math.Min(spellTarget.Stamina.Current + boost, spellTarget.Stamina.MaxValue);
                            srcVital = "stamina";
                            spellTarget.UpdateVital(spellTarget.Stamina, (uint)newSpellTargetVital);
                            break;
                        default:   // Health
                            srcVital = "health";
                            if (spellTarget.Health.Current <= 0 && boost < 0)
                            {
                                boost = 0;
                                break;
                            }
                            newSpellTargetVital = (int)Math.Min(spellTarget.Health.Current + boost, spellTarget.Health.MaxValue);
                            spellTarget.UpdateVital(spellTarget.Health, (uint)newSpellTargetVital);

                            if (boost >= 0)
                                spellTarget.DamageHistory.OnHeal((uint)boost);
                            else
                                spellTarget.DamageHistory.Add(this, DamageType.Health, damage);
                            break;
                    }

                    if (this is Player)
                    {
                        if (spell.BaseRangeConstant > 0)
                        {
                            string msg;
                            if (boost <= 0)
                            {
                                msg = $"You drain {Math.Abs(boost).ToString()} points of {srcVital} from {spellTarget.Name}";
                                enchantmentStatus.message = new GameMessageSystemChat(msg, ChatMessageType.Combat);
                            }
                            else
                            {
                                msg = $"You restore {Math.Abs(boost).ToString()} points of {srcVital} to {spellTarget.Name}";
                                enchantmentStatus.message = new GameMessageSystemChat(msg, ChatMessageType.Magic);
                            }
                        }
                        else
                            enchantmentStatus.message = new GameMessageSystemChat($"You restore {Math.Abs(boost).ToString()} {srcVital}", ChatMessageType.Magic);
                    }
                    else
                        enchantmentStatus.message = null;

                    if (target is Player && spell.BaseRangeConstant > 0)
                    {
                        string msg;
                        if (boost <= 0)
                        {
                            msg = $"{Name} casts {spell.Name} and drains {Math.Abs(boost).ToString()} points of your {srcVital}";
                            targetMsg = new GameMessageSystemChat(msg, ChatMessageType.Combat);
                        }
                        else
                        {
                            msg = $"{Name} casts {spell.Name} and restores {Math.Abs(boost).ToString()} points of your {srcVital}";
                            targetMsg = new GameMessageSystemChat(msg, ChatMessageType.Magic);
                        }
                    }

                    if (player != null && srcVital != null && srcVital.Equals("health"))
                        player.Session.Network.EnqueueSend(new GameEventUpdateHealth(player.Session, target.Guid.Full, (float)spellTarget.Health.Current / spellTarget.Health.MaxValue));

                    break;

                case SpellType.Transfer:

                    // Calculate the change in vitals of the target
                    Creature caster;
                    if (spell.BaseRangeConstant == 0 && spell.BaseRangeMod == 1)
                        caster = spellTarget;
                    else
                        caster = (Creature)this;
                    uint vitalChange, casterVitalChange;
                    ResistanceType resistanceDrain, resistanceBoost;
                    if (spell.Source == PropertyAttribute2nd.Mana)
                        resistanceDrain = ResistanceType.ManaDrain;
                    else if (spell.Source == PropertyAttribute2nd.Stamina)
                        resistanceDrain = ResistanceType.StaminaDrain;
                    else
                        resistanceDrain = ResistanceType.HealthDrain;
                    vitalChange = (uint)((spellTarget.GetCurrentCreatureVital(spell.Source) * spell.Proportion) * spellTarget.GetNaturalResistance(resistanceDrain));
                    if (spell.TransferCap != 0)
                    {
                        if (vitalChange > spell.TransferCap)
                            vitalChange = (uint)spell.TransferCap;
                    }
                    if (spell.Destination == PropertyAttribute2nd.Mana)
                        resistanceBoost = ResistanceType.ManaDrain;
                    else if (spell.Source == PropertyAttribute2nd.Stamina)
                        resistanceBoost = ResistanceType.StaminaDrain;
                    else
                        resistanceBoost = ResistanceType.HealthDrain;
                    casterVitalChange = (uint)((vitalChange * (1.0f - spell.LossPercent)) * spellTarget.GetNaturalResistance(resistanceBoost));
                    vitalChange = (uint)(casterVitalChange / (1.0f - spell.LossPercent));

                    // Apply the change in vitals to the target
                    switch (spell.Source)
                    {
                        case PropertyAttribute2nd.Mana:
                            srcVital = "mana";
                            vitalChange = (uint)-spellTarget.UpdateVitalDelta(spellTarget.Mana, -(int)vitalChange);
                            break;
                        case PropertyAttribute2nd.Stamina:
                            srcVital = "stamina";
                            vitalChange = (uint)-spellTarget.UpdateVitalDelta(spellTarget.Stamina, -(int)vitalChange);
                            break;
                        default:   // Health
                            srcVital = "health";
                            vitalChange = (uint)-spellTarget.UpdateVitalDelta(spellTarget.Health, -(int)vitalChange);
                            spellTarget.DamageHistory.Add(this, DamageType.Health, vitalChange);
                            break;
                    }
                    damage = vitalChange;

                    // Apply the scaled change in vitals to the caster
                    switch (spell.Destination)
                    {
                        case PropertyAttribute2nd.Mana:
                            destVital = "mana";
                            casterVitalChange = (uint)caster.UpdateVitalDelta(caster.Mana, casterVitalChange);
                            break;
                        case PropertyAttribute2nd.Stamina:
                            destVital = "stamina";
                            casterVitalChange = (uint)caster.UpdateVitalDelta(caster.Stamina, casterVitalChange);
                            break;
                        default:   // Health
                            destVital = "health";
                            casterVitalChange = (uint)caster.UpdateVitalDelta(caster.Health, casterVitalChange);
                            caster.DamageHistory.OnHeal(casterVitalChange);
                            break;
                    }

                    if (this is Player)
                    {
                        if (target.Guid == player.Guid)
                        {
                            enchantmentStatus.message = new GameMessageSystemChat($"You drain {vitalChange} points of {srcVital} and apply {casterVitalChange} points of {destVital} to yourself", ChatMessageType.Magic);
                        }
                        else
                            enchantmentStatus.message = new GameMessageSystemChat($"You drain {vitalChange} points of {srcVital} from {spellTarget.Name} and apply {casterVitalChange} to yourself", ChatMessageType.Combat);
                    }
                    else
                        enchantmentStatus.message = null;

                    if (target is Player && target != this)
                        targetMsg = new GameMessageSystemChat($"You lose {vitalChange} points of {srcVital} due to {Name} casting {spell.Name} on you", ChatMessageType.Combat);

                    if (player != null && srcVital != null && srcVital.Equals("health"))
                        player.Session.Network.EnqueueSend(new GameEventUpdateHealth(player.Session, target.Guid.Full, (float)spellTarget.Health.Current / spellTarget.Health.MaxValue));

                    break;

                case SpellType.LifeProjectile:

                    caster = (Creature)this;

                    if (spell.Name.Contains("Blight"))
                    {
                        var tryDamage = (int)Math.Round(caster.GetCurrentCreatureVital(PropertyAttribute2nd.Mana) * caster.GetNaturalResistance(ResistanceType.ManaDrain));
                        damage = (uint)-caster.UpdateVitalDelta(caster.Mana, -tryDamage);
                    }
                    else if (spell.Name.Contains("Tenacity"))
                    {
                        var tryDamage = (int)Math.Round(spellTarget.GetCurrentCreatureVital(PropertyAttribute2nd.Stamina) * spellTarget.GetNaturalResistance(ResistanceType.StaminaDrain));
                        damage = (uint)-caster.UpdateVitalDelta(caster.Stamina, -tryDamage);
                    }
                    else
                    {
                        var tryDamage = (int)Math.Round(spellTarget.GetCurrentCreatureVital(PropertyAttribute2nd.Stamina) * spellTarget.GetNaturalResistance(ResistanceType.HealthDrain));
                        damage = (uint)-caster.UpdateVitalDelta(caster.Health, -tryDamage);
                        caster.DamageHistory.Add(this, DamageType.Health, damage);
                    }

                    var sp = CreateSpellProjectile(spell, target, damage);
                    LaunchSpellProjectile(sp);

                    if (caster.Health.Current <= 0)
                        caster.Die();

                    enchantmentStatus.message = null;
                    break;

                case SpellType.Dispel:
                    damage = 0;
                    enchantmentStatus.message = new GameMessageSystemChat("Spell not implemented, yet!", ChatMessageType.Magic);
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
        /// Wrapper around CreateEnchantment for Creature Magic
        /// </summary>
        protected EnchantmentStatus CreatureMagic(WorldObject target, Spell spell, WorldObject itemCaster = null)
        {
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

            Player player = CurrentLandblock?.GetObject(Guid) as Player;
            if (player == null && ((this as Player) != null)) player = this as Player;

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
                        Position linkedPortal = null;

                        if (itemCaster != null)
                            portalId = itemCaster.GetProperty(PropertyDataId.LinkedPortalOne) ?? 0;
                        else
                        {
                            if (spell.Name.Contains("Summon Primary"))
                            {
                                linkedPortal = GetPosition(PositionType.LinkedPortalOne);
                            }
                            if (spell.Name.Contains("Summon Secondary"))
                            {
                                linkedPortal = GetPosition(PositionType.LinkedPortalTwo);
                            }

                            if (linkedPortal != null)
                                portalId = 1955;
                        }

                        if (portalId != 0)
                        {
                            var portal = WorldObjectFactory.CreateNewWorldObject(portalId);
                            portal.SetupTableId = 33556212;
                            portal.RadarBehavior = ACE.Entity.Enum.RadarBehavior.ShowNever;
                            portal.Name = "Gateway";
                            portal.Location = Location.InFrontOf();

                            if (portalId == 1955)
                                portal.Destination = linkedPortal;

                            portal.EnterWorld();

                            // Create portal decay
                            ActionChain despawnChain = new ActionChain();
                            despawnChain.AddDelaySeconds(spell.PortalLifetime);
                            despawnChain.AddAction(portal, () => portal.CurrentLandblock?.RemoveWorldObject(portal.Guid, false));
                            despawnChain.EnqueueChain();
                        }
                        else
                        {
                            // You must link to a portal to summon it!
                            player.Session.Network.EnqueueSend(new GameEventWeenieError(player.Session, WeenieError.YouMustLinkToPortalToSummonIt));
                        }
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
        /// Launches a War Magic spell projectile (untargeted)
        /// </summary>
        protected void WarMagic(Spell spell)
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
            var stackType = target.EnchantmentManager.Add(enchantment, caster);

            var player = this as Player;
            var playerTarget = target as Player;
            var creatureTarget = target as Creature;

            // build message
            var suffix = "";
            switch (stackType)
            {
                case StackType.Refresh:
                    suffix = $", refreshing {spell.Name}";
                    break;
                case StackType.Surpass:
                    suffix = $", surpassing {target.EnchantmentManager.Surpass.Name}";
                    break;
                case StackType.Surpassed:
                    suffix = $", but it is surpassed by {target.EnchantmentManager.Surpass.Name}";
                    break;
            }

            var targetName = this == target ? "yourself" : target.Name;

            string message;
            if (stackType == StackType.Undef)
                message = null;
            else
            {
                if (stackType == StackType.None)
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
                if (stackType != StackType.Undef)
                {
                    if (stackType != StackType.Surpassed)
                        playerTarget.Session.Network.EnqueueSend(new GameEventMagicUpdateEnchantment(playerTarget.Session, enchantment));

                    if (playerTarget != this)
                        playerTarget.Session.Network.EnqueueSend(new GameMessageSystemChat($"{Name} cast {spell.Name} on you{suffix}", ChatMessageType.Magic));
                }
            }

            if (message != null)
                enchantmentStatus.message = new GameMessageSystemChat(message, ChatMessageType.Magic);
            else
                enchantmentStatus.message = null;
            enchantmentStatus.stackType = stackType;
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

            if (isTuskerFists)
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
    }
}
