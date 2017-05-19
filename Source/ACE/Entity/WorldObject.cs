using ACE.Entity.Enum;
using ACE.Network;
using ACE.Network.Enum;
using ACE.Network.GameMessages.Messages;
using ACE.Network.Sequence;
using System.IO;
using ACE.Managers;
using ACE.Network.Motion;
using log4net;

namespace ACE.Entity
{
    public abstract class WorldObject
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ObjectGuid Guid { get; }

        public ObjectType Type { get; protected set; }

        /// <summary>
        /// wcid - stands for weenie class id
        /// </summary>
        public uint WeenieClassid { get; protected set; }

        public uint Icon { get; set; }

        public string Name { get; protected set; }

        /// <summary>
        /// tick-stamp for the last time this object changed in any way.
        /// </summary>
        public double LastUpdatedTicks { get; set; }

        // <summary>
        /// Time when this object will despawn, -1 is never.
        /// </summary>
        public double DespawnTime { get; set; } = -1;

        public virtual Position Location
        {
            get { return PhysicsData.Position; }
            protected set
            {
                if (PhysicsData.Position != null)
                    LastUpdatedTicks = WorldManager.PortalYearTicks;

                log.Debug($"{Name} moved to {PhysicsData.Position}");

                PhysicsData.Position = value;
            }
        }

        /// <summary>
        /// tick-stamp for the last time a movement update was sent
        /// </summary>
        public double LastMovementBroadcastTicks { get; set; }

        /// <summary>
        /// tick-stamp for the server time of the last time the player moved.
        /// TODO: implement
        /// </summary>
        public double LastAnimatedTicks { get; set; }

        public ObjectDescriptionFlag DescriptionFlags { get; protected set; }

        public WeenieHeaderFlag WeenieFlags { get; protected set; }

        public WeenieHeaderFlag2 WeenieFlags2 { get; protected set; }

        public UpdatePositionFlag PositionFlag { get; set; }

        public CombatMode CombatMode { get; private set; }

        public virtual void PlayScript(Session session) { }

        public virtual float ListeningRadius { get; protected set; } = 5f;

        public ModelData ModelData { get; }

        public PhysicsData PhysicsData { get; }

        public GameData GameData { get; }

        public SequenceManager Sequences { get; }

        protected WorldObject(ObjectType type, ObjectGuid guid)
        {
            Type = type;
            Guid = guid;

            GameData = new GameData();
            ModelData = new ModelData();

            Sequences = new SequenceManager();
            Sequences.AddOrSetSequence(SequenceType.ObjectPosition, new UShortSequence());
            Sequences.AddOrSetSequence(SequenceType.ObjectMovement, new UShortSequence());
            Sequences.AddOrSetSequence(SequenceType.ObjectState, new UShortSequence());
            Sequences.AddOrSetSequence(SequenceType.ObjectVector, new UShortSequence());
            Sequences.AddOrSetSequence(SequenceType.ObjectTeleport, new UShortSequence());
            Sequences.AddOrSetSequence(SequenceType.ObjectServerControl, new UShortSequence());
            Sequences.AddOrSetSequence(SequenceType.ObjectForcePosition, new UShortSequence());
            Sequences.AddOrSetSequence(SequenceType.ObjectVisualDesc, new UShortSequence());
            Sequences.AddOrSetSequence(SequenceType.ObjectInstance, new UShortSequence());
            Sequences.AddOrSetSequence(SequenceType.Motion, new UShortSequence(1));

            PhysicsData = new PhysicsData(Sequences);
        }

        public void SetCombatMode(CombatMode newCombatMode)
        {
            log.InfoFormat("Changing combat mode for {0} to {1}", this.Guid, newCombatMode);
            // TODO: any sort of validation
            CombatMode = newCombatMode;
            switch (CombatMode)
            {
                case CombatMode.Peace:
                    SetMotionState(new UniversalMotion(MotionStance.Standing));
                    break;
                case CombatMode.Melee:
                    var gm = new UniversalMotion(MotionStance.UANoShieldAttack);
                    gm.MovementData.CurrentStyle = (ushort)MotionStance.UANoShieldAttack;
                    SetMotionState(gm);
                    break;
            }
        }

        public void SetMotionState(MotionState motionState)
        {
            Player p = (Player)this;
            PhysicsData.CurrentMotionState = motionState;
            var updateMotion = new GameMessageUpdateMotion(this, p.Session, motionState);
            p.Session.Network.EnqueueSend(updateMotion);
        }

        public virtual void SerializeUpdateObject(BinaryWriter writer)
        {
            // content of these 2 is the same? TODO: Validate that?
            SerializeCreateObject(writer);
        }

