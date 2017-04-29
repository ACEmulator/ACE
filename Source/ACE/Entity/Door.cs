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

        // private static MotionState motionOpen = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.On));
        private static MotionState motionStateOpen = new UniversalMotion(MotionStance.Standing, movementOpen);
        private static MotionState motionStateClosed = new UniversalMotion(MotionStance.Standing, movementClosed);
        // private static MotionState motionClosed = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.Off));

        private static UniversalMotion motionOpen = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.On));
        private static UniversalMotion motionClosed = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.Off));

        public Door(AceObject aceO)
            : base((ObjectType)aceO.TypeId, new ObjectGuid(aceO.AceObjectId))
        {
            // this.Name = aceO.Name;
            // this.DescriptionFlags = (ObjectDescriptionFlag)aceO.WdescBitField;
            // this.Location = aceO.Position;
            // this.WeenieClassid = aceO.WeenieClassId;
            // this.WeenieFlags = (WeenieHeaderFlag)aceO.WeenieFlags;

            // this.PhysicsData.MTableResourceId = aceO.MotionTableId;
            // this.PhysicsData.Stable = aceO.SoundTableId;
            // this.PhysicsData.CSetup = aceO.ModelTableId;

            //// this should probably be determined based on the presence of data.
            // this.PhysicsData.PhysicsDescriptionFlag = (PhysicsDescriptionFlag)aceO.PhysicsBitField;
            // this.PhysicsData.PhysicsState = (PhysicsState)aceO.PhysicsState;

            //// game data min required flags;
            // this.Icon = aceO.IconId;

            // this.GameData.Usable = (Usable)aceO.Usability;
            // this.GameData.RadarColour = (RadarColor)aceO.BlipColor;
            // this.GameData.RadarBehavior = (RadarBehavior)aceO.Radar;
            // this.GameData.UseRadius = aceO.UseRadius;

            // aceO.AnimationOverrides.ForEach(ao => this.ModelData.AddModel(ao.Index, (ushort)ao.AnimationId));
            // aceO.TextureOverrides.ForEach(to => this.ModelData.AddTexture(to.Index, (ushort)to.OldId, (ushort)to.NewId));
            // aceO.PaletteOverrides.ForEach(po => this.ModelData.AddPalette(po.SubPaletteId, po.Offset, po.Length));

            this.Name = aceO.Name;
            // if (this.Name == null)
            //    this.Name = "NULL";

            this.DescriptionFlags = (ObjectDescriptionFlag)aceO.WdescBitField;
            this.Location = aceO.Position;
            this.WeenieClassid = aceO.WeenieClassId;
            this.WeenieFlags = (WeenieHeaderFlag)aceO.WeenieFlags;

            this.PhysicsData.MTableResourceId = aceO.MotionTableId;
            this.PhysicsData.Stable = aceO.SoundTableId;
            this.PhysicsData.CSetup = aceO.ModelTableId;
            this.PhysicsData.Petable = aceO.PhysicsTableId;

            // this should probably be determined based on the presence of data.
            this.PhysicsData.PhysicsDescriptionFlag = (PhysicsDescriptionFlag)aceO.PhysicsBitField;
            // this.PhysicsData.PhysicsState = (PhysicsState)aceO.PhysicsState;
            this.PhysicsData.PhysicsState |= PhysicsState.HasPhysicsBsp | PhysicsState.ReportCollision;

            // if (aceO.CurrentMotionState == "0")
            //    this.PhysicsData.CurrentMotionState = null;
            // else
            //    this.PhysicsData.CurrentMotionState = new UniversalMotion(Convert.FromBase64String(aceO.CurrentMotionState));

            this.PhysicsData.CurrentMotionState = motionStateClosed;

            // this.PhysicsData.CurrentMotionState = new GeneralMotion(MotionStance.Standing, new MotionItem(MotionCommand.Off));
            // this.PhysicsData.CurrentMotionState = new GeneralMotion(MotionStance.Standing, new MotionItem(MotionCommand.Dead));

            this.PhysicsData.ObjScale = aceO.ObjectScale;
            this.PhysicsData.AnimationFrame = aceO.AnimationFrameId;
            this.PhysicsData.Translucency = aceO.Translucency;

            // game data min required flags;
            this.Icon = aceO.IconId;

            // if (this.GameData.NamePlural == null)
            //    this.GameData.NamePlural = "NULLs";

            this.GameData.Type = aceO.WeenieClassId;

            this.GameData.Usable = (Usable)aceO.Usability;
            this.GameData.RadarColour = (RadarColor)aceO.BlipColor;
            this.GameData.RadarBehavior = (RadarBehavior)aceO.Radar;
            this.GameData.UseRadius = aceO.UseRadius;

            // this.GameData.HookType = (ushort)aceO.HookTypeId;
            // this.GameData.HookItemTypes = aceO.HookItemTypes;
            // this.GameData.Burden = (ushort)aceO.Burden;
            // this.GameData.Value = aceO.Value;
            // this.GameData.ItemCapacity = aceO.ItemsCapacity;

            aceO.AnimationOverrides.ForEach(ao => this.ModelData.AddModel(ao.Index, ao.AnimationId));
            aceO.TextureOverrides.ForEach(to => this.ModelData.AddTexture(to.Index, to.OldId, to.NewId));
            aceO.PaletteOverrides.ForEach(po => this.ModelData.AddPalette(po.SubPaletteId, po.Offset, po.Length));
            // aceO.PaletteOverrides.ForEach(po => this.ModelData.AddPalette(po.SubPaletteId, (byte)(po.Offset / 8), (byte)(po.Length / 8)));
            this.ModelData.PaletteGuid = aceO.PaletteId;

            movementOpen.ForwardCommand = (ushort)MotionCommand.On;
            movementClosed.ForwardCommand = (ushort)MotionCommand.Off;
        }

        public override void OnUse(Player player)
        {
            // TODO: implement auto close timer, check if door is locked, send locked soundfx if locked and fail to open.

            if (this.PhysicsData.CurrentMotionState == motionStateClosed)
            {
                // test here
                // player.EnqueueMovementEvent(motionOpen, player.Guid);
                // var motionOpen = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.On));

                // player.EnqueueMovementEvent(motionOpen, this.Guid);
                // this.PhysicsData.CurrentMotionState = motionOpen;
                // this.PhysicsData.PhysicsState |= PhysicsState.Ethereal;

                Open(player);
                // var autoEvent = new System.Threading.AutoResetEvent(false);
                //// var test = new System.Threading.Timer(Door.Close(stateInfo, player), autoEvent, 5000, System.Threading.Timeout.Infinite);
                // StateObjClass StateObj = new StateObjClass();
                // System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(Close);
                //// var test = new System.Threading.Timer(TimerDelegate, autoEvent, 5000, 0);
                // System.Threading.Timer TimerItem = new System.Threading.Timer(TimerDelegate, StateObj, 2000, 2000);
            }
            else
            {
                // var motionOpen = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.Off));

                // player.EnqueueMovementEvent(motionClosed, this.Guid);
                // this.PhysicsData.CurrentMotionState = motionClosed;
                // this.PhysicsData.PhysicsState ^= PhysicsState.Ethereal;

                // Close(player);
            }

            var sendUseDoneEvent = new GameEventUseDone(player.Session);
            player.Session.Network.EnqueueSend(sendUseDoneEvent);
        }

        private void Open(Player player)
        {
            player.EnqueueMovementEvent(motionOpen, this.Guid);
            this.PhysicsData.CurrentMotionState = motionOpen;
            this.PhysicsData.PhysicsState |= PhysicsState.Ethereal;
        }

        private void Close(Player player, object StateObj)
        //private void Close(Object stateInfo)
        // private void Close(Player player)
        {
            // this.StateObj.player.EnqueueMovementEvent(motionClosed, this.Guid);
            this.PhysicsData.CurrentMotionState = motionClosed;
            this.PhysicsData.PhysicsState ^= PhysicsState.Ethereal;
        }

        // private class StateObjClass
        // {
        //    // Used to hold parameters for calls to TimerTask.  
        //    public int SomeValue;
        //    public System.Threading.Timer TimerReference;
        //    public bool TimerCanceled;
        //
        //    public Player player;
        // }
    }
}
