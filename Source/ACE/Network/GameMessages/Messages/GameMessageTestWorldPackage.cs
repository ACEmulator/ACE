using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Network.Enum;
using ACE.Entity.WorldPackages;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageTestWorldPackage : GameMessage
    {
        public GameMessageTestWorldPackage(Position pos): base(GameMessageOpcode.ObjectCreate, GameMessageGroup.Group0A)
        {
            //test world object

            //create segments..
            WorldObjectSegmentModelData modeldata = new WorldObjectSegmentModelData();
            WorldObjectSegmentPhysicsData physicsdata = new WorldObjectSegmentPhysicsData();
            WolrdObjectSegmentGameData gamedata = new WolrdObjectSegmentGameData();

            physicsdata.PhysicsDescriptionFlag = PhysicsDescriptionFlag.Position  | PhysicsDescriptionFlag.MTable | PhysicsDescriptionFlag.Stable | PhysicsDescriptionFlag.Petable | PhysicsDescriptionFlag.CSetup;
            physicsdata.PhysicsState = PhysicsState.Static;

            //only need to fill in flags above
            physicsdata.Position = pos;
            physicsdata.MTableResourceId = 0x09000001u;
            physicsdata.Stable = 0x20000001u;
            physicsdata.Petable = 0x34000004u;
            physicsdata.CSetup = 0x02000001u;

            //tech will need to update correct seq for certain action
            //just a example below..
            physicsdata.PositionSequance = (ushort)1;

            //game data min required flags;
            gamedata.Name = "Test Object";
            gamedata.Type = (ushort)1;
            gamedata.Icon = (ushort)0x1036;
            gamedata.ObjetDescriptionFlag = ObjectDescriptionFlag.Attackable;      

            //wenie flags
            gamedata.WeenieHeaderFlags = WeenieHeaderFlag.ItemCapacity | WeenieHeaderFlag.ContainerCapacity | WeenieHeaderFlag.Useability | WeenieHeaderFlag.BlipColour | WeenieHeaderFlag.Radar;
            gamedata.ItemCapacity = (byte)102;
            gamedata.ContainerCapacity = (byte)7;
            gamedata.Useability = 1u;
            gamedata.BlipColour = (byte)9;
            gamedata.Radar = (byte)4;

            //render data packet;
            ObjectGuid guid = new ObjectGuid(100);
            WolrdObjectPackage worldpackage = new WolrdObjectPackage(ObjectType.Creature, guid, modeldata, physicsdata, gamedata);
            worldpackage.Render(Writer);

        }
}
}