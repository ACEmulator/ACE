using System;
using System.Collections.Generic;
using ACE.Network;
using ACE.Network.Enum;
using System.IO;

namespace ACE.Entity.WorldPackages
{
    public class WorldObjectSegmentPhysicsData
    {
        public uint CSetup;
        public uint PhysicsDescriptionFlag;
        private uint Unknown = 0;

        public Position Position;
        public uint MTableResourceId;
        public uint SoundsResourceId;
        public uint Stable;
        public uint Petable;

        public uint Equipper;
        public EquipMask EquipperFlags;

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
        public void Render()
        {

        }

    }

    //todo: /// move into its own file..

    /// <summary>
    /// This Class is used to add children 
    /// </summary>
    public class EquipedItem
    {
        public uint ModelId { get; }
        public EquipMask EquipperFlags { get; }

        public EquipedItem(uint modelid, EquipMask equipmask)
        {
            ModelId = modelid;
            EquipperFlags = equipmask;
        }
    }

}
