using System;
using ACE.Entity.Enum;
using ACE.Managers;
using ACE.Network.Enum;
using System.Collections.Generic;
using System.Linq;
using ACE.Network.Motion;

namespace ACE.Entity
{
    public class Container : WorldObject
    {
        private readonly Dictionary<ObjectGuid, WorldObject> inventory = new Dictionary<ObjectGuid, WorldObject>();

        public Container(ObjectType type, ObjectGuid guid, string name, ushort weenieClassId, ObjectDescriptionFlag descriptionFlag, WeenieHeaderFlag weenieFlag, Position position)
            : base(type, guid)
        {
            Name = name;
            DescriptionFlags = descriptionFlag;
            WeenieFlags = weenieFlag;
            Location = position;
            WeenieClassid = weenieClassId;
        }

        /// <summary>
        /// Load from saved object
        /// </summary>
        /// <param name="baseObject"></param>
        public Container(AceObject baseObject)
            : base(baseObject) { }

        public Container(ObjectGuid guid, AceObject baseAceObject)
            : base((ObjectType)baseAceObject.ItemType, guid)
        {
            Name = baseAceObject.Name ?? "NULL";
            DescriptionFlags = (ObjectDescriptionFlag)baseAceObject.AceObjectDescriptionFlags;
            WeenieClassid = baseAceObject.WeenieClassId;
            PhysicsData.MTableResourceId = baseAceObject.MotionTableId;
            PhysicsData.Stable = baseAceObject.SoundTableId;
            PhysicsData.CSetup = baseAceObject.ModelTableId;
            PhysicsData.Petable = baseAceObject.PhysicsTableId;
            PhysicsData.PhysicsState = (PhysicsState)baseAceObject.PhysicsState;
            PhysicsData.ObjScale = baseAceObject.DefaultScale ?? 0u;
            PhysicsData.AnimationFrame = baseAceObject.AnimationFrameId;
            PhysicsData.Translucency = baseAceObject.Translucency ?? 0u;
            PhysicsData.DefaultScript = baseAceObject.DefaultScript;
            PhysicsData.DefaultScriptIntensity = (float?)baseAceObject.PhysicsScriptIntensity;
            PhysicsData.Elasticity = baseAceObject.Elasticity;
            PhysicsData.Parent = baseAceObject.Parent;
            PhysicsData.ParentLocation = baseAceObject.ParentLocation;
            PhysicsData.Friction = baseAceObject.Friction;
            if (baseAceObject.CurrentMotionState == "0" || baseAceObject.CurrentMotionState == null)
                PhysicsData.CurrentMotionState = null;
            else
                PhysicsData.CurrentMotionState = new UniversalMotion(Convert.FromBase64String(baseAceObject.CurrentMotionState));

            // game data min required flags;
            Icon = baseAceObject.IconId;
            AmmoType = (AmmoType?)baseAceObject.AmmoType;
            Burden = baseAceObject.Burden;
            CombatUse = (CombatUse?)baseAceObject.CombatUse;
            ContainerCapacity = baseAceObject.ContainersCapacity;
            Cooldown = baseAceObject.CooldownId;
            CooldownDuration = baseAceObject.CooldownDuration;
            HookItemTypes = baseAceObject.HookItemTypes;
            HookType = baseAceObject.HookType;
            IconOverlay = baseAceObject.IconOverlayId;
            IconUnderlay = baseAceObject.IconUnderlayId;
            ItemCapacity = baseAceObject.ItemsCapacity;
            Material = (Material?)baseAceObject.MaterialType;
            MaxStackSize = baseAceObject.MaxStackSize;
            Wielder = baseAceObject.WielderId;
            ContainerId = baseAceObject.ContainerId;
            ClothingBase = baseAceObject.ClothingBase;
            CurrentWieldedLocation = (EquipMask?)baseAceObject.CurrentWieldedLocation;

            // TODO: this needs to be pulled in from pcap data. Missing - Name Plural never set need to address

            Priority = (CoverageMask?)baseAceObject.Priority;
            RadarBehavior = (RadarBehavior?)baseAceObject.Radar;
            RadarColor = (RadarColor?)baseAceObject.BlipColor;
            Script = baseAceObject.PhysicsScript;
            Spell = (Spell?)baseAceObject.SpellId;
            StackSize = baseAceObject.StackSize;
            Structure = baseAceObject.Structure;
            TargetType = baseAceObject.TargetTypeId;
            GameDataType = baseAceObject.ItemType;
            UiEffects = (UiEffects?)baseAceObject.UiEffects;
            Usable = (Usable?)baseAceObject.ItemUseable;
            UseRadius = baseAceObject.UseRadius;
            ValidLocations = (EquipMask?)baseAceObject.ValidLocations;
            Value = baseAceObject.Value;
            Workmanship = baseAceObject.Workmanship;

            PhysicsData.SetPhysicsDescriptionFlag(this);
            WeenieFlags = SetWeenieHeaderFlag();
            WeenieFlags2 = SetWeenieHeaderFlag2();

            baseAceObject.AnimationOverrides.ForEach(ao => ModelData.AddModel(ao.Index, ao.AnimationId));
            baseAceObject.TextureOverrides.ForEach(to => ModelData.AddTexture(to.Index, to.OldId, to.NewId));
            baseAceObject.PaletteOverrides.ForEach(po => ModelData.AddPalette(po.SubPaletteId, po.Offset, po.Length));
            ModelData.PaletteGuid = baseAceObject.PaletteId;
        }

