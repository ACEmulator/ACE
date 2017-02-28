using ACE.Entity.Enum;
using ACE.Network;
using ACE.Network.Enum;
using ACE.Network.GameMessages;
using ACE.Network.GameMessages.Messages;
using ACE.Network.Managers;
using ACE.Entity.WorldPackages;

namespace ACE.Entity.WorldPackages
{
    public class WolrdObjectSegmentGameData
    {
        public WeenieHeaderFlag WenieHeaderFlags;
        public string DisplayName;
        public uint Type;
        public uint Icon;  // - 0x06000000
        public ObjectType ObjectType;
        public ObjectDescriptionFlag ObjetDescriptionFlag;
        public WeenieHeaderFlag2 WenieHeaderFlags2;

        //WenieHeaderFlags
        public string NamePlural;
        public byte ItemCapacity;
        public byte ContainerCapacity;
        public AmmoType AmmoType;
        public uint Value;
        public uint Useability;
        public float UseRadius;
        public uint TargetType;
        public UiEffects UiEffects;
        public CombatUse CombatUse;
        public ushort Struture; // UsesRemaining;
        public ushort MaxStructure; // MaxUses;
        public ushort StackSize;
        public uint ContainerId;
        public uint Wielder;
        public EquipMask ValidLocations;
        public EquipMask Location;
        public CoverageMask Priority;
        public byte BlipColour;
        public byte Radar;
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

        //WeenieHeaderFlag2
        public uint Cooldown;
        public decimal CooldownDuration;
        public uint PetOwner;

        //todo: render object network code
        public void Render()
        {

        }

    }

}
