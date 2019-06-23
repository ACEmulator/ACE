using System;

using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Physics.Animation;
using ACE.Server.WorldObjects.Entity;

namespace ACE.Server.WorldObjects
{
    public class Healer : WorldObject
    {
        public ushort? UsesLeft
        {
            get => Structure;
            set => Structure = value;
        }

        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Healer(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Healer(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
            BaseDescriptionFlags |= ObjectDescriptionFlag.Healer;
        }

        public override void HandleActionUseOnTarget(Player healer, WorldObject target)
        {
            if (healer.IsBusy || healer.Teleporting)
            {
                healer.SendUseDoneEvent(WeenieError.YoureTooBusy);
                return;
            }

            if (!(target is Player targetPlayer) || targetPlayer.Teleporting)
            {
                healer.SendUseDoneEvent(WeenieError.YouCantHealThat);
                return;
            }

            // Validate healing attribute against maxValue
            CreatureVital vital = GetVitalsByKitType(BoosterEnum, targetPlayer).Item2;
            if (vital.Current == vital.MaxValue)
            {
                healer.Session.Network.EnqueueSend(new GameEventWeenieErrorWithString(healer.Session, WeenieErrorWithString._IsAtFullHealth, target.Name));
                healer.SendUseDoneEvent();
                return;
            }

            if (!healer.Equals(targetPlayer))
            {
                // perform moveto
                healer.CreateMoveToChain(target, (success) => DoHealMotion(healer, targetPlayer, success));
            }
            else
                DoHealMotion(healer, targetPlayer, true);
        }

        public void DoHealMotion(Player healer, Player target, bool success)
        {
            if (!success)
            {
                healer.SendUseDoneEvent();
                return;
            }

            healer.IsBusy = true;

            var motionCommand = healer.Equals(target) ? MotionCommand.SkillHealSelf : MotionCommand.SkillHealOther;

            var motion = new Motion(healer, motionCommand);
            var animLength = MotionTable.GetAnimationLength(healer.MotionTableId, healer.CurrentMotionState.Stance, motionCommand);

            var startPos = new Position(healer.Location);

            var actionChain = new ActionChain();
            actionChain.AddAction(healer, () => healer.EnqueueBroadcastMotion(motion));
            actionChain.AddDelaySeconds(animLength);
            actionChain.AddAction(healer, () =>
            {
                // check windup move distance cap
                var endPos = new Position(healer.Location);
                var dist = startPos.DistanceTo(endPos);

                // only PKs affected by these caps?
                if (dist < Player.Windup_MaxMove || PlayerKillerStatus == PlayerKillerStatus.NPK)
                    DoHealing(healer, target);
                else
                    healer.Session.Network.EnqueueSend(new GameMessageSystemChat("Your movement disrupted healing!", ChatMessageType.Broadcast));

                healer.IsBusy = false;

                healer.SendUseDoneEvent();
            });

            healer.EnqueueMotion(actionChain, MotionCommand.Ready);

            actionChain.EnqueueChain();
        }

        public void DoHealing(Player healer, Player target)
        {
            var remainingMsg = $"Your {Name} has {--UsesLeft} uses left.";
            var stackSize = new GameMessagePublicUpdatePropertyInt(this, PropertyInt.Structure, UsesLeft.Value);
            var targetName = healer == target ? "yourself" : target.Name;
            var vitals = GetVitalsByKitType(BoosterEnum, target);

            Vital vital = vitals.Item1;
            CreatureVital creatureVital = vitals.Item2;

            // skill check
            var difficulty = 0;
            var skillCheck = DoSkillCheck(healer, target, ref difficulty);
            if (!skillCheck)
            {
                var failMsg = new GameMessageSystemChat($"You fail to heal {targetName}. {remainingMsg}", ChatMessageType.Broadcast);
                healer.Session.Network.EnqueueSend(failMsg, stackSize);
                if (healer != target)
                    target.Session.Network.EnqueueSend(new GameMessageSystemChat($"{healer.Name} fails to heal you.", ChatMessageType.Broadcast));
                if (UsesLeft <= 0)
                    healer.TryConsumeFromInventoryWithNetworking(this, 1);
                return;
            }

            // heal up
            var healAmount = GetHealAmount(healer, target, creatureVital, out var critical, out var staminaCost);

            healer.UpdateVitalDelta(healer.Stamina, (int)(-staminaCost));
            target.UpdateVitalDelta(creatureVital, healAmount);
            if (vital == Vital.Health)
                target.DamageHistory.OnHeal(healAmount);

            //if (target.Fellowship != null)
            //target.Fellowship.OnVitalUpdate(target);

            var healingSkill = healer.GetCreatureSkill(Skill.Healing);
            Proficiency.OnSuccessUse(healer, healingSkill, difficulty);

            var updateHealth = new GameMessagePrivateUpdateAttribute2ndLevel(target, vital, creatureVital.Current);
            var crit = critical ? "expertly " : "";
            GameMessageSystemChat message = null;

            if (UsesLeft <= 0)
                message = new GameMessageSystemChat($"You {crit}heal {targetName} for {healAmount} {BoosterEnum.ToString()} points with {Name}. Your {Name} is used up.", ChatMessageType.Broadcast);
            else
                message = new GameMessageSystemChat($"You {crit}heal {targetName} for {healAmount} {BoosterEnum.ToString()} points. {remainingMsg}", ChatMessageType.Broadcast);

            healer.Session.Network.EnqueueSend(message, stackSize);
            target.Session.Network.EnqueueSend(updateHealth);

            if (healer != target)
                target.Session.Network.EnqueueSend(new GameMessageSystemChat($"{healer.Name} heals you for {healAmount} {BoosterEnum.ToString()} points.", ChatMessageType.Broadcast));

            if (UsesLeft <= 0)
                healer.TryConsumeFromInventoryWithNetworking(this, 1);
        }

        /// <summary>
        /// Determines if healer successfully heals target for attempt
        /// </summary>
        public bool DoSkillCheck(Player healer, Player target, ref int difficulty)
        {
            // skill check:
            // (healing skill + healing kit boost) * trainedMod
            // vs. damage * 2 * combatMod
            var healingSkill = healer.GetCreatureSkill(Skill.Healing);
            var trainedMod = healingSkill.AdvancementClass == SkillAdvancementClass.Specialized ? 1.5f : 1.1f;

            var combatMod = healer.CombatMode == CombatMode.NonCombat ? 1.0f : 1.1f;

            var effectiveSkill = (int)Math.Round(healingSkill.Current + (Boost ?? 0) * trainedMod);
            difficulty = (int)Math.Round((target.Health.MaxValue - target.Health.Current) * 2 * combatMod);

            var skillCheck = SkillCheck.GetSkillChance(effectiveSkill, difficulty);
            return skillCheck >= ThreadSafeRandom.Next(0.0f, 1.0f);
        }

        /// <summary>
        /// Returns the healing amount for this attempt
        /// </summary>
        public uint GetHealAmount(Player healer, Player target, CreatureVital vital, out bool criticalHeal, out uint staminaCost)
        {
            // factors: healing skill, healing kit bonus, stamina, critical chance
            var healingSkill = healer.GetCreatureSkill(Skill.Healing).Current;
            var healBase = healingSkill * (float)HealkitMod.Value;

            // todo: determine applicable range from pcaps
            var healMin = healBase * 0.2f;      // ??
            var healMax = healBase * 0.5f;
            var healAmount = ThreadSafeRandom.Next(healMin, healMax);

            // verify healing boost comes from target instead of healer?
            // sounds like target in LumAugHealingRating...
            var healRatingMod = Creature.GetPositiveRatingMod(target.GetHealingBoostRating());
            var healResistRatingMod = Creature.GetNegativeRatingMod(target.GetHealingResistRating());

            healAmount *= healRatingMod * healResistRatingMod;

            // chance for critical healing
            criticalHeal = ThreadSafeRandom.Next(0.0f, 1.0f) < 0.1f;
            if (criticalHeal) healAmount *= 2;

            // cap to missing health
            var missingHealth = vital.MaxValue - vital.Current;
            if (healAmount > missingHealth)
                healAmount = missingHealth;

            // stamina check? On the Q&A board a dev posted that stamina directly effects the amount of damage you can heal
            // low stam = less health healed. I don't have exact numbers for it. Working through forum archive.

            // stamina cost: 1 stamina per 5 health
            staminaCost = (uint)Math.Round(healAmount / 5.0f);
            if (staminaCost > healer.Stamina.Current)
            {
                staminaCost = healer.Stamina.Current;
                healAmount = staminaCost * 5;
            }
            return (uint)Math.Round(healAmount);
        }

        public PropertyAttribute2nd BoosterEnum
        {
            get => (PropertyAttribute2nd)(GetProperty(PropertyInt.BoosterEnum) ?? (int)PropertyAttribute2nd.Undef);
            set { if (value == 0) RemoveProperty(PropertyInt.BoosterEnum); else SetProperty(PropertyInt.BoosterEnum, (int)value); }
        }

        /// <summary>
        /// Returns the appropriate Vital and CreatureVital for a given healing kit type
        /// </summary>
        private Tuple<Vital, CreatureVital> GetVitalsByKitType(PropertyAttribute2nd type, Player target)
        {
            Vital vital = Vital.Undefined;
            CreatureVital creatureVital = null;
            switch (BoosterEnum)
            {
                case PropertyAttribute2nd.Health:
                    goto default;
                case PropertyAttribute2nd.Stamina:
                    vital = Vital.Stamina;
                    creatureVital = target.Stamina;
                    break;
                case PropertyAttribute2nd.Mana:
                    vital = Vital.Mana;
                    creatureVital = target.Mana;
                    break;
                default:
                    vital = Vital.Health;
                    creatureVital = target.Health;
                    break;
            }
            return Tuple.Create(vital, creatureVital);
        }
    }
}
