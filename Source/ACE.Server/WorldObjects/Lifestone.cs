using System;

using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    public class Lifestone : WorldObject
    {
        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Lifestone(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Lifestone(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
            ObjectDescriptionFlags |= ObjectDescriptionFlag.LifeStone;

            RadarColor = ACE.Entity.Enum.RadarColor.LifeStone;
        }

        /// <summary>
        /// This is raised by Player.HandleActionUseItem.<para />
        /// The item does not exist in the players possession.<para />
        /// If the item was outside of range, the player will have been commanded to move using DoMoveTo before ActOnUse is called.<para />
        /// When this is called, it should be assumed that the player is within range.
        /// </summary>
        public override void ActOnUse(WorldObject worldObject)
        {
            if (!(worldObject is Player player))
                return;

            var actionChain = new ActionChain();
            if (player.CombatMode != CombatMode.NonCombat)
            {
                var stanceTime = player.SetCombatMode(CombatMode.NonCombat);
                actionChain.AddDelaySeconds(stanceTime);

                player.LastUseTime += stanceTime;
            }

            actionChain.AddAction(this, () => player.EnqueueBroadcast(new GameMessageSound(player.Guid, Sound.LifestoneOn, 1.0f)));

            player.LastUseTime += player.EnqueueMotion(actionChain, MotionCommand.Sanctuary);

            actionChain.AddAction(this, () =>
            {
                if (player.IsWithinUseRadiusOf(this))
                {
                    player.Sanctuary = new Position(player.Location);
                    player.Session.Network.EnqueueSend(new GameMessageSystemChat(GetProperty(PropertyString.UseMessage), ChatMessageType.Magic));
                    var newStamina = (uint)Math.Round(player.Stamina.Current / 2f);
                    player.UpdateVital(player.Stamina, newStamina);
                }
                else
                    player.Session.Network.EnqueueSend(new GameEventWeenieError(player.Session, WeenieError.YouHaveMovedTooFar));
            });

            actionChain.EnqueueChain();
        }
    }
}
