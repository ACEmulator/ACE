using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Network.Enum;
using ACE.Entity.WorldPackages;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageTestWorldPackage : GameMessage
    {
        public GameMessageTestWorldPackage(Player player): base(GameMessageOpcode.ObjectCreate, GameMessageGroup.Group0A)
        {

            //openly thanking Pea @ PhatAC for ref to life stone proper ids..
            //test world object

            //create segments..
            WorldObjectSegmentModelData modeldata = new WorldObjectSegmentModelData();
            WorldObjectSegmentPhysicsData physicsdata = new WorldObjectSegmentPhysicsData();
            WolrdObjectSegmentGameData gamedata = new WolrdObjectSegmentGameData();

            physicsdata.PhysicsDescriptionFlag = PhysicsDescriptionFlag.Position  | PhysicsDescriptionFlag.MTable | PhysicsDescriptionFlag.Stable | PhysicsDescriptionFlag.CSetup;
            physicsdata.PhysicsState = PhysicsState.Gravity | PhysicsState.IgnoreCollision;

            //only need to fill in flags above
            //physicsdata.Position = player.Position;
            Position newPosition = new Position(player.Position.Cell, player.Position.Offset.X, player.Position.Offset.Y, player.Position.Offset.Z, player.Position.Facing.X, player.Position.Facing.Y, player.Position.Facing.Z, player.Position.Facing.W);
            newPosition.Offset = new System.Numerics.Vector3(player.Position.Offset.X, player.Position.Offset.Y, player.Position.Offset.Z + 1.5f);
            newPosition.Facing = new System.Numerics.Quaternion(0, 0, 0, 0);
            physicsdata.Position = newPosition;

            physicsdata.MTableResourceId = 0x09000026u;
            physicsdata.Stable = 0x20000014u;
            //physicsdata.Petable = 0x34000004u;

            //modelid = 0x02000001u
            physicsdata.CSetup = (uint)0x020002EEu;

            //tech will need to update correct seq for certain action
            //just a example below..
            physicsdata.PositionSequance = (ushort)1;

            //game data min required flags;
            gamedata.Name = "Life Stone";
            gamedata.Type = (ushort)0x1355;
            gamedata.Icon = (ushort)0x1036;
            gamedata.ObjetDescriptionFlag = ObjectDescriptionFlag.LifeStone;      

            //wenie flags
            gamedata.WeenieHeaderFlags = WeenieHeaderFlag.Useability | WeenieHeaderFlag.BlipColour | WeenieHeaderFlag.Radar | WeenieHeaderFlag.UseRadius;
            gamedata.Useability = (uint)32;
            gamedata.BlipColour = (byte)1;
            gamedata.Radar = (byte)4;
            gamedata.UseRadius = 4f;

            //render data packet;
            player.FakeGlobalGuid++;
            ObjectGuid guid = new ObjectGuid(player.FakeGlobalGuid);
            WolrdObjectPackage worldpackage = new WolrdObjectPackage(ObjectType.Creature, guid, modeldata, physicsdata, gamedata);
            worldpackage.Render(Writer);

        }
}
}