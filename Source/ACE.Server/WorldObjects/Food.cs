using System;
using log4net;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
using ACE.Server.Entity;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Physics;

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

            if (player.IsBusy || player.Teleporting || player.suicideInProgress)
            {
                player.SendWeenieError(WeenieError.YoureTooBusy);
                return;
            }

            if (player.IsJumping)
            {
                player.SendWeenieError(WeenieError.YouCantDoThatWhileInTheAir);
                return;
            }

            var motionCommand = GetUseSound() == Sound.Eat1 ? MotionCommand.Eat : MotionCommand.Drink;

            player.ApplyConsumable(motionCommand, () => ApplyConsumable(player));
        }

        /// <summary>
        /// Applies the boost from the consumable, broadcasts the sound,
        /// sends message to player, and consumes from inventory
        /// </summary>
        public void ApplyConsumable(Player player)
        {
            if (player.IsDead) return;

            // trying to use a dispel potion while pk timer is active
            // send error message and cancel - do not consume item
            if (SpellDID != null)
            {
                var spell = new Spell(SpellDID.Value);

                if (spell.MetaSpellType == SpellType.Dispel && !VerifyDispelPKStatus(this, player))
                    return;
            }

            if (BoosterEnum != PropertyAttribute2nd.Undef)
            {
                BoostVital(player);
            }

            if (SpellDID != null)
            {
                CastSpell(player);
            }

            var soundEvent = new GameMessageSound(player.Guid, GetUseSound(), 1.0f);
            player.EnqueueBroadcast(soundEvent);

            player.TryConsumeFromInventoryWithNetworking(this, 1);
        }

        public void BoostVital(Player player)
        {
            var vital = player.GetCreatureVital(BoosterEnum);

            if (vital == null)
            {
                player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{Name} ({Guid}) contains invalid vital {BoosterEnum}", ChatMessageType.Broadcast));
                return;
            }

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

            player.Session.Network.EnqueueSend(new GameMessageSystemChat($"The {Name} {verb} {vitalChange} points of your {BoosterEnum}.", ChatMessageType.Broadcast));
        }

        public void CastSpell(Player player)
        {
            var spell = new Spell(SpellDID.Value);

            if (spell.NotFound)
            {
                if (spell._spellBase != null)
                    player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{spell.Name} spell not implemented, yet!", ChatMessageType.System));
                else
                    player.Session.Network.EnqueueSend(new GameMessageSystemChat($"Invalid spell id {SpellDID ?? 0}", ChatMessageType.System));

                return;
            }

            // should be 'You cast', instead of 'Item cast'
            // omitting the item caster here, so player is also used for enchantment registry caster,
            // which could prevent some scenarios with spamming enchantments from multiple food sources to protect against dispels
            player.TryCastSpell(spell, player, this, tryResist: false);
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