        public virtual void SerializeCreateObject(BinaryWriter writer)
        {
            writer.WriteGuid(Guid);

            ModelData.Serialize(writer);
            PhysicsData.Serialize(this, writer);

            writer.Write((uint)WeenieFlags);
            writer.WriteString16L(Name);
            writer.WritePackedDword(WeenieClassid);
            writer.WritePackedDwordOfKnownType(Icon, 0x6000000);
            writer.Write((uint)Type);
            writer.Write((uint)DescriptionFlags);
            writer.Align();

            if ((DescriptionFlags & ObjectDescriptionFlag.AdditionFlags) != 0)
                writer.Write((uint)WeenieFlags2);

            if ((WeenieFlags & WeenieHeaderFlag.PuralName) != 0)
                writer.WriteString16L(GameData.NamePlural);

            if ((WeenieFlags & WeenieHeaderFlag.ItemCapacity) != 0)
                writer.Write(GameData.ItemCapacity ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.ContainerCapacity) != 0)
                writer.Write(GameData.ContainerCapacity ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.AmmoType) != 0)
                writer.Write((ushort?)GameData.AmmoType ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.Value) != 0)
                writer.Write(GameData.Value ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.Usable) != 0)
                writer.Write((uint?)GameData.Usable ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.UseRadius) != 0)
                writer.Write(GameData.UseRadius ?? 0.00f);

            if ((WeenieFlags & WeenieHeaderFlag.TargetType) != 0)
                writer.Write(GameData.TargetType ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.UiEffects) != 0)
                writer.Write((uint?)GameData.UiEffects ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.CombatUse) != 0)
                writer.Write((byte?)GameData.CombatUse ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.Struture) != 0)
                writer.Write(GameData.Structure ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.MaxStructure) != 0)
                writer.Write(GameData.MaxStructure ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.StackSize) != 0)
                writer.Write(GameData.StackSize ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.MaxStackSize) != 0)
                writer.Write(GameData.MaxStackSize ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.Container) != 0)
                writer.Write(GameData.ContainerId ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.Wielder) != 0)
                writer.Write(GameData.Wielder ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.ValidLocations) != 0)
                writer.Write((uint?)GameData.ValidLocations ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.CurrentlyWeildedLocation) != 0)
                writer.Write((uint?)GameData.Location ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.Priority) != 0)
                writer.Write((uint?)GameData.Priority ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.RadarBlipColor) != 0)
                writer.Write((byte?)GameData.RadarColor ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.RadarBehavior) != 0)
                writer.Write((byte?)GameData.RadarBehavior ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.Script) != 0)
                writer.Write(GameData.Script ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.Workmanship) != 0)
                writer.Write((float)GameData.Workmanship);

            if ((WeenieFlags & WeenieHeaderFlag.Burden) != 0)
                writer.Write(GameData.Burden ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.Spell) != 0)
                writer.Write((uint?)GameData.Spell ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.HouseOwner) != 0)
                writer.Write(GameData.HouseOwner ?? 0u);

            /*if ((WeenieFlags & WeenieHeaderFlag.HouseRestrictions) != 0)
            {
            }*/

            if ((WeenieFlags & WeenieHeaderFlag.HookItemTypes) != 0)
                writer.Write(GameData.HookItemTypes ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.Monarch) != 0)
                writer.Write(GameData.Monarch ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.HookType) != 0)
                writer.Write(GameData.HookType ?? 0u);

            if ((WeenieFlags & WeenieHeaderFlag.IconOverlay) != 0)
                writer.WritePackedDwordOfKnownType((uint)GameData.IconOverlay, 0x6000000);

            if ((WeenieFlags2 & WeenieHeaderFlag2.IconUnderlay) != 0)
                writer.WritePackedDwordOfKnownType((uint)GameData.IconUnderlay, 0x6000000);

            if (((WeenieFlags & WeenieHeaderFlag.Material) != 0) && GameData.Monarch != null)
                writer.Write((uint)GameData.Material);

            /*if ((WeenieFlags2 & WeenieHeaderFlag2.Cooldown) != 0)
                writer.Write(0u);*/

            /*if ((WeenieFlags2 & WeenieHeaderFlag2.CooldownDuration) != 0)
                writer.Write(0.0d);*/

            /*if ((WeenieFlags2 & WeenieHeaderFlag2.PetOwner) != 0)
                writer.Write(0u);*/

            writer.Align();
        }

        public void WriteUpdatePositionPayload(BinaryWriter writer)
        {
            writer.WriteGuid(Guid);
            Location.Serialize(writer, PositionFlag);
            writer.Write(Sequences.GetCurrentSequence(SequenceType.ObjectInstance));
            writer.Write(Sequences.GetNextSequence(SequenceType.ObjectPosition));
            writer.Write(Sequences.GetCurrentSequence(SequenceType.ObjectTeleport));
            writer.Write(Sequences.GetCurrentSequence(SequenceType.ObjectForcePosition));
        }
    }
}