        // Inventory Management Functions
        public virtual void AddToInventory(WorldObject inventoryItem)
        {
            if (!inventory.ContainsKey(inventoryItem.Guid))
            {
                inventory.Add(inventoryItem.Guid, inventoryItem);
            }
        }

        public virtual void RemoveFromInventory(ObjectGuid inventoryItemGuid)
        {
            if (inventory.ContainsKey(inventoryItemGuid))
            {
                inventory.Remove(inventoryItemGuid);
                Burden = UpdateBurden();
            }
            else
            {
                // Ok maybe it is inventory in one of our packs
                var containers = inventory.Where(wo => wo.Value.ItemCapacity > 0).ToList();

                foreach (var cnt in containers)
                {
                    if (((Container)cnt.Value).inventory.ContainsKey(inventoryItemGuid))
                    {
                        ((Container)cnt.Value).inventory.Remove(inventoryItemGuid);
                        // update pack burden
                        ((Container)cnt.Value).Burden = ((Container)cnt.Value).UpdateBurden();
                        break;
                    }
                }
                Burden = UpdateBurden();
            }
        }

        public ushort UpdateBurden()
        {
            ushort calculatedBurden = 0;
            foreach (KeyValuePair<ObjectGuid, WorldObject> entry in inventory)
            {
                calculatedBurden += entry.Value.Burden ?? 0;
            }
            return calculatedBurden;
        }

        public void UpdateWieldedItem(Container container, uint itemId)
        {
            // TODO: need to make pack aware - just coding for main pack now.
            ObjectGuid itemGuid = new ObjectGuid(itemId);
            WorldObject inventoryItem = GetInventoryItem(itemGuid);
            if (inventoryItem.ContainerId != container.Guid.Full)
            {
                RemoveFromInventory(itemGuid);
                container.AddToInventory(inventoryItem);
            }
            switch (inventoryItem.ContainerId)
            {
                case null:
                    inventoryItem.ContainerId = container.Guid.Full;
                    inventoryItem.Wielder = null;
                    break;
                default:
                    inventoryItem.ContainerId = null;
                    inventoryItem.Wielder = container.Guid.Full;
                    break;
            }
            inventoryItem.WeenieFlags = inventoryItem.SetWeenieHeaderFlag();
        }

        public virtual List<KeyValuePair<ObjectGuid, WorldObject>> GetCurrentlyWieldedItems()
        {
            return inventory.Where(wo => wo.Value.Wielder != null).ToList();
        }

        public virtual WorldObject GetInventoryItem(ObjectGuid objectGuid)
        {
            var containers = inventory.Where(wo => wo.Value.ItemCapacity > 0).ToList();

            if (inventory.ContainsKey(objectGuid))
                return inventory[objectGuid];
            foreach (var cnt in containers)
            {
                if (((Container)cnt.Value).inventory.ContainsKey(objectGuid))
                    return ((Container)cnt.Value).inventory[objectGuid];
            }

            if (inventory.ContainsKey(objectGuid))
                return inventory[objectGuid];
            return null;
        }
    }
}
