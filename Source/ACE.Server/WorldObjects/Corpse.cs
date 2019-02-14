using System;
using System.Linq;

using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Managers;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    public partial class Corpse : Container
    {
        /// <summary>
        /// The maximum number of seconds for an empty corpse to stick around
        /// </summary>
        public static readonly double EmptyDecayTime = 15.0;

        /// <summary>
        /// Flag indicates if a corpse is from a monster or a player
        /// </summary>
        public bool IsMonster = false;

        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Corpse(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Corpse(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
            BaseDescriptionFlags |= ObjectDescriptionFlag.Corpse;

            CurrentMotionState = new Motion(MotionStance.NonCombat, MotionCommand.Dead);

            ContainerCapacity = 10;
            ItemCapacity = 120;
        }

        /// <summary>
        /// Sets the object description for a corpse
        /// </summary>
        public override ObjDesc CalculateObjDesc()
        {
            if (Biota.BiotaPropertiesAnimPart.Count == 0 && Biota.BiotaPropertiesPalette.Count == 0 && Biota.BiotaPropertiesTextureMap.Count == 0)
                return base.CalculateObjDesc(); // No Saved ObjDesc, let base handle it.

            var objDesc = new ObjDesc();

            AddBaseModelData(objDesc);

            foreach (var animPart in Biota.BiotaPropertiesAnimPart.OrderBy(b => b.Order))
                objDesc.AnimPartChanges.Add(new AnimationPartChange { PartIndex = animPart.Index, PartID = animPart.AnimationId });

            foreach (var subPalette in Biota.BiotaPropertiesPalette)
                objDesc.SubPalettes.Add(new SubPalette { SubID = subPalette.SubPaletteId, Offset = subPalette.Offset, NumColors = subPalette.Length });

            foreach (var textureMap in Biota.BiotaPropertiesTextureMap.OrderBy(b => b.Order))
                objDesc.TextureChanges.Add(new TextureMapChange { PartIndex = textureMap.Index, OldTexture = textureMap.OldId, NewTexture = textureMap.NewId });

            return objDesc;
        }

        /// <summary>
        /// Sets the decay time for player corpse.
        /// This should be called AFTER the items (if any) have been added to the corpse.
        /// Corpses that have no items will decay much faster.
        /// </summary>
        public void RecalculateDecayTime(Player player)
        {
            // empty corpses decay faster
            if (Inventory.Count == 0)
                TimeToRot = EmptyDecayTime;
            else
                // a player corpse decays after 5 mins * playerLevel with a minimum of 1 hour
                TimeToRot = Math.Max(3600, (player.Level ?? 1) * 300);
        }

        /// <summary>
        /// Called when a player attempts to loot a corpse
        /// </summary>
        public override void Open(Player player)
        {
            // check for looting permission
            if (!HasPermission(player))
            {
                player.Session.Network.EnqueueSend(new GameMessageSystemChat("You don't have permission to loot the " + Name, ChatMessageType.Broadcast));
                return;
            }
            base.Open(player);
        }

        /// <summary>
        /// Returns TRUE if input player has permission to loot this corpse
        /// </summary>
        public bool HasPermission(Player player)
        {
            // players can loot their own corpses
            if (player.Guid.Full == VictimId)
                return true;

            // players can loot monsters they killed
            if (KillerId != null && player.Guid.Full == KillerId)
                return true;

            // players can /permit other players to loot their corpse
            if (player.HasLootPermission(new ObjectGuid(VictimId.Value)))
                return true;

            // players in the same fellowship as the killer w/ loot sharing enabled
            if (player.Fellowship != null && player.Fellowship.ShareLoot)
            {
                var onlinePlayer = PlayerManager.GetOnlinePlayer(KillerId ?? 0);
                if (onlinePlayer != null && onlinePlayer.Fellowship != null && player.Fellowship == onlinePlayer.Fellowship)
                    return true;
            }
            return false;
        }
    }
}
