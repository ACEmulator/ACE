using ACE.Entity.Enum;
using ACE.Network.Enum;
using ACE.Network.Motion;
using System;

namespace ACE.Entity
{
    public class TownCrierAluvianFemale : Vendor
    {
        public TownCrierAluvianFemale(ObjectType type, ObjectGuid guid, string name, ushort weenieClassId, ObjectDescriptionFlag descriptionFlag, WeenieHeaderFlag weenieFlag, Position position)
            : base(type, guid, name, weenieClassId, descriptionFlag, weenieFlag, position)
        {
        }

        public TownCrierAluvianFemale(ObjectGuid guid, BaseAceObject baseAceObject)
            : base(guid, baseAceObject)
        {
            // this.Name = baseAceObject.Name;
            // if (this.Name == null)
            //    this.Name = "NULL";

            // this.DescriptionFlags = (ObjectDescriptionFlag)baseAceObject.WdescBitField;
            // this.WeenieClassid = baseAceObject.AceObjectId;
            // this.WeenieFlags = (WeenieHeaderFlag)baseAceObject.WeenieFlags;

            // this.PhysicsData.MTableResourceId = baseAceObject.MotionTableId;
            // this.PhysicsData.Stable = baseAceObject.SoundTableId;
            // this.PhysicsData.CSetup = baseAceObject.ModelTableId;
            // this.PhysicsData.Petable = baseAceObject.PhysicsTableId;

            //// this should probably be determined based on the presence of data.
            // this.PhysicsData.PhysicsDescriptionFlag = (PhysicsDescriptionFlag)baseAceObject.PhysicsBitField;
            // this.PhysicsData.PhysicsState = (PhysicsState)baseAceObject.PhysicsState;

            // if (baseAceObject.CurrentMotionState == "0")
            //    this.PhysicsData.CurrentMotionState = null;
            // else
            //    this.PhysicsData.CurrentMotionState = new UniversalMotion(Convert.FromBase64String(baseAceObject.CurrentMotionState));

            // this.PhysicsData.ObjScale = baseAceObject.ObjectScale;
            // this.PhysicsData.AnimationFrame = baseAceObject.AnimationFrameId;
            // this.PhysicsData.Translucency = baseAceObject.Translucency;

            //// game data min required flags;
            // this.Icon = baseAceObject.IconId;

            // if (this.GameData.NamePlural == null)
            //    this.GameData.NamePlural = "NULLs";

            // this.GameData.Type = (ushort)baseAceObject.AceObjectId;

            // this.GameData.Usable = (Usable)baseAceObject.Usability;
            // this.GameData.RadarColour = (RadarColor)baseAceObject.BlipColor;
            // this.GameData.RadarBehavior = (RadarBehavior)baseAceObject.Radar;
            // this.GameData.UseRadius = baseAceObject.UseRadius;

            // this.GameData.HookType = (ushort)baseAceObject.HookTypeId;
            // this.GameData.HookItemTypes = baseAceObject.HookItemTypes;
            // this.GameData.Burden = (ushort)baseAceObject.Burden;
            // this.GameData.Value = baseAceObject.Value;
            // this.GameData.ItemCapacity = baseAceObject.ItemsCapacity;

            // if (this.PhysicsData.AnimationFrame != 0)
            //    this.PhysicsData.PhysicsDescriptionFlag |= PhysicsDescriptionFlag.AnimationFrame;
            // this.PhysicsData.DefaultScript = baseAceObject.DefaultScript;
            // this.PhysicsData.DefaultScriptIntensity = baseAceObject.DefaultScriptIntensity;
            // this.PhysicsData.Elastcity = baseAceObject.Elasticity;
            // this.PhysicsData.Friction = baseAceObject.Friction;

            // baseAceObject.AnimationOverrides.ForEach(ao => this.ModelData.AddModel(ao.Index, ao.AnimationId));
            // baseAceObject.TextureOverrides.ForEach(to => this.ModelData.AddTexture(to.Index, to.OldId, to.NewId));
            // baseAceObject.PaletteOverrides.ForEach(po => this.ModelData.AddPalette(po.SubPaletteId, po.Offset, po.Length));
            // this.ModelData.PaletteGuid = baseAceObject.PaletteId;
        }

        public TownCrierAluvianFemale(AceObject aceO)
            : this(new ObjectGuid(aceO.AceObjectId), aceO)
        {
            // this.Location = aceO.Position;
            // this.WeenieClassid = aceO.WeenieClassId;

            // this.GameData.Type = aceO.WeenieClassId;
        }

        public override void OnUse(Player player)
        {
            // TODO: Implement
        }
    }
}
