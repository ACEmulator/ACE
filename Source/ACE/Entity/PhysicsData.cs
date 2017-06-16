using ACE.Entity.Enum;
using ACE.Network;
using ACE.Network.Enum;
using ACE.Network.Sequence;
using ACE.Network.Motion;
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
        public AceVector3 Acceleration;
        public float Translucency;
        public AceVector3 Velocity = null;
        public AceVector3 Omega = new AceVector3(0f, 0f, 0f);

        private MotionState currentMotionState = null;
        public MotionState CurrentMotionState
        {
            get { return currentMotionState; }
            set { currentMotionState = value; }
        }

        public uint DefaultScript;
        public float DefaultScriptIntensity;

        private SequenceManager sequences;

        public PhysicsData(SequenceManager sequences)
        {
            this.sequences = sequences;
        }

        public void AddEquipedItem(uint index, EquipMask equiperflag)
        {
            EquippedItem newitem = new EquippedItem(index, equiperflag);
            children.Add(newitem);
        }

        // todo: return bytes of data for network write ? ?
        public void Serialize(WorldObject wo, BinaryWriter writer)
        {
            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Velocity) > 0 && this.Velocity == null)
            {
                // velocity is null, but the flag wants to include it.  unset the flag.
                PhysicsDescriptionFlag ^= PhysicsDescriptionFlag.Velocity;
            }

            writer.Write((uint)PhysicsDescriptionFlag);

            writer.Write((uint)PhysicsState);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Movement) != 0)
            {
                if (currentMotionState != null)
                {
                    var movementData = currentMotionState.GetPayload(wo.Guid, wo.Sequences);
                    writer.Write(movementData.Length); // number of bytes in movement object
                    writer.Write(movementData);
                    uint autonomous = currentMotionState.IsAutonomous ? (ushort)1 : (ushort)0;
                    writer.Write(autonomous);
                }
                else // create a new current motion state and send it.
                {
                    currentMotionState = new UniversalMotion(MotionStance.Standing);
                    var movementData = currentMotionState.GetPayload(wo.Guid, wo.Sequences);
                    writer.Write(movementData.Length);
                    writer.Write(movementData);
                    uint autonomous = currentMotionState.IsAutonomous ? (ushort)1 : (ushort)0;
                    writer.Write(autonomous);
                }
            }

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

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Elasticity) != 0)
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

            // TODO: There are 9 of these - but we need to research the correct sequence.   I know that the last one is instance (totalLogins) Og II
            writer.Write(sequences.GetCurrentSequence(SequenceType.ObjectPosition));
            writer.Write(sequences.GetCurrentSequence(SequenceType.ObjectMovement));
            writer.Write(sequences.GetCurrentSequence(SequenceType.ObjectState));
            writer.Write(sequences.GetCurrentSequence(SequenceType.ObjectVector));
            writer.Write(sequences.GetCurrentSequence(SequenceType.ObjectTeleport));
            writer.Write(sequences.GetCurrentSequence(SequenceType.ObjectServerControl));
            writer.Write(sequences.GetCurrentSequence(SequenceType.ObjectForcePosition));
            writer.Write(sequences.GetCurrentSequence(SequenceType.ObjectVisualDesc));
            writer.Write(sequences.GetCurrentSequence(SequenceType.ObjectInstance));

            writer.Align();
        }
    }
}
