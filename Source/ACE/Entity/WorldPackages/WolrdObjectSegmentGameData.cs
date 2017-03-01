using ACE.Entity.Enum;
using ACE.Network;
using ACE.Network.Enum;
using ACE.Network.GameMessages;
using ACE.Network.GameMessages.Messages;
using ACE.Network.Managers;
using ACE.Entity.WorldPackages;
using System.IO;

namespace ACE.Entity.WorldPackages
{
    public class WolrdObjectSegmentGameData
    {
        public WeenieHeaderFlag WeenieHeaderFlags;
        public string Name;
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

        public ushort Struture; // uses;
        public ushort MaxStructure; // usesLimit;
        public ushort StackSize;
        public ushort MaxStackSize; //stack max size

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
        public void Render(BinaryWriter writer)
        {
            writer.Write((uint)WeenieHeaderFlags);
            writer.WriteString16L(Name);
            writer.Write((uint)Type);
            writer.Write((uint)Icon);       
            writer.Write((uint)ObjectType);
            writer.Write((uint)ObjetDescriptionFlag);

            if ((ObjetDescriptionFlag & ObjectDescriptionFlag.AdditionFlags) != 0)
                writer.Write((uint)WenieHeaderFlags2);

            if ((WeenieHeaderFlags & WeenieHeaderFlag.PuralName) != 0)
                writer.WriteString16L(NamePlural);

            if ((WeenieHeaderFlags & WeenieHeaderFlag.ItemCapacity) != 0)
                writer.Write((byte)ItemCapacity);

            if ((WeenieHeaderFlags & WeenieHeaderFlag.ContainerCapacity) != 0)
                writer.Write((byte)ContainerCapacity);

            if ((WeenieHeaderFlags & WeenieHeaderFlag.AmmoType) != 0)
                writer.Write((ushort)AmmoType);

            if ((WeenieHeaderFlags & WeenieHeaderFlag.Value) != 0)
                writer.Write((uint)Value);

            if ((WeenieHeaderFlags & WeenieHeaderFlag.Useability) != 0)
                writer.Write((uint)Useability);

            if ((WeenieHeaderFlags & WeenieHeaderFlag.UseRadius) != 0)
                writer.Write((float)UseRadius);

            if ((WeenieHeaderFlags & WeenieHeaderFlag.TargetType) != 0)
                writer.Write(TargetType);

            if ((WeenieHeaderFlags & WeenieHeaderFlag.UiEffects) != 0)
                writer.Write((uint)UiEffects);

            if ((WeenieHeaderFlags & WeenieHeaderFlag.CombatUse) != 0)
                writer.Write((byte)CombatUse);

            if ((WeenieHeaderFlags & WeenieHeaderFlag.Struture) != 0)
                writer.Write((ushort)Struture);

            if ((WeenieHeaderFlags & WeenieHeaderFlag.MaxStructure) != 0)
                writer.Write((ushort)MaxStructure);

            if ((WeenieHeaderFlags & WeenieHeaderFlag.StackSize) != 0)
                writer.Write((ushort)StackSize);

            if ((WeenieHeaderFlags & WeenieHeaderFlag.MaxStackSize) != 0)
                writer.Write((ushort)MaxStackSize);

            if ((WeenieHeaderFlags & WeenieHeaderFlag.Container) != 0)
                writer.Write(ContainerId);
            
            if ((WeenieHeaderFlags & WeenieHeaderFlag.Wielder) != 0)
                writer.Write(Wielder);

            if ((WeenieHeaderFlags & WeenieHeaderFlag.ValidLocations) != 0)
                writer.Write((uint)ValidLocations);

            if ((WeenieHeaderFlags & WeenieHeaderFlag.Location) != 0)
                writer.Write((uint)Location);

            if ((WeenieHeaderFlags & WeenieHeaderFlag.Priority) != 0)
                writer.Write((uint)Priority);

            if ((WeenieHeaderFlags & WeenieHeaderFlag.BlipColour) != 0)
                writer.Write((byte)BlipColour);

            if ((WeenieHeaderFlags & WeenieHeaderFlag.Radar) != 0)
                writer.Write((byte)Radar);

            if ((WeenieHeaderFlags & WeenieHeaderFlag.Script) != 0)
                writer.Write((ushort)Script);

            if ((WeenieHeaderFlags & WeenieHeaderFlag.Workmanship) != 0)
                writer.Write((float)Workmanship);

            if ((WeenieHeaderFlags & WeenieHeaderFlag.Burden) != 0)
                writer.Write((ushort)Burden);

            if ((WeenieHeaderFlags & WeenieHeaderFlag.Spell) != 0)
                writer.Write((ushort)Spell);

            if ((WeenieHeaderFlags & WeenieHeaderFlag.HouseOwner) != 0)
                writer.Write(HouseOwner);

            //Requires a ACL LIST..
            /*if ((WeenieHeaderFlags & WeenieHeaderFlag.HouseRestrictions) != 0)
            {
            }
            if ((WeenieHeaderFlags & WeenieHeaderFlag.HookItemTypes) != 0)
                writer.Write(0u);
            */

            if ((WeenieHeaderFlags & WeenieHeaderFlag.Monarch) != 0)
                writer.Write(Monarch);

            //if ((WeenieHeaderFlags & WeenieHeaderFlag.HookType) != 0)
            //    writer.Write((ushort)0);

            if ((WeenieHeaderFlags & WeenieHeaderFlag.IconOverlay) != 0)
                writer.Write((ushort)IconOverlay);

            /*if ((WeenieHeaderFlags2 & WeenieHeaderFlag2.IconUnderlay) != 0)
                writer.Write((ushort)0);*/

            if ((WeenieHeaderFlags & WeenieHeaderFlag.Material) != 0)
                writer.Write((uint)Material);

            /*if ((WeenieHeaderFlags2 & WeenieHeaderFlag2.Cooldown) != 0)
                writer.Write(0u);*/

            /*if ((WeenieHeaderFlags2 & WeenieHeaderFlag2.CooldownDuration) != 0)
                writer.Write(0.0d);*/

            /*if ((WeenieHeaderFlags2 & WeenieHeaderFlag2.PetOwner) != 0)
                writer.Write(0u);*/

            writer.Align();
        }

    }

}
