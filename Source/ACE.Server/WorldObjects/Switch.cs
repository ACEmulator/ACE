using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity.Actions;

namespace ACE.Server.WorldObjects
{
    public class Switch : WorldObject
    {
        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Switch(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Switch(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
        }

        public uint? UseTargetAnimation
        {
            get => GetProperty(PropertyDataId.UseTargetAnimation);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.UseTargetAnimation); else SetProperty(PropertyDataId.UseTargetAnimation, value.Value); }
        }

        /// <summary>
        /// Called when a player or creature uses a switch
        /// </summary>
        public override void ActOnUse(WorldObject worldObject)
        {
            var creature = worldObject as Creature;
            if (creature == null) return;

            var actionChain = new ActionChain();

            // creature rotates towards switch
            if (!Visibility)
            {
                var rotateTime = creature.Rotate(this);
                actionChain.AddDelaySeconds(rotateTime);
            }

            actionChain.AddAction(creature, () => EnqueueBroadcast(new Network.GameMessages.Messages.GameMessageSound(creature.Guid, Sound.TriggerActivated)));

            // switch activate animation
            if (MotionTableId != 0)
            {
                var useAnimation = UseTargetAnimation != null ? (MotionCommand)UseTargetAnimation : MotionCommand.Twitch1;
                EnqueueMotion(actionChain, useAnimation);
            }            

            var sendUseDone = true;
            actionChain.AddAction(creature, () =>
            {
                // switch activates target
                if (ActivationTarget > 0)
                {
                    var target = CurrentLandblock?.GetObject(new ObjectGuid(ActivationTarget));

                    target.ActOnUse(creature);

                    sendUseDone = false;
                }

                // only affects players?
                var player = creature as Player;
                if (player != null)
                {
                    if (SpellDID.HasValue && (ActivationResponse & ActivationResponse.CastSpell) != 0)
                    {
                        var spell = new Server.Entity.Spell((uint)SpellDID);

                        TryCastSpell(spell, player);
                    }

                    if ((ActivationResponse & ActivationResponse.Emote) != 0)
                    {                        
                        EmoteManager.OnActivation(player);
                    }

                    if (sendUseDone)
                        player.SendUseDoneEvent();
                }
            });
            actionChain.EnqueueChain();
        }

        public override void SetLinkProperties(WorldObject wo)
        {
            wo.ActivationTarget = Guid.Full;
        }
    }
}
