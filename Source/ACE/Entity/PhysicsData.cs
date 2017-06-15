using System;
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

    /// <summary>
    /// TODO: Remove and use WorldObject.AceObject
    /// </summary>
    public class PhysicsData
    {
        public uint? CSetup;

        // apply default for back compat with player object
        public PhysicsDescriptionFlag PhysicsDescriptionFlag;
        public PhysicsState PhysicsState = 0;

        public Position Position;
        public uint? MTableResourceId;
        public uint? SoundsResourceId;
        public uint? Stable;
        public uint? Petable;

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
        public AceVector3 Omega = null;

        private MotionState currentMotionState;
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

        public PhysicsDescriptionFlag SetPhysicsDescriptionFlag(WorldObject wo)
        {
            var physicsDescriptionFlag = PhysicsDescriptionFlag.None;

            if (currentMotionState != null)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.Movement;

            if ((AnimationFrame != null) && (AnimationFrame != 0))
                physicsDescriptionFlag |= PhysicsDescriptionFlag.AnimationFrame;

            if ((Position != null) && (wo.Wielder == null) && (wo.ContainerId == null))
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

            if (Parent != 0)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.Parent;

            if ((ObjScale != null) && (Math.Abs((float)ObjScale) >= 0.001))
                physicsDescriptionFlag |= PhysicsDescriptionFlag.ObjScale;

            if (Friction != null)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.Friction;

            if (Elasticity != null)
                physicsDescriptionFlag |= PhysicsDescriptionFlag.Elasticity;

            if ((Translucency != null) && (Math.Abs((float)Translucency) >= 0.001))
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
            // TODO: Remove this really ugly hack - POC on how equipted items (weapons and shields maybe other things)
            // We do not have a way to save or load this data yet.   I am looking for Rand the humter in holtburg
            // I am adding his spear as a child item.
            if (wo.Guid.Full == 0xDBB12C7E)
            {
                // this is implementation for children mapping to equipMask is  wrong.
                // Just made an interesting discovery.PhysicsData.Children is never greater than 2 in any of the 4 million create objects.
                // Maybe everyone knew this but It looks like that is only used for items that can be selected when wielded.weapon caster
                // or shield in either hand.that is represented as id (the item you are wielding) and a location_id
                // we currently have that mapped to equipedmask but that is not correct.There are only 2 values I can find 1 or 3.
                // 1 seems to be your weapon or caster while 3 is shield.I don't see any enums that map to that.
                var item = new EquippedItem(0xDBAC8105, EquipMask.MeleeWeapon);
                wo.PhysicsData.children.Add(item);
                ItemsEquipedCount = (uint)wo.PhysicsData.children.Count;
            }

            // Here I am looking for the spear and making Rand the parent.
            // There is a squence here that we will need to figure out.   You only see this if you login, then log out
            // and log back in.   I THINK we need have the landblock create an times that have a weilder_Id - I am not sure
            // but either the spear must exist and have the holder assigned as it's parent or the holder has to exist first not sure
            // which but there is some sequence.
            // TODO: Remove this really ugly hack - POC on how equipted items (weapons and shields maybe other things)

            if (wo.Guid.Full == 0xDBAC8105)
            {
                wo.ValidLocations = EquipMask.BraceletRight;
                wo.CurrentWieldedLocation = EquipMask.BraceletRight;
                wo.PhysicsData.Parent = 0xDBB12C7E;
            }

            PhysicsDescriptionFlag = SetPhysicsDescriptionFlag(wo);

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
                writer.Write(MTableResourceId ?? 0u);

            // stable_id =  BYTE1(v12) & 8 )  =  8
            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Stable) != 0)
                writer.Write(Stable ?? 0u);

            // setup id
            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Petable) != 0)
                writer.Write(Petable ?? 0u);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.CSetup) != 0)
                writer.Write(CSetup ?? 0u);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Parent) != 0)
            {
                writer.Write(Parent);
                writer.Write((uint)EquipperPhysicsDescriptionFlag);
            }

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Children) != 0)
            {
                writer.Write(ItemsEquipedCount);
                foreach (var child in children)
                {
                    // TODO : need to figure out the location (second value) to write - it is not the mask 1 is in left hand 3 look to be in right?
                    // maybe some sort of slot enum?   We can probably find it in the client.
                    writer.Write(child.Guid);
                    // writer.Write((uint)child.EquipMask);  - this is not right.
                    writer.Write(1u);
                }
            }

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.ObjScale) != 0)
                writer.Write(ObjScale ?? 0u);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Friction) != 0)
                writer.Write(Friction ?? 0u);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Elasticity) != 0)
                writer.Write(Elasticity ?? 0u);

            if ((PhysicsDescriptionFlag & PhysicsDescriptionFlag.Translucency) != 0)
                writer.Write(Translucency ?? 0u);

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
                writer.Write(DefaultScriptIntensity ?? 0u);

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
