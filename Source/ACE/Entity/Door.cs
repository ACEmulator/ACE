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
            Location = new Position(aceO.Location);
            WeenieClassid = aceO.WeenieClassId;
            WeenieFlags = (WeenieHeaderFlag)aceO.WeenieHeaderFlags;

            PhysicsData.MTableResourceId = aceO.MotionTableId;
            PhysicsData.Stable = aceO.SoundTableId;
            PhysicsData.CSetup = aceO.ModelTableId;
            PhysicsData.Petable = aceO.PhysicsTableId;
            // this.PhysicsData.PhysicsState = (PhysicsState)aceO.PhysicsState;
            PhysicsData.PhysicsState |= PhysicsState.HasPhysicsBsp | PhysicsState.ReportCollision;

            PhysicsData.CurrentMotionState = motionStateClosed;

            PhysicsData.ObjScale = aceO.DefaultScale;
            PhysicsData.AnimationFrame = aceO.AnimationFrameId;
            PhysicsData.Translucency = aceO.Translucency;

            // game data min required flags;
            Icon = aceO.IconId;

            GameDataType = aceO.ItemType;

            Usable = (Usable?)aceO.ItemUseable;
            RadarColor = (RadarColor?)aceO.BlipColor;
            RadarBehavior = (RadarBehavior?)aceO.Radar;
            UseRadius = aceO.UseRadius;

            aceO.AnimationOverrides.ForEach(ao => ModelData.AddModel(ao.Index, ao.AnimationId));
            aceO.TextureOverrides.ForEach(to => ModelData.AddTexture(to.Index, to.OldId, to.NewId));
            aceO.PaletteOverrides.ForEach(po => ModelData.AddPalette(po.SubPaletteId, po.Offset, po.Length));
            ModelData.PaletteGuid = (uint?)aceO.PaletteId;

            movementOpen.ForwardCommand = (ushort)MotionCommand.On;
            movementClosed.ForwardCommand = (ushort)MotionCommand.Off;
        }

        public override void OnUse(Player player)
        {
            // TODO: implement auto close timer, check if door is locked, send locked soundfx if locked and fail to open.

            if (PhysicsData.CurrentMotionState == motionStateClosed)
                Open(player);
            else
                Close(player);

            var sendUseDoneEvent = new GameEventUseDone(player.Session);
            player.Session.Network.EnqueueSend(sendUseDoneEvent);
        }

        private void Open(Player player)
        {
            player.EnqueueMovementEvent(motionOpen, Guid);
            PhysicsData.CurrentMotionState = motionStateOpen;
            PhysicsData.PhysicsState |= PhysicsState.Ethereal;
        }

        private void Close(Player player)
        {
            player.EnqueueMovementEvent(motionClosed, Guid);
            PhysicsData.CurrentMotionState = motionStateClosed;
            PhysicsData.PhysicsState ^= PhysicsState.Ethereal;
        }
    }
}
