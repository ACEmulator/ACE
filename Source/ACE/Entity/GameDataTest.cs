using ACE.Entity.Enum;
using ACE.Network;
using ACE.Network.Enum;
using ACE.Network.GameMessages;
using ACE.Network.GameMessages.Messages;
using ACE.Network.Managers;
using System.IO;

namespace ACE.Entity
{
    public class GameDataTest
    {
        public WeenieHeaderFlag WeenieFlags { get; set; } = WeenieHeaderFlag.None;
        public string Name { get; set; }

        /// <summary>
        /// wcid - stands for weenie class id
        /// </summary>
        public ushort WeenieClassId { get; }
        public uint Icon { get; set; }
        public ObjectType Type { get; }
        public ObjectDescriptionFlag DescriptionFlags { get; set; } = ObjectDescriptionFlag.None;
        public WeenieHeaderFlag2 WeenieFlags2 { get; private set; } = WeenieHeaderFlag2.None;

        public GameDataTest(ObjectType type, string name, uint weenieClassId, uint icon)
        {
            Name = name;
            if (weenieClassId < 0x8000u)
                this.WeenieClassId = (ushort)weenieClassId;
            else
                this.WeenieClassId = (ushort)(weenieClassId - 0x8000);

            Icon = icon;
            Type = type;
        }

        // WenieHeaderFlags
        public string PluralName
        {
            get
            {
                return pluralName;
            }
            set
            {
                pluralName = value;
                WeenieFlags |= WeenieHeaderFlag.PuralName;
            }
        }

        public byte ItemCapacity
        {
            get
            {
                return itemCapacity;
            }

            set
            {
                itemCapacity = value;
                WeenieFlags |= WeenieHeaderFlag.ItemCapacity;
            }
        }

        public byte ContainerCapacity
        {
            get
            {
                return containerCapacity;
            }

            set
            {
                containerCapacity = value;
                WeenieFlags |= WeenieHeaderFlag.ContainerCapacity;
            }
        }

        public AmmoType AmmoType
        {
            get
            {
                return ammoType;
            }

            set
            {
                ammoType = value;
                WeenieFlags |= WeenieHeaderFlag.AmmoType;
            }
        }

        public uint Value
        {
            get
            {
                return value;
            }

            set
            {
                this.value = value;
                WeenieFlags |= WeenieHeaderFlag.Value;
            }
        }

        public Usable Usable
        {
            get
            {
                return usable;
            }

            set
            {
                usable = value;
                WeenieFlags |= WeenieHeaderFlag.Usable;
            }
        }

        public float UseRadius
        {
            get
            {
                return useRadius;
            }

            set
            {
                useRadius = value;
                WeenieFlags |= WeenieHeaderFlag.UseRadius;
            }
        }

        public uint TargetType
        {
            get
            {
                return targetType;
            }

            set
            {
                targetType = value;
                WeenieFlags |= WeenieHeaderFlag.TargetType;
            }
        }

        public UiEffects UiEffects
        {
            get
            {
                return uiEffects;
            }

            set
            {
                uiEffects = value;
                WeenieFlags |= WeenieHeaderFlag.UiEffects;
            }
        }

        public CombatUse CombatUse
        {
            get
            {
                return combatUse;
            }

            set
            {
                combatUse = value;
                WeenieFlags |= WeenieHeaderFlag.CombatUse;
            }
        }

        public ushort Uses
        {
            get
            {
                return struture;
            }

            set
            {
                struture = value;
                WeenieFlags |= WeenieHeaderFlag.Struture;
            }
        }

        public ushort UsesLimit
        {
            get
            {
                return maxStructure;
            }

            set
            {
                maxStructure = value;
                WeenieFlags |= WeenieHeaderFlag.MaxStructure;
            }
        }

        public ushort StackSize
        {
            get
            {
                return stackSize;
            }

            set
            {
                stackSize = value;
                WeenieFlags |= WeenieHeaderFlag.StackSize;
            }
        }

        public ushort MaxStackSize
        {
            get
            {
                return maxStackSize;
            }

            set
            {
                maxStackSize = value;
                WeenieFlags |= WeenieHeaderFlag.MaxStackSize;
            }
        }

        public uint ContainerId
        {
            get
            {
                return containerId;
            }

            set
            {
                containerId = value;
                WeenieFlags |= WeenieHeaderFlag.Container;
            }
        }

        public uint Wielder
        {
            get
            {
                return wielder;
            }

            set
            {
                wielder = value;
                WeenieFlags |= WeenieHeaderFlag.Wielder;
            }
        }

        public EquipMask ValidEquipLocations
        {
            get
            {
                return validLocations;
            }

            set
            {
                validLocations = value;
                WeenieFlags |= WeenieHeaderFlag.ValidLocations;
            }
        }

        public EquipMask EquipLocation
        {
            get
            {
                return location;
            }

            set
            {
                location = value;
                WeenieFlags |= WeenieHeaderFlag.Location;
            }
        }

        public CoverageMask Coverage
        {
            get
            {
                return priority;
            }

            set
            {
                priority = value;
                WeenieFlags |= WeenieHeaderFlag.Priority;
            }
        }

