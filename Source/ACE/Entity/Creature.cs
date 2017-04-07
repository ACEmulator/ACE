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
            : base((ObjectType)aceC.CreatureData.TypeId, new ObjectGuid(aceC.Id, GuidType.Creature), aceC.CreatureData.Name, aceC.WeenieClassId, 
                  (ObjectDescriptionFlag)aceC.CreatureData.WdescBitField, (WeenieHeaderFlag)aceC.CreatureData.WeenieFlags, aceC.Position)
        {
            if (aceC.WeenieClassId < 0x8000u)
                this.WeenieClassid = aceC.WeenieClassId;
            else
                this.WeenieClassid = (ushort)(aceC.WeenieClassId - 0x8000);

            this.PhysicsData.MTableResourceId = aceC.CreatureData.MotionTableId;
            this.PhysicsData.Stable = aceC.CreatureData.SoundTableId;
            this.PhysicsData.CSetup = aceC.CreatureData.ModelTableId;
            this.PhysicsData.ObjScale = aceC.CreatureData.ObjectScale;

            // this should probably be determined based on the presence of data.
            this.PhysicsData.PhysicsDescriptionFlag = (PhysicsDescriptionFlag)aceC.CreatureData.PhysicsBitField;
            this.PhysicsData.PhysicsState = (PhysicsState)aceC.CreatureData.PhysicsState;

            // game data min required flags;
            this.Icon = (ushort)aceC.CreatureData.IconId;

            this.GameData.Usable = (Usable)aceC.CreatureData.Usability;
            // intersting finding: the radar color is influenced over the weenieClassId and NOT the blipcolor
            // the blipcolor in DB is 0 whereas the enum suggests it should be 2
            this.GameData.RadarColour = (RadarColor)aceC.CreatureData.BlipColor;
            this.GameData.RadarBehavior = (RadarBehavior)aceC.CreatureData.Radar;
            this.GameData.UseRadius = aceC.CreatureData.UseRadius;

            aceC.CreatureData.WeenieAnimationOverrides.ForEach(ao => this.ModelData.AddModel(ao.Index, (ushort)(ao.AnimationId - 0x01000000)));
            aceC.CreatureData.WeenieTextureMapOverrides.ForEach(to => this.ModelData.AddTexture(to.Index, (ushort)(to.OldId - 0x05000000), (ushort)(to.NewId - 0x05000000)));
            aceC.CreatureData.WeeniePaletteOverrides.ForEach(po => this.ModelData.AddPalette((ushort)(po.SubPaletteId - 0x04000000), (byte)po.Offset, (byte)(po.Length / 8)));
            this.ModelData.PaletteGuid = aceC.CreatureData.PaletteId - 0x04000000;
        }
    }
}
