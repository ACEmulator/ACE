using ACE.Entity.Enum;
using ACE.Network.Enum;
using ACE.Network.Motion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity
{
    public class DebugObject : CollidableObject
    {
        public DebugObject(ObjectType type, ObjectGuid guid, string name, ushort weenieClassId, ObjectDescriptionFlag descriptionFlag, WeenieHeaderFlag weenieFlag, Position position)
            : base(type, guid, name, weenieClassId, descriptionFlag, weenieFlag, position)
        {
        }

        public DebugObject(AceObject aceO)
            : base((ObjectType)aceO.TypeId, new ObjectGuid(aceO.AceObjectId))
        {
            this.Name = aceO.Name;
            if (this.Name == null)
                this.Name = "NULL";

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
            this.PhysicsData.PhysicsState = (PhysicsState)aceO.PhysicsState;

            if (aceO.CurrentMotionState == "0")
                this.PhysicsData.CurrentMotionState = null;
            else
                this.PhysicsData.CurrentMotionState = new UniversalMotion(Convert.FromBase64String(aceO.CurrentMotionState));

            // this.PhysicsData.CurrentMotionState = new GeneralMotion(MotionStance.Standing, new MotionItem(MotionCommand.Off));
            // this.PhysicsData.CurrentMotionState = new GeneralMotion(MotionStance.Standing, new MotionItem(MotionCommand.Dead));

            this.PhysicsData.ObjScale = aceO.ObjectScale;
            this.PhysicsData.AnimationFrame = aceO.AnimationFrameId;

            // game data min required flags;
            this.Icon = aceO.IconId;

            if (this.GameData.NamePlural == null)
                this.GameData.NamePlural = "NULLs";

            this.GameData.Type = aceO.WeenieClassId;

            this.GameData.Usable = (Usable)aceO.Usability;
            this.GameData.RadarColour = (RadarColor)aceO.BlipColor;
            this.GameData.RadarBehavior = (RadarBehavior)aceO.Radar;
            this.GameData.UseRadius = aceO.UseRadius;

            this.GameData.HookType = (ushort)aceO.HookTypeId;
            this.GameData.HookItemTypes = aceO.HookItemTypes;
            this.GameData.Burden = (ushort)aceO.Burden;
            this.GameData.Value = aceO.Value;
            this.GameData.ItemCapacity = aceO.ItemsCapacity;

            aceO.AnimationOverrides.ForEach(ao => this.ModelData.AddModel(ao.Index, ao.AnimationId));
            aceO.TextureOverrides.ForEach(to => this.ModelData.AddTexture(to.Index, to.OldId, to.NewId));
            aceO.PaletteOverrides.ForEach(po => this.ModelData.AddPalette(po.SubPaletteId, po.Offset, po.Length));
            // aceO.PaletteOverrides.ForEach(po => this.ModelData.AddPalette(po.SubPaletteId, (byte)(po.Offset / 8), (byte)(po.Length / 8)));
            this.ModelData.PaletteGuid = aceO.PaletteId;
        }

        public override void OnCollide(Player player)
        {
            // TODO: Implement
        }
    }
}
