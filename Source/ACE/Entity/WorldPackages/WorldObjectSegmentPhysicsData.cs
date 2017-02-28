using ACE.Entity.Enum;
using ACE.Network;
using ACE.Network.Enum;
using System.Collections.Generic;
using System.IO;

namespace ACE.Entity.WorldPackages
{
    public class WorldObjectSegmentPhysicsData
    {
        public uint CSetup;
        public PhysicsDescriptionFlag PhysicsDescriptionFlag;
        private PhysicsState PhysicsState = 0;

        public Position Position;
        public uint MTableResourceId;
        public uint SoundsResourceId;
        public uint Stable;
        public uint Petable;

        public uint Equipper;
        public EquipMask EquipperPhysicsDescriptionFlag;

        public uint ItemsEquipedCount;

        private List<EquipedItem> EquipedItems = new List<EquipedItem>();

        public float ObjScale;
        public float Friction;
        public float Elastcity;
        public float Translucency;
        public Position Velocity;
        public Position Acceleration;
        public Position Omega; // rotation

        public uint DefaultScript;
        public float DefaultScriptIntensity;

        //thanks Kaezin for help understanding this structure.
        //Update this when the object moves
        public ushort PositionSequance;
        public ushort unknownseq0 = (ushort)1; // unknown for now
        public ushort PhysicsSequance; // physics state change 
        public ushort JumpSequance; // increments when you Jump.
        public ushort PortalSequance; //increments when you portal
        public ushort unknownseq1 = (ushort)0;
        public ushort SpawnSequance; // increments with spawn player / critter / boss ?


        public void AddEquipedItem(uint index, EquipMask equiperflag)
        {
            EquipedItem newitem = new EquipedItem(index, equiperflag);
            EquipedItems.Add(newitem);
        }

        //todo: return bytes of data for network write ? ?
        public void Render(BinaryWriter writer)
        {

            writer.Write((uint)PhysicsDescriptionFlag);
            writer.Write((uint)PhysicsState);

            /*if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Movement) != 0)
            {
            }*/

            /*if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.AnimationFrame) != 0)
            {
            }*/

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Position) != 0)
                Position.Write(writer);

            // TODO:
            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.MTable) != 0)
                writer.Write(0x09000001u);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Stable) != 0)
                writer.Write(0x20000001u);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Petable) != 0)
                writer.Write(0x34000004u);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.CSetup) != 0)
                writer.Write(0x02000001u);

            /*if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Parent) != 0)
            {
            }*/

            /*if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Children) != 0)
            {
            }*/

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.ObjScale) != 0)
                writer.Write(0.0f);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Friction) != 0)
                writer.Write(0.0f);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Elastcity) != 0)
                writer.Write(0.0f);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Translucency) != 0)
                writer.Write(0.0f);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Velocity) != 0)
            {
                writer.Write(0.0f);
                writer.Write(0.0f);
                writer.Write(0.0f);
            }

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Acceleration) != 0)
            {
                writer.Write(0.0f);
                writer.Write(0.0f);
                writer.Write(0.0f);
            }

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Omega) != 0)
            {
                writer.Write(0.0f);
                writer.Write(0.0f);
                writer.Write(0.0f);
            }

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.DefaultScript) != 0)
                writer.Write(0u);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.DefaultScriptIntensity) != 0)
                writer.Write(0.0f);

            writer.Write((ushort)PositionSequance);
            writer.Write((ushort)unknownseq0);
            writer.Write((ushort)(PhysicsSequance));
            writer.Write((ushort)JumpSequance);
            writer.Write((ushort)PortalSequance);
            writer.Write((ushort)unknownseq1);
            writer.Write((ushort)0);
            writer.Write((ushort)0);
            writer.Write((ushort)(SpawnSequance));

            writer.Align();

         
        }

    }

    //todo: /// move into its own file..

    /// <summary>
    /// This Class is used to add children 
    /// </summary>
    public class EquipedItem
    {
        public uint ModelId { get; }
        public EquipMask EquipperPhysicsDescriptionFlag { get; }

        public EquipedItem(uint modelid, EquipMask equipmask)
        {
            ModelId = modelid;
            EquipperPhysicsDescriptionFlag = equipmask;
        }
    }

}
