using System;
using System.Collections.Generic;
using System.Linq;

using log4net;

using ACE.Common;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Server.Entity.Actions;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Factories;
using ACE.Server.Managers;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    public partial class Corpse : Container
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
            ObjectDescriptionFlags |= ObjectDescriptionFlag.Corpse;

            CurrentMotionState = new Motion(MotionStance.NonCombat, MotionCommand.Dead);

            ContainerCapacity = 10;
            ItemCapacity = 120;

            SuppressGenerateEffect = true;
        }

        protected override void OnInitialInventoryLoadCompleted()
        {
            if (Level.HasValue)
            {
                var dtTimeToRot = DateTime.UtcNow.AddSeconds(TimeToRot ?? 0);
                var tsDecay = dtTimeToRot - DateTime.UtcNow;

                log.Info($"{Name} (0x{Guid.ToString()}) Reloaded from Database: Corpse Level: {Level ?? 0} | InventoryLoaded: {InventoryLoaded} | Inventory.Count: {Inventory.Count} | TimeToRot: {TimeToRot} | CreationTimestamp: {CreationTimestamp} ({Time.GetDateTimeFromTimestamp(CreationTimestamp ?? 0).ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss")}) | Corpse should not decay before: {dtTimeToRot.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss")}, {tsDecay.ToString("%d")} day(s), {tsDecay.ToString("%h")} hours, {tsDecay.ToString("%m")} minutes, and {tsDecay.ToString("%s")} seconds from now.");
            }
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

            var dtTimeToRot = DateTime.UtcNow.AddSeconds(TimeToRot ?? 0);
            var tsDecay = dtTimeToRot - DateTime.UtcNow;

            Level = player.Level ?? 1;

            log.Info($"{Name}.RecalculateDecayTime({player.Name}): Player Level: {player.Level} | Inventory.Count: {Inventory.Count} | TimeToRot: {TimeToRot} | CreationTimestamp: {CreationTimestamp} ({Time.GetDateTimeFromTimestamp(CreationTimestamp ?? 0).ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss")}) | Corpse should not decay before: {dtTimeToRot.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss")}, {tsDecay.ToString("%d")} day(s), {tsDecay.ToString("%h")} hours, {tsDecay.ToString("%m")} minutes, and {tsDecay.ToString("%s")} seconds from now.");
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
            if (VictimId == null || player.Guid.Full == VictimId)
                return true;

            // players can loot monsters they killed
            if (KillerId != null && player.Guid.Full == KillerId || IsLooted)
                return true;

            // players can /permit other players to loot their corpse
            if (player.HasLootPermission(new ObjectGuid(VictimId.Value)))
                return true;

            // all players can loot monster corpses after 1/2 decay time
            if (TimeToRot != null && TimeToRot < HalfLife && !new ObjectGuid(VictimId.Value).IsPlayer())
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

        public bool IsLooted;

        /// <summary>
        /// The number of seconds before all players can loot a monster corpse
        /// </summary>
        public static int HalfLife = 180;


        public override void Close(Player player)
        {
            base.Close(player);

            if (VictimId != null && !new ObjectGuid(VictimId.Value).IsPlayer())
                IsLooted = true;
        }

        public bool CorpseGeneratedRare
        {
            get => GetProperty(PropertyBool.CorpseGeneratedRare) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.CorpseGeneratedRare); else SetProperty(PropertyBool.CorpseGeneratedRare, value); }
        }

        public override void EnterWorld()
        {
            var actionChain = new ActionChain();

            base.EnterWorld();

            actionChain.AddDelaySeconds(.5);
            actionChain.AddAction(this, () =>
            {
                if (Location != null)
                {
                    if (CorpseGeneratedRare)
                    {
                        EnqueueBroadcast(new GameMessageSystemChat($"{killerName} has discovered the {rareGenerated.Name}!", ChatMessageType.System));
                        ApplySoundEffects(Sound.TriggerActivated, 10);
                    }
                }
            });
            actionChain.EnqueueChain();
        }

        private WorldObject rareGenerated;
        private string killerName;

        /// <summary>
        /// Called to generate rare and add to corpse inventory
        /// </summary>
        public void GenerateRare(WorldObject killer)
        {
            //todo: calculate chances for killer's luck (rare timers)

            var wo = LootGenerationFactory.CreateRare();
            if (wo == null)
                return;

            if (!wo.IconUnderlayId.HasValue || wo.IconUnderlayId.Value != 0x6005B0C) // ensure icon underlay exists for rare (loot profiles use this)
                wo.IconUnderlayId = 0x6005B0C;

            var tier = LootGenerationFactory.GetRareTier(wo.WeenieClassId);
            LootGenerationFactory.RareChances.TryGetValue(tier, out var chance);

            log.Info($"[RARE] {Name} ({Guid}) generated rare {wo.Name} ({wo.Guid}) for {killer.Name} ({killer.Guid})");
            log.Info($"[RARE] Tier {tier} -- 1 / {chance:N0} chance");

            if (TryAddToInventory(wo))
            {
                rareGenerated = wo;
                killerName = killer.Name.TrimStart('+');
                CorpseGeneratedRare = true;
                LongDesc += " This corpse generated a rare item!";
            }
            else
                log.Error($"[RARE] failed to add to corpse inventory");
        }
    }
}
