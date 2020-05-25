using System;

using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Models;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    /// <summary>
    /// Activates an object based on collision
    /// </summary>
    public class PressurePlate : WorldObject
    {
        /// <summary>
        /// The last time this pressure plate was activated
        /// </summary>
        public DateTime LastUseTime;

        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public PressurePlate(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public PressurePlate(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
            if (UseSound == 0)
                UseSound = Sound.TriggerActivated;
        }

        public override void SetLinkProperties(WorldObject wo)
        {
            wo.ActivationTarget = Guid.Full;
        }

        /// <summary>
        /// Called when a player runs over the pressure plate
        /// </summary>
        public override void OnCollideObject(WorldObject wo)
        {
            OnActivate(wo);
        }

        /// <summary>
        /// Activates the object linked to a pressure plate
        /// </summary>
        public override void OnActivate(WorldObject activator)
        {
            // handle monsters walking on pressure plates
            if (!(activator is Player player))
                return;

            // prevent continuous event stream
            // TODO: should this go in base.OnActivate()?

            var currentTime = DateTime.UtcNow;
            if (currentTime < LastUseTime + TimeSpan.FromSeconds(2))
                return;

            LastUseTime = currentTime;

            player.EnqueueBroadcast(new GameMessageSound(player.Guid, UseSound));

            base.OnActivate(activator);
        }

        public override void ActOnUse(WorldObject wo)
        {
            // Do nothing
        }
    }
}
