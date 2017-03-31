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
            : base((ObjectType)aceC.TypeId, new ObjectGuid(aceC.AceObjectId))
        {
            this.Name = aceC.Name;
            this.DescriptionFlags = (ObjectDescriptionFlag)aceC.WdescBitField;
            this.Position = aceC.Position;
            if (aceC.WeenieClassId < 0x8000u)
                this.WeenieClassid = aceC.WeenieClassId;
            else
                this.WeenieClassid = (ushort)(aceC.WeenieClassId - 0x8000);
            this.WeenieFlags = (WeenieHeaderFlag)aceC.WeenieFlags;

            this.PhysicsData.MTableResourceId = aceC.MotionTableId;
            this.PhysicsData.Stable = aceC.SoundTableId;
            this.PhysicsData.CSetup = aceC.ModelTableId;
            // does this have to be hardcoded or ist the scale in one of the dat files?
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

            // palette, texture and model stuff which should come from DB
            // palette: 67112812,0,2048
            uint palguid = 0x04000F6C - 0x04000000;
            uint palid = 0x04000F6C - 0x04000000;
            uint len = 2048 / 8;
            this.ModelData.PaletteGuid = (ushort)palguid;
            this.ModelData.AddPalette((ushort)palid, 0, (byte)len);

            // texture: 9,83892467,83892468^12,83892467,83892468
            // - 0x05000000 = - 83886080
            uint oldt = 83892467 - 83886080;
            uint newt = 83892468 - 83886080;
            this.ModelData.AddTexture(9, (ushort)oldt, (ushort)newt);
            oldt = 83892467 - 83886080;
            newt = 83892468 - 83886080;
            this.ModelData.AddTexture(12, (ushort)oldt, (ushort)newt);

            // model: 9,16784289^12,16784289
            // - 0x01000000 = - 16777216
            uint mid = 16784289 - 16777216;
            this.ModelData.AddModel(9, (ushort)mid);
            mid = 16784289 - 16777216;
            this.ModelData.AddModel(12, (ushort)mid);
        }
    }
}
