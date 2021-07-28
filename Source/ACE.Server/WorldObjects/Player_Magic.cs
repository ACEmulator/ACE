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
        // TODO: get rid of this, only used for determining if TurnTo is required
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

        public bool DebugSpell { get; set; }

        public string DebugDamageBuffer { get; set; }

        public RecordCast RecordCast { get; set; }

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
            //Console.WriteLine($"{Name}.HandleActionCastTargetedSpell({targetGuid:X8}, {spellId}, {builtInSpell})");

            if (CombatMode != CombatMode.Magic)
            {
                log.Error($"{Name}.HandleActionCastTargetedSpell({targetGuid:X8}, {spellId}, {builtInSpell}) - CombatMode mismatch {CombatMode}, LastCombatMode: {LastCombatMode}");

                if (LastCombatMode == CombatMode.Magic)
                    CombatMode = CombatMode.Magic;
                else
                {
                    SendUseDoneEvent();
                    return;
                }
            }

            if (FastTick && PhysicsObj.MovementManager.MotionInterpreter.InterpretedState.CurrentStyle != (uint)MotionStance.Magic)
            {
                log.Warn($"{Name} CombatMode: {CombatMode}, CurrentMotionState: {CurrentMotionState.Stance}.{CurrentMotionState.MotionState.ForwardCommand}, Physics: {(MotionStance)PhysicsObj.MovementManager.MotionInterpreter.InterpretedState.CurrentStyle}.{(MotionCommand)PhysicsObj.MovementManager.MotionInterpreter.InterpretedState.ForwardCommand}");
                ApplyPhysicsMotion(new Motion(MotionStance.Magic));
                SendUseDoneEvent(WeenieError.YoureTooBusy);
                return;
            }

            if (IsJumping)
            {
                SendUseDoneEvent(WeenieError.YouCantDoThatWhileInTheAir);
                return;
            }

            if (PKLogout)
            {
                SendUseDoneEvent(WeenieError.YouHaveBeenInPKBattleTooRecently);
                return;
            }

            if (IsBusy && MagicState.CanQueue)
            {
                MagicState.CastQueue = new CastQueue(CastQueueType.Targeted, targetGuid, spellId, builtInSpell);
                MagicState.CanQueue = false;
                return;
            }

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

            MagicState.OnCastStart();
            MagicState.SetWindupParams(targetGuid, spellId, builtInSpell);

            StartPos = new Physics.Common.Position(PhysicsObj.Position);

            if (RecordCast.Enabled)
                RecordCast.OnCastTargetedSpell(new Spell(spellId), target);

            if (targetCategory != TargetCategory.WorldObject && targetCategory != TargetCategory.Wielded)
            {
                if (!CreatePlayerSpell(target, targetCategory, spellId, builtInSpell))
                    MagicState.OnCastDone();

                return;
            }

            // start turning
            if (!FastTick)
            {
                var rotateTarget = target;
                if (rotateTarget.WielderId != null)
                    rotateTarget = CurrentLandblock?.GetObject(rotateTarget.WielderId.Value);

                var rotateTime = Rotate(rotateTarget);
                var actionChain = new ActionChain();
                actionChain.AddDelaySeconds(rotateTime);

                actionChain.AddAction(this, () =>
                {
                    // ensure target still exists
                    targetCategory = GetTargetCategory(targetGuid, spellId, out target);

                    if (target == null)
                    {
                        SendUseDoneEvent(WeenieError.TargetNotAcquired);
                        MagicState.OnCastDone();
                        return;
                    }

                    if (!CreatePlayerSpell(target, targetCategory, spellId, builtInSpell))
                        MagicState.OnCastDone();
                });

                actionChain.EnqueueChain();
            }
            else
                TurnTo_Magic(target);
        }

        public void DoWindup(WindupParams windupParams, bool checkAngle)
        {
            //Console.WriteLine($"{Name}.DoWindup()");

            // ensure target still exists
            var targetCategory = GetTargetCategory(windupParams.TargetGuid, windupParams.SpellId, out var target);

            if (target == null)
            {
                SendUseDoneEvent(WeenieError.TargetNotAcquired);
                MagicState.OnCastDone();
                return;
            }

            if (!checkAngle || IsWithinAngle(target))
            {
                if (!CreatePlayerSpell(target, targetCategory, windupParams.SpellId, windupParams.BuiltInSpell))
                    MagicState.OnCastDone();
            }
            else
            {
                // restart turn if required
                if (PhysicsObj.MovementManager.MotionInterpreter.InterpretedState.TurnCommand == 0)
                    TurnTo_Magic(target);
                else
                    MagicState.PendingTurnRelease = true;
            }
        }

        public TargetCategory GetTargetCategory(uint targetGuid, uint spellId, out WorldObject target)
        {
            // fellowship spell
            var spell = new Spell(spellId);
            if ((spell.Flags & SpellFlags.FellowshipSpell) != 0)
            {
                target = this;
                return TargetCategory.Fellowship;
            }

            // direct landblock object
            target = CurrentLandblock?.GetObject(targetGuid);

            if (target != null)
                return targetGuid == Guid.Full ? TargetCategory.Self : TargetCategory.WorldObject;

            // self-wielded
            target = GetEquippedItem(targetGuid);
            if (target != null)
                return TargetCategory.Inventory;

            // inventory item
            target = GetInventoryItem(targetGuid);
            if (target != null)
                return TargetCategory.Inventory;

            // other selectable wielded
            target = CurrentLandblock?.GetWieldedObject(targetGuid, true);
            if (target != null)
                return TargetCategory.Wielded;

            // known trade objects
            var tradePartner = GetKnownTradeObj(new ObjectGuid(targetGuid));
            if (tradePartner != null)
            {
                target = tradePartner.GetEquippedItem(targetGuid);
                if (target != null)
                    return TargetCategory.Wielded;

                target = tradePartner.GetInventoryItem(targetGuid);
                if (target != null)
                    return TargetCategory.Inventory;
            }

            return TargetCategory.Undef;
        }

        /// <summary>
        /// Handles player untargeted casting message
        /// </summary>
        public void HandleActionMagicCastUnTargetedSpell(uint spellId)
        {
            //Console.WriteLine($"{Name}.HandleActionCastUnTargetedSpell({spellId})");

            if (CombatMode != CombatMode.Magic)
            {
                log.Error($"{Name}.HandleActionMagicCastUnTargetedSpell({spellId}) - CombatMode mismatch {CombatMode}, LastCombatMode {LastCombatMode}");

                if (LastCombatMode == CombatMode.Magic)
                    CombatMode = CombatMode.Magic;
                else
                {
                    SendUseDoneEvent();
                    return;
                }
            }

            if (FastTick && PhysicsObj.MovementManager.MotionInterpreter.InterpretedState.CurrentStyle != (uint)MotionStance.Magic)
            {
                log.Warn($"{Name} CombatMode: {CombatMode}, CurrentMotionState: {CurrentMotionState.Stance}.{CurrentMotionState.MotionState.ForwardCommand}, Physics: {(MotionStance)PhysicsObj.MovementManager.MotionInterpreter.InterpretedState.CurrentStyle}.{(MotionCommand)PhysicsObj.MovementManager.MotionInterpreter.InterpretedState.ForwardCommand}");
                ApplyPhysicsMotion(new Motion(MotionStance.Magic));
                SendUseDoneEvent(WeenieError.YoureTooBusy);
                return;
            }

            if (IsJumping)
            {
                SendUseDoneEvent(WeenieError.YouCantDoThatWhileInTheAir);
                return;
            }

            if (PKLogout)
            {
                SendUseDoneEvent(WeenieError.YouHaveBeenInPKBattleTooRecently);
                return;
            }

            if (IsBusy && MagicState.CanQueue)
            {
                MagicState.CastQueue = new CastQueue(CastQueueType.Untargeted, 0, spellId, false);
                MagicState.CanQueue = false;
                return;
            }

            if (!VerifyBusy())
                return;

            // verify spell is contained in player's spellbook,
            // or in the weapon's spellbook in the case of built-in spells
            if (!VerifySpell(spellId))
                return;

            if (RecordCast.Enabled)
                RecordCast.OnCastUntargetedSpell(new Spell(spellId));

            MagicState.OnCastStart();

            StartPos = new Physics.Common.Position(PhysicsObj.Position);

            if (!CreatePlayerSpell(spellId))
                MagicState.OnCastDone();
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
            if (IsBusy || Teleporting || suicideInProgress)
            {
                SendUseDoneEvent(WeenieError.YoureTooBusy);
                return false;
            }
            return true;
        }

        public Spell ValidateSpell(uint spellId, bool isWeaponSpell = false)
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
            if (!isWeaponSpell && !HasComponentsForSpell(spell))
            {
                SendUseDoneEvent(WeenieError.YouDontHaveAllTheComponents);
                return null;
            }

            return spell;
        }

        public bool VerifySpellTarget(Spell spell, WorldObject target)
        {
            if (IsInvalidTarget(spell, target))
            {
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, $"{spell.Name} cannot be cast on {target.Name}."));
                SendUseDoneEvent(WeenieError.None);
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

            // ensure target is enchantable
            if (!target.IsEnchantable) return true;

            // Self targeted spells should have a target of self
            if (spell.Flags.HasFlag(SpellFlags.SelfTargeted) && target != this)
                return true;

            // Invalidate non Item Enchantment spells cast against non Creatures or Players
            if (spell.School != MagicSchool.ItemEnchantment && targetCreature == null)
                return true;

            // Invalidate beneficial spells against Creature/Non-player targets
            if (targetCreature != null && targetPlayer == null && spell.IsBeneficial)
                return true;

            // check item spells
            if (targetCreature == null && target.WielderId != null)
            {
                var parent = CurrentLandblock.GetObject(target.WielderId.Value) as Player;

                // Invalidate beneficial spells against monster wielded items
                if (parent == null && spell.IsBeneficial)
                    return true;

                // Invalidate harmful spells against player wielded items, depending on pk status
                if (parent != null && spell.IsHarmful && CheckPKStatusVsTarget(parent, spell) != null)
                    return true;
            }

            // verify target type for item enchantment
            if (spell.School == MagicSchool.ItemEnchantment && !VerifyNonComponentTargetType(spell, target))
            {
                if (spell.DispelSchool != MagicSchool.ItemEnchantment || !PropertyManager.GetBool("item_dispel").Item)
                    return true;
            }

            // brittlemail / lure / other negative item spells cannot be cast with player as target

            // TODO: by end of retail, players couldn't cast any negative spells on themselves
            // this feature is currently in ace for dev testing...
            if (target == this && spell.IsNegativeRedirectable)
                return true;

            if (targetCreature != null && targetCreature != this && spell.NonComponentTargetType == ItemType.Creature && !CanDamage(targetCreature))
                return true;

            return false;
        }

        public bool VerifySpellRange(WorldObject target, TargetCategory targetCategory, Spell spell, uint magicSkill)
        {
            if (targetCategory != TargetCategory.WorldObject && targetCategory != TargetCategory.Wielded || target.Guid == Guid)
                return true;

            var targetLoc = target;
            if (targetLoc.WielderId != null)
                targetLoc = CurrentLandblock?.GetObject(targetLoc.WielderId.Value);

            float distanceTo = Location.Distance2D(targetLoc.Location);

            var maxRange = Math.Min(spell.BaseRangeConstant + magicSkill * spell.BaseRangeMod, MaxRadarRange_Outdoors);

            if (distanceTo > maxRange)
            {
                SendUseDoneEvent(WeenieError.MissileOutOfRange);
                return false;
            }

            // bootstrapping this function for indoor/outdoor check, since it is called both before and after windup
            if (spell.Flags.HasFlag(SpellFlags.NotIndoor))
            {
                if (Location.Indoors || target != null && target.Location.Indoors)
                {
                    SendUseDoneEvent(WeenieError.YourSpellCannotBeCastInside);
                    return false;
                }
            }
            if (spell.Flags.HasFlag(SpellFlags.NotOutdoor))
            {
                if (!Location.Indoors || target != null && !target.Location.Indoors)
                {
                    SendUseDoneEvent(WeenieError.YourSpellCannotBeCastOutside);
                    return false;
                }
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
                EnqueueBroadcast(new GameMessageHearSpeech(spellWords, Name, Guid.Full, ChatMessageType.Spellcasting), LocalBroadcastRange, ChatMessageType.Spellcasting);
        }

        public static float CastSpeed = 2.0f;       // from retail pcaps, player animation speed for windup / first half of cast gesture

        public void DoWindupGestures(Spell spell, bool isWeaponSpell, ActionChain castChain)
        {
            if (spell.Flags.HasFlag(SpellFlags.FastCast) || isWeaponSpell)
                return;

            if (FastTick)
            {
                castChain.AddAction(this, () =>
                {
                    PhysicsObj.StopCompletely(false);

                    MagicState.TurnStarted = false;
                    MagicState.IsTurning = false;
                });
            }

            var windupTime = 0.0f;

            foreach (var windupGesture in spell.Formula.WindupGestures)
            {
                if (RecordCast.Enabled)
                {
                    castChain.AddAction(this, () =>
                    {
                        var animLength = Physics.Animation.MotionTable.GetAnimationLength(MotionTableId, CurrentMotionState.Stance, windupGesture, CastSpeed);
                        RecordCast.Log($"Windup Gesture: {windupGesture}, Windup Time: {animLength}");
                    });
                }

                // don't mess with CurrentMotionState here?
                if (!FastTick)
                    windupTime = EnqueueMotionMagic(castChain, windupGesture, CastSpeed);

                /*Console.WriteLine($"{spell.Name}");
                Console.WriteLine($"Windup Gesture: " + windupGesture);
                Console.WriteLine($"Windup time: " + windupTime);
                Console.WriteLine("-------");*/
            }

            if (FastTick)
                windupTime = EnqueueMotionAction(castChain, spell.Formula.WindupGestures, CastSpeed, MotionStance.Magic);
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

            if (RecordCast.Enabled)
            {
                castChain.AddAction(this, () =>
                {
                    var animLength = Physics.Animation.MotionTable.GetAnimationLength(MotionTableId, CurrentMotionState.Stance, MagicState.CastGesture, CastSpeed);
                    RecordCast.Log($"Cast Gesture: {MagicState.CastGesture}, Cast Time: {animLength}");
                });
            }

            castChain.AddAction(this, () =>
            {
                MagicState.CastGestureStartTime = DateTime.UtcNow;

                if (FastTick)
                    PhysicsObj.StopCompletely(false);
            });

            if (MagicState.CastGesture == MotionCommand.Invalid)
                MagicState.CastGesture = MotionCommand.Ready;

            var castTime = 0.0f;
            if (FastTick)
                castTime = EnqueueMotion(castChain, MagicState.CastGesture, CastSpeed, true, null, true);
            else
                castTime = EnqueueMotionMagic(castChain, MagicState.CastGesture, CastSpeed);

            //Console.WriteLine($"Cast Gesture: " + MagicState.CastGesture);
            //Console.WriteLine($"Cast time: " + castTime);
        }

        // 20 from MoveToManager threshold?
        public static readonly float MaxAngle = 5;

        public void DoCastSpell(MagicState _state, bool checkAngle = true)
        {
            //Console.WriteLine("DoCastSpell");

            if (!MagicState.IsCasting)
                return;

            var state = _state?.CastSpellParams;

            if (state == null)
            {
                log.Warn($"{Name}.DoCastSpell(): null state detected");
                log.Warn(_state);

                // send UseDone?
                SendUseDoneEvent(WeenieError.BadCast);

                return;
            }

            DoCastSpell(state.Spell, state.IsWeaponSpell, state.MagicSkill, state.ManaUsed, state.Target, state.Status, checkAngle);
        }

        public bool IsWithinAngle(WorldObject target)
        {
            // TODO: investigate this more, difference for GetAngle() between ACE and ac physics engine
            var angle = 0.0f;
            if (target != this)
            {
                if (target.CurrentLandblock == null)
                {
                    FindObject(target.Guid.Full, SearchLocations.Everywhere, out _, out var rootOwner, out _);

                    if (rootOwner == null)
                        log.Error($"{Name}.IsWithinAngle({target.Name} ({target.Guid})) - couldn't find rootOwner");

                    else if (rootOwner != this)
                        angle = GetAngle(rootOwner);
                }
                else
                    angle = GetAngle(target);
            }

            //Console.WriteLine($"Angle: " + angle);
            var maxAngle = PropertyManager.GetDouble("spellcast_max_angle").Item;

            if (RecordCast.Enabled)
                RecordCast.Log($"DoCastSpell(angle={angle} vs. {maxAngle})");

            return angle <= maxAngle;
        }

        public void DoCastSpell(Spell spell, bool isWeaponSpell, uint magicSkill, uint manaUsed, WorldObject target, CastingPreCheckStatus castingPreCheckStatus, bool checkAngle = true)
        {
            if (target != null)
            {
                // verify target still exists
                var targetCategory = GetTargetCategory(target.Guid.Full, spell.Id, out target);

                if (target == null)
                {
                    SendWeenieError(WeenieError.TargetNotAcquired);
                    FinishCast();
                    return;
                }

                // do second rotate, if applicable
                // TODO: investigate this more, difference for GetAngle() between ACE and ac physics engine
                if (checkAngle && !IsWithinAngle(target))
                {
                    if (!FastTick)
                    {
                        var rotateTime = Rotate(target);

                        var actionChain = new ActionChain();
                        actionChain.AddDelaySeconds(rotateTime);
                        actionChain.AddAction(this, () => DoCastSpell(spell, isWeaponSpell, magicSkill, manaUsed, target, castingPreCheckStatus, false));
                        actionChain.EnqueueChain();
                    }
                    else
                    {
                        if (PhysicsObj.MovementManager.MotionInterpreter.InterpretedState.TurnCommand == 0)
                            TurnTo_Magic(target);
                        else
                            MagicState.PendingTurnRelease = true;
                    }

                    return;
                }

                // verify spell range
                if (!VerifySpellRange(target, targetCategory, spell, magicSkill))
                {
                    FinishCast();
                    return;
                }
            }

            if (IsDead)
            {
                FinishCast();
                return;
            }

            DoCastSpell_Inner(spell, isWeaponSpell, manaUsed, target, castingPreCheckStatus);
        }

        public WorldObject TurnTarget;

        public void TurnTo_Magic(WorldObject target)
        {
            //Console.WriteLine($"{Name}.TurnTo_Magic()");
            TurnTarget = target;

            MagicState.TurnStarted = true;
            MagicState.IsTurning = true;

            if (FastTick)
            {
                if (PropertyManager.GetDouble("spellcast_max_angle").Item > 5.0f && IsWithinAngle(target))
                {
                    // emulate current gdle TurnTo - doesn't match retail, but some players may prefer this
                    OnMoveComplete_Magic(WeenieError.None);
                    return;
                }

                // verify cast radius before every automatic TurnTo after windup
                if (!VerifyCastRadius())
                    return;

                var stopCompletely = !MagicState.CastMotionDone;
                //var stopCompletely = true;

                CreateTurnToChain2(target, null, null, stopCompletely, MagicState.AlwaysTurn);

                MagicState.AlwaysTurn = false;
            }
        }

        public Physics.Common.Position StartPos { get; set; }

        public void DoCastSpell_Inner(Spell spell, bool isWeaponSpell, uint manaUsed, WorldObject target, CastingPreCheckStatus castingPreCheckStatus, bool finishCast = true)
        {
            if (RecordCast.Enabled)
                RecordCast.Log($"DoCastSpell_Inner()");

            if (MagicState.CastMeter)
            {
                var gestureTime = Physics.Animation.MotionTable.GetAnimationLength(MotionTableId, CurrentMotionState.Stance, MagicState.CastGesture, CastSpeed);
                var castTime = DateTime.UtcNow - MagicState.CastGestureStartTime;
                var efficiency = 1.0f - (float)castTime.TotalSeconds / gestureTime;
                var msg = $"Cast efficiency: {efficiency * 100}%";
                Session.Network.EnqueueSend(new GameMessageSystemChat(msg, ChatMessageType.Broadcast));
            }

            // consume mana
            var caster = GetEquippedWand();  // TODO: persist this from the beginning, since this is done with delay

            var itemCaster = isWeaponSpell ? caster : null;

            if (!isWeaponSpell)
                UpdateVitalDelta(Mana, -(int)manaUsed);
            else
            {
                if (itemCaster != null)
                    itemCaster.ItemCurMana -= (int)manaUsed;
                else
                    castingPreCheckStatus = CastingPreCheckStatus.CastFailed;
            }

            // consume spell components
            if (!isWeaponSpell)
                TryBurnComponents(spell);

            // check windup move distance cap
            var dist = StartPos.Distance(PhysicsObj.Position);

            // only PKs affected by these caps?
            if (dist > Windup_MaxMove && PlayerKillerStatus != PlayerKillerStatus.NPK)
            {
                //player.Session.Network.EnqueueSend(new GameEventWeenieError(player.Session, WeenieError.YouHaveMovedTooFar));
                Session.Network.EnqueueSend(new GameMessageSystemChat("Your movement disrupted spell casting!", ChatMessageType.Magic));

                EnqueueBroadcast(new GameMessageScript(Guid, PlayScript.Fizzle, 0.5f));

                if (finishCast)
                    FinishCast();

                return;
            }

            var pk_error = CheckPKStatusVsTarget(target, spell);
            if (pk_error != null)
                castingPreCheckStatus = CastingPreCheckStatus.InvalidPKStatus;

            switch (castingPreCheckStatus)
            {
                case CastingPreCheckStatus.Success:

                    if ((spell.Flags & SpellFlags.FellowshipSpell) == 0)
                        CreatePlayerSpell(target, spell, isWeaponSpell);
                    else
                    {
                        var fellows = GetFellowshipTargets();
                        foreach (var fellow in fellows)
                            CreatePlayerSpell(fellow, spell, isWeaponSpell);
                    }

                    // handle self procs
                    if (spell.IsHarmful && target != this)
                        TryProcEquippedItems(this, this, true, caster);

                    break;

                case CastingPreCheckStatus.InvalidPKStatus:

                    if (spell.NumProjectiles > 0)
                    {
                        switch (spell.School)
                        {
                            case MagicSchool.WarMagic:
                                WarMagic(target, spell, caster, isWeaponSpell);
                                break;
                            case MagicSchool.VoidMagic:
                                VoidMagic(target, spell, caster, isWeaponSpell);
                                break;
                            case MagicSchool.LifeMagic:
                                LifeMagic(spell, out uint damage, out var enchantmentStatus, target, itemCaster, caster, isWeaponSpell);
                                break;
                        }
                    }
                    break;

                default:
                    EnqueueBroadcast(new GameMessageScript(Guid, PlayScript.Fizzle, 0.5f));
                    SendWeenieError(WeenieError.YourSpellFizzled);
                    break;
            }


            if (pk_error != null && spell.NumProjectiles == 0)
            {
                Session.Network.EnqueueSend(new GameEventWeenieErrorWithString(Session, pk_error[0], target.Name));

                if (target is Player targetPlayer)
                    targetPlayer.Session.Network.EnqueueSend(new GameEventWeenieErrorWithString(targetPlayer.Session, pk_error[1], Name));
            }

            if (finishCast)
                FinishCast();
        }

        public void FinishCast()
        {
            var hasWindupGestures = MagicState.CastSpellParams?.HasWindupGestures ?? true;
            var castGesture = MagicState.CastGesture;

            if (FastTick)
                castGesture = hasWindupGestures ? CurrentMotionState.MotionState.ForwardCommand : MagicState.CastGesture;

            var selfTarget = !hasWindupGestures && MagicState.CastSpellParams.Target == this;

            MagicState.OnCastDone();

            IsBusy = true;

            var queue = PropertyManager.GetBool("spellcast_recoil_queue").Item;

            if (queue)
                MagicState.CanQueue = true;

            if (FastTick)
            {
                var fastbuff = selfTarget && PropertyManager.GetBool("fastbuff").Item;

                // return to magic ready stance
                var actionChain = new ActionChain();
                EnqueueMotion(actionChain, MotionCommand.Ready, 1.0f, true, castGesture, false, fastbuff);
                actionChain.AddAction(this, () =>
                {
                    IsBusy = false;
                    SendUseDoneEvent();

                    if (queue)
                        HandleCastQueue();

                    //Console.WriteLine("====================================");
                });
                actionChain.EnqueueChain();
            }
            else
            {
                // temporarily old version:

                // return to magic combat stance
                var returnStance = new Motion(MotionStance.Magic, MotionCommand.Ready, 1.0f);
                EnqueueBroadcastMotion(returnStance);

                var actionChain = new ActionChain();
                actionChain.AddDelaySeconds(1.0f);   // TODO: get actual recoil timing
                actionChain.AddAction(this, () => {

                    IsBusy = false;
                    SendUseDoneEvent();

                    if (queue)
                        HandleCastQueue();
                });
                actionChain.EnqueueChain();
            }

        }

        /// <summary>
        /// Method used for handling player targeted spell casts
        /// </summary>
        /// <param name="builtInSpell">If TRUE, casting a built-in spell from a weapon</param>
        public bool CreatePlayerSpell(WorldObject target, TargetCategory targetCategory, uint spellId, bool builtInSpell = false)
        {
            var creatureTarget = target as Creature;

            var spell = ValidateSpell(spellId, builtInSpell);
            if (spell == null)
                return false;

            if (!VerifySpellTarget(spell, target))
                return false;

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
                return false;

            // get casting pre-check status
            var castingPreCheckStatus = GetCastingPreCheckStatus(spell, magicSkill, isWeaponSpell);

            // calculate mana usage
            if (!CalculateManaUsage(castingPreCheckStatus, spell, target, isWeaponSpell, out var manaUsed))
                return false;

            // spell words
            DoSpellWords(spell, isWeaponSpell);

            var spellChain = new ActionChain();
            //StartPos = new Physics.Common.Position(PhysicsObj.Position);

            // do wind-up gestures: fastcast has no windup (creature enchantments)
            DoWindupGestures(spell, isWeaponSpell, spellChain);

            // cast spell
            DoCastGesture(spell, isWeaponSpell, spellChain);

            MagicState.SetCastParams(spell, isWeaponSpell, magicSkill, manaUsed, target, castingPreCheckStatus);

            if (!FastTick)
                spellChain.AddAction(this, () => DoCastSpell(MagicState));

            spellChain.EnqueueChain();

            return true;
        }

        public List<Player> GetFellowshipTargets()
        {
            if (Fellowship != null)
                return Fellowship.GetFellowshipMembers().Values.ToList();
            else
                return new List<Player>() { this };
        }

        private void CreatePlayerSpell(WorldObject target, Spell spell, bool isWeaponSpell)
        {
            var targetCreature = target as Creature;
            var targetPlayer = target as Player;

            bool targetDeath;
            var enchantmentStatus = new EnchantmentStatus(spell);

            LastSuccessCast_School = spell.School;
            LastSuccessCast_Time = Time.GetUnixTime();

            var caster = GetEquippedWand();

            var itemCaster = isWeaponSpell ? caster : null;

            // verify after windup, still consumes mana
            if (spell.MetaSpellType == SpellType.Dispel && !VerifyDispelPKStatus(this, target))
                return;

            switch (spell.School)
            {
                case MagicSchool.WarMagic:
                    WarMagic(target, spell, caster, isWeaponSpell);
                    break;
                case MagicSchool.VoidMagic:
                    VoidMagic(target, spell, caster, isWeaponSpell);
                    break;

                case MagicSchool.CreatureEnchantment:

                    if (targetPlayer == null)
                        OnAttackMonster(targetCreature);

                    if (TryResistSpell(target, spell, itemCaster))
                        break;

                    if (targetCreature != null && targetCreature.NonProjectileMagicImmune)
                    {
                        Session.Network.EnqueueSend(new GameMessageSystemChat($"You fail to affect {targetCreature.Name} with {spell.Name}", ChatMessageType.Magic));
                        break;
                    }

                    EnqueueBroadcast(new GameMessageScript(target.Guid, spell.TargetEffect, spell.Formula.Scale));
                    enchantmentStatus = CreatureMagic(target, spell);
                    if (enchantmentStatus.Message != null)
                        Session.Network.EnqueueSend(enchantmentStatus.Message);

                    if (spell.IsHarmful)
                    {
                        if (targetCreature != null)
                            Proficiency.OnSuccessUse(this, GetCreatureSkill(Skill.CreatureEnchantment), targetCreature.GetCreatureSkill(Skill.MagicDefense).Current);

                        // handle target procs
                        if (targetCreature != null && targetCreature != this)
                            TryProcEquippedItems(this, targetCreature, false, caster);

                        if (targetPlayer != null)
                            UpdatePKTimers(this, targetPlayer);
                    }
                    else
                        Proficiency.OnSuccessUse(this, GetCreatureSkill(Skill.CreatureEnchantment), spell.PowerMod);

                    break;

                case MagicSchool.LifeMagic:

                    if (spell.MetaSpellType != SpellType.LifeProjectile)
                    {
                        if (targetPlayer == null)
                            OnAttackMonster(targetCreature);

                        if (TryResistSpell(target, spell, itemCaster))
                            break;

                        if (targetCreature != null && targetCreature.NonProjectileMagicImmune)
                        {
                            Session.Network.EnqueueSend(new GameMessageSystemChat($"You fail to affect {targetCreature.Name} with {spell.Name}", ChatMessageType.Magic));
                            break;
                        }
                    }

                    targetDeath = LifeMagic(spell, out uint damage, out enchantmentStatus, target, itemCaster, caster, isWeaponSpell);

                    if (spell.MetaSpellType != SpellType.LifeProjectile)
                    {
                        if (target != null)
                            EnqueueBroadcast(new GameMessageScript(target.Guid, spell.TargetEffect, spell.Formula.Scale));

                        if (spell.IsHarmful)
                        {
                            if (targetCreature != null)
                                Proficiency.OnSuccessUse(this, GetCreatureSkill(Skill.LifeMagic), targetCreature.GetCreatureSkill(Skill.MagicDefense).Current);

                            // handle target procs
                            if (targetCreature != null && targetCreature != this)
                                TryProcEquippedItems(this, targetCreature, false, caster);

                            if (targetPlayer != null)
                                UpdatePKTimers(this, targetPlayer);
                        }
                        else
                            Proficiency.OnSuccessUse(this, GetCreatureSkill(Skill.LifeMagic), spell.PowerMod);
                    }

                    if (targetDeath == true)
                    {
                        targetCreature.OnDeath(new DamageHistoryInfo(this), DamageType.Health, false);
                        targetCreature.Die();
                    }
                    else
                    {
                        if (enchantmentStatus.Message != null)
                            Session.Network.EnqueueSend(enchantmentStatus.Message);
                    }
                    break;

                case MagicSchool.ItemEnchantment:

                    TryCastItemEnchantment_WithRedirects(spell, target, itemCaster);

                    // use target resistance?
                    Proficiency.OnSuccessUse(this, GetCreatureSkill(Skill.ItemEnchantment), spell.PowerMod);

                    if (spell.IsHarmful)
                    {
                        var playerRedirect = targetPlayer;
                        if (playerRedirect == null && target?.WielderId != null)
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
        public bool CreatePlayerSpell(uint spellId)
        {
            var spell = ValidateSpell(spellId);
            if (spell == null)
                return false;

            // get player's current magic skill
            var magicSkill = GetCreatureSkill(spell.School).Current;

            var castingPreCheckStatus = GetCastingPreCheckStatus(spell, magicSkill, false);

            // calculate mana usage
            if (!CalculateManaUsage(castingPreCheckStatus, spell, null, false, out var manaUsed))
                return false;

            // begin spellcasting
            DoSpellWords(spell, false);

            var spellChain = new ActionChain();

            //StartPos = new Physics.Common.Position(PhysicsObj.Position);

            // do wind-up gestures: fastcast has no windup (creature enchantments)
            DoWindupGestures(spell, false, spellChain);

            // do cast gesture
            DoCastGesture(spell, false, spellChain);

            // cast untargeted spell
            MagicState.SetCastParams(spell, false, magicSkill, manaUsed, null, castingPreCheckStatus);

            if (!FastTick)
                spellChain.AddAction(this, () => DoCastSpell(MagicState));

            spellChain.EnqueueChain();

            return true;
        }

        public void TryBurnComponents(Spell spell)
        {
            if (SafeSpellComponents || PropertyManager.GetBool("safe_spell_comps").Item)
                return;

            var burned = spell.TryBurnComponents(this);
            if (burned.Count == 0) return;

            // decrement components
            for (var i = burned.Count - 1; i >= 0; i--)
            {
                var component = burned[i];

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
                    if (SpellComponentsRequired && PropertyManager.GetBool("require_spell_comps").Item)
                        log.Warn($"{Name}.TryBurnComponents({spellComponent.Name}): not found in inventory");
                    else
                        burned.RemoveAt(i);

                    continue;
                }
                TryConsumeFromInventoryWithNetworking(item, 1);
            }

            if (burned.Count == 0)
                return;

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

                    LifeMagic(spell, out uint damage, out enchantmentStatus, player);
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

            if (!SpellComponentsRequired || !PropertyManager.GetBool("require_spell_comps").Item)
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
            //Console.WriteLine($"HandleMotionDone_Magic({(MotionCommand)motionID}, {success})");

            if (!FastTick || !MagicState.IsCasting) return;

            if (motionID == (uint)MagicState.CastGesture)
            {
                if (RecordCast.Enabled)
                    RecordCast.Log($"{Name}.HandleMotionDone_Magic({(MotionCommand)motionID}, {success}) - cast gesture done");

                MagicState.CastMotionDone = true;

                var actionChain = new ActionChain();
                actionChain.AddDelayForOneTick();
                actionChain.AddAction(this, () =>
                {
                    if (!MagicState.IsCasting)
                        return;

                    MagicState.AlwaysTurn = true;

                    DoCastSpell(MagicState);
                });
                actionChain.EnqueueChain();
            }
        }

        public void OnMoveComplete_Magic(WeenieError status)
        {
            //Console.WriteLine($"OnMoveComplete_Magic({status})");

            if (!FastTick || !MagicState.IsCasting || !MagicState.TurnStarted)
                return;

            // this occurs after the player is done turning
            // before the windup, or after the first half of the cast motion
            // either completed or cancelled

            if (RecordCast.Enabled)
                RecordCast.Log($"{Name}.OnMoveComplete_Magic({status}) - DoCastSpell");

            MagicState.IsTurning = false;

            var checkAngle = status != WeenieError.None;

            var actionChain = new ActionChain();
            actionChain.AddDelayForOneTick();
            actionChain.AddAction(this, () =>
            {
                if (!MagicState.IsCasting) return;

                if (!MagicState.CastMotionDone)
                    DoWindup(MagicState.WindupParams, checkAngle);
                else
                    DoCastSpell(MagicState, checkAngle);
            });
            actionChain.EnqueueChain();
        }

        public void FailCast(bool tryFizzle = true)
        {
            var parms = MagicState.CastSpellParams;

            var werror = WeenieError.None;

            if (parms != null && tryFizzle)
            {
                DoCastSpell_Inner(parms.Spell, parms.IsWeaponSpell, parms.ManaUsed, parms.Target, CastingPreCheckStatus.CastFailed, false);

                werror = WeenieError.YourSpellFizzled;
            }
            SendUseDoneEvent(werror);

            MagicState.OnCastDone();
        }

        public void OnTurnRelease()
        {
            MagicState.PendingTurnRelease = false;

            if (!MagicState.CastMotionDone)
                DoWindup(MagicState.WindupParams, true);
            else
                DoCastSpell(MagicState, true);
        }

        public bool VerifyCastRadius()
        {
            if (MagicState.CastGestureStartTime != DateTime.MinValue)
            {
                var dist = StartPos.Distance(PhysicsObj.Position);

                if (dist > Windup_MaxMove && PlayerKillerStatus != PlayerKillerStatus.NPK)
                {
                    FailCast();
                    return false;
                }
            }
            return true;
        }

        public void CheckTurn()
        {
            // verify cast radius while manually moving after windup
            if (!VerifyCastRadius())
                return;

            if (TurnTarget != null && IsWithinAngle(TurnTarget))
            {
                if (MagicState.PendingTurnRelease)
                    OnTurnRelease();
                else
                    PhysicsObj.StopCompletely(false);
            }
        }

        public void HandleCastQueue()
        {
            MagicState.CanQueue = false;

            if (MagicState.CastQueue != null)
            {
                if (MagicState.CastQueue.Type == CastQueueType.Targeted)
                    HandleActionCastTargetedSpell(MagicState.CastQueue.TargetGuid, MagicState.CastQueue.SpellId, MagicState.CastQueue.BuiltInSpell);
                else
                    HandleActionMagicCastUnTargetedSpell(MagicState.CastQueue.SpellId);
            }
        }

        public static bool VerifyNonComponentTargetType(Spell spell, WorldObject target)
        {
            // untargeted spell projectiles
            if (target == null)
                return spell.NonComponentTargetType == ItemType.None;

            switch (spell.NonComponentTargetType)
            {
                case ItemType.Creature:
                    return target is Creature;

                // banes / lures
                case ItemType.Vestements:
                    //return target is Clothing || target.IsShield;
                    return target is Creature || target is Clothing || target.IsShield;

                case ItemType.Weapon:
                    //return target is MeleeWeapon || target is MissileLauncher;
                    return target is Creature || target is MeleeWeapon || target is MissileLauncher;

                case ItemType.Caster:
                    //return target is Caster;
                    return target is Creature || target is Caster;

                case ItemType.WeaponOrCaster:
                    //return target is MeleeWeapon || target is MissileLauncher || target is Caster;
                    return target is Creature || target is MeleeWeapon || target is MissileLauncher || target is Caster;

                case ItemType.Portal:

                    if (spell.MetaSpellType == SpellType.PortalRecall || spell.MetaSpellType == SpellType.PortalSummon)
                        return target is Creature;
                    else
                        return target is Portal;

                case ItemType.LockableMagicTarget:
                    return target is Door || target is Chest;

                // Essence Lull?
                case ItemType.Item:
                    return !(target is Creature);

                case ItemType.LifeStone:
                    return target is Lifestone;
            }

            log.Error($"VerifyNonComponentTargetType({spell.Id} - {spell.Name}, {target.Name}) - unexpected NonComponentTargetType {spell.NonComponentTargetType}");
            return false;
        }
    }
}
