using ACE.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Factories
{
    public class MonsterFactory
    {
        public static WorldObject CreateMonster(uint templateId, Position newPosition)
        {
            // TODO: Implement

            // read template from the database, create an object
            // do whatever else it takes to make a monster

            // assign it the position

            // result of reading the database for a drudge sneaker:
            uint wcid = 35442;
            uint newwcid;
            if (wcid < 0x8000)
                newwcid = (ushort)wcid;
            else
                //newwcid = (wcid << 16) | ((wcid >> 16) | 0x8000);
                newwcid = wcid - 0x8000;
            //ushort wcid = 1609;
            string name = "Drudge Sneaker";
            uint setupid = 0x020007DDu;
            uint physicsstate = 1032;
            uint stableid = 536870919;
            uint phstableid = 872415258;
            //uint iconid = 100667445 - 100663296;
            uint iconid = 0x06001035 - 0x6000000;
            iconid = 0x1035;
            uint itemscapacity = 255;
            uint containerscapacity = 255;
            ObjectDescriptionFlag objectdescription = ObjectDescriptionFlag.Stuck | ObjectDescriptionFlag.Attackable; // (uint) 20, 0x14

            //var weenie = WeenieHeaderFlag.Radar | WeenieHeaderFlag.Usable | WeenieHeaderFlag.ItemCapacity;
            var weenie = WeenieHeaderFlag.BlipColour | WeenieHeaderFlag.Radar | WeenieHeaderFlag.TargetType | WeenieHeaderFlag.ItemCapacity;
            ImmutableWorldObject wo = new ImmutableWorldObject(ObjectType.Creature, new ObjectGuid(GuidManager.NewMonsterGuid()), name, (ushort)newwcid, objectdescription, weenie, newPosition);

            var bPhysicsDesc = PhysicsDescriptionFlag.Position | PhysicsDescriptionFlag.Stable | PhysicsDescriptionFlag.Petable | PhysicsDescriptionFlag.CSetup;
            wo.PhysicsData.PhysicsDescriptionFlag = bPhysicsDesc;
            if ((physicsstate & (uint)PhysicsState.Hidden) != 0)
                wo.PhysicsData.PhysicsState = (PhysicsState)physicsstate ^ PhysicsState.Hidden; //  remove hidden so we can see it

            wo.PhysicsData.Stable = stableid;
            wo.PhysicsData.Petable = phstableid;
            wo.PhysicsData.CSetup = setupid;

            // these were defaulting to 1, set to 0
            wo.PhysicsData.PositionSequence = 0;
            wo.PhysicsData.unknownseq0 = 0;
            wo.PhysicsData.PhysicsSequence = 0;
            wo.PhysicsData.JumpSequence = 0;
            wo.PhysicsData.PortalSequence = 0;
            wo.PhysicsData.unknownseq1 = 0;
            wo.PhysicsData.SpawnSequence = 0;

            //bWO.GameDataType = wcid; !!! FINDEN !!!
            wo.GameData.Icon = (ushort)iconid;
            wo.GameData.ItemCapacity = (byte)itemscapacity;
            wo.GameData.ContainerCapacity = (byte)containerscapacity;
            wo.GameData.RadarBehavior = RadarBehavior.ShowAlways;
            wo.GameData.RadarColour = RadarColor.Creature;
            wo.GameData.Usable = Usable.UsableNo;

            // palette, texture and model stuff which should come from DB
            // palette: 67112812,0,2048
            uint palguid = 0x04000F6C - 0x04000000;
            uint palid = 0x04000F6C - 0x04000000;
            uint len = 2048 / 8;
            wo.ModelData.PalleteGuid = (ushort)palguid;
            wo.ModelData.AddPallet((ushort)palid, 0, (byte)len);

            // texture: 9,83892467,83892468^12,83892467,83892468
            // - 0x05000000 = - 83886080
            uint oldt = 83892467 - 83886080;
            uint newt = 83892468 - 83886080;
            wo.ModelData.AddTexture(9, (ushort)oldt, (ushort)newt);
            oldt = 83892467 - 83886080;
            newt = 83892468 - 83886080;
            wo.ModelData.AddTexture(12, (ushort)oldt, (ushort)newt);

            // model: 9,16784289^12,16784289
            // - 0x01000000 = - 16777216
            uint mid = 16784289 - 16777216;
            wo.ModelData.AddModel(9, (ushort)mid);
            mid = 16784289 - 16777216;
            wo.ModelData.AddModel(12, (ushort)mid);

            return wo;
        }
    }
}
