using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Common;
using ACE.DatLoader;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Managers;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Physics;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        public enum TargetCategory
        {
            Undef,
            WorldObject,
            Wielded,
            Inventory,
            Self,
            Fellowship
        }

        public MagicState MagicState;

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
        /// <param name="builtInSpell">If TRUE, casting a built-in spell from a weapon</param>
        public void HandleActionCastTargetedSpell(uint targetGuid, uint spellId, bool builtInSpell = false)
        {
            //Console.WriteLine($"{Name}.HandleActionCastTargetedSpell({targetGuid:X8}, {spellId}, {builtInSpell}");
            if (!VerifyBusy())
                return;

            // verify spell is contained in player's spellbook,
            // or in the weapon's spellbook in the case of built-in spells
            if (!VerifySpell(spellId, builtInSpell))
            {
                SendUseDoneEvent(WeenieError.MagicInvalidSpellType);
                return;
            }

            var targetCategory = GetTargetCategory(targetGuid, spellId, out var target);

            if (target == null || target.Teleporting)
            {
                SendUseDoneEvent(WeenieError.TargetNotAcquired);
                return;
            }

            if (targetCategory != TargetCategory.WorldObject && targetCategory != TargetCategory.Wielded)
            {
                CreatePlayerSpell(target, targetCategory, spellId, builtInSpell);
            }
            else
            {
                var rotateTarget = target;
                if (rotateTarget.WielderId != null)
                    rotateTarget = CurrentLandblock?.GetObject(rotateTarget.WielderId.Value);

                // turn if required
                CreateTurnToChain(rotateTarget, (success) =>
                {
                    if (!success)
                    {
                        SendUseDoneEvent(WeenieError.ActionCancelled);
                        return;
                    }

                    // ensure target still exists
                    targetCategory = GetTargetCategory(targetGuid, spellId, out target);

                    if (target == null)
                    {
                        SendUseDoneEvent(WeenieError.TargetNotAcquired);
                        return;
                    }

                    CreatePlayerSpell(target, targetCategory, spellId, builtInSpell);
                });
            }
        }

        public TargetCategory GetTargetCategory(uint targetGuid, uint spellId, out WorldObject target)
        {
            target = CurrentLandblock?.GetObject(targetGuid);
            var targetCategory = TargetCategory.Undef;

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

            return targetCategory;
        }

        /// <summary>
        /// Handles player untargeted casting message
        /// </summary>
        public void HandleActionMagicCastUnTargetedSpell(uint spellId)
        {
            if (!VerifyBusy())
                return;

            // verify spell is contained in player's spellbook,
            // or in the weapon's spellbook in the case of built-in spells
            if (!VerifySpell(spellId))
                return;

            CreatePlayerSpell(spellId);
        }

        /// <summary>
        /// Verifies spell is contained in player's spellbook,
        /// or in the weapon's spellbook in the case of built-in spells
        /// </summary>
        /// <param name="builtInSpell">If TRUE, casting a built-in spell from a weapon</param>
        public bool VerifySpell(uint spellId, bool builtInSpell = false)
        {
            if (builtInSpell)
                return IsWeaponSpell(spellId);
            else
                return SpellIsKnown(spellId);

            // send error message?
        }

        /// <summary>
        /// Returns TRUE if the currently equipped casting implement
        /// has a built-in spell
        /// </summary>
        public bool IsWeaponSpell(uint spellId)
        {
            var caster = GetEquippedWand();
            if (caster == null || caster.SpellDID == null)
                return false;

            return caster.SpellDID == spellId;
        }

        public enum CastingPreCheckStatus
        {
            CastFailed,
            InvalidPKStatus,
            Success
        }

        public static float Windup_MaxMove = 6.0f;
        public static float Windup_MaxMoveSq = Windup_MaxMove * Windup_MaxMove;

        public bool VerifyBusy()
        {
            if (IsBusy || Teleporting)
            {
                SendUseDoneEvent(WeenieError.YoureTooBusy);
                return false;
            }
            return true;
        }

        public Spell ValidateSpell(uint spellId)
        {
            var spell = new Spell(spellId);

            if (spell.NotFound)
            {
                if (spell._spellBase == null)
                {
                    Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, $"SpellId {spell.Id} Invalid."));
                    SendUseDoneEvent(WeenieError.None);
                }
                else
                {
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"{spell.Name} spell not implemented, yet!", ChatMessageType.System));
                    SendUseDoneEvent(WeenieError.MagicInvalidSpellType);
                }
                return null;
            }
            if (!HasComponentsForSpell(spell))
            {
                SendUseDoneEvent(WeenieError.YouDontHaveAllTheComponents);
                return null;
            }

            return spell;
        }

        public bool VerifySpellTarget(Spell spell, WorldObject target)
        {
            if (IsInvalidTarget(this, spell, target))
            {
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, $"{spell.Name} cannot be cast on {target.Name}."));
                SendUseDoneEvent(WeenieError.None);
                return false;
            }
            return true;
        }

        public bool VerifySpellRange(WorldObject target, TargetCategory targetCategory, Spell spell, uint magicSkill)
        {
            if (targetCategory != TargetCategory.WorldObject || target.Guid == Guid)
                return true;

            var targetLoc = target;
            if (targetLoc.WielderId != null)
                targetLoc = CurrentLandblock?.GetObject(targetLoc.WielderId.Value);

            float distanceTo = Location.Distance2D(targetLoc.Location);

            if (distanceTo > spell.BaseRangeConstant + magicSkill * spell.BaseRangeMod)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"Target is out of range!", ChatMessageType.Magic));
                SendUseDoneEvent(WeenieError.None);
                return false;
            }
            return true;
        }

        public CastingPreCheckStatus GetCastingPreCheckStatus(Spell spell, uint magicSkill, bool isWeaponSpell)
        {
            var difficulty = spell.Power;

            var castingPreCheckStatus = CastingPreCheckStatus.CastFailed;

            if (magicSkill > 0 && magicSkill >= (int)difficulty - 50)
            {
                var chance = SkillCheck.GetMagicSkillChance((int)magicSkill, (int)difficulty);
                var rng = ThreadSafeRandom.Next(0.0f, 1.0f);
                if (chance > rng)
                    castingPreCheckStatus = CastingPreCheckStatus.Success;
            }

            // build-in spells never fizzle
            if (isWeaponSpell)
                castingPreCheckStatus = CastingPreCheckStatus.Success;

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
            return castingPreCheckStatus;
        }

        public bool CalculateManaUsage(CastingPreCheckStatus castingPreCheckStatus, Spell spell, WorldObject target, bool isWeaponSpell, out uint manaUsed)
        {
            manaUsed = 0;
            if (castingPreCheckStatus == CastingPreCheckStatus.Success)
                manaUsed = CalculateManaUsage(this, spell, target);
            else if (castingPreCheckStatus == CastingPreCheckStatus.CastFailed)
                manaUsed = 5;   // todo: verify with retail

            var currentMana = Mana.Current;
            if (isWeaponSpell)
            {
                var caster = GetEquippedWand();
                currentMana = (uint)(caster.ItemCurMana ?? 0);
            }

            if (manaUsed > currentMana)
            {
                SendUseDoneEvent(WeenieError.YouDontHaveEnoughManaToCast);
                return false;
            }

            Proficiency.OnSuccessUse(this, GetCreatureSkill(Skill.ManaConversion), spell.PowerMod);

            return true;
        }

        public void DoSpellWords(Spell spell, bool isWeaponSpell)
        {
            spell.Formula.GetPlayerFormula(this);

            var spellWords = spell._spellBase.GetSpellWords(DatManager.PortalDat.SpellComponentsTable);
            if (!string.IsNullOrWhiteSpace(spellWords) && !isWeaponSpell)
                EnqueueBroadcast(new GameMessageCreatureMessage(spellWords, Name, Guid.Full, ChatMessageType.Spellcasting), LocalBroadcastRange);
        }

        public static new float CastSpeed = 2.0f;       // from retail pcaps, player animation speed for windup / first half of cast gesture

        public void DoWindupGestures(Spell spell, bool isWeaponSpell, ActionChain castChain)
        {
            if (!spell.Flags.HasFlag(SpellFlags.FastCast) && !isWeaponSpell)
            {
                foreach (var windupGesture in spell.Formula.WindupGestures)
                {
                    var windupTime = EnqueueMotion(castChain, windupGesture, CastSpeed);

                    /*Console.WriteLine($"{spell.Name}");
                    Console.WriteLine($"Windup Gesture: " + windupGesture);
                    Console.WriteLine($"Windup time: " + windupTime);
                    Console.WriteLine("-------");*/
                }
            }
        }

        public void DoCastGesture(Spell spell, bool isWeaponSpell, ActionChain castChain)
        {
            MagicState.CastGesture = spell.Formula.CastGesture;
            if (isWeaponSpell)
            {
                var caster = GetEquippedWand();
                if (caster.UseUserAnimation != 0)
                    MagicState.CastGesture = caster.UseUserAnimation;
            }

            var castTime = EnqueueMotion(castChain, MagicState.CastGesture, CastSpeed);

            //Console.WriteLine($"Cast Gesture: " + MagicState.CastGesture);
            //Console.WriteLine($"Cast time: " + castTime);
        }

        // 20 from MoveToManager threshold?
        public static readonly float MaxAngle = 5;

        public void DoCastSpell(MagicState _state)
        {
            var state = _state.CastSpellParams;

            DoCastSpell(state.Spell, state.IsWeaponSpell, state.ManaUsed, state.Target, state.Status);
        }

        public void DoCastSpell(Spell spell, bool isWeaponSpell, uint manaUsed, WorldObject target, CastingPreCheckStatus castingPreCheckStatus)
        {
            if (target != null)
            {
                // verify target still exists
                var targetCategory = GetTargetCategory(target.Guid.Full, spell.Id, out target);

                if (target == null)
                {
                    FinishCast(WeenieError.TargetNotAcquired);
                    return;
                }

                // do second rotate, if applicable
                // TODO: investigate this more, difference for GetAngle() between ACE and ac physics engine
                var angle = 0.0f;
                if (target != this)
                    angle = Math.Abs(GetAngle_Physics2(target));

                if (angle > MaxAngle)
                {
                    TurnTo_Magic(target);
                    return;
                }
            }
            DoCastSpell_Inner(spell, isWeaponSpell, manaUsed, target, castingPreCheckStatus);
        }

        public void TurnTo_Magic(WorldObject target)
        {
            //Console.WriteLine($"{Name}.TurnTo_Magic()");

            CreateTurnToChain(target, null);

            MagicState.CastTurn = true;
            MagicState.CastTurnStarted = true;
        }

        public Position StartPos;

        public void DoCastSpell_Inner(Spell spell, bool isWeaponSpell, uint manaUsed, WorldObject target, CastingPreCheckStatus castingPreCheckStatus)
        {
            // consume mana
            if (!isWeaponSpell)
                UpdateVitalDelta(Mana, -(int)manaUsed);
            else
            {
                var caster = GetEquippedWand();     // TODO: persist this from the beginning, since this is done with delay
                caster.ItemCurMana -= (int)manaUsed;
            }

            // consume spell components
            if (!isWeaponSpell)
                TryBurnComponents(spell);

            // check windup move distance cap
            var endPos = new Position(Location);
            var dist = StartPos.DistanceTo(endPos);

            bool movedTooFar = false;

            // only PKs affected by these caps?
            if (dist > Windup_MaxMove && PlayerKillerStatus != PlayerKillerStatus.NPK)
            {
                castingPreCheckStatus = CastingPreCheckStatus.CastFailed;
                movedTooFar = true;
            }

            var pk_error = CheckPKStatusVsTarget(this, target, spell);
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

                    // handle self procs
                    if (spell.IsHarmful && target != this)
                        TryProcEquippedItems(this, true);

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
                                LifeMagic(spell, out uint damage, out bool critical, out var enchantmentStatus, target);
                                break;
                        }
                    }
                    break;

                default:
                    useDone = WeenieError.YourSpellFizzled;
                    EnqueueBroadcast(new GameMessageScript(Guid, PlayScript.Fizzle, 0.5f));
                    break;
            }


            if (pk_error != null && spell.NumProjectiles == 0)
            {
                Session.Network.EnqueueSend(new GameEventWeenieErrorWithString(Session, pk_error[0], target.Name));

                if (target is Player targetPlayer)
                    targetPlayer.Session.Network.EnqueueSend(new GameEventWeenieErrorWithString(targetPlayer.Session, pk_error[1], Name));
            }

            if (movedTooFar)
            {
                //player.Session.Network.EnqueueSend(new GameEventWeenieError(player.Session, WeenieError.YouHaveMovedTooFar));
                Session.Network.EnqueueSend(new GameMessageSystemChat("Your movement disrupted spell casting!", ChatMessageType.Magic));
            }

            FinishCast(useDone);
        }

        public void FinishCast(WeenieError useDone)
        {
            var queue = PropertyManager.GetBool("spellcast_recoil_queue").Item;

            if (queue)
                MagicState.OnCastDone();

            // return to magic ready stance
            var actionChain = new ActionChain();
            EnqueueMotion(actionChain, MotionCommand.Ready, 1.0f, true, true);
            actionChain.AddAction(this, () =>
            {
                if (!queue)
                    MagicState.OnCastDone();

                SendUseDoneEvent(useDone);

                //Console.WriteLine("====================================");
            });
            actionChain.EnqueueChain();
        }

        /// <summary>
        /// Method used for handling player targeted spell casts
        /// </summary>
        /// <param name="builtInSpell">If TRUE, casting a built-in spell from a weapon</param>
        public void CreatePlayerSpell(WorldObject target, TargetCategory targetCategory, uint spellId, bool builtInSpell = false)
        {
            var creatureTarget = target as Creature;

            if (!VerifyBusy())
                return;

            var spell = ValidateSpell(spellId);
            if (spell == null)
                return;

            if (!VerifySpellTarget(spell, target))
                return;

            // if casting implement has spell built in,
            // use spellcraft from the item, instead of player's magic skill?
            var caster = GetEquippedWand();
            var isWeaponSpell = builtInSpell && IsWeaponSpell(spell.Id);

            // Grab player's skill level in the spell's Magic School
            var magicSkill = GetCreatureSkill(spell.School).Current;
            if (isWeaponSpell && caster.ItemSpellcraft != null)
                magicSkill = (uint)caster.ItemSpellcraft;

            // verify spell range
            if (!VerifySpellRange(target, targetCategory, spell, magicSkill))
                return;

            // get casting pre-check status
            var castingPreCheckStatus = GetCastingPreCheckStatus(spell, magicSkill, isWeaponSpell);

            // calculate mana usage
            if (!CalculateManaUsage(castingPreCheckStatus, spell, target, isWeaponSpell, out var manaUsed))
                return;

            // spell words
            DoSpellWords(spell, isWeaponSpell);

            // begin spellcasting
            MagicState.OnCastStart();

            var spellChain = new ActionChain();
            StartPos = new Position(Location);

            // do wind-up gestures: fastcast has no windup (creature enchantments)
            DoWindupGestures(spell, isWeaponSpell, spellChain);

            // cast spell
            DoCastGesture(spell, isWeaponSpell, spellChain);

            //spellChain.AddAction(this, () => DoCastSpell(spell, isWeaponSpell, manaUsed, target, castingPreCheckStatus, spellChain));
            MagicState.SetCastParams(spell, isWeaponSpell, manaUsed, target, castingPreCheckStatus);

            spellChain.EnqueueChain();
        }

        public List<Player> GetFellowshipTargets()
        {
            if (Fellowship != null)
                return Fellowship.GetFellowshipMembers().Values.ToList();
            else
                return new List<Player>() { this };
        }

        private void CreatePlayerSpell(WorldObject target, Spell spell)
        {
            var targetCreature = target as Creature;
            var targetPlayer = target as Player;

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

                    if (targetPlayer == null)
                        OnAttackMonster(targetCreature);

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
                        Session.Network.EnqueueSend(enchantmentStatus.Message);

                    if (spell.IsHarmful)
                    {
                        Proficiency.OnSuccessUse(this, GetCreatureSkill(Skill.CreatureEnchantment), targetCreature.GetCreatureSkill(Skill.MagicDefense).Current);

                        // handle target procs
                        if (targetCreature != null && targetCreature != this)
                            TryProcEquippedItems(targetCreature, false);

                        if (targetPlayer != null)
                            UpdatePKTimers(this, targetPlayer);
                    }
                    else
                        Proficiency.OnSuccessUse(this, GetCreatureSkill(Skill.CreatureEnchantment), spell.PowerMod);

                    break;

                case MagicSchool.LifeMagic:

                    if (targetPlayer == null)
                        OnAttackMonster(targetCreature);

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
                    targetDeath = LifeMagic(spell, out uint damage, out bool critical, out enchantmentStatus, target);

                    if (spell.MetaSpellType != SpellType.LifeProjectile)
                    {
                        if (spell.IsHarmful)
                        {
                            Proficiency.OnSuccessUse(this, GetCreatureSkill(Skill.LifeMagic), targetCreature.GetCreatureSkill(Skill.MagicDefense).Current);

                            // handle target procs
                            if (targetCreature != null && targetCreature != this)
                                TryProcEquippedItems(targetCreature, false);

                            if (targetPlayer != null)
                                UpdatePKTimers(this, targetPlayer);
                        }
                        else
                            Proficiency.OnSuccessUse(this, GetCreatureSkill(Skill.LifeMagic), spell.PowerMod);
                    }

                    if (targetDeath == true)
                    {
                        targetCreature.OnDeath(this, DamageType.Health, false);
                        targetCreature.Die();
                    }
                    else
                    {
                        if (enchantmentStatus.Message != null)
                            Session.Network.EnqueueSend(enchantmentStatus.Message);
                    }
                    break;

                case MagicSchool.ItemEnchantment:

                    // if negative item spell, can be resisted by the wielder
                    if (spell.IsHarmful)
                    {
                        var targetResist = targetCreature;

                        if (targetResist == null && target.WielderId != null)
                            targetResist = CurrentLandblock?.GetObject(target.WielderId.Value) as Creature;

                        if (targetResist != null)
                        {
                            var resisted = ResistSpell(targetResist, spell);
                            if (resisted == true)
                                break;
                            if (resisted == null)
                            {
                                log.Error("Something went wrong with the Magic resistance check");
                                break;
                            }
                        }
                    }

                    if (spell.IsImpenBaneType)
                    {
                        // impen / bane / brittlemail / lure

                        // a lot of these will already be filtered out by IsInvalidTarget()
                        if (targetCreature == null)
                        {
                            // targeting an individual item / wo
                            enchantmentStatus = ItemMagic(target, spell);

                            EnqueueBroadcast(new GameMessageScript(target.Guid, spell.TargetEffect, spell.Formula.Scale));

                            if (enchantmentStatus.Message != null)
                                Session.Network.EnqueueSend(enchantmentStatus.Message);
                        }
                        else
                        {
                            // targeting a creature
                            if (targetPlayer == this)
                            {
                                // targeting self
                                var items = EquippedObjects.Values.Where(i => (i.WeenieType == WeenieType.Clothing || i.IsShield) && i.IsEnchantable);

                                foreach (var item in items)
                                {
                                    enchantmentStatus = ItemMagic(item, spell);
                                    if (enchantmentStatus.Message != null)
                                        Session.Network.EnqueueSend(enchantmentStatus.Message);
                                }
                                if (items.Count() > 0)
                                    EnqueueBroadcast(new GameMessageScript(Guid, spell.TargetEffect, spell.Formula.Scale));
                            }
                            else
                            {
                                // targeting another player or monster
                                var item = targetCreature.EquippedObjects.Values.FirstOrDefault(i => i.IsShield && i.IsEnchantable);

                                if (item != null)
                                {
                                    enchantmentStatus = ItemMagic(item, spell);
                                    EnqueueBroadcast(new GameMessageScript(item.Guid, spell.TargetEffect, spell.Formula.Scale));
                                    if (enchantmentStatus.Message != null)
                                        Session.Network.EnqueueSend(enchantmentStatus.Message);
                                }
                                else
                                {
                                    // 'fails to affect'?
                                    Session.Network.EnqueueSend(new GameMessageSystemChat($"You fail to affect {targetCreature.Name} with {spell.Name}", ChatMessageType.Magic));

                                    if (targetPlayer != null)
                                        targetPlayer.Session.Network.EnqueueSend(new GameMessageSystemChat($"{Name} fails to affect you with {spell.Name}", ChatMessageType.Magic));
                                }
                            }
                        }
                    }
                    else if (spell.IsOtherNegativeRedirectable)
                    {
                        // blood loather, spirit loather, lure blade, turn blade, leaden weapon, hermetic void
                        if (targetCreature == null)
                        {
                            // targeting an individual item / wo
                            enchantmentStatus = ItemMagic(target, spell);

                            EnqueueBroadcast(new GameMessageScript(target.Guid, spell.TargetEffect, spell.Formula.Scale));

                            if (enchantmentStatus.Message != null)
                                Session.Network.EnqueueSend(enchantmentStatus.Message);
                        }
                        else
                        {
                            // targeting a creature, try to redirect to primary weapon
                            var weapon = targetCreature.GetEquippedWeapon() ?? targetCreature.GetEquippedWand();

                            if (weapon != null)
                            {
                                enchantmentStatus = ItemMagic(weapon, spell);

                                EnqueueBroadcast(new GameMessageScript(weapon.Guid, spell.TargetEffect, spell.Formula.Scale));

                                if (enchantmentStatus.Message != null)
                                    Session.Network.EnqueueSend(enchantmentStatus.Message);
                            }
                            else
                            {
                                // 'fails to affect'?
                                Session.Network.EnqueueSend(new GameMessageSystemChat($"You fail to affect {targetCreature.Name} with {spell.Name}", ChatMessageType.Magic));

                                if (targetPlayer != null)
                                    targetPlayer.Session.Network.EnqueueSend(new GameMessageSystemChat($"{Name} fails to affect you with {spell.Name}", ChatMessageType.Magic));
                            }
                        }
                    }
                    else
                    {
                        // all other item spells, cast directly on target
                        enchantmentStatus = ItemMagic(target, spell);

                        EnqueueBroadcast(new GameMessageScript(target.Guid, spell.TargetEffect, spell.Formula.Scale));

                        if (enchantmentStatus.Message != null)
                            Session.Network.EnqueueSend(enchantmentStatus.Message);
                    }

                    // use target resistance?
                    Proficiency.OnSuccessUse(this, GetCreatureSkill(Skill.ItemEnchantment), spell.PowerMod);

                    if (spell.IsHarmful)
                    {
                        var playerRedirect = targetPlayer;
                        if (playerRedirect == null && target.WielderId != null)
                            playerRedirect = CurrentLandblock?.GetObject(target.WielderId.Value) as Player;

                        if (playerRedirect != null)
                            UpdatePKTimers(this, playerRedirect);
                    }
                    break;
            }
        }

        /// <summary>
        /// Method used for handling player untargeted spell casts
        /// </summary>
        public void CreatePlayerSpell(uint spellId)
        {
            if (!VerifyBusy())
                return;

            var spell = ValidateSpell(spellId);
            if (spell == null)
                return;

            // get player's current magic skill
            var magicSkill = GetCreatureSkill(spell.School).Current;

            var castingPreCheckStatus = GetCastingPreCheckStatus(spell, magicSkill, false);

            // calculate mana usage
            if (!CalculateManaUsage(castingPreCheckStatus, spell, null, false, out var manaUsed))
                return;

            // begin spellcasting
            DoSpellWords(spell, false);

            MagicState.OnCastStart();

            var spellChain = new ActionChain();

            StartPos = new Position(Location);

            // do wind-up gestures: fastcast has no windup (creature enchantments)
            DoWindupGestures(spell, false, spellChain);

            // do cast gesture
            DoCastGesture(spell, false, spellChain);

            // cast untargeted spell
            //DoCastSpell(spell, false, manaUsed, null, castingPreCheckStatus, spellChain);
            MagicState.SetCastParams(spell, false, manaUsed, null, castingPreCheckStatus);

            spellChain.EnqueueChain();
        }

        public void TryBurnComponents(Spell spell)
        {
            if (SafeSpellComponents) return;

            var burned = spell.TryBurnComponents(this);
            if (burned.Count == 0) return;

            // decrement components
            foreach (var component in burned)
            {
                if (!SpellFormula.SpellComponentsTable.SpellComponents.TryGetValue(component, out var spellComponent))
                {
                    log.Error($"{Name}.TryBurnComponents(): Couldn't find SpellComponent {component}");
                    continue;
                }

                var wcid = Spell.GetComponentWCID(component);
                if (wcid == 0) continue;

                var item = GetInventoryItemsOfWCID(wcid).FirstOrDefault();
                if (item == null)
                {
                    log.Warn($"{Name}.TryBurnComponents({spellComponent.Name}): not found in inventory");
                    continue;
                }

                TryConsumeFromInventoryWithNetworking(item, 1);
            }

            // send message to player
            var msg = Spell.GetConsumeString(burned);
            Session.Network.EnqueueSend(new GameMessageSystemChat(msg, ChatMessageType.Magic));
        }

        /// <summary>
        /// Handles equipping an item casting a spell on a player
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

        /// <summary>
        /// Called from consumables or sentinel buffs
        /// </summary>
        /// <returns>TRUE if cast success, or FALSE is spell doesn't exist on server</returns>
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

                    LifeMagic(spell, out uint damage, out bool critical, out enchantmentStatus, player);
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
        /// Returns TRUE if the player has the required number of components to cast spell
        /// </summary>
        public bool HasComponentsForSpell(Spell spell)
        {
            spell.Formula.GetPlayerFormula(this);

            if (!SpellComponentsRequired)
                return true;

            var requiredComps = spell.Formula.GetRequiredComps();

            foreach (var kvp in requiredComps)
            {
                var wcid = kvp.Key;
                var required = kvp.Value;

                var available = GetNumInventoryItemsOfWCID(wcid);

                if (required > available)
                    return false;
            }
            return true;
        }

        public void HandleMotionDone_Magic(uint motionID, bool success)
        {
            if (!MagicState.IsCasting) return;

            if (motionID == (uint)MagicState.CastGesture)
            {
                //Console.WriteLine($"{Name}.HandleMotionDone_Magic({(MotionCommand)motionID}, {success}) - cast gesture done");
                MagicState.CastMotionDone = true;
                DoCastSpell(MagicState);
            }

            // this occurs after a normal secondary turn that has been cancelled
            // we handle this separately here, else the player re-turns too quickly
            if (MagicState.CastTurn && PhysicsObj.MovementManager.MoveToManager.PendingActions.Count == 0)
            {
                //Console.WriteLine($"{Name}.HandleMotionDone_Magic({(MotionCommand)motionID}, {success}) - turn done");
                MagicState.CastTurn = false;
                DoCastSpell(MagicState);
            }
        }

        public void OnMotionQueueDone_Magic()
        {
            if (!MagicState.IsCasting || !MagicState.CastTurn) return;

            if (PhysicsObj.MovementManager.MoveToManager.PendingActions.Count > 0)
            {
                // this occurs after a jammed up secondary turn
                //Console.WriteLine($"{Name}.OnMotionQueueDone_Magic() - restarting bugged turn");
                PhysicsObj.cancel_moveto();
            }

            //Console.WriteLine($"{Name}.OnMotionQueueDone_Magic() - DoCastSpell");
            MagicState.CastTurn = false;
            DoCastSpell(MagicState);
        }

        public void OnMoveComplete_Magic(WeenieError status, int cycles)
        {
            if (!MagicState.IsCasting || !MagicState.CastTurnStarted || status != WeenieError.None)
                return;

            // this occurs after a normal secondary turn
            //Console.WriteLine($"{Name}.OnMoveComplete_Magic({status}) - DoCastSpell");
            MagicState.CastTurn = false;
            DoCastSpell(MagicState);
        }
    }
}
