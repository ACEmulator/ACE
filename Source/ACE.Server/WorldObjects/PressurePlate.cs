using System;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity;
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
            Activate(wo);
        }

        /// <summary>
        /// Activates the object linked to a pressure plate
        /// </summary>
        public override void Activate(WorldObject activator)
        {
            if (!Active) return;

            var player = activator as Player;
            if (player == null)
                return;

            // prevent continuous event stream
            var currentTime = DateTime.UtcNow;
            if (currentTime < LastUseTime + TimeSpan.FromSeconds(2))
                return;

            // TODO: this should simply be forwarding the activation event
            // along to the activation target...

            // ensure activation target
            if (ActivationTarget == 0) return;

            var target = CurrentLandblock?.GetObject(new ObjectGuid(ActivationTarget));
            if (target == null)
            {
                Console.WriteLine($"{Name}.OnCollideObject({activator.Name}): couldn't find activation target {ActivationTarget:X8}");
                return;
            }

            LastUseTime = currentTime;
            //Console.WriteLine($"{activator.Name} activated {Name}");

            // activate pressure plate sound
            player.EnqueueBroadcast(new GameMessageSound(player.Guid, (Sound)UseSound));

            // activate target -

            // default use action
            if (target.ActivationResponse.HasFlag(ActivationResponse.Use))
            {
                target.ActOnUse(this);
            }

            // perform motion animation - rarely used (only 4 instances in PY16 db)
            if (target.ActivationResponse.HasFlag(ActivationResponse.Animate))
            {
                var motion = new Motion(target, ActivationAnimation);
                target.EnqueueBroadcastMotion(motion);
            }

            // send chat text - rarely used (only 8 instances in PY16 db)
            if (target.ActivationResponse.HasFlag(ActivationResponse.Talk))
            {
                // todo: verify the format of this message
                player.Session.Network.EnqueueSend(new GameMessageSystemChat(ActivationTalk, ChatMessageType.Broadcast));
                //target.EnqueueBroadcast(new GameMessageSystemChat(ActivationTalk, ChatMessageType.Broadcast));
            }

            // perform activation emote
            if (target.ActivationResponse.HasFlag(ActivationResponse.Emote))
            {
                target.EmoteManager.OnActivation(player);
            }

            // cast a spell on the player (spell traps)
            if (target.ActivationResponse.HasFlag(ActivationResponse.CastSpell))
            {
                if (target.SpellDID != null)
                {
                    var spell = new Server.Entity.Spell((uint)target.SpellDID);
                    target.TryCastSpell(spell, player);
                }
            }

            // call to generator to spawn new object
            if (target.ActivationResponse.HasFlag(ActivationResponse.Generate))
            {
                if (target.IsGenerator)
                    target.SelectProfilesMax();
            }
        }
    }
}
