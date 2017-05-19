using ACE.Network.Enum;

namespace ACE.Entity
{
    /// <summary>
    /// GameData - which of these items get written out is controled by DescriptionFlags
    /// </summary>
    public class GameData
    {
        public string Name;
        public ushort Type;
        public string NamePlural;
        public byte? ItemCapacity;
        public byte? ContainerCapacity;
        public AmmoType? AmmoType;
        public uint? Value;
        public Usable? Usable;
        public float? UseRadius = 0.25f;
        public uint? TargetType;
        public UiEffects? UiEffects;
        public CombatUse? CombatUse;

        public ushort? Structure; // uses;
        public ushort? MaxStructure; // usesLimit;
        public ushort? StackSize;
        public ushort? MaxStackSize; // stack max size

        public uint? ContainerId;
        public uint? Wielder;
        public EquipMask? ValidLocations;
        public EquipMask? Location;
        public CoverageMask? Priority;
        public RadarColor? RadarColor;
        public RadarBehavior? RadarBehavior;
        public ushort? Script;
        public float? Workmanship;
        public ushort? Burden;
        public Spell? Spell;

        // Housing links to another packet, that needs sent.. The HouseRestrictions ACL Control list that contains all the housing data
        public uint? HouseOwner;
        public uint? HouseRestrictions;

        public uint? HookItemTypes;
        public uint? Monarch;
        public ushort? HookType;
        public ushort? IconOverlay;
        public ushort? IconUnderlay;

        public Material? Material;
        public uint? PetOwner;

        // WeenieHeaderFlag2
        public uint? Cooldown;
        public decimal? CooldownDuration;
    }
}