        public RadarColor RadarColour
        {
            get
            {
                return radarColour;
            }

            set
            {
                radarColour = value;
                WeenieFlags |= WeenieHeaderFlag.BlipColour;
            }
        }

        public RadarBehavior RadarBehavior
        {
            get
            {
                return radarBehavior;
            }

            set
            {
                radarBehavior = value;
                WeenieFlags |= WeenieHeaderFlag.Radar;
            }
        }

        public ushort Script
        {
            get
            {
                return script;
            }

            set
            {
                script = value;
                WeenieFlags |= WeenieHeaderFlag.Script;
            }
        }

        public float Workmanship
        {
            get
            {
                return workmanship;
            }

            set
            {
                workmanship = value;
                WeenieFlags |= WeenieHeaderFlag.Workmanship;
            }
        }

        public ushort Burden
        {
            get
            {
                return burden;
            }

            set
            {
                burden = value;
                WeenieFlags |= WeenieHeaderFlag.Burden;
            }
        }

        public Spell Spell
        {
            get
            {
                return spell;
            }

            set
            {
                spell = value;
                WeenieFlags |= WeenieHeaderFlag.Spell;
            }
        }

        public uint HouseOwner
        {
            get
            {
                return houseOwner;
            }

            set
            {
                houseOwner = value;
                WeenieFlags |= WeenieHeaderFlag.HouseOwner;
            }
        }

        public uint HouseRestrictions
        {
            get
            {
                return houseRestrictions;
            }

            set
            {
                houseRestrictions = value;
                // TODO: build out the HouseACL structure
                // WeenieFlags |= WeenieHeaderFlag.HouseRestrictions;
            }
        }

        public uint HookItemTypes
        {
            get
            {
                return hookItemTypes;
            }

            set
            {
                hookItemTypes = value;
                WeenieFlags |= WeenieHeaderFlag.HookItemTypes;
            }
        }

        public uint Monarch
        {
            get
            {
                return monarch;
            }

            set
            {
                monarch = value;
                WeenieFlags |= WeenieHeaderFlag.Monarch;
            }
        }

        public ushort HookType
        {
            get
            {
                return hookType;
            }

            set
            {
                hookType = value;
                WeenieFlags |= WeenieHeaderFlag.HookType;
            }
        }

        public ushort IconOverlay
        {
            get
            {
                return iconOverlay;
            }

            set
            {
                iconOverlay = value;
                WeenieFlags |= WeenieHeaderFlag.IconOverlay;
            }
        }

        public ushort IconUnderlay
        {
            get
            {
                return iconUnderlay;
            }

            set
            {
                iconUnderlay = value;
                // DescriptionFlags |= ObjectDescriptionFlag.AdditionalFlags;
                WeenieFlags2 |= WeenieHeaderFlag2.IconUnderlay;
            }
        }

        public Material Material
        {
            get
            {
                return material;
            }

            set
            {
                material = value;
                WeenieFlags |= WeenieHeaderFlag.Material;
            }
        }

        public uint PetOwner
        {
            get
            {
                return petOwner;
            }

            set
            {
                petOwner = value;
                // DescriptionFlags |= ObjectDescriptionFlag.AdditionalFlags;
                WeenieFlags2 |= WeenieHeaderFlag2.PetOwner;
            }
        }

        public uint Cooldown
        {
            get
            {
                return cooldown;
            }

            set
            {
                cooldown = value;
                // DescriptionFlags |= ObjectDescriptionFlag.AdditionalFlags;
                WeenieFlags2 |= WeenieHeaderFlag2.Cooldown;
            }
        }

        public decimal CooldownDuration
        {
            get
            {
                return cooldownDuration;
            }

            set
            {
                cooldownDuration = value;
                // DescriptionFlags |= ObjectDescriptionFlag.AdditionalFlags;
                WeenieFlags2 |= WeenieHeaderFlag2.CooldownDuration;
            }
        }

        private string pluralName;
        private byte itemCapacity;
        private byte containerCapacity;
        private AmmoType ammoType;
        private uint value;
        private Usable usable = Usable.UsableUndef;
        private float useRadius;
        private uint targetType;
        private UiEffects uiEffects;
        private CombatUse combatUse;

        private ushort struture; // uses;
        private ushort maxStructure; // usesLimit;
        private ushort stackSize;
        private ushort maxStackSize; // stack max size

        private uint containerId;
        private uint wielder;
        private EquipMask validLocations;
        private EquipMask location;
        private CoverageMask priority;
        private RadarColor radarColour;
        private RadarBehavior radarBehavior;
        private ushort script;
        private float workmanship;
        private ushort burden;
        private Spell spell;

        // Housing links to another packet, that needs sent.. The HouseRestrictions ACL Control list that contains all the housing data
        private uint houseOwner;
        private uint houseRestrictions;

        private uint hookItemTypes;
        private uint monarch;
        private ushort hookType;
        private ushort iconOverlay;
        private ushort iconUnderlay;

        private Material material;
        private uint petOwner;

        // WeenieHeaderFlag2
        private uint cooldown;
        private decimal cooldownDuration;

