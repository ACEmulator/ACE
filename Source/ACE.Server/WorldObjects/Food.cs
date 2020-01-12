using System;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using log4net;

namespace ACE.Server.WorldObjects
{
    public class Food : Stackable
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Food(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Food(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
            ObjectDescriptionFlags |= ObjectDescriptionFlag.Food;
        }

        /// <summary>
        /// This is raised by Player.HandleActionUseItem.<para />
        /// The item should be in the players possession.
        /// </summary>
        public override void ActOnUse(WorldObject activator)
        {
            if (!(activator is Player player))
                return;

            if (player.IsBusy || player.Teleporting)
            {
                player.Session.Network.EnqueueSend(new GameEventWeenieError(player.Session, WeenieError.YoureTooBusy));
                return;
            }

            player.IsBusy = true;

            var actionChain = new ActionChain();

            // if something other that NonCombat.Ready,
            // manually send this swap
            var prevStance = player.CurrentMotionState.Stance;

            if (prevStance != MotionStance.NonCombat)
                player.EnqueueMotion_Force(actionChain, MotionStance.NonCombat, MotionCommand.Ready, (MotionCommand)prevStance);

            // start the eat/drink motion
            var motionCommand = GetUseSound() == Sound.Eat1 ? MotionCommand.Eat : MotionCommand.Drink;

            player.EnqueueMotion_Force(actionChain, MotionStance.NonCombat, motionCommand);

            // apply consumable
            actionChain.AddAction(player, () => ApplyConsumable(player));

            // return to ready stance
            player.EnqueueMotion_Force(actionChain, MotionStance.NonCombat, MotionCommand.Ready, motionCommand);

            if (prevStance != MotionStance.NonCombat)
                player.EnqueueMotion_Force(actionChain, prevStance, MotionCommand.Ready, MotionCommand.NonCombat);

            actionChain.AddAction(player, () => { player.IsBusy = false; });

            actionChain.EnqueueChain();
        }

        public enum ConsumableBuffType
        {
            Spell   = 0,
            Health  = 2,
            Stamina = 4,
            Mana    = 6
        }

        /// <summary>
        /// Applies the boost from the consumable, broadcasts the sound,
        /// sends message to player, and consumes from inventory
        /// </summary>
        public void ApplyConsumable(Player player)
        {
            if (player.IsDead) return;

            var buffType = (ConsumableBuffType)BoosterEnum;
            GameMessageSystemChat buffMessage = null;

            // spells
            if (buffType == ConsumableBuffType.Spell)
            {
                var spellID = SpellDID ?? 0;

                var result = player.CreateSingleSpell(spellID);

                if (result)
                {
                    var spell = new Server.Entity.Spell(spellID);
                    buffMessage = new GameMessageSystemChat($"{Name} casts {spell.Name} on you.", ChatMessageType.Magic);
                }
                else
                    buffMessage = new GameMessageSystemChat($"{Name} has invalid spell id {spellID}", ChatMessageType.Broadcast);
            }
            // vitals
            else
            {
                var vital = player.GetCreatureVital(BoosterEnum);

                if (vital != null)
                {
                    // only apply to restoration food?
                    var ratingMod = BoostValue > 0 ? player.GetHealingRatingMod() : 1.0f;

                    var boostValue = (int)Math.Round(BoostValue * ratingMod);

                    var vitalChange = (uint)Math.Abs(player.UpdateVitalDelta(vital, boostValue));

                    if (BoosterEnum == PropertyAttribute2nd.Health)
                    {
                        if (BoostValue >= 0)
                            player.DamageHistory.OnHeal(vitalChange);
                        else
                            player.DamageHistory.Add(this, DamageType.Health, vitalChange);
                    }

                    var verb = BoostValue >= 0 ? "restores" : "takes";
                    buffMessage = new GameMessageSystemChat($"The {Name} {verb} {vitalChange} points of your {BoosterEnum}.", ChatMessageType.Broadcast);
                }
                else
                {
                    buffMessage = new GameMessageSystemChat($"{Name} ({Guid}) contains invalid vital {BoosterEnum}", ChatMessageType.Broadcast);
                }
            }

            var soundEvent = new GameMessageSound(player.Guid, GetUseSound(), 1.0f);
            player.EnqueueBroadcast(soundEvent);

            player.Session.Network.EnqueueSend(buffMessage);

            player.TryConsumeFromInventoryWithNetworking(this, 1);
        }

        public Sound GetUseSound()
        {
            var useSound = UseSound;

            if (useSound == Sound.Invalid)
                useSound = Sound.Eat1;

            return useSound;
        }
    }
}
