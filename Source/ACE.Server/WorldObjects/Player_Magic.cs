using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Common;
using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.DatLoader;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Structure;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        public enum TargetCategory
        {
            UnDef,
            WorldObject,
            Wielded,
            Inventory,
            Self,
            Fellowship
        }

        /// <summary>
        /// The last spell projectile launched by this player
        /// to successfully collided with a target
        /// </summary>
        public Spell LastHitSpellProjectile;

        /// <summary>
        /// Limiter for switching between war and void magic
        /// </summary>
        public double LastSuccessCast_Time;
        public MagicSchool LastSuccessCast_School;

        /// <summary>
        /// Returns the magic skill associated with the magic school
        /// for the last collided spell projectile
        /// </summary>
        public Skill GetCurrentMagicSkill()
        {
            if (LastHitSpellProjectile == null)
                return Skill.WarMagic;  // this should never happen, but just in case

            switch (LastHitSpellProjectile.School)
            {
                case MagicSchool.WarMagic:
                default:
                    return Skill.WarMagic;
                case MagicSchool.LifeMagic:
                    return Skill.LifeMagic;
                case MagicSchool.CreatureEnchantment:
                    return Skill.CreatureEnchantment;
                case MagicSchool.ItemEnchantment:
                    return Skill.ItemEnchantment;
                case MagicSchool.VoidMagic:
                    return Skill.VoidMagic;
            }
        }

        /// <summary>
        /// Handles player targeted casting message
        /// </summary>
        public void HandleActionCastTargetedSpell(uint targetGuid, uint spellId)
        {
            var target = CurrentLandblock?.GetObject(targetGuid);
            var targetCategory = TargetCategory.UnDef;

            if (target != null)
            {
                if (targetGuid == Guid.Full)
                    targetCategory = TargetCategory.Self;
                else
                    targetCategory = TargetCategory.WorldObject;
            }
            else
            {
                target = GetEquippedItem(targetGuid);
                if (target != null)
                    targetCategory = TargetCategory.Wielded;
                else
                {
                    target = GetInventoryItem(targetGuid);
                    if (target != null)
                        targetCategory = TargetCategory.Inventory;
                    else
                    {
                        target = CurrentLandblock?.GetWieldedObject(targetGuid);
                        if (target != null)
                            targetCategory = TargetCategory.Wielded;
                    }
                }
            }

            var spell = new Spell(spellId);
            if ((spell.Flags & SpellFlags.FellowshipSpell) != 0)
            {
                targetCategory = TargetCategory.Fellowship;
                target = this;
            }

            if (target == null)
            {
                Session.Network.EnqueueSend(new GameEventUseDone(Session, WeenieError.TargetNotAcquired));
                return;
            }

            if (targetCategory != TargetCategory.WorldObject && targetCategory != TargetCategory.Wielded)
            {
                CreatePlayerSpell(target, targetCategory, spellId);
            }
            else
            {
                var rotateTarget = target;
                if (rotateTarget.WielderId != null)
                    rotateTarget = CurrentLandblock?.GetObject(rotateTarget.WielderId.Value);

                // turn if required
                var rotateTime = Rotate(rotateTarget);
                var actionChain = new ActionChain();
                actionChain.AddDelaySeconds(rotateTime);

                actionChain.AddAction(this, () => CreatePlayerSpell(target, targetCategory, spellId));
                actionChain.EnqueueChain();
            }

            if (UnderLifestoneProtection)
                LifestoneProtectionDispel();
        }

        /// <summary>
        /// Handles player untargeted casting message
        /// </summary>
        public void HandleActionMagicCastUnTargetedSpell(uint spellId)
        {
            CreatePlayerSpell(spellId);

            if (UnderLifestoneProtection)
                LifestoneProtectionDispel();
        }

        /// <summary>
        ///  Learns spells in bulk, without notification, filtered by school and level
        /// </summary>
        public void LearnSpellsInBulk(MagicSchool school, uint spellLevel, bool withNetworking = true)
        {
            var spellTable = DatManager.PortalDat.SpellTable;

            foreach (var spellID in PlayerSpellTable)
            {
                if (!spellTable.Spells.ContainsKey(spellID))
                {
                    Console.WriteLine($"Unknown spell ID in PlayerSpellID table: {spellID}");
                    continue;
                }
                var spell = new Spell(spellID, false);
                if (spell.School == school && spell.Formula.Level == spellLevel)
                {
                    if (withNetworking)
                        LearnSpellWithNetworking(spell.Id, false);
                    else
                        AddKnownSpell(spell.Id);
                }
            }
        }

        /// <summary>
        /// Method used for handling items casting spells on the player who is either equipping the item, or using a gem in possessions
        /// </summary>
        /// <param name="spellId">the spell id</param>
        /// <returns>FALSE - the spell was NOT created because the spell is invalid or not implemented yet.<para />TRUE - the spell was created or it is surpassed</returns>
        public bool CreateSingleSpell(uint spellId)
        {
            var player = this;
            var spell = new Spell(spellId);

            if (spell.NotFound)
            {
                if (spell._spellBase == null)
                    Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, $"SpellId {spellId} Invalid."));
                else
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"{spell.Name} spell not implemented, yet!", ChatMessageType.System));

                return false;
            }

            var enchantmentStatus = new EnchantmentStatus(spell);

            switch (spell.School)
            {
                case MagicSchool.CreatureEnchantment:

                    enchantmentStatus = CreatureMagic(player, spell);
                    if (enchantmentStatus.Message != null)
                        EnqueueBroadcast(new GameMessageScript(player.Guid, spell.TargetEffect, spell.Formula.Scale));
                    break;

                case MagicSchool.LifeMagic:

                    LifeMagic(player, spell, out uint damage, out bool critical, out enchantmentStatus);
                    if (enchantmentStatus.Message != null)
                        EnqueueBroadcast(new GameMessageScript(player.Guid, spell.TargetEffect, spell.Formula.Scale));
                    break;

                case MagicSchool.ItemEnchantment:

                    if (spell.IsPortalSpell)
                    {
                        var playScript = spell.CasterEffect > 0 ? spell.CasterEffect : spell.TargetEffect;
                        EnqueueBroadcast(new GameMessageScript(player.Guid, playScript, spell.Formula.Scale));
                        enchantmentStatus = ItemMagic(player, spell);
                    }
                    else
                    {
                        if (spell.HasItemCategory)
                            enchantmentStatus = ItemMagic(player, spell);

                        EnqueueBroadcast(new GameMessageScript(player.Guid, spell.TargetEffect, spell.Formula.Scale));
                    }
                    break;

                default:
                    Console.WriteLine("Unknown magic school: " + spell.School);
                    break;
            }
            return true;
        }

        /// <summary>
        /// Handles an item casting a spell on a player
        /// </summary>
        public override EnchantmentStatus CreateItemSpell(WorldObject item, uint spellID)
        {
            var spell = new Spell(spellID);

            if (spell.NotFound)
            {
                if (spell._spellBase == null)
                    Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, $"SpellID {spellID} Invalid."));
                else
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"{spell.Name} spell not implemented, yet!", ChatMessageType.System));

                return new EnchantmentStatus(false);
            }

            var enchantmentStatus = base.CreateItemSpell(item, spellID);

            if (enchantmentStatus.Message != null)
                Session.Network.EnqueueSend(enchantmentStatus.Message);

            return enchantmentStatus;
        }

        private enum CastingPreCheckStatus
        {
            CastFailed,
            InvalidPKStatus,
            Success
        }

        public static float Windup_MaxMove = 6.0f;
        public static float Windup_MaxMoveSq = Windup_MaxMove * Windup_MaxMove;

        /// <summary>
        /// Method used for handling player targeted spell casts
        /// </summary>
        public void CreatePlayerSpell(WorldObject target, TargetCategory targetCategory, uint spellId)
        {
            var player = this;
            var creatureTarget = target as Creature;

            if (player.IsBusy == true)
            {
                player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, WeenieError.YoureTooBusy));
                return;
            }
            player.IsBusy = true;

            var spell = new Spell(spellId);

            if (spell.NotFound)
            {
                if (spell._spellBase == null)
                {
                    Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, $"SpellId {spellId} Invalid."));
                    Session.Network.EnqueueSend(new GameEventUseDone(Session, WeenieError.None));
                }
                else
                {
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"{spell.Name} spell not implemented, yet!", ChatMessageType.System));
                    Session.Network.EnqueueSend(new GameEventUseDone(Session, WeenieError.MagicInvalidSpellType));
                }

                player.IsBusy = false;
                return;
            }

            if (IsInvalidTarget(spell, target))
            {
                player.Session.Network.EnqueueSend(new GameEventCommunicationTransientString(player.Session, $"{spell.Name} cannot be cast on {target.Name}."));
                player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, WeenieError.None));
                player.IsBusy = false;
                return;
            }

            // if casting implement has spell built in,
            // use spellcraft from the item, instead of player's magic skill?
            var caster = GetEquippedWand();
            var isWeaponSpell = IsWeaponSpell(spell);

            // Grab player's skill level in the spell's Magic School
            var magicSkill = player.GetCreatureSkill(spell.School).Current;
            if (isWeaponSpell && caster.ItemSpellcraft != null)
                magicSkill = (uint)caster.ItemSpellcraft;

            if (targetCategory == TargetCategory.WorldObject)
            {
                if (target.Guid != Guid)
                {
                    var targetLoc = target;
                    if (targetLoc.WielderId != null)
                        targetLoc = CurrentLandblock?.GetObject(targetLoc.WielderId.Value);

                    float distanceTo = Location.Distance2D(targetLoc.Location);

                    if (distanceTo > spell.BaseRangeConstant + magicSkill * spell.BaseRangeMod)
                    {
                        player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, WeenieError.None),
                            new GameMessageSystemChat($"Target is out of range!", ChatMessageType.Magic));
                        player.IsBusy = false;
                        return;
                    }
                }
            }

            var difficulty = spell.Power;

            // is this needed? should talismans remain the same, regardless of player spell formula?
            spell.Formula.GetPlayerFormula(player);

            var castingPreCheckStatus = CastingPreCheckStatus.CastFailed;

            if (magicSkill > 0 && magicSkill >= (int)difficulty - 50)
            {
                var chance = 1.0f - SkillCheck.GetMagicSkillChance((int)magicSkill, (int)difficulty);
                var rng = ThreadSafeRandom.Next(0.0f, 1.0f);
                if (chance < rng || isWeaponSpell)
                    castingPreCheckStatus = CastingPreCheckStatus.Success;
            }

            // limit casting time between war and void
            if (spell.School == MagicSchool.VoidMagic && LastSuccessCast_School == MagicSchool.WarMagic ||
                spell.School == MagicSchool.WarMagic && LastSuccessCast_School == MagicSchool.VoidMagic)
            {
                // roll each time?
                var timeLimit = ThreadSafeRandom.Next(3.0f, 5.0f);

                if (Time.GetUnixTime() - LastSuccessCast_Time < timeLimit)
                {
                    var curType = spell.School == MagicSchool.WarMagic ? "War" : "Void";
                    var prevType = LastSuccessCast_School == MagicSchool.VoidMagic ? "Nether" : "Elemental";

                    Session.Network.EnqueueSend(new GameMessageSystemChat($"The {prevType} energies permeating your blood cause this {curType} magic to fail.", ChatMessageType.Magic));

                    castingPreCheckStatus = CastingPreCheckStatus.CastFailed;
                }
            }

            // Calculate mana usage
            uint manaUsed = CalculateManaUsage(player, spell, target);

            var currentMana = player.Mana.Current;
            if (isWeaponSpell)
                currentMana = (uint)(caster.ItemCurMana ?? 0);

            if (manaUsed > currentMana)
            {
                player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, WeenieError.YouDontHaveEnoughManaToCast));
                IsBusy = false; // delay?
                return;
            }

            // begin spellcasting
            Proficiency.OnSuccessUse(player, player.GetCreatureSkill(Skill.ManaConversion), spell.PowerMod);

            if (!isWeaponSpell)
                player.UpdateVitalDelta(player.Mana, -(int)manaUsed);
            else
                caster.ItemCurMana -= (int)manaUsed;

            spell.Formula.GetPlayerFormula(player);

            string spellWords = spell._spellBase.GetSpellWords(DatManager.PortalDat.SpellComponentsTable);
            if (spellWords != null && !isWeaponSpell)
                EnqueueBroadcast(new GameMessageCreatureMessage(spellWords, Name, Guid.Full, ChatMessageType.Spellcasting));

            var spellChain = new ActionChain();
            var castSpeed = 2.0f;   // hardcoded for player spell casting?

            var startPos = new Position(Location);

            // do wind-up gestures: fastcast has no windup (creature enchantments)
            if (!spell.Flags.HasFlag(SpellFlags.FastCast) && !isWeaponSpell)
            {
                // note that ACE is currently sending the windup motion and the casting gesture
                // at the same time. the client is automatically queueing these animations to run at the correct time.

                foreach (var windupGesture in spell.Formula.WindupGestures)
                {
                    spellChain.AddAction(this, () =>
                    {
                        var motionWindUp = new Motion(MotionStance.Magic, windupGesture, castSpeed);
                        EnqueueBroadcastMotion(motionWindUp);
                    });
                }
            }

            // cast spell
            spellChain.AddAction(this, () =>
            {
                var castGesture = spell.Formula.CastGesture;
                if (isWeaponSpell && caster.UseUserAnimation != 0)
                    castGesture = caster.UseUserAnimation;

                var motionCastSpell = new Motion(MotionStance.Magic, castGesture, castSpeed);
                EnqueueBroadcastMotion(motionCastSpell);
            });

            var castingDelay = spell.Formula.GetCastTime(MotionTableId, castSpeed, isWeaponSpell);
            spellChain.AddDelaySeconds(castingDelay);

            bool movedTooFar = false;
            spellChain.AddAction(this, () =>
            {
                if (!isWeaponSpell)
                    TryBurnComponents(spell);

                // check windup move distance cap
                var endPos = new Position(Location);
                var dist = startPos.DistanceTo(endPos);

                // only PKs affected by these caps?
                if (dist > Windup_MaxMove && PlayerKillerStatus != PlayerKillerStatus.NPK)
                {
                    castingPreCheckStatus = CastingPreCheckStatus.CastFailed;
                    movedTooFar = true;
                }

                var pk_error = CheckPKStatusVsTarget(player, target, spell);
                if (pk_error != null)
                    castingPreCheckStatus = CastingPreCheckStatus.InvalidPKStatus;

                var useDone = WeenieError.None;

                switch (castingPreCheckStatus)
                {
                    case CastingPreCheckStatus.Success:

                        if ((spell.Flags & SpellFlags.FellowshipSpell) == 0)
                            CreatePlayerSpell(target, spell);
                        else
                        {
                            var fellows = GetFellowshipTargets();
                            foreach (var fellow in fellows)
                                CreatePlayerSpell(fellow, spell);
                        }
                        break;

                    case CastingPreCheckStatus.InvalidPKStatus:

                        if (spell.NumProjectiles > 0)
                        {
                            switch (spell.School)
                            {
                                case MagicSchool.WarMagic:
                                    WarMagic(target, spell);
                                    break;
                                case MagicSchool.VoidMagic:
                                    VoidMagic(target, spell);
                                    break;
                                case MagicSchool.LifeMagic:
                                    LifeMagic(target, spell, out uint damage, out bool critical, out var enchantmentStatus);
                                    break;
                            }
                        }
                        break;

                    default:
                        useDone = WeenieError.YourSpellFizzled;
                        EnqueueBroadcast(new GameMessageScript(Guid, ACE.Entity.Enum.PlayScript.Fizzle, 0.5f));
                        break;
                }

                // return to magic combat stance
                var returnStance = new Motion(MotionStance.Magic, MotionCommand.Ready, 1.0f);
                EnqueueBroadcastMotion(returnStance);

                if (pk_error != null && spell.NumProjectiles == 0)
                {
                    player.Session.Network.EnqueueSend(new GameEventWeenieErrorWithString(player.Session, pk_error[0], target.Name));

                    if (target is Player targetPlayer)
                        targetPlayer.Session.Network.EnqueueSend(new GameEventWeenieErrorWithString(targetPlayer.Session, pk_error[1], Name));
                }

                if (movedTooFar)
                {
                    //player.Session.Network.EnqueueSend(new GameEventWeenieError(player.Session, WeenieError.YouHaveMovedTooFar));
                    Session.Network.EnqueueSend(new GameMessageSystemChat("Your movement disrupted spell casting!", ChatMessageType.Magic));
                }

                player.SendUseDoneEvent(useDone);
            });

            spellChain.AddDelaySeconds(1.0f);   // TODO: get actual recoil timing
            spellChain.AddAction(this, () => { player.IsBusy = false; });
            spellChain.EnqueueChain();

            return;
        }

        private void CreatePlayerSpell(WorldObject target, Spell spell)
        {
            var player = this as Player;
            var creatureTarget = target as Creature;

            bool targetDeath;
            var enchantmentStatus = new EnchantmentStatus(spell);

            LastSuccessCast_School = spell.School;
            LastSuccessCast_Time = Time.GetUnixTime();

            switch (spell.School)
            {
                case MagicSchool.WarMagic:
                    WarMagic(target, spell);
                    break;
                case MagicSchool.VoidMagic:
                    VoidMagic(target, spell);
                    break;
                case MagicSchool.CreatureEnchantment:

                    if (player != null && !(target is Player))
                        player.OnAttackMonster(creatureTarget);

                    if (spell.IsHarmful)
                    {
                        var resisted = ResistSpell(target, spell);
                        if (resisted == true)
                            break;
                        if (resisted == null)
                        {
                            log.Error("Something went wrong with the Magic resistance check");
                            break;
                        }
                    }

                    EnqueueBroadcast(new GameMessageScript(target.Guid, spell.TargetEffect, spell.Formula.Scale));
                    enchantmentStatus = CreatureMagic(target, spell);
                    if (enchantmentStatus.Message != null)
                        player.Session.Network.EnqueueSend(enchantmentStatus.Message);

                    if (spell.IsHarmful)
                        Proficiency.OnSuccessUse(player, player.GetCreatureSkill(Skill.CreatureEnchantment), (target as Creature).GetCreatureSkill(Skill.MagicDefense).Current);
                    else
                        Proficiency.OnSuccessUse(player, player.GetCreatureSkill(Skill.CreatureEnchantment), spell.PowerMod);

                    break;

                case MagicSchool.LifeMagic:

                    if (player != null && !(target is Player))
                        player.OnAttackMonster(creatureTarget);

                    if (spell.MetaSpellType != SpellType.LifeProjectile)
                    {
                        if (spell.IsHarmful)
                        {
                            var resisted = ResistSpell(target, spell);
                            if (resisted == true)
                                break;
                            if (resisted == null)
                            {
                                log.Error("Something went wrong with the Magic resistance check");
                                break;
                            }
                        }
                    }

                    EnqueueBroadcast(new GameMessageScript(target.Guid, spell.TargetEffect, spell.Formula.Scale));
                    targetDeath = LifeMagic(target, spell, out uint damage, out bool critical, out enchantmentStatus);

                    if (spell.MetaSpellType != SpellType.LifeProjectile)
                    {
                        if (spell.IsHarmful)
                            Proficiency.OnSuccessUse(player, player.GetCreatureSkill(Skill.LifeMagic), (target as Creature).GetCreatureSkill(Skill.MagicDefense).Current);
                        else
                            Proficiency.OnSuccessUse(player, player.GetCreatureSkill(Skill.LifeMagic), spell.PowerMod);
                    }

                    if (targetDeath == true)
                    {
                        creatureTarget.OnDeath(this, DamageType.Health, false);
                        creatureTarget.Die();
                    }
                    else
                    {
                        if (enchantmentStatus.Message != null)
                            player.Session.Network.EnqueueSend(enchantmentStatus.Message);
                    }
                    break;

                case MagicSchool.ItemEnchantment:

                    if (spell.Category < SpellCategory.ArmorValueRaising || spell.Category > SpellCategory.AcidicResistanceLowering)
                    {
                        // Non-impen/bane spells
                        enchantmentStatus = ItemMagic(target, spell);
                        if (target.Guid == Guid)
                            EnqueueBroadcast(new GameMessageScript(Guid, spell.CasterEffect, spell.Formula.Scale));
                        else
                        {
                            if (spell.MetaSpellType == SpellType.PortalLink)
                                EnqueueBroadcast(new GameMessageScript(Guid, spell.CasterEffect, spell.Formula.Scale));
                            else
                                EnqueueBroadcast(new GameMessageScript(target.Guid, spell.TargetEffect, spell.Formula.Scale));
                        }
                        if (enchantmentStatus.Message != null)
                            player.Session.Network.EnqueueSend(enchantmentStatus.Message);
                    }
                    else
                    {
                        if ((target as Player) == null)
                        {
                            // Individual impen/bane WeenieType.Clothing target
                            enchantmentStatus = ItemMagic(target, spell);
                            if (target.Guid == Guid)
                                EnqueueBroadcast(new GameMessageScript(Guid, spell.CasterEffect, spell.Formula.Scale));
                            else
                                EnqueueBroadcast(new GameMessageScript(target.Guid, spell.TargetEffect, spell.Formula.Scale));
                            if (enchantmentStatus.Message != null)
                                player.Session.Network.EnqueueSend(enchantmentStatus.Message);
                        }
                        else
                        {
                            // Impen/bane targeted at a player
                            var items = ((Player)target).EquippedObjects.Values;
                            foreach (var item in items)
                            {
                                if (item.WeenieType == WeenieType.Clothing)
                                {
                                    enchantmentStatus = ItemMagic(item, spell);
                                    EnqueueBroadcast(new GameMessageScript(target.Guid, spell.TargetEffect, spell.Formula.Scale));
                                    if (enchantmentStatus.Message != null)
                                        player.Session.Network.EnqueueSend(enchantmentStatus.Message);
                                }
                            }
                        }
                    }
                    Proficiency.OnSuccessUse(player, player.GetCreatureSkill(Skill.ItemEnchantment), spell.PowerMod);
                    break;
            }
        }

        /// <summary>
        /// Method used for handling player untargeted spell casts
        /// </summary>
        public void CreatePlayerSpell(uint spellId)
        {
            var castingPreCheckStatus = CastingPreCheckStatus.CastFailed;

            if (IsBusy)
            {
                Session.Network.EnqueueSend(new GameEventUseDone(Session, errorType: WeenieError.YoureTooBusy));
                return;
            }
            IsBusy = true;

            var spell = new Spell(spellId);

            if (spell.NotFound)
            {
                if (spell._spellBase == null)
                {
                    Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, $"SpellId {spellId} Invalid."));
                    Session.Network.EnqueueSend(new GameEventUseDone(Session, WeenieError.None));
                }
                else
                {
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"{spell.Name} spell not implemented, yet!", ChatMessageType.System));
                    Session.Network.EnqueueSend(new GameEventUseDone(Session, WeenieError.MagicInvalidSpellType));
                }

                IsBusy = false;
                return;
            }

            // Grab player's skill level in the spell's Magic School
            var magicSkill = GetCreatureSkill(spell.School).Current;

            if ((ThreadSafeRandom.Next(0.0f, 1.0f) > (1.0f - SkillCheck.GetMagicSkillChance((int)magicSkill, (int)spell.Power)))
                && (magicSkill >= (int)spell.Power - 50) && (magicSkill > 0))
                castingPreCheckStatus = CastingPreCheckStatus.Success;

            // Calculating mana usage
            // FIXME: refactor duplicated logic between casting targeted and untargeted spells
            uint manaUsed = CalculateManaUsage(this, spell);

            if (manaUsed > Mana.Current)
            {
                Session.Network.EnqueueSend(new GameEventUseDone(Session, WeenieError.YouDontHaveEnoughManaToCast));
                IsBusy = false;  // delay?
                return;
            }

            // begin spellcasting
            UpdateVital(Mana, Mana.Current - manaUsed);
            Proficiency.OnSuccessUse(this, GetCreatureSkill(Skill.ManaConversion), spell.PowerMod);

            spell.Formula.GetPlayerFormula(this);

            string spellWords = spell._spellBase.GetSpellWords(DatManager.PortalDat.SpellComponentsTable);
            if (spellWords != null)
                EnqueueBroadcast(new GameMessageCreatureMessage(spellWords, Name, Guid.Full, ChatMessageType.Magic));

            ActionChain spellChain = new ActionChain();
            var castSpeed = 2.0f;   // hardcoded for player spell casting?

            var startPos = new Position(Location);

            // do wind-up gestures: fastcast has no windup (creature enchantments)
            if (!spell.Flags.HasFlag(SpellFlags.FastCast))
            {
                // note that ACE is currently sending the windup motion and the casting gesture
                // at the same time. the client is automatically queueing these animations to run at the correct time.

                foreach (var windupMotion in spell.Formula.WindupGestures)
                {
                    spellChain.AddAction(this, () =>
                    {
                        var motionWindUp = new Motion(MotionStance.Magic, windupMotion, castSpeed);
                        EnqueueBroadcastMotion(motionWindUp);
                    });
                }
            }
            spellChain.AddAction(this, () =>
            {
                var motionCastSpell = new Motion(MotionStance.Magic, spell.Formula.CastGesture, castSpeed);
                EnqueueBroadcastMotion(motionCastSpell);
            });

            var castingDelay = spell.Formula.GetCastTime(MotionTableId, castSpeed);
            spellChain.AddDelaySeconds(castingDelay);

            bool movedTooFar = false;

            spellChain.AddAction(this, () =>
            {
                TryBurnComponents(spell);

                var endPos = new Position(Location);
                var dist = startPos.DistanceTo(endPos);

                // only PKs affected by these caps?
                if (dist > Windup_MaxMove && PlayerKillerStatus != PlayerKillerStatus.NPK)
                {
                    castingPreCheckStatus = CastingPreCheckStatus.CastFailed;
                    movedTooFar = true;
                }

                var useDone = WeenieError.None;

                switch (castingPreCheckStatus)
                {
                    case CastingPreCheckStatus.Success:
                        // TODO - Add other untargeted spells below
                        switch (spell.School)
                        {
                            case MagicSchool.WarMagic:
                            case MagicSchool.VoidMagic:
                                WarMagic(spell);
                                break;
                            default:
                                Session.Network.EnqueueSend(new GameMessageSystemChat("Untargeted SpellID " + spellId + " not yet implemented!", ChatMessageType.System));
                                break;
                        }
                        break;
                    default:
                        useDone = WeenieError.YourSpellFizzled;
                        EnqueueBroadcast(new GameMessageScript(Guid, ACE.Entity.Enum.PlayScript.Fizzle, 0.5f));
                        break;
                }

                // return to magic combat stance
                var returnStance = new Motion(MotionStance.Magic, MotionCommand.Ready, 1.0f);
                EnqueueBroadcastMotion(returnStance);

                if (movedTooFar)
                {
                    //Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouHaveMovedTooFar));
                    Session.Network.EnqueueSend(new GameMessageSystemChat("Your movement disrupted spell casting!", ChatMessageType.Magic));
                }

                Session.Network.EnqueueSend(new GameEventUseDone(Session, useDone));
            });

            spellChain.AddDelaySeconds(1.0f);   // TODO: get actual recoil timing
            spellChain.AddAction(this, () => IsBusy = false);
            spellChain.EnqueueChain();

            return;
        }

        public void CreateSentinelBuffPlayers(IEnumerable<Player> players, bool self = false, ulong maxLevel = 8)
        {
            if (!(Session.AccessLevel >= AccessLevel.Sentinel)) return;

            var SelfOrOther = self ? "Self" : "Other";

            // ensure level 8s are installed
            var maxSpellLevel = Math.Clamp(maxLevel, 1, 8);
            if (maxSpellLevel == 8 && DatabaseManager.World.GetCachedSpell((uint)SpellId.ArmorOther8) == null)
                maxSpellLevel = 7;

            var tySpell = typeof(SpellId);
            List<BuffMessage> buffMessages = new List<BuffMessage>();
            // prepare messages
            List<string> buffsNotImplementedYet = new List<string>();
            foreach (var spell in Buffs)
            {
                var spellNamPrefix = spell;
                bool isBane = false;
                if (spellNamPrefix.StartsWith("@"))
                {
                    isBane = true;
                    spellNamPrefix = spellNamPrefix.Substring(1);
                }
                string fullSpellEnumName = spellNamPrefix + ((isBane) ? string.Empty : SelfOrOther) + maxSpellLevel;
                string fullSpellEnumNameAlt = spellNamPrefix + ((isBane) ? string.Empty : ((SelfOrOther == "Self") ? "Other" : "Self")) + maxSpellLevel;
                uint spellID = (uint)Enum.Parse(tySpell, fullSpellEnumName);
                var buffMsg = BuildBuffMessage(spellID);

                if (buffMsg == null)
                {
                    spellID = (uint)Enum.Parse(tySpell, fullSpellEnumNameAlt);
                    buffMsg = BuildBuffMessage(spellID);
                }

                if (buffMsg != null)
                {
                    buffMsg.Bane = isBane;
                    buffMessages.Add(buffMsg);
                }
                else
                {
                    buffsNotImplementedYet.Add(fullSpellEnumName);
                }
            }
            // buff each player
            players.ToList().ForEach(targetPlayer =>
            {
                if (buffMessages.Any(k => !k.Bane))
                {
                    // bake player into the messages
                    buffMessages.Where(k => !k.Bane).ToList().ForEach(k => k.SetTargetPlayer(targetPlayer));
                    // update client-side enchantments
                    targetPlayer.Session.Network.EnqueueSend(buffMessages.Where(k => !k.Bane).Select(k => k.SessionMessage).ToArray());
                    // run client-side effect scripts, omitting duplicates
                    targetPlayer.EnqueueBroadcast(buffMessages.Where(k => !k.Bane).ToList().GroupBy(m => m.Spell.TargetEffect).Select(a => a.First().LandblockMessage).ToArray());
                    // update server-side enchantments

                    var buffsForPlayer = buffMessages.Where(k => !k.Bane).ToList().Select(k => k.Enchantment);

                    var lifeBuffsForPlayer = buffsForPlayer.Where(k => k.Spell.School == MagicSchool.LifeMagic).ToList();
                    var critterBuffsForPlayer = buffsForPlayer.Where(k => k.Spell.School == MagicSchool.CreatureEnchantment).ToList();
                    var itemBuffsForPlayer = buffsForPlayer.Where(k => k.Spell.School == MagicSchool.ItemEnchantment).ToList();

                    bool crit = false;
                    uint dmg = 0;
                    EnchantmentStatus ec;
                    lifeBuffsForPlayer.ForEach(spl =>
                    {
                        bool casted = targetPlayer.LifeMagic(targetPlayer, spl.Spell, out dmg, out crit, out ec, this);
                    });
                    critterBuffsForPlayer.ForEach(spl =>
                    {
                        ec = targetPlayer.CreatureMagic(targetPlayer, spl.Spell, this);
                    });
                    itemBuffsForPlayer.ForEach(spl =>
                    {
                        ec = targetPlayer.ItemMagic(targetPlayer, spl.Spell, this);
                    });
                }
                if (buffMessages.Any(k => k.Bane))
                {
                    // Impen/bane
                    var items = targetPlayer.EquippedObjects.Values.ToList();
                    var itembuffs = buffMessages.Where(k => k.Bane).ToList();
                    foreach (var itemBuff in itembuffs)
                    {
                        foreach (var item in items)
                        {
                            if (item.WeenieType == WeenieType.Clothing || item.IsShield)
                            {
                                itemBuff.SetLandblockMessage(item.Guid);
                                var enchantmentStatus = targetPlayer.ItemMagic(item, itemBuff.Spell, this);
                                targetPlayer?.EnqueueBroadcast(itemBuff.LandblockMessage);
                            }
                        }
                    }
                }
            });
        }
        private static string[] Buffs = new string[] {
#region spells
            // @ indicates impenetrability or a bane
            "Strength",
            "Invulnerability",
            "FireProtection",
            "Armor",
            "Rejuvenation",
            "Regeneration",
            "ManaRenewal",
            "Impregnability",
            "MagicResistance",
            "AxeMastery",    // light weapons
            "DaggerMastery", // finesse weapons
            //"MaceMastery",
            //"SpearMastery",
            //"StaffMastery",
            "SwordMastery",  // heavy weapons
            //"UnarmedCombatMastery",
            "BowMastery",    // missile weapons
            //"CrossbowMastery",
            //"ThrownWeaponMastery",
            "AcidProtection",
            "CreatureEnchantmentMastery",
            "ItemEnchantmentMastery",
            "LifeMagicMastery",
            "WarMagicMastery",
            "ManaMastery",
            "ArcaneEnlightenment",
            "ArcanumSalvaging",
            "ArmorExpertise",
            "ItemExpertise",
            "MagicItemExpertise",
            "WeaponExpertise",
            "MonsterAttunement",
            "PersonAttunement",
            "DeceptionMastery",
            "HealingMastery",
            "LeadershipMastery",
            "LockpickMastery",
            "Fealty",
            "JumpingMastery",
            "Sprint",
            "BludgeonProtection",
            "ColdProtection",
            "LightningProtection",
            "BladeProtection",
            "PiercingProtection",
            "Endurance",
            "Coordination",
            "Quickness",
            "Focus",
            "Willpower",
            "CookingMastery",
            "FletchingMastery",
            "AlchemyMastery",
            "VoidMagicMastery",
            "SummoningMastery",
            "SwiftKiller",
            "Defender",
            "BloodDrinker",
            "HeartSeeker",
            "HermeticLink",
            "SpiritDrinker",
            "@Impenetrability",
            "@PiercingBane",
            "@BludgeonBane",
            "@BladeBane",
            "@AcidBane",
            "@FlameBane",
            "@FrostBane",
            "@LightningBane",
#endregion
            };

        private class BuffMessage
        {
            public bool Bane { get; set; } = false;
            public GameEventMagicUpdateEnchantment SessionMessage { get; set; } = null;
            public GameMessageScript LandblockMessage { get; set; } = null;
            public Spell Spell { get; set; } = null;
            public Enchantment Enchantment { get; set; } = null;
            public void SetTargetPlayer(Player p)
            {
                Enchantment.Target = p;
                SessionMessage = new GameEventMagicUpdateEnchantment(p.Session, Enchantment);
                SetLandblockMessage(p.Guid);
            }
            public void SetLandblockMessage(ObjectGuid target)
            {
                LandblockMessage = new GameMessageScript(target, Spell.TargetEffect, 1f);
            }
        }

        private static BuffMessage BuildBuffMessage(uint spellID)
        {
            BuffMessage buff = new BuffMessage();
            buff.Spell = new Spell(spellID);
            if (buff.Spell.NotFound) return null;
            buff.Enchantment = new Enchantment(null, 0, spellID, 1, (EnchantmentMask)buff.Spell.StatModType, buff.Spell.StatModVal);
            return buff;
        }

        public void TryBurnComponents(Spell spell)
        {
            if (SafeSpellComponents) return;

            var burned = spell.TryBurnComponents();
            if (burned.Count == 0) return;

            // decrement components
            foreach (var component in burned)
            {
                if (!SpellFormula.SpellComponentsTable.SpellComponents.TryGetValue(component, out var spellComponent))
                {
                    Console.WriteLine($"{Name}.TryBurnComponents(): Couldn't find SpellComponent {component}");
                    continue;
                }

                var wcid = Spell.GetComponentWCID(component);
                if (wcid == 0) continue;

                var item = GetInventoryItemsOfWCID(wcid).FirstOrDefault();
                if (item == null)
                {
                    Console.WriteLine($"{Name}.TryBurnComponents({spellComponent.Name}): not found in inventory");
                    continue;
                }

                item.SetStackSize(item.StackSize - 1);
                if (item.StackSize > 0)
                    Session.Network.EnqueueSend(new GameMessageSetStackSize(item));
                else
                    TryConsumeFromInventoryWithNetworking(item);
            }

            // send message to player
            var msg = Spell.GetConsumeString(burned);
            Session.Network.EnqueueSend(new GameMessageSystemChat(msg, ChatMessageType.Magic));
        }

        public static Dictionary<MagicSchool, uint> FociWCIDs = new Dictionary<MagicSchool, uint>()
        {
            { MagicSchool.CreatureEnchantment, 15268 },   // Foci of Enchantment
            { MagicSchool.ItemEnchantment,     15269 },   // Foci of Artifice
            { MagicSchool.LifeMagic,           15270 },   // Foci of Verdancy
            { MagicSchool.WarMagic,            15271 },   // Foci of Strife
            { MagicSchool.VoidMagic,           43173 },   // Foci of Shadow
        };

        public bool HasFoci(MagicSchool school)
        {
            switch (school)
            {
                case MagicSchool.CreatureEnchantment:
                    if (AugmentationInfusedCreatureMagic > 0)
                        return true;
                    break;
                case MagicSchool.ItemEnchantment:
                    if (AugmentationInfusedItemMagic > 0)
                        return true;
                    break;
                case MagicSchool.LifeMagic:
                    if (AugmentationInfusedLifeMagic > 0)
                        return true;
                    break;
                case MagicSchool.VoidMagic:
                    if (AugmentationInfusedVoidMagic > 0)
                        return true;
                    break;
                case MagicSchool.WarMagic:
                    if (AugmentationInfusedWarMagic > 0)
                        return true;
                    break;
            }

            var wcid = FociWCIDs[school];
            return Inventory.Values.FirstOrDefault(i => i.WeenieClassId == wcid) != null;
        }

        /// <summary>
        /// Returns TRUE if the currently equipped casting implement
        /// has a built-in spell
        /// </summary>
        public bool IsWeaponSpell(Spell spell)
        {
            var caster = GetEquippedWand();
            if (caster == null || caster.SpellDID == null)
                return false;

            return caster.SpellDID == spell.Id;
        }

        public void HandleSpellbookFilters(SpellBookFilterOptions filters)
        {
            Character.SpellbookFilters = (uint)filters;
        }

        public void HandleSetDesiredComponentLevel(uint component_wcid, uint amount)
        {
            // ensure wcid is spell component
            if (!SpellComponent.IsValid(component_wcid))
            {
                log.Warn($"{Name}.HandleSetDesiredComponentLevel({component_wcid}, {amount}): invalid spell component wcid");
                return;
            }
            if (amount > 0)
            {
                var existing = Character.GetFillComponent(component_wcid, CharacterDatabaseLock);

                if (existing == null)
                    Character.AddFillComponent(component_wcid, amount, CharacterDatabaseLock, out bool exists);
                else
                    existing.QuantityToRebuy = (int)amount;
            }
            else
                Character.TryRemoveFillComponent(component_wcid, out var _, CharacterDatabaseLock);

            CharacterChangesDetected = true;
        }

        public List<Player> GetFellowshipTargets()
        {
            if (Fellowship != null)
                return Fellowship.FellowshipMembers;
            else
                return new List<Player>() { this };
        }
    }
}
