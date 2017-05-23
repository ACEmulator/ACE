using ACE.Entity.Enum;
using ACE.Network;
using ACE.Network.Enum;
using ACE.Network.Sequence;
using ACE.Network.Motion;
using System.Collections.Generic;
using System.IO;

namespace ACE.Entity
{
    using System.Diagnostics;

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
        public EquipMask? EquipperPhysicsDescriptionFlag;
        private readonly List<EquippedItem> children = new List<EquippedItem>();

        public float? ObjScale;
        public float? Friction;
        public float? Elasticity;
        public uint? AnimationFrame;
        public AceVector3 Acceleration;
        public float? Translucency;
        public AceVector3 Velocity = null;
        public AceVector3 Omega = new AceVector3(0f, 0f, 0f);

        private MotionState currentMotionState = null;
        public MotionState CurrentMotionState
        {
            get { return currentMotionState; }
            set { currentMotionState = value; }
        }

        public uint? DefaultScript;
        public float? DefaultScriptIntensity;

        private readonly SequenceManager sequences;

        public PhysicsData(SequenceManager sequences)
        {
            this.sequences = sequences;
        }

        public void AddEquipedItem(uint index, EquipMask equipflag)
        {
            var newitem = new EquippedItem(index, equipflag);
            children.Add(newitem);
        }

        public PhysicsDescriptionFlag SetPhysicsDescriptionFlag()
        {
            var physicsDescriptionFlag = PhysicsDescriptionFlag.None;

            if (currentMotionState != null)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.Movement;

            if ((AnimationFrame != null) && (AnimationFrame != 0))
                physicsDescriptionFlag |= PhysicsDescriptionFlag.AnimationFrame;

            if (Position != null)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.Position;

            // NOTE: While we fill with 0 the flag still has to reflect that we are not really making this entry for the client.
            if (MTableResourceId != 0)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.MTable;

            if (Stable != 0)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.Stable;

            if (Petable != 0)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.Petable;

            if (CSetup != 0)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.CSetup;

            if (ItemsEquipedCount != 0)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.Children;

            if ((ObjScale != null) && (ObjScale >= 0.00f))
                physicsDescriptionFlag |= PhysicsDescriptionFlag.ObjScale;

            if (Friction <= 0.00f)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.Friction;

            if (Elasticity <= 0.00f)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.Elasticity;

            if (Translucency <= 0.00f)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.Translucency;

            if (Velocity != null)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.Velocity;

            if (Acceleration != null)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.Acceleration;

            if (Omega != null)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.Omega;

            if (DefaultScript != null)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.DefaultScript;

            if (DefaultScriptIntensity != null)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.DefaultScriptIntensity;

            return physicsDescriptionFlag;
        }
        // todo: return bytes of data for network write ? ?
        public void Serialize(WorldObject wo, BinaryWriter writer)
        {
            PhysicsDescriptionFlag = SetPhysicsDescriptionFlag();

            writer.Write((uint)PhysicsDescriptionFlag);

            writer.Write((uint)PhysicsState);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Movement) != 0)
            {
                if (currentMotionState != null)
                {
                    var movementData = currentMotionState.GetPayload(wo);
                    writer.Write(movementData.Length); // number of bytes in movement object
                    writer.Write(movementData);
                    uint autonomous = currentMotionState.IsAutonomous ? (ushort)1 : (ushort)0;
                    writer.Write(autonomous);
                }
                else // create a new current motion state and send it.
                {
                    currentMotionState = new UniversalMotion(MotionStance.Standing);
                    var movementData = currentMotionState.GetPayload(wo);
                    writer.Write(movementData.Length);
                    writer.Write(movementData);
                    uint autonomous = currentMotionState.IsAutonomous ? (ushort)1 : (ushort)0;
                    writer.Write(autonomous);
                }
            }

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.AnimationFrame) != 0)
                writer.Write((AnimationFrame ?? 0u));

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Position) != 0)
                Position.Serialize(writer);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.MTable) != 0)
                writer.Write(MTableResourceId);

            // stable_id =  BYTE1(v12) & 8 )  =  8
            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Stable) != 0)
                writer.Write(Stable);

            // setup id
            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Petable) != 0)
                writer.Write(Petable);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.CSetup) != 0)
                writer.Write(CSetup);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Children) != 0)
            {
                writer.Write(ItemsEquipedCount);
                foreach (var child in children)
                {
                    writer.Write(child.Guid);
                    writer.Write((uint)child.EquipMask);
                }
            }

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.ObjScale) != 0)
                writer.Write(ObjScale ?? 0.00f);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Friction) != 0)
                writer.Write(Friction ?? 0.00f);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Elasticity) != 0)
                writer.Write(Elasticity ?? 0.00f);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Translucency) != 0)
                writer.Write(Translucency ?? 0.00f);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Velocity) != 0)
            {
                Debug.Assert(Velocity != null, "Velocity != null");
                // We do a null check above and unset the flag so this has to be good.
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
                writer.Write(DefaultScript ?? 0u);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.DefaultScriptIntensity) != 0)
                writer.Write(DefaultScriptIntensity ?? 0.00f);

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
