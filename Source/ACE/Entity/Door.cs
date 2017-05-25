using ACE.Entity.Enum;
using ACE.Network.Enum;
using ACE.Network.GameEvent.Events;
using ACE.Network.Motion;

namespace ACE.Entity
{
    public class Door : UsableObject
    {
        private static readonly MovementData movementOpen = new MovementData();
        private static readonly MovementData movementClosed = new MovementData();

        private static readonly MotionState motionStateOpen = new UniversalMotion(MotionStance.Standing, movementOpen);
        private static readonly MotionState motionStateClosed = new UniversalMotion(MotionStance.Standing, movementClosed);

        private static readonly UniversalMotion motionOpen = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.On));
        private static readonly UniversalMotion motionClosed = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.Off));

        public Door(AceObject aceO)
            : base((ObjectType)aceO.ItemType, new ObjectGuid(aceO.AceObjectId))
        {
            Name = aceO.Name;

            DescriptionFlags = (ObjectDescriptionFlag)aceO.AceObjectDescriptionFlags;
            Location = aceO.Position;
            WeenieClassid = aceO.WeenieClassId;
            WeenieFlags = (WeenieHeaderFlag)aceO.WeenieHeaderFlags;

            MTableResourceId = aceO.MotionTableId;
            Stable = aceO.SoundTableId;
            CSetup = aceO.ModelTableId;
            Petable = aceO.PhysicsTableId;
            // PhysicsState = (PhysicsState)aceO.PhysicsState;
            PhysicsState |= PhysicsState.HasPhysicsBsp | PhysicsState.ReportCollision;

            CurrentMotionState = motionStateClosed;

            ObjScale = aceO.DefaultScale;
            AnimationFrame = aceO.AnimationFrameId;
            Translucency = aceO.Translucency;

            // game data min required flags;
            Icon = aceO.IconId;

            GameDataType = (ushort)aceO.ItemType;

            Usable = (Usable?)aceO.ItemUseable;
            RadarColor = (RadarColor?)aceO.BlipColor;
            RadarBehavior = (RadarBehavior?)aceO.Radar;
            UseRadius = aceO.UseRadius;

            aceO.AnimationOverrides.ForEach(ao => AddModel(ao.Index, ao.AnimationId));
            aceO.TextureOverrides.ForEach(to => AddTexture(to.Index, to.OldId, to.NewId));
            aceO.PaletteOverrides.ForEach(po => AddPalette(po.SubPaletteId, po.Offset, po.Length));
            PaletteGuid = (uint?)aceO.PaletteId;

            movementOpen.ForwardCommand = (ushort)MotionCommand.On;
            movementClosed.ForwardCommand = (ushort)MotionCommand.Off;
        }

        public override void OnUse(Player player)
        {
            // TODO: implement auto close timer, check if door is locked, send locked soundfx if locked and fail to open.

            if (CurrentMotionState == motionStateClosed)
                Open(player);
            else
                Close(player);

            var sendUseDoneEvent = new GameEventUseDone(player.Session);
            player.Session.Network.EnqueueSend(sendUseDoneEvent);
        }

        private void Open(Player player)
        {
            player.EnqueueMovementEvent(motionOpen, Guid);
            CurrentMotionState = motionStateOpen;
            PhysicsState |= PhysicsState.Ethereal;
        }

        private void Close(Player player)
        {
            player.EnqueueMovementEvent(motionClosed, Guid);
            CurrentMotionState = motionStateClosed;
            PhysicsState ^= PhysicsState.Ethereal;
        }
    }
}
