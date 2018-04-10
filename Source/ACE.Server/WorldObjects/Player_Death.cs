using System;
using System.Collections.Generic;
using System.Text;

using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity.Actions;
using ACE.Server.Network;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Motion;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        /// <summary>
        /// Player Death/Kill, use this to kill a session's player
        /// </summary>
        /// <remarks>
        ///     TODO:
        ///         1. Find the best vitae formula and add vitae
        ///         2. Generate the correct death message, or have it passed in as a parameter.
        ///         3. Find the correct player death noise based on the player model and play on death.
        ///         4. Determine if we need to Send Queued Action for Lifestone Materialize, after Action Location.
        ///         5. Find the health after death formula and Set the correct health
        /// </remarks>
        private void OnKill(Session killerSession)
        {
            ObjectGuid killerId = killerSession.Player.Guid;

            IsAlive = false;
            Health.Current = 0; // Set the health to zero
            NumDeaths++; // Increase the NumDeaths counter
            DeathLevel++; // Increase the DeathLevel

            // TODO: Find correct vitae formula/value
            VitaeCpPool = 0; // Set vitae

            // TODO: Generate a death message based on the damage type to pass in to each death message:
            string currentDeathMessage = $"died to {killerSession.Player.Name}.";

            // Send Vicitim Notification, or "Your Death" event to the client:
            // create and send the client death event, GameEventYourDeath
            var msgYourDeath = new GameEventYourDeath(Session, $"You have {currentDeathMessage}");
            var msgHealthUpdate = new GameMessagePrivateUpdateAttribute2ndLevel(this, Vital.Health, Health.Current);
            var msgNumDeaths = new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.NumDeaths, NumDeaths.Value);
            var msgDeathLevel = new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.DeathLevel, DeathLevel.Value);
            var msgVitaeCpPool = new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.VitaeCpPool, VitaeCpPool.Value);
            var msgPurgeEnchantments = new GameEventPurgeAllEnchantments(Session);
            // var msgDeathSound = new GameMessageSound(Guid, Sound.Death1, 1.0f);

            // Send first death message group
            Session.Network.EnqueueSend(msgHealthUpdate, msgYourDeath, msgNumDeaths, msgDeathLevel, msgVitaeCpPool, msgPurgeEnchantments);

            // Broadcast the 019E: Player Killed GameMessage
            ActionBroadcastKill($"{Name} has {currentDeathMessage}", Guid, killerId);
        }

        public override void DoOnKill(Session killerSession)
        {
            // First do on-kill
            OnKill(killerSession);
            // Then get onKill from our parent
            ActionChain killChain = base.OnKillInternal(killerSession);

            // Send the teleport out after we animate death
            killChain.AddAction(this, () =>
            {
                // teleport to sanctuary or best location
                Position newPosition = PositionSanctuary ?? PositionLastPortal ?? Location;

                // Enqueue a teleport action, followed by Stand-up
                // Queue the teleport to lifestone
                ActionChain teleportChain = GetTeleportChain(newPosition);

                teleportChain.AddAction(this, () =>
                {
                    // Regenerate/ressurect?
                    UpdateVitalInternal(Health, 5);

                    // Stand back up
                    DoMotion(new UniversalMotion(MotionStance.Standing));

                    // add a Corpse at the current location via the ActionQueue to honor the motion and teleport delays
                    // QueuedGameAction addCorpse = new QueuedGameAction(this.Guid.Full, corpse, true, GameActionType.ObjectCreate);
                    // AddToActionQueue(addCorpse);
                    // If the player is outside of the landblock we just died in, then reboadcast the death for
                    // the players at the lifestone.
                    if (Positions.ContainsKey(PositionType.LastOutsideDeath) && Positions[PositionType.LastOutsideDeath].Cell != newPosition.Cell)
                    {
                        string currentDeathMessage = $"died to {killerSession.Player.Name}.";
                        ActionBroadcastKill($"{Name} has {currentDeathMessage}", Guid, killerSession.Player.Guid);
                    }
                });
                teleportChain.EnqueueChain();
            });
            killChain.EnqueueChain();
        }

        public void HandleActionDie()
        {
            // Create smite action chain... then send it
            new ActionChain(this, () =>
            {
                DoOnKill(Session);
            }).EnqueueChain();
        }
    }
}
