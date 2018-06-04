
using ACE.Database;
using ACE.DatLoader;
using ACE.Entity.Enum;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Structure;

namespace ACE.Server.WorldObjects
{
    partial class WorldObject
    {
        /// <summary>
        /// This is used to determine how close you need to be to use an item.
        /// NOTE: cheat factor (2) added for items with null use radius. Og II
        /// </summary>
        public float UseRadiusSquared => ((UseRadius ?? 2) + CSetup.Radius) * ((UseRadius ?? 2) + CSetup.Radius);

        public bool IsWithinUseRadiusOf(WorldObject wo)
        {
            return Location.SquaredDistanceTo(wo.Location) <= wo.UseRadiusSquared;
        }

        /// <summary>
        /// This is raised by Player.HandleActionUseItem, and is wrapped in ActionChain.<para />
        /// The actor of the ActionChain is the player using the item.<para />
        /// The item should be in the players possession.
        /// </summary>
        public virtual void UseItem(Player player, ActionChain actionChain)
        {
            // Do Nothing by default
#if DEBUG
            var message = $"Default UseItem reached, this object ({Name}) not programmed yet.";
            player.Session.Network.EnqueueSend(new GameMessageSystemChat(message, ChatMessageType.System));
            log.Error(message);
#endif

            player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session));
        }

        /// <summary>
        /// This is raised by Player.HandleActionUseItem, and is wrapped in ActionChain.<para />
        /// The actor of the ActionChain is the item being used.<para />
        /// The item does not exist in the players possession.<para />
        /// If the item was outside of range, the player will have been commanded to move using DoMoveTo before ActOnUse is called.<para />
        /// When this is called, it should be assumed that the player is within range.
        /// </summary>
        public virtual void ActOnUse(WorldObject worldObject)
        {
            //if (Usable.HasValue && Usable == ACE.Entity.Enum.Usable.ViewedRemote && Spell.HasValue && SpellDID.HasValue)
            //{
            //    //taken from Gem.UseItem
            //    var spellTable = DatManager.PortalDat.SpellTable;
            //    if (!spellTable.Spells.ContainsKey((uint)SpellDID)) return;
            //    var spellBase = DatManager.PortalDat.SpellTable.Spells[(uint)SpellDID];
            //    var spell = DatabaseManager.World.GetCachedSpell((uint)SpellDID);
            //    var msg = string.Empty;
            //    player.PlayParticleEffect((PlayScript)spellBase.TargetEffect, player.Guid);
            //    LifeMagic(player, spellBase, spell, out msg);
            //    player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session));
            //    return;
            //}

            // Do Nothing by default
            if (worldObject is Player)
            {
                var player = worldObject as Player;
#if DEBUG
                var message = $"Default ActOnUse reached, this object ({Name}) not programmed yet.";
                player.Session.Network.EnqueueSend(new GameMessageSystemChat(message, ChatMessageType.System));
                log.Error(message);
#endif
                player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session));
            }
        }
    }
}
