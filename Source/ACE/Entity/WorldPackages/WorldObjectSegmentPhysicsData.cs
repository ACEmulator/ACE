﻿using ACE.Entity.Enum;
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
        public PhysicsState PhysicsState = 0;

        public Position Position;
        public uint MTableResourceId;
        public uint SoundsResourceId;
        public uint Stable;
        public uint Petable;

        //this are all related
        public uint ItemsEquipedCount;
        public uint Parent;
        public EquipMask EquipperPhysicsDescriptionFlag;
        private List<EquipedItem> children = new List<EquipedItem>();

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
        public ushort PositionSequence = (ushort)1;
        public ushort unknownseq0 = (ushort)1; // unknown for now
        public ushort PhysicsSequence = (ushort)1; // physics state change 
        public ushort JumpSequence = (ushort)1; // increments when you Jump.
        public ushort PortalSequence = (ushort)1; //increments when you portal
        public ushort unknownseq1 = (ushort)1;
        public ushort SpawnSequence = (ushort)1; // increments with spawn player / critter / boss ?


        public void AddEquipedItem(uint index, EquipMask equiperflag)
        {
            EquipedItem newitem = new EquipedItem(index, equiperflag);
            children.Add(newitem);
        }

        //todo: return bytes of data for network write ? ?
        public void Render(BinaryWriter writer)
        {

            writer.Write((uint)PhysicsDescriptionFlag);
            writer.Write((uint)PhysicsState);

            //if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Movement) != 0)
            //{
            //}

            //if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.AnimationFrame) != 0)
            //{
            //}

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Position) != 0)
                Position.Write(writer);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.MTable) != 0)
                writer.Write((uint)MTableResourceId);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Stable) != 0)
                writer.Write((uint)Stable);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Petable) != 0)
                writer.Write((uint)Petable);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.CSetup) != 0)
                writer.Write((uint)CSetup);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Children) != 0)
            {
                writer.Write((uint)CSetup);
            }

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Children) != 0)
            {
                writer.Write((uint)ItemsEquipedCount);
                foreach (EquipedItem child in children)
                {
                    writer.Write((uint)child.Guid);
                    writer.Write((uint)child.EquipMask);
                }
            }

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.ObjScale) != 0)
                writer.Write(ObjScale);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Friction) != 0)
                writer.Write(Friction);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Elastcity) != 0)
                writer.Write(Elastcity);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Translucency) != 0)
                writer.Write(Translucency);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Velocity) != 0)
            {
                Velocity.Write(writer);
            }

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Acceleration) != 0)
            {
                Acceleration.Write(writer);
            }

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Omega) != 0)
            {
                Omega.Write(writer);
            }

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.DefaultScript) != 0)
                writer.Write((uint)DefaultScript);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.DefaultScriptIntensity) != 0)
                writer.Write((float)DefaultScriptIntensity);

            //this look to be all se
            writer.Write((ushort)PositionSequence);
            writer.Write((ushort)unknownseq0);
            writer.Write((ushort)(PhysicsSequence));
            writer.Write((ushort)JumpSequence);
            writer.Write((ushort)PortalSequence);
            writer.Write((ushort)unknownseq1);
            writer.Write((ushort)0);
            writer.Write((ushort)0);
            writer.Write((ushort)(SpawnSequence));

            writer.Align();        
        }

    }
}
