using System;

using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.GameMessages.Messages;

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

        public override void OnActivate(WorldObject activator)
        {
            if (!(activator is Creature)) return;

            // move this to base?
            EnqueueBroadcast(new GameMessageSound(activator.Guid, Sound.TriggerActivated));

            var actionChain = new ActionChain();

            if (MotionTableId != 0)
            {
                var useAnimation = UseTargetAnimation != null ? (MotionCommand)UseTargetAnimation : MotionCommand.Twitch1;

                EnqueueMotion(actionChain, useAnimation, 1, false);
            }

            actionChain.AddAction(this, () => base.OnActivate(activator));

            actionChain.EnqueueChain();
        }

        public override void ActOnUse(WorldObject activator)
        {
            if (SpellDID != null && (ActivationResponse & ActivationResponse.CastSpell) != 0)
            {
                var spell = new Server.Entity.Spell((uint)SpellDID);

                TryCastSpell(spell, activator);
            }
        }

        public override void SetLinkProperties(WorldObject wo)
        {
            wo.ActivationTarget = Guid.Full;
        }
    }
}
