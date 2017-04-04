using ACE.Entity.Enum;
using ACE.Network.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity
{
    public class Creature : MutableWorldObject
    {
        public Creature(AceCreatureStaticLocation aceC)
            : base((ObjectType)aceC.TypeId, new ObjectGuid(aceC.AceObjectId), aceC.Name, aceC.WeenieClassId, (ObjectDescriptionFlag)aceC.WdescBitField, (WeenieHeaderFlag)aceC.WeenieFlags, aceC.Position)
        {
            if (aceC.WeenieClassId < 0x8000u)
                this.WeenieClassid = aceC.WeenieClassId;
            else
                this.WeenieClassid = (ushort)(aceC.WeenieClassId - 0x8000);

            this.PhysicsData.MTableResourceId = aceC.MotionTableId;
            this.PhysicsData.Stable = aceC.SoundTableId;
            this.PhysicsData.CSetup = aceC.ModelTableId;
            // does this have to be hardcoded or is the scale in one of the dat files?
            this.PhysicsData.ObjScale = 1.0f;

            // this should probably be determined based on the presence of data.
            this.PhysicsData.PhysicsDescriptionFlag = (PhysicsDescriptionFlag)aceC.PhysicsBitField;
            this.PhysicsData.PhysicsState = (PhysicsState)aceC.PhysicsState;

            // game data min required flags;
            this.Icon = (ushort)aceC.IconId;

            this.GameData.Usable = (Usable)aceC.Usability;
            // intersting finding: the radar color is influenced over the weenieClassId and NOT the blipcolor
            // the blipcolor in DB is 0 whereas the enum suggests it should be 2
            this.GameData.RadarColour = (RadarColor)aceC.BlipColor;
            this.GameData.RadarBehavior = (RadarBehavior)aceC.Radar;
            this.GameData.UseRadius = aceC.UseRadius;

            aceC.WeenieAnimationOverrides.ForEach(ao => this.ModelData.AddModel(ao.Index, (ushort)(ao.AnimationId - 0x01000000)));
            aceC.WeenieTextureMapOverrides.ForEach(to => this.ModelData.AddTexture(to.Index, (ushort)(to.OldId - 0x05000000), (ushort)(to.NewId - 0x05000000)));
            aceC.WeeniePaletteOverrides.ForEach(po => this.ModelData.AddPalette((ushort)(po.SubPaletteId - 0x04000000), (byte)po.Offset, (byte)(po.Length / 8)));
            this.ModelData.PaletteGuid = aceC.PaletteId - 0x04000000;
        }
    }
}
