using ACE.Entity.Enum;
using ACE.Network.Enum;
using ACE.Network.GameEvent.Events;
using ACE.Network.Motion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity
{
    public class Door : UsableObject
    {
        private static MovementData movementOpen = new MovementData();
        private static MovementData movementClosed = new MovementData();

        private static MotionState motionStateOpen = new UniversalMotion(MotionStance.Standing, movementOpen);
        private static MotionState motionStateClosed = new UniversalMotion(MotionStance.Standing, movementClosed);

        private static UniversalMotion motionOpen = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.On));
        private static UniversalMotion motionClosed = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.Off));

        public Door(AceObject aceO)
            : base((ObjectType)aceO.ItemType, new ObjectGuid(aceO.AceObjectId))
        {
            this.Name = aceO.Name;

            this.DescriptionFlags = (ObjectDescriptionFlag)aceO.AceObjectDescriptionFlags;
            this.Location = aceO.Position;
            this.WeenieClassid = aceO.WeenieClassId;
            this.WeenieFlags = (WeenieHeaderFlag)aceO.WeenieHeaderFlags;

            this.PhysicsData.MTableResourceId = aceO.MotionTableId;
            this.PhysicsData.Stable = aceO.SoundTableId;
            this.PhysicsData.CSetup = aceO.ModelTableId;
            this.PhysicsData.Petable = aceO.PhysicsTableId;

            // this should probably be determined based on the presence of data.
            this.PhysicsData.PhysicsDescriptionFlag = (PhysicsDescriptionFlag)aceO.PhysicsDescriptionFlag;
            // this.PhysicsData.PhysicsState = (PhysicsState)aceO.PhysicsState;
            this.PhysicsData.PhysicsState |= PhysicsState.HasPhysicsBsp | PhysicsState.ReportCollision;

            this.PhysicsData.CurrentMotionState = motionStateClosed;

            this.PhysicsData.ObjScale = (float)aceO.DefaultScale;
            this.PhysicsData.AnimationFrame = aceO.AnimationFrameId;
            this.PhysicsData.Translucency = (float)aceO.Translucency;

            // game data min required flags;
            this.Icon = aceO.IconId;

            this.GameData.Type = aceO.WeenieClassId;

            this.GameData.Usable = (Usable)aceO.ItemUseable;
            this.GameData.RadarColour = (RadarColor)aceO.BlipColor;
            this.GameData.RadarBehavior = (RadarBehavior)aceO.Radar;
            this.GameData.UseRadius = aceO.UseRadius;

            aceO.AnimationOverrides.ForEach(ao => this.ModelData.AddModel(ao.Index, ao.AnimationId));
            aceO.TextureOverrides.ForEach(to => this.ModelData.AddTexture(to.Index, to.OldId, to.NewId));
            aceO.PaletteOverrides.ForEach(po => this.ModelData.AddPalette(po.SubPaletteId, po.Offset, po.Length));
            this.ModelData.PaletteGuid = (uint)aceO.PaletteId;

            movementOpen.ForwardCommand = (ushort)MotionCommand.On;
            movementClosed.ForwardCommand = (ushort)MotionCommand.Off;
        }

        public override void OnUse(Player player)
        {
            // TODO: implement auto close timer, check if door is locked, send locked soundfx if locked and fail to open.

            if (this.PhysicsData.CurrentMotionState == motionStateClosed)
                Open(player);
            else
                Close(player);

            var sendUseDoneEvent = new GameEventUseDone(player.Session);
            player.Session.Network.EnqueueSend(sendUseDoneEvent);
        }

        private void Open(Player player)
        {
            player.EnqueueMovementEvent(motionOpen, this.Guid);
            this.PhysicsData.CurrentMotionState = motionStateOpen;
            this.PhysicsData.PhysicsState |= PhysicsState.Ethereal;
        }

        private void Close(Player player)
        {
            player.EnqueueMovementEvent(motionClosed, this.Guid);
            this.PhysicsData.CurrentMotionState = motionStateClosed;
            this.PhysicsData.PhysicsState ^= PhysicsState.Ethereal;
        }
    }
}
