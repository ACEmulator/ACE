using System;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Motion;
using ACE.Server.Physics.Animation;

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
        public void HandleActionUseOnTarget(Player healer, Player target)
        {
            if (target.Health.Current == target.Health.MaxValue)
            {
                healer.Session.Network.EnqueueSend(new GameEventUseDone(healer.Session, WeenieError.None));
                return;
            }
            DoHealMotion(healer, target);
        }

        public void DoHealMotion(Player healer, Player target)
        {
            var healAnimation = new MotionItem(MotionCommand.SkillHealSelf, 1.0f);
            var animLength = MotionTable.GetAnimationLength(healer.MotionTableId, healer.CurrentMotionState.Stance, healAnimation);

            var motion = new UniversalMotion(healer.CurrentMotionState.Stance, healAnimation);
            motion.MovementData.CurrentStyle = (uint)healer.CurrentMotionState.Stance;
            healer.CurrentMotionState = motion;

            var actionChain = new ActionChain();
            actionChain.AddAction(healer, () => healer.DoMotion(motion));
            actionChain.AddDelaySeconds(animLength);
            actionChain.AddAction(healer, () =>
            {
                DoHealing(healer, target);
                healer.Session.Network.EnqueueSend(new GameEventUseDone(healer.Session, WeenieError.None));
            });
            actionChain.EnqueueChain();
        }

        public void DoHealing(Player healer, Player target)
        {
            var remainingMsg = $"Your {Name} has {--UsesLeft} uses left.";
            var stackSize = new GameMessagePublicUpdatePropertyInt(this, PropertyInt.Structure, UsesLeft.Value);
            var targetName = healer == target ? "yourself" : target.Name;

            // skill check
            var skillCheck = DoSkillCheck(healer, target);
            if (!skillCheck)
            {
                var failMsg = new GameMessageSystemChat($"You fail to heal {targetName}. {remainingMsg}", ChatMessageType.Broadcast);
                healer.Session.Network.EnqueueSend(failMsg, stackSize);
                if (healer != target)
                    target.Session.Network.EnqueueSend(new GameMessageSystemChat($"{healer.Name} fails to heal you.", ChatMessageType.Broadcast));
                if (UsesLeft <= 0)
                    healer.TryRemoveItemFromInventoryWithNetworking(this, 1);
                return;
            }

            // heal up
            var healAmount = GetHealAmount(healer, target, out var critical, out var staminaCost);

            healer.UpdateVitalDelta(healer.Stamina, (int)(-staminaCost));
            target.UpdateVitalDelta(target.Health, healAmount);
            target.DamageHistory.OnHeal(healAmount);

            var updateHealth = new GameMessagePrivateUpdateAttribute2ndLevel(target, Vital.Health, target.Health.Current);
            var crit = critical ? "expertly " : "";
            GameMessageSystemChat message = null;

            if (UsesLeft <= 0)
                message = new GameMessageSystemChat($"You {crit}heal {targetName} for {healAmount} points with {Name}.", ChatMessageType.Broadcast);
            else
                message = new GameMessageSystemChat($"You {crit}heal {targetName} for {healAmount} points. {remainingMsg}", ChatMessageType.Broadcast);

            healer.Session.Network.EnqueueSend(message, stackSize);
            target.Session.Network.EnqueueSend(updateHealth);

            if (healer != target)
                target.Session.Network.EnqueueSend(new GameMessageSystemChat($"{healer.Name} heals you for {healAmount} points.", ChatMessageType.Broadcast));

            if (UsesLeft <= 0)
                healer.TryRemoveItemFromInventoryWithNetworking(this, 1);
        }

        /// <summary>
        /// Determines if healer successfully heals target for attempt
        /// </summary>
        public bool DoSkillCheck(Player healer, Player target)
        {
            // skill check:
            // (healing skill + healing kit boost) * trainedMod
            // vs. damage * 2 * combatMod
            var healingSkill = healer.GetCreatureSkill(Skill.Healing);
            var trainedMod = healingSkill.AdvancementClass == SkillAdvancementClass.Specialized ? 1.5f : 1.1f;

            var combatMod = healer.CombatMode == CombatMode.NonCombat ? 1.0f : 1.1f;

            var effectiveSkill = (int)Math.Round(healingSkill.Current + (Boost ?? 0) * trainedMod);
            var difficulty = (int)Math.Round((target.Health.MaxValue - target.Health.Current) * 2 * combatMod);

            var skillCheck = SkillCheck.GetSkillChance(effectiveSkill, difficulty);
            return skillCheck >= Physics.Common.Random.RollDice(0.0f, 1.0f);
        }

        /// <summary>
        /// Returns the healing amount for this attempt
        /// </summary>
        public uint GetHealAmount(Player healer, Player target, out bool criticalHeal, out uint staminaCost)
        {
            // factors: healing skill, healing kit bonus, stamina, critical chance
            var healingSkill = healer.GetCreatureSkill(Skill.Healing).Current;
            var healBase = healingSkill * (float)HealkitMod.Value;

            // todo: determine applicable range from pcaps
            var healMin = healBase * 0.2f;      // ??
            var healMax = healBase * 0.5f;
            var healAmount = (uint)Math.Round(Physics.Common.Random.RollDice(healMin, healMax));

            // chance for critical healing
            criticalHeal = Physics.Common.Random.RollDice(0.0f, 1.0f) < 0.1f;
            if (criticalHeal) healAmount *= 2;

            // cap to missing health
            var missingHealth = target.Health.MaxValue - target.Health.Current;
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
            return healAmount;
        }
    }
}