        public void Serialize(BinaryWriter writer)
        {
            writer.Write((uint)WeenieFlags);
            writer.WriteString16L(Name);
            writer.Write((ushort)WeenieClassId);
            writer.WritePackedDwordOfKnownType(Icon, 0x6000000);
            writer.Write((uint)Type);
            writer.Write((uint)DescriptionFlags);

            // if ((DescriptionFlags & ObjectDescriptionFlag.AdditionalFlags) != 0)
                // writer.Write((uint)WeenieFlags2);

            if ((WeenieFlags & WeenieHeaderFlag.PuralName) != 0)
                writer.WriteString16L(PluralName);

            if ((WeenieFlags & WeenieHeaderFlag.ItemCapacity) != 0)
                writer.Write(ItemCapacity);

            if ((WeenieFlags & WeenieHeaderFlag.ContainerCapacity) != 0)
                writer.Write(ContainerCapacity);

            if ((WeenieFlags & WeenieHeaderFlag.AmmoType) != 0)
                writer.Write((ushort)AmmoType);

            if ((WeenieFlags & WeenieHeaderFlag.Value) != 0)
                writer.Write(Value);

            if ((WeenieFlags & WeenieHeaderFlag.Usable) != 0)
                writer.Write((uint)Usable);

            if ((WeenieFlags & WeenieHeaderFlag.UseRadius) != 0)
                writer.Write(UseRadius);

            if ((WeenieFlags & WeenieHeaderFlag.TargetType) != 0)
                writer.Write((uint)TargetType);

            if ((WeenieFlags & WeenieHeaderFlag.UiEffects) != 0)
                writer.Write((uint)UiEffects);

            if ((WeenieFlags & WeenieHeaderFlag.CombatUse) != 0)
                writer.Write((byte)CombatUse);

            if ((WeenieFlags & WeenieHeaderFlag.Struture) != 0)
                writer.Write((ushort)Uses);

            if ((WeenieFlags & WeenieHeaderFlag.MaxStructure) != 0)
                writer.Write((ushort)UsesLimit);

            if ((WeenieFlags & WeenieHeaderFlag.StackSize) != 0)
                writer.Write(StackSize);

            if ((WeenieFlags & WeenieHeaderFlag.MaxStackSize) != 0)
                writer.Write(MaxStackSize);

            if ((WeenieFlags & WeenieHeaderFlag.Container) != 0)
                writer.Write(ContainerId);

            if ((WeenieFlags & WeenieHeaderFlag.Wielder) != 0)
                writer.Write(Wielder);

            if ((WeenieFlags & WeenieHeaderFlag.ValidLocations) != 0)
                writer.Write((uint)ValidEquipLocations);

            if ((WeenieFlags & WeenieHeaderFlag.Location) != 0)
                writer.Write((uint)EquipLocation);

            if ((WeenieFlags & WeenieHeaderFlag.Priority) != 0)
                writer.Write((uint)Coverage);

            if ((WeenieFlags & WeenieHeaderFlag.BlipColour) != 0)
                writer.Write((byte)RadarColour);

            if ((WeenieFlags & WeenieHeaderFlag.Radar) != 0)
                writer.Write((byte)RadarBehavior);

            if ((WeenieFlags & WeenieHeaderFlag.Script) != 0)
                writer.Write(Script);

            if ((WeenieFlags & WeenieHeaderFlag.Workmanship) != 0)
                writer.Write(Workmanship);

            if ((WeenieFlags & WeenieHeaderFlag.Burden) != 0)
                writer.Write(Burden);

            if ((WeenieFlags & WeenieHeaderFlag.Spell) != 0)
                writer.Write((uint)Spell);

            if ((WeenieFlags & WeenieHeaderFlag.HouseOwner) != 0)
                writer.Write(HouseOwner);

            if ((WeenieFlags & WeenieHeaderFlag.HouseRestrictions) != 0)
            {
                // TODO
            }

            if ((WeenieFlags & WeenieHeaderFlag.HookItemTypes) != 0)
                writer.Write(HookItemTypes);

            if ((WeenieFlags & WeenieHeaderFlag.Monarch) != 0)
                writer.Write(Monarch);

            if ((WeenieFlags & WeenieHeaderFlag.HookType) != 0)
                writer.Write(HookType);

            if ((WeenieFlags & WeenieHeaderFlag.IconOverlay) != 0)
                writer.Write(IconOverlay);

            if ((WeenieFlags2 & WeenieHeaderFlag2.IconUnderlay) != 0)
                writer.Write(IconUnderlay);

            if ((WeenieFlags & WeenieHeaderFlag.Material) != 0)
                writer.Write((uint)Material);

            if ((WeenieFlags2 & WeenieHeaderFlag2.Cooldown) != 0)
                writer.Write(Cooldown);

            if ((WeenieFlags2 & WeenieHeaderFlag2.CooldownDuration) != 0)
                writer.Write(CooldownDuration);

            if ((WeenieFlags2 & WeenieHeaderFlag2.PetOwner) != 0)
                writer.Write(PetOwner);

            writer.Align();
        }
    }
}