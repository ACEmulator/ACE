using ACE.Entity.Enum;
using ACE.Network;
using ACE.Network.Enum;
using ACE.Network.GameMessages;
using ACE.Network.GameMessages.Messages;
using ACE.Network.Managers;
using System.IO;

namespace ACE.Entity
{
    public class GameData
    {
        public string Name;

        public ushort Type;

        public ushort Icon;  // - 0x06000000

        // WenieHeaderFlags
        public string NamePlural;
        public byte ItemCapacity;
        public byte ContainerCapacity;
        public AmmoType AmmoType;
        public uint Value;
        public Usable Usable = Usable.UsableNo;
        public float UseRadius = 0.25f;
        public uint TargetType;
        public UiEffects UiEffects;
        public CombatUse CombatUse;

        public ushort Struture; // uses;
        public ushort MaxStructure; // usesLimit;
        public ushort StackSize;
        public ushort MaxStackSize; // stack max size

        public uint ContainerId;
        public uint Wielder;
        public EquipMask ValidLocations;
        public EquipMask Location;
        public CoverageMask Priority;
        public RadarColor RadarColour;
        public RadarBehavior RadarBehavior;
        public ushort Script;
        public float Workmanship;
        public ushort Burden;
        public ushort Spell;

        // Housing links to another packet, that needs sent.. The HouseRestrictions ACL Control list that contains all the housing data
        public uint HouseOwner;
        public uint HouseRestrictions;

        public uint HookItemTypes;
        public uint Monarch;
        public ushort HookType;
        public ushort IconOverlay;
        public ushort IconUnderlay;

        public Material Material;
        public uint PetOwner;

        // WeenieHeaderFlag2
        public uint Cooldown;
        public decimal CooldownDuration;

    }

}