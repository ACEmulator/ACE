﻿using ACE.Entity.Enum;
using ACE.Network;
using ACE.Network.Enum;
using System.Collections.Generic;
using System.IO;

namespace ACE.Entity
{
    public class PhysicsData
    {
        public uint CSetup;

        // apply default for back compat with player object
        public PhysicsDescriptionFlag PhysicsDescriptionFlag;
        public PhysicsState PhysicsState = 0;

        public Position Position;
        public uint MTableResourceId;
        public uint SoundsResourceId;
        public uint Stable;
        public uint Petable;

        // these are all related
        public uint ItemsEquipedCount;
        public uint Parent;
        public EquipMask EquipperPhysicsDescriptionFlag;
        private List<EquippedItem> children = new List<EquippedItem>();

        public float ObjScale;
        public float Friction;
        public float Elastcity;
        public uint AnimationFrame;
        public Position Acceleration;
        public float Translucency;
        public Position Velocity;
        public Position Omega; // rotation

        public uint DefaultScript;
        public float DefaultScriptIntensity;

        // thanks Kaezin for help understanding this structure.
        // Update this when the object moves
        public ushort PositionSequence = (ushort)1;
        public ushort unknownseq0 = (ushort)1; // unknown for now
        public ushort PhysicsSequence = (ushort)1; // physics state change
        public ushort JumpSequence = (ushort)1; // increments when you Jump.
        public ushort PortalSequence = (ushort)1; // increments when you portal
        public ushort unknownseq1 = (ushort)1;
        public ushort SpawnSequence = (ushort)1; // increments with spawn player / critter / boss ?

        public PhysicsData()
        {

        }

        public void AddEquipedItem(uint index, EquipMask equiperflag)
        {
            EquippedItem newitem = new EquippedItem(index, equiperflag);
            children.Add(newitem);
        }

        // todo: return bytes of data for network write ? ?
        public void Serialize(BinaryWriter writer)
        {

            writer.Write((uint)PhysicsDescriptionFlag);

            // autonomous_movement - required always ?
            // if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Movement) != 0)
            writer.Write((uint)PhysicsState);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.AnimationFrame) != 0)
                writer.Write((uint)AnimationFrame);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Position) != 0)
                Position.Serialize(writer);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.MTable) != 0)
                writer.Write((uint)MTableResourceId);

            // stable_id =  BYTE1(v12) & 8 )  =  8
            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Stable) != 0)
                writer.Write((uint)Stable);

            // setup id
            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Petable) != 0)
                writer.Write((uint)Petable);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.CSetup) != 0)
                writer.Write((uint)CSetup);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Children) != 0)
            {
                writer.Write((uint)ItemsEquipedCount);
                foreach (EquippedItem child in children)
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
                Velocity.Serialize(writer);
            }

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Acceleration) != 0)
            {
                Acceleration.Serialize(writer);
            }

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Omega) != 0)
            {
                Omega.Serialize(writer);
            }

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.DefaultScript) != 0)
                writer.Write((uint)DefaultScript);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.DefaultScriptIntensity) != 0)
                writer.Write((float)DefaultScriptIntensity);

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